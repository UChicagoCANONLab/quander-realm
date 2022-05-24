using Firebase;
using Firebase.Database;
using UnityEngine;

namespace Wrapper
{
    public class SaveManager
    {
        private FirebaseApp app;
        private DatabaseReference m_reference;
        private UserSave userSave;
        
        public static readonly string firebaseURL = "https://filament-zombies-default-rtdb.firebaseio.com/";

        public SaveManager()
        {
            Events.SubmitResearchCode += Login;
            userSave = new UserSave();
        }

        ~SaveManager()
        {
            Events.SubmitResearchCode -= Login;
        }

        public void InitFirebase()
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
                    //Create and hold a reference to your FirebaseApp,
                    //where app is a Firebase.FirebaseApp property of your application class.
                    app = FirebaseApp.DefaultInstance;
                    app.Options.DatabaseUrl = new System.Uri(firebaseURL);
                    //Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    //Firebase Unity SDK is not safe to use here.
                }
            });

            m_reference = FirebaseDatabase.DefaultInstance.RootReference;
        }

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

        public bool Save(UserSave data)
        {
            if (m_reference == null)
            {
                Debug.LogError("No database reference on save");
                return false;
            }

            string json = JsonUtility.ToJson(data);
            if (json == "")
            {
                Debug.LogWarning("empty userSave");
                return false;
            }

            m_reference.Child("data").Child(data.id).SetRawJsonValueAsync(json);
            return true;
        }
    }
}
