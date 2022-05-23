using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get { return _instance; } }
        private static GameManager _instance;

        private void Awake()
        {
            InitSingleton();
        }

        private void OnEnable()
        {
            Events.OpenMinigame += OpenMinigame;
        }

            FirebaseCall();

            Events.OpenMinigame.AddListener(OpenMinigame);
            backButton.onClick.AddListener(() => 
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                Debug.Log("Back To Main");
                SceneManager.LoadScene(0); 
            });
        private void OnDisable()
        {
            Events.OpenMinigame -= OpenMinigame;
        }

        private void OpenMinigame(Minigame minigame)
        {
            SceneManager.LoadScene(minigame.StartScene);
        }

        private void FirebaseCall()
        {
            FirebaseControl firebaseControl = new FirebaseControl();
            UserSave data = new UserSave(2345, "Rob");

            firebaseControl.Init();
            firebaseControl.Save(data);
        }

        #region Helpers

        private void InitSingleton()
        {
            DontDestroyOnLoad(this);

            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
                _instance = this;
        }

        #endregion
    }
}
