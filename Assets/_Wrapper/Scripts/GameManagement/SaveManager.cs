using Firebase;
using Firebase.Database;
using System.Collections;
using UnityEngine;
using BeauRoutine;

namespace Wrapper
{
    public class SaveManager : MonoBehaviour
    {
        private bool isDatabaseReady = false;
        private FirebaseApp app;
        private DatabaseReference dbReference;
        private DataSnapshot databaseSnapshot;
        private UserSave currentUserSave = null;
        
        [HideInInspector] public bool isUserLoggedIn = false;
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
            Events.UpdateRemoteSave += UpdateRemoteSave;
            Events.GetMinigameSaveData += GetMinigameSaveData;
            Events.UpdateMinigameSaveData += UpdateMinigameSaveData;
        }

        private void OnDisable()
        {
            Events.AddReward -= AddReward;
            Events.SubmitResearchCode -= Login;
            Events.UpdateRemoteSave -= UpdateRemoteSave;
            Events.GetMinigameSaveData -= GetMinigameSaveData;
            Events.UpdateMinigameSaveData -= UpdateMinigameSaveData;
        }

        public IEnumerator InitFirebase()
        {
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
            yield return Routine.Start(GetDatabaseSnapshot());
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
                Routine.WaitCondition(() => UpdateRemoteSave());
            }

            currentUserSave = JsonUtility.FromJson<UserSave>(
                databaseSnapshot.Child("userData").Child(formattedCode).GetRawJsonValue());

            Events.UpdateLoginStatus?.Invoke(LoginStatus.Success);
            isUserLoggedIn = true;
        }

        private IEnumerator GetDatabaseSnapshot()
        {
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
        }

        #endregion
    }
}
