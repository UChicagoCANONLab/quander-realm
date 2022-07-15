using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeauRoutine;
using UnityEngine.UI;

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
        [SerializeField] private Button debugButton;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private GameObject loadingScreenPrefab;

        public readonly string rewardsPath = "_Wrapper/Rewards/RewardAssets/";
        public RewardAsset[] rewardAssets;
        
        #region Unity Functions

        private void Awake()
        {
            InitSingleton();
            InitRewardAssetArray();
            Routine.Start(IntroDialogueRoutine()); //todo: also wait for loadingScreenGO to be null?
            debugButton.onClick.AddListener(() => debugPanel.SetActive(!(debugPanel.activeInHierarchy))); //todo: debug, delete later
            Input.multiTouchEnabled = false;
        }

        private void Start()
        {
            if (!(saveManager.isUserLoggedIn))
                loginScreen.SetActive(true);
        }
        private void OnEnable()
        {
            Events.OpenMinigame += OpenMinigame;
            Events.ToggleLoadingScreen += ToggleLoadingScreen;
            Events.CollectAndDisplayReward += CollectAndDisplayReward;
        }

        private void OnDisable()
        {
            Events.OpenMinigame -= OpenMinigame;
            Events.ToggleLoadingScreen -= ToggleLoadingScreen;
            Events.CollectAndDisplayReward -= CollectAndDisplayReward;
        }

        #endregion

        private void OpenMinigame(Minigame minigame)
        {
            SceneManager.LoadScene(minigame.StartScene);
        }

        private void ToggleLoadingScreen()
        {
            if (loadingScreenGO == null)
                loadingScreenGO = Instantiate(loadingScreenPrefab);
            else
                Routine.Start(DestroyLoadingScreen()); // todo: debug, delete later
        }

        private void CollectAndDisplayReward(Game game, int level)
        {
            RewardAsset levelReward = Array.Find(rewardAssets, (reward) => reward.game == game && reward.level == level);
            if (levelReward == null)
            {
                Debug.LogFormat("No Reward found for game {1} level {2}", game, level);
                return;
            }

            Events.AddReward(levelReward.rewardID);

            //todo: call a function that creates the card and displays it in the reward card panel
            Debug.LogFormat("Won Reward {0} in game {1} at level {2}", levelReward.rewardID, game, level);
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

        private void InitRewardAssetArray()
        {
            rewardAssets = Resources.LoadAll<RewardAsset>(rewardsPath);
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