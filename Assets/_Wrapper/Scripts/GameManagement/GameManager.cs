using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeauRoutine;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get { return _instance; } }

        private static GameManager _instance;
        private GameObject loadingScreenGO = null;
        private const string introSequenceID = "W_Intro";

        [SerializeField] private float loadingToggleDelay = 0.5f;
        [SerializeField] private GameObject loginScreen;
        [SerializeField] private GameObject debugPanel;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private GameObject loadingScreenPrefab;

        private void Awake()
        {
            InitSingleton();
            Routine.Start(IntroDialogueRoutine()); //todo: also wait for loadingScreenGO to be null?
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
                debugPanel.SetActive(!(debugPanel.activeInHierarchy));
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

        private void OpenMinigame(Minigame minigame)
        {
            SceneManager.LoadScene(minigame.StartScene);
        }

        private void ToggleLoadingScreen()
        {
            if (loadingScreenGO == null)
                loadingScreenGO = Instantiate(loadingScreenPrefab);
            else
                Routine.Start(DestroyLoadingScreen()); //todo: debug, delete later
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

        // todo: debug, delete later
        private IEnumerator DestroyLoadingScreen()
        {
            yield return new WaitForSeconds(loadingToggleDelay);
            Destroy(loadingScreenGO);
            loadingScreenGO = null;
        }

        private IEnumerator IntroDialogueRoutine()
        {
            while (!(saveManager.isUserLoggedIn) || !(saveManager.currentUserSave != null))
                yield return null;

            if (saveManager.HasPlayerSeenIntroDialogue())
                yield break;

            Events.StartDialogueSequence?.Invoke(introSequenceID);
            saveManager.ToggleIntroDialogueSeen(true);
        }

        #endregion
    }
}