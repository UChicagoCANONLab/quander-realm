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

        private void Awake()
        {
            InitSingleton();

            Events.OpenMinigame += OpenMinigame;

            backButton.onClick.AddListener(() =>
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                Debug.Log("Back To Main");
                SceneManager.LoadScene(0);
            });
        }

        private void Start()
        {
            Events.PrintDialogue?.Invoke("W_Tutorial");
        }

        private void OpenMinigame(Minigame minigame)
        {
            Debug.Log("Opening " + minigame.name);
            SceneManager.LoadScene(minigame.StartScene);
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
