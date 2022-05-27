using Firebase;
using Firebase.Database;
using System.Collections;
using UnityEngine;
using BeauRoutine;

namespace Wrapper
{
    public class SaveManager
    {
        private bool isFirebaseReady = false;
        private FirebaseApp app;
        private DatabaseReference dbReference;
        private UserSave currentUserSave;
        
        public bool isUserLoggedIn = false;
        public static readonly string firebaseURL = "https://filament-zombies-default-rtdb.firebaseio.com/";

        public SaveManager()
        {
            Events.SubmitResearchCode += Login;
            Events.UpdateRemoteSave += UpdateRemoteSave;

            Routine.Start(InitFirebase());
        }

        ~SaveManager()
        {
            Events.SubmitResearchCode -= Login;
            Events.UpdateRemoteSave -= UpdateRemoteSave;
        }

        public IEnumerator InitFirebase()
        {
            AppOptions devEnvOptions = new AppOptions
            {
                ApiKey = "AIzaSyBdbF7HlY93besU9gJKdB4LKN3E5uFpG78",
                AppId = "1:721683477410:android:7ff5322e48d5134cd8b8fb",
                ProjectId = "filament-zombies"
            };

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                DependencyStatus dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    app = FirebaseApp.DefaultInstance;
                    app.Options.DatabaseUrl = new System.Uri(firebaseURL);
                    isFirebaseReady = true;
                }
                else
                    Debug.LogErrorFormat("Could not resolve all Firebase dependencies: {0}", dependencyStatus);
            });

            yield return Routine.Race
            (
                Routine.WaitCondition(() => isFirebaseReady),
                Routine.WaitSeconds(5)
            );

            Debug.LogFormat("FireBaseReady = {0}", isFirebaseReady);
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            //TestSave();
        }

        private void TestSave()
        {
            currentUserSave = new UserSave("Sibi", "CU_01");

            BBSaveData bbSave = new BBSaveData { lastCompletedLevelID = "L04", IntroDialogueSeen = false };
            Events.UpdateMinigameSaveData(Game.BlackBox, bbSave);
            Events.AddReward?.Invoke("CU_02");
        }

        private void Login(string researchCode)
        {
            Routine.Start(LoginRoutine(researchCode));
        }

        private IEnumerator LoginRoutine(string researchCode)
        {
            Future<UserSave> futureSave = Future.Create<UserSave>();
            Routine loadRoutine = Routine.Start(LoadUser(futureSave, researchCode));
            futureSave.LinkTo(loadRoutine);
            yield return futureSave;

            if (futureSave.IsComplete())
            {
                currentUserSave = futureSave;
                isUserLoggedIn = true;
            }

            Events.LoginEvent?.Invoke(isUserLoggedIn);
        }

        private IEnumerator LoadUser(Future<UserSave> futureSave, string researchCode)
        {
            string formattedCode = researchCode.Trim().ToLower();

            if (formattedCode.Equals(string.Empty))
            {
                Debug.LogError("Empty research code entered");
                futureSave.Fail();
            }
            else if (dbReference == null)
            {
                Debug.LogError("No database reference on load");
                futureSave.Fail();
            }
            else
            {
                dbReference.Child("data").Child(formattedCode).GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("Error loading user data using research code");
                        futureSave.Fail();
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        if (!(snapshot.Exists))
                        {
                            futureSave.Fail();
                        }
                        else
                        {
                            UserSave save = JsonUtility.FromJson<UserSave>(snapshot.GetRawJsonValue());
                            futureSave.Complete(save);
                        }
                    }
                });
            }

            yield return Routine.Race
            (
                Routine.WaitCondition(futureSave.IsDone),
                Routine.WaitSeconds(3)
            );
        }

        private bool UpdateRemoteSave()
        {
            if (dbReference == null)
            {
                Debug.LogError("No database reference on save");
                return false;
            }

            string json = JsonUtility.ToJson(currentUserSave);
            if (json == "")
            {
                Debug.LogError("Empty UserSave");
                return false;
            }

            dbReference.Child("data").Child(currentUserSave.id).SetRawJsonValueAsync(json);
            return true;
        }
    }
}
