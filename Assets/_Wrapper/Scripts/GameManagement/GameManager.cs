using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get { return _instance; } }
        private static GameManager _instance;

        [SerializeField] private GameObject loginScreen;
        [SerializeField] private GameObject DebugPanel;
        [SerializeField] private SaveManager saveManager;

        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            if (!(saveManager.isUserLoggedIn))
                loginScreen.SetActive(true);
        }

#if !PRODUCTION_FB
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
                DebugPanel.SetActive(!(DebugPanel.activeInHierarchy));
        }
#endif
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