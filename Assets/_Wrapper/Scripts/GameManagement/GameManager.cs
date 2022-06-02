using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get { return _instance; } }
        private static GameManager _instance;

        private SaveManager saveManager;

        private void Awake()
        {
            InitSingleton();
            saveManager = new SaveManager();
        }

        private void OnEnable()
        {
            Events.OpenMinigame += OpenMinigame;
        }

        private void OnDisable()
        {
            Events.OpenMinigame -= OpenMinigame;
        }

        private void OpenMinigame(Minigame minigame)
        {
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
