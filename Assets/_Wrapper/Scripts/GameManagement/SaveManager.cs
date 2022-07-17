using System.Collections;
using UnityEngine;
using BeauRoutine;
using System;
#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase;
using Firebase.Database;
#else
using System.Runtime.InteropServices;
#endif

namespace Wrapper
{
    public class SaveManager : MonoBehaviour
    {
        private bool isDatabaseReady = false;
#if !UNITY_WEBGL || UNITY_EDITOR
        private FirebaseApp app;
        private DatabaseReference dbReference;
        private DataSnapshot databaseSnapshot;
#else
        private string researchCodeExists = "";
        private string loadDataJson = "";
#endif
        
        [HideInInspector] public bool isUserLoggedIn = false;
        public UserSave currentUserSave = null;
        public int researchCodeLength = 6;

#if PRODUCTION_FB
        public static readonly string firebaseURL = "https://quander-production-default-rtdb.firebaseio.com/";
#else
        public static readonly string firebaseURL = "https://filament-zombies-default-rtdb.firebaseio.com/";
#endif

        private void Awake()
        {
            Routine.Start(InitFirebase());   
        }

        private void OnEnable()
        {
            Events.AddReward += AddReward;
            Events.SubmitResearchCode += Login;
            Events.IsRewardUnlocked += IsRewardUnlocked;
            Events.UpdateRemoteSave += UpdateRemoteSave;
            Events.GetMinigameSaveData += GetMinigameSaveData;
            Events.UpdateMinigameSaveData += UpdateMinigameSaveData;
        }

        private void OnDisable()
        {
            Events.AddReward -= AddReward;
            Events.SubmitResearchCode -= Login;
            Events.IsRewardUnlocked -= IsRewardUnlocked;
            Events.UpdateRemoteSave -= UpdateRemoteSave;
            Events.GetMinigameSaveData -= GetMinigameSaveData;
            Events.UpdateMinigameSaveData -= UpdateMinigameSaveData;
        }

        private bool IsRewardUnlocked(string rewardID)
        {
            return currentUserSave.HasReward(rewardID);
        }

        private IEnumerator InitFirebase()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
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
                Routine.WaitSeconds(5));

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
            yield return GetDatabaseSnapshot();
            if(!(isDatabaseReady))
            {
                Debug.LogError("Error: Could not retrieve database snapshot");
                Events.UpdateLoginStatus?.Invoke(LoginStatus.DatabaseError);
                yield break;
            }

            string formattedCode = researchCode.Trim().ToLower();
            if (researchCode.Length != researchCodeLength)
            {
                Debug.LogErrorFormat("Error: Research code must be a {0} character long, lowercase, alphanumeric", researchCodeLength);
                Events.UpdateLoginStatus?.Invoke(LoginStatus.FormatError);
                yield break;
            }

#if !UNITY_WEBGL || UNITY_EDITOR
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
                    Routine.WaitSeconds(5));

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
#if !UNITY_WEBGL || UNITY_EDITOR
            if (dbReference == null)
                yield break;

            dbReference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Debug.LogError("db snapshot task is faulted");
                
                else if (task.IsCompleted)
                {
                    databaseSnapshot = task.Result;
                    isDatabaseReady = true;
                }
            });

            yield return Routine.Race(
                Routine.WaitCondition(() => isDatabaseReady),
                Routine.WaitSeconds(5));
#else // is Unity WebGL
            isDatabaseReady = true;
            yield return null;
#endif
        }

        #endregion

        #region UserSave

        private void AddReward(string rewardID)
        {
            currentUserSave.AddReward(rewardID);
            UpdateRemoteSave();
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
#if !UNITY_WEBGL || UNITY_EDITOR
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

#if UNITY_WEBGL && !UNITY_EDITOR

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
    }
}
