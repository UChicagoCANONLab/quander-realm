using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get { return _instance; } }
        private static GameManager _instance;

        public Button backButton;

        private Firebase.FirebaseApp app;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
                _instance = this;

            FirebaseInit();

            Events.OpenMinigame.AddListener(OpenMinigame);
            backButton.onClick.AddListener(() => 
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                Debug.Log("Back To Main");
                SceneManager.LoadScene(0); 
            });
        }

        private void OpenMinigame(Minigame minigame)
        {
            Debug.Log("Opening " + minigame.name);
            SceneManager.LoadScene(minigame.StartScene);
        }

        private void FirebaseInit()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }
    }
}
