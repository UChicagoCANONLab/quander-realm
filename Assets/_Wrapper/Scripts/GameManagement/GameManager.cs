using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeauRoutine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get { return _instance; } }

        private static GameManager _instance;
        private GameObject loadingScreenGO = null;
        private const string introSequenceID = "W_Intro";

        [SerializeField] private float loadingToggleDelay = 0.5f;
        [SerializeField] private DebugScreen debugScreen;
        [SerializeField] private Button debugButton;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private CardPopup cardPopup;
        [SerializeField] private BadgePopup badgePopup;
        [SerializeField] private GamePopup gamePopup;
        [SerializeField] private AgePopup agePopup;
        [SerializeField] private CoinPopup coinPopup;
        [SerializeField] private GameObject loadingScreenPrefab;
        [SerializeField] Button universalBackButton;
        [SerializeField] private Trackers trackers;

        [Header("Reward Card Objects")]
        [SerializeField] private GameObject BBRewardPrefab;
        [SerializeField] private GameObject CTRewardPrefab;
        [SerializeField] private GameObject LARewardPrefab;
        [SerializeField] private GameObject QBRewardPrefab;
        [SerializeField] private GameObject QURewardPrefab;

        public readonly string rewardsPath = "_Wrapper/Rewards/RewardAssets/";
        public RewardAsset[] rewardAssets;
        public Dictionary<CardType, Color> colorDict;
        public Dictionary<Game, GameObject> prefabDict;

        [Header("Badge Objects")]
        public readonly string badgePath = "_Wrapper/Incentives/Badges";
        public BadgeAsset[] badgeAssets;
        [SerializeField] private GameObject badgePrefab;

        [SerializeField, Tooltip("For the first card received in each minigame, if none keep blank")] GameCardDialogPair[] rewardDialogIDs;
        [SerializeField] MinigameTitles minigameTitles;
        Game currentGame = Game.None;

        [System.Serializable]
        struct GameCardDialogPair
        {
            public Game game;
            public string cardDialog;
        }

        #region Unity Functions

        private void Awake()
        {
            InitSingleton();
            InitColorDict();
            InitPrefabDict();
            InitRewardAssetArray();
            InitBadgeAssetArray();
            //Routine.Start(IntroDialogueRoutine()); //todo: also wait for loadingScreenGO to be null?      -> moved to its own method to call after title screen
            if (debugScreen.DebugEnabled)
            {
                debugButton.gameObject.SetActive(true);
                debugButton.onClick.AddListener(() => debugScreen.gameObject.SetActive(!(debugScreen.gameObject.activeInHierarchy)));
            }
            else debugButton.gameObject.SetActive(false);
            Input.multiTouchEnabled = false;
        }

        private void Start()
        {
            BackToMain();
        }

        private void OnEnable()
        {
            Events.OpenMinigame += OpenMinigame;
            Events.CreatRewardCard += CreateCard;
            if (debugScreen.DebugEnabled) Events.ShowCardPopup += ShowCardPopup; // Debug
            Events.ToggleLoadingScreen += ToggleLoadingScreen;
            Events.CollectAndDisplayReward += CollectAndDisplayReward;
            Events.CollectAndDisplayBadge += CollectAndDisplayBadge;
            Events.UnlockAndDisplayGame += UnlockAndDisplayGame;
            Events.DisplayAgeSelector += DisplayAgeSelector;
            Events.DisplayCoinsCollected += DisplayCoinsCollected;
            Events.ToggleBackButton += ToggleBackButton;
            Events.Logout += Logout;
            Events.PlayIntroDialog += PlayIntroDialog;
            Events.MinigameClosed += BackToMain;
            Events.GetMinigameTitle += GetGameTitle;
            Events.GetCurrentGame += GetCurrentGame;
            Events.IsDebugEnabled += () => debugScreen.DebugEnabled;
            Events.Delay += DelayEvent;
        }

        private void OnDisable()
        {
            Events.OpenMinigame -= OpenMinigame;
            Events.CreatRewardCard -= CreateCard;
            if (debugScreen.DebugEnabled) Events.ShowCardPopup -= ShowCardPopup; // Debug
            Events.ToggleLoadingScreen -= ToggleLoadingScreen;
            Events.CollectAndDisplayReward -= CollectAndDisplayReward;
            Events.CollectAndDisplayBadge -= CollectAndDisplayBadge;
            Events.UnlockAndDisplayGame -= UnlockAndDisplayGame;
            Events.DisplayAgeSelector -= DisplayAgeSelector;
            Events.DisplayCoinsCollected -= DisplayCoinsCollected;
            Events.ToggleBackButton -= ToggleBackButton;
            Events.Logout -= Logout;
            Events.PlayIntroDialog -= PlayIntroDialog;
            Events.MinigameClosed -= BackToMain;
            Events.GetMinigameTitle -= GetGameTitle;
            Events.GetCurrentGame -= GetCurrentGame;
            Events.IsDebugEnabled -= () => debugScreen.DebugEnabled;
            Events.Delay -= DelayEvent;
        }

        #endregion

        private void OpenMinigame(Minigame minigame)
        {
            SceneManager.LoadScene(minigame.StartScene);
            currentGame = minigame.gameValue;
            trackers.ToggleTrackers(false);
        }

        void BackToMain()
        {
            if (!saveManager.isUserLoggedIn) Events.OpenLoginScreen?.Invoke();
            else
            {
                Events.CloseLoginScreen?.Invoke();
                Events.ToggleTitleScreen?.Invoke(false);
                
                Events.InitializeStarTracker?.Invoke();
                trackers.ToggleTrackers(true);
            }
            Events.PlayMusic?.Invoke("W_Music");
            currentGame = Game.None;
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
                Debug.LogFormat("No Reward found for {0} level {1}", game.ToString(), level);
                return;
            }

            bool rewardAdded = Events.AddReward?.Invoke(levelReward.rewardID) ?? false;

            if (rewardAdded)
            {
                Routine.Start(cardPopup.DisplayCard(CreateCard(levelReward.rewardID, cardPopup.GetContainerMount(), DisplayType.CardPopup)));
                // Routine.Start(cardPopup.DisplayCard(CreateCard(levelReward.rewardID, cardPopup.GetContainerMount(), DisplayType.Featured)));

                // if this is the first reward from this game, display the reward dialog 
                if (Events.GetFirstRewardBool(levelReward.rewardID.Substring(0, 2).ToLower()))
                    Events.StartDialogueSequence?.Invoke(rewardDialogIDs[(int)game].cardDialog);
            }
            ////todo: call a function that creates the card and displays it in the reward card panel
        }

        private void CollectAndDisplayBadge(Game game, int level, int stars)
        {
            BadgeAsset[] gameBadges = Array.FindAll(badgeAssets, (badge) => badge.game == game);

            BadgeAsset badgeAwarded = null; int num = -1;
            foreach (BadgeAsset bAsset in gameBadges)
            {
                if (bAsset.criteriaType == CriteriaType.Level) {
                    if (Array.Exists(bAsset.criteria, temp => temp == level)) {
                        badgeAwarded = bAsset;
                        num = level;
                        break;
                    }
                } else if (bAsset.criteriaType == CriteriaType.Star) {
                    for (int i=0; i<3; i++) {
                        if (Array.Exists(bAsset.criteria, temp => temp == stars-i)) {
                            badgeAwarded = bAsset;
                            num = stars - i;
                            break;
                        }
                    } 
                } else if (bAsset.criteriaType == CriteriaType.Card) {
                    int cards = Events.HasRewardsFromGame.Invoke(game);
                    if (Array.Exists(bAsset.criteria, temp => temp == cards)) {
                        badgeAwarded = bAsset;
                        num = cards;
                        break;                        
                    }
                }
                // TO DO: Add other badge criterias
            }
            if (badgeAwarded == null || num == -1) return;

            if (Events.AddBadge.Invoke($"{badgeAwarded.name}_{num}"))
                Routine.Start(badgePopup.DisplayBadge(CreateBadge(badgeAwarded, badgePopup.GetContainerMount())));

        }

        private void UnlockAndDisplayGame(Game game) {
            Routine.Start(gamePopup.DisplayGame(game));
        }

        private void DisplayAgeSelector() {
            Routine.Start(agePopup.DisplayAgePopup());
        }

        private void DisplayCoinsCollected(int coins) {
            Routine.Start(coinPopup.DisplayCoins(coins));
        }

        private GameObject CreateCard(string rewardID, GameObject mount, DisplayType displayType)
        {
            RewardAsset rAsset = Resources.Load<RewardAsset>(Path.Combine(rewardsPath, rewardID));
            if (rAsset == null)
            {
                Debug.LogErrorFormat("Could not find card {0} to feature", rewardID);
                return null;
            }

            ClearChildren(mount);
            GameObject rewardGO = Instantiate(prefabDict[rAsset.game], mount.transform);
            rewardGO.GetComponent<Reward>().SetContent(rAsset, colorDict[rAsset.cardType], displayType);

            return rewardGO;
        }

        private GameObject CreateCard(RewardAsset rAsset, GameObject mount, DisplayType displayType)
        {
            GameObject rewardGO = Instantiate(prefabDict[rAsset.game], mount.transform);
            rewardGO.GetComponent<Reward>().SetContent(rAsset, colorDict[rAsset.cardType], displayType);

            return rewardGO;
        }

        private GameObject CreateBadge(BadgeAsset bAsset, GameObject mount)
        {
            GameObject badgeGO = Instantiate(badgePrefab, mount.transform);
            badgeGO.name = bAsset.name;
            badgeGO.GetComponent<Badge>().InitBadge(bAsset);

            return badgeGO;
        }

        void ToggleBackButton(bool show)
        {
            universalBackButton.gameObject.SetActive(show);
        }

        void Logout()
        {
            saveManager.Logout();
            BackToMain();
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

        private void InitRewardAssetArray()
        {
            rewardAssets = Resources.LoadAll<RewardAsset>(rewardsPath);
        }

        private void InitBadgeAssetArray()
        {
            badgeAssets = Resources.LoadAll<BadgeAsset>(badgePath);
        }

        private void InitColorDict()
        {
            ColorUtility.TryParseHtmlString("#89d7ff", out Color visualColor);
            ColorUtility.TryParseHtmlString("#ffe698", out Color charColor);
            ColorUtility.TryParseHtmlString("#ff8062", out Color conceptColor);
            ColorUtility.TryParseHtmlString("#97fb9b", out Color compPartColor);
            ColorUtility.TryParseHtmlString("#c382ff", out Color hintColor);

            colorDict = new Dictionary<CardType, Color>
            {
                { CardType.Visual, visualColor },
                { CardType.Character, charColor },
                { CardType.Concept, conceptColor },
                { CardType.Computer_Part, compPartColor },
                { CardType.Hint, hintColor }
            };
        }

        private void InitPrefabDict()
        {
            prefabDict = new Dictionary<Game, GameObject>
            {
                { Game.BlackBox,  BBRewardPrefab },
                { Game.Circuits,  CTRewardPrefab },
                { Game.Labyrinth, LARewardPrefab },
                { Game.QueueBits, QBRewardPrefab },
                { Game.Qupcakes,  QURewardPrefab }
            };
        }

        void PlayIntroDialog()
        {
            Routine.Start(IntroDialogueRoutine());
        }

        private IEnumerator IntroDialogueRoutine()
        {
            while (!(saveManager.isUserLoggedIn) || !(saveManager.currentUserSave != null))
                yield return null;

            if (saveManager.HasPlayerSeenIntroDialogue())
                yield break;

            Events.StartDialogueSequence?.Invoke(introSequenceID);
            saveManager.ToggleIntroDialogueSeen(true);
            Events.SetNewPlayerStatus?.Invoke(false);
        }

        // todo: debug, delete later
        private IEnumerator DestroyLoadingScreen()
        {
            yield return new WaitForSeconds(loadingToggleDelay);
            Destroy(loadingScreenGO);
            loadingScreenGO = null;
        }

        private void ShowCardPopup(string rewardID)
        {
            RewardAsset rAsset = Resources.Load<RewardAsset>(Path.Combine(rewardsPath, rewardID));
            if (rAsset == null)
            {
                Debug.LogFormat("Could not find card {0} to display", rewardID);
                return;
            }

            Routine.Start(cardPopup.DisplayCard(CreateCard(rewardID, cardPopup.GetContainerMount(), DisplayType.CardPopup)));
            // Routine.Start(cardPopup.DisplayCard(CreateCard(rewardID, cardPopup.GetContainerMount(), DisplayType.Featured)));
        }

        //todo: Utils class?
        private void ClearChildren(GameObject mount)
        {
            foreach (Transform child in mount.transform)
                Destroy(child.gameObject);
        }

        string GetGameTitle(Game game)
        {
            return minigameTitles.Entries[(int)game];
        }

        Game GetCurrentGame()
        {
            return currentGame;
        }

        // Used to invoke a few second delay in non-IEnumerator functions
        void DelayEvent(float time)
        {
            Routine.Start(Delay(time));
        }
        IEnumerator Delay(float time) 
        {
            yield return time;
        }

        #endregion
    }
}