using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get { return _instance; } }
        private static GameManager _instance;
        private GameObject loadingScreenGO = null;

        [SerializeField] private GameObject loginScreen;
        [SerializeField] private GameObject DebugPanel;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private GameObject loadingScreenPrefab;

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
            Events.ToggleLoadingScreen += ToggleLoadingScreen;
        }

        private void OnDisable()
        {
            Events.OpenMinigame -= OpenMinigame;
            Events.ToggleLoadingScreen -= ToggleLoadingScreen;
        }

        private void ToggleLoadingScreen()
        {
            if (loadingScreenGO == null)
                loadingScreenGO = Instantiate(loadingScreenPrefab);
            else
                StartCoroutine("DestroyLoadingScreen");
        }

        private IEnumerator DestroyLoadingScreen()
        {
            yield return new WaitForSeconds(2f);
            Destroy(loadingScreenGO);
            loadingScreenGO = null;
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