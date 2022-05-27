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
        private DatabaseReference m_reference;
        private UserSave currentUserSave;
        
#if PRODUCTION_FB
        public static readonly string firebaseURL = "https://quander-production-default-rtdb.firebaseio.com/";
#else
        public static readonly string firebaseURL = "https://filament-zombies-default-rtdb.firebaseio.com/";
#endif



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
            m_reference = FirebaseDatabase.DefaultInstance.RootReference;

            //TestSave();
        }

        //private void TestSave()
        //{
        //    currentUserSave = new UserSave("Sibi", "CU_01");

        //    BBSaveData bbSave = new BBSaveData { lastCompletedLevelID = "L04", IntroDialogueSeen = false };
        //    Events.UpdateMinigameSaveData(Game.BlackBox, bbSave);
        //    Events.AddReward?.Invoke("CU_02");
        //}

        private bool Login(string rCode)
        {
            bool isLoggedIn = true;

            Debug.Log("Attempting Login");
            //check if user with code exists
            //Future<UserSave> future = Future.Create<UserSave>();
            //Routine routine = Routine.Start(LoadRoutine(future, email));
            //future.LinkTo(routine);

            //fetch the save file
            //set current usersave to that
            Debug.Log("Login Successful");

            return isLoggedIn;
        }

        private bool UpdateRemoteSave()
        {
            if (m_reference == null)
            {
                Debug.LogError("No database reference on save");
                return false;
            }

            string json = JsonUtility.ToJson(currentUserSave);
            if (json == "")
            {
                Debug.LogWarning("empty userSave");
                return false;
            }

            m_reference.Child("data").Child(currentUserSave.id).SetRawJsonValueAsync(json);
            return true;
        }
    }
}
