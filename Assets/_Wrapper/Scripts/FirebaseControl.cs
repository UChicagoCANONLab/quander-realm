using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class FirebaseControl
    {
        private FirebaseApp app;
        private DatabaseReference m_reference;
        public static readonly string firebaseURL = "https://filament-zombies-default-rtdb.firebaseio.com/";

        public void Init()
        {
            AppOptions devEnvOptions = new AppOptions
            {
                ApiKey = "AIzaSyBdbF7HlY93besU9gJKdB4LKN3E5uFpG78",
                //AppId = "1:721683477410:android:7ff5322e48d5134cd8b8fb",
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

        public bool Save(UserSave data)
        {
            if (m_reference != null)
            {
                string json = JsonUtility.ToJson(data);
                if (json == "")
                {
                    Debug.LogWarning("empty userSave");
                    return false;
                }
                m_reference.Child("users").Child(json.Replace(".", ",")).SetRawJsonValueAsync(json);
                return true;
            }
            else
            {
                Debug.LogError("No database reference on save");
                return false;
            }
        }

    }
}
