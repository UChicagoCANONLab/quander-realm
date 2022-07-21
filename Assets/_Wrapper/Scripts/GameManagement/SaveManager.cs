using System.Collections;
using UnityEngine;
using BeauRoutine;
using System;
using UnityEngine.Networking;
using System.Linq;
#if !UNITY_WEBGL
using Firebase;
using Firebase.Database;
#else
using System.Runtime.InteropServices;
#endif

namespace Wrapper
{
    public class SaveManager : MonoBehaviour
    {
        [HideInInspector] public bool isUserLoggedIn = false;
        public UserSave currentUserSave = null;
        public int researchCodeLength = 6;

        private float networkRequestTimeout = 7f;
        private bool isConnectedToInternet = false;
        private bool isDatabaseReady = false;

#if !UNITY_WEBGL
        private FirebaseApp app;
        private DatabaseReference dbReference;
        private DataSnapshot databaseSnapshot;
#else
        private string researchCodeExists = "";
        private string loadDataJson = "";
#endif

#if PRODUCTION_FB
        public static readonly string firebaseURL = "https://quander-production-default-rtdb.firebaseio.com/";
        public readonly string testConnectionURL = "https://console.firebase.google.com/project/quander-production/database/quander-production-default-rtdb/data";
#else
        public static readonly string firebaseURL = "https://filament-zombies-default-rtdb.firebaseio.com/";
        public readonly string testConnectionURL = "https://console.firebase.google.com/project/filament-zombies/database/filament-zombies-default-rtdb/data";
#endif

        private void Awake()
        {
            Routine.Start(InitFirebase());
        }

        private void OnEnable()
        {
            Events.AddReward += AddReward;
            Events.ClearSaveFile += ClearSave;
            Events.SubmitResearchCode += Login;
            Events.IsRewardUnlocked += IsRewardUnlocked;
            Events.UpdateRemoteSave += UpdateRemoteSave;
            Events.GetMinigameSaveData += GetMinigameSaveData;
            Events.UpdateMinigameSaveData += UpdateMinigameSaveData;
        }

        private void OnDisable()
        {
            Events.AddReward -= AddReward;
            Events.ClearSaveFile -= ClearSave;
            Events.SubmitResearchCode -= Login;
            Events.IsRewardUnlocked -= IsRewardUnlocked;
            Events.UpdateRemoteSave -= UpdateRemoteSave;
            Events.GetMinigameSaveData -= GetMinigameSaveData;
            Events.UpdateMinigameSaveData -= UpdateMinigameSaveData;
        }

        private IEnumerator InitFirebase()
        {
#if !UNITY_WEBGL
            bool isFirebaseReady = false;
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                DependencyStatus dependencyStatus = task.Result;

                if (dependencyStatus != DependencyStatus.Available)
                    Debug.LogError("Error: Could not resolve all Firebase dependencies");
                else
                {
                    app = FirebaseApp.DefaultInstance;
                    app.Options.DatabaseUrl = new System.Uri(firebaseURL);
                    isFirebaseReady = true;
                }
            });

            yield return Routine.Race(
                Routine.WaitCondition(() => isFirebaseReady),
                Routine.WaitSeconds(networkRequestTimeout));

            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            dbReference.KeepSynced(true);
#else   // Is WebGL
            yield return null;
#endif
        }

        #region Login

        private void Login(string researchCode)
        {
            isUserLoggedIn = false;
            currentUserSave = new UserSave();
            Routine.Start(LoginRoutine(researchCode));
        }

        private IEnumerator LoginRoutine(string researchCode)
        {
#if !UNITY_WEBGL
            yield return TestInternetConnection();
            if (!(isConnectedToInternet))
            {
                Debug.LogError("Error: Internet connection issue");
                Events.UpdateLoginStatus?.Invoke(LoginStatus.ConnectionError);
                yield break;
            }
#endif
            yield return GetDatabaseSnapshot();
            if (!(isDatabaseReady))
            {
                Debug.LogError("Error: Could not retrieve database snapshot");
                Events.UpdateLoginStatus?.Invoke(LoginStatus.DatabaseError);
                yield break;
            }

            string formattedCode = researchCode.Trim();
            if (researchCode.Length != researchCodeLength || !(researchCode.All(char.IsLetterOrDigit)))
            {
                Debug.LogErrorFormat("Error: Research code must be a {0} character long alphanumeric string", researchCodeLength);
                Events.UpdateLoginStatus?.Invoke(LoginStatus.FormatError);
                yield break;
            }

#if !UNITY_WEBGL
            bool isUserVerified = databaseSnapshot.Child("researchCodes").HasChild(formattedCode);
            if (!(isUserVerified))
            {
                Debug.LogErrorFormat("Error: Could not find user {0} in database", formattedCode);
                Events.UpdateLoginStatus?.Invoke(LoginStatus.NonExistentUserError);
                yield break;
            }

            bool isUserDataPresent = databaseSnapshot.Child("userData").HasChild(formattedCode);
            if (!isUserDataPresent)
            {
                currentUserSave.id = formattedCode;

                yield return Routine.Race(
                    Routine.WaitCondition(() => UpdateRemoteSave()),
                    Routine.WaitSeconds(networkRequestTimeout));

                yield return GetDatabaseSnapshot();
            }

            currentUserSave = JsonUtility.FromJson<UserSave>(
                databaseSnapshot.Child("userData").Child(formattedCode).GetRawJsonValue());
#else // is Unity WebGL
            DoesResearchCodeExist(formattedCode);
            while(string.IsNullOrEmpty(researchCodeExists))
                yield return null;

            // Check if code exists
            if(researchCodeExists == "F")
            {
                researchCodeExists = "";
                Debug.LogErrorFormat("Error: Could not find user {0} in database", formattedCode);
                yield break;
            }
            researchCodeExists = "";

            // Code exists, great. Now let's see if they have save data already
            LoadData(formattedCode);
            while(string.IsNullOrEmpty(loadDataJson))
                yield return null;
            
            Debug.Log(loadDataJson);
            if(loadDataJson == "none")
            {
                Debug.LogWarning("User doesn't exist, creating it now.");
            }
            currentUserSave = JsonUtility.FromJson<UserSave>(loadDataJson);
#endif
            Events.UpdateLoginStatus?.Invoke(LoginStatus.Success);
            isUserLoggedIn = true;
        }

        private IEnumerator GetDatabaseSnapshot()
        {
#if !UNITY_WEBGL
            if (dbReference == null)
                yield break;

            dbReference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Debug.LogErrorFormat("db snapshot task is faulted: {0}", task.Exception);

                else if (task.IsCompleted)
                {
                    databaseSnapshot = task.Result;
                    isDatabaseReady = true;
                }
            });

            yield return Routine.Race(
                Routine.WaitCondition(() => isDatabaseReady),
                Routine.WaitSeconds(networkRequestTimeout));
#else // is Unity WebGL
            isDatabaseReady = true;
            yield return null;
#endif
        }

        private IEnumerator TestInternetConnection()
        {
            isConnectedToInternet = false;

            UnityWebRequest request = new UnityWebRequest(testConnectionURL);
            request.timeout = (int)networkRequestTimeout;
            yield return request.SendWebRequest();

            if (request.error == null && request.result != UnityWebRequest.Result.ConnectionError)
                isConnectedToInternet = true;
        }

        #endregion

        #region UserSave

        private void AddReward(string rewardID)
        {
            currentUserSave.AddReward(rewardID);
            UpdateRemoteSave();
        }

        private bool IsRewardUnlocked(string rewardID)
        {
            return currentUserSave.HasReward(rewardID);
        }

        private string GetMinigameSaveData(Game game)
        {
            return currentUserSave.GetMinigameSave(game);
        }

        private void UpdateMinigameSaveData(Game game, object minigameSave)
        {
            currentUserSave.UpdateMinigameSave(game, minigameSave);
            UpdateRemoteSave();
        }

        private bool UpdateRemoteSave()
        {
#if !UNITY_WEBGL
            if (dbReference == null)
            {
                Debug.LogError("No database reference on save");
                return false;
            }

            string json = JsonUtility.ToJson(currentUserSave);
            if (json.Equals(string.Empty))
            {
                Debug.LogError("Empty UserSave");
                return false;
            }

            dbReference.Child("userData").Child(currentUserSave.id).SetRawJsonValueAsync(json);
            return true;
#else   // is Unity WebGL
            string json = JsonUtility.ToJson(currentUserSave);
            if (json.Equals(string.Empty))
            {
                Debug.LogError("Empty UserSave");
                return false;
            }
            SaveData(currentUserSave.id, json);
            return true;
#endif
        }

        private void ClearSave()
        {
            string id = currentUserSave.id;
            currentUserSave = new UserSave(id);
            UpdateRemoteSave();
        }

        //todo: merge intro dialogue methods or make it a property with get/set
        public bool HasPlayerSeenIntroDialogue()
        {
            return currentUserSave.introDialogueSeen;
        }

        public void ToggleIntroDialogueSeen(bool hasSeen)
        {
            currentUserSave.introDialogueSeen = hasSeen;
            UpdateRemoteSave();
        }

        #endregion

        #region WebGL dll imports

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void DoesResearchCodeExist(string codeString);
        [DllImport("__Internal")]
        private static extern void LoadData(string codeString);
        [DllImport("__Internal")]
        private static extern void SaveData(string codeString, string json);
        public void ResearchCodeCallback(string str)
        {
            researchCodeExists = str;
        }

        public void LoadDataCallback(string str)
        {
            loadDataJson = str;
        }
#endif

        #endregion
    }
}
