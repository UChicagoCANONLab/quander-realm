using System;
using System.Collections;
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
        [SerializeField] private GameObject loginScreen;
        [SerializeField] private GameObject debugScreen;
        [SerializeField] private Button debugButton;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private CardPopup cardPopup;
        [SerializeField] private GameObject loadingScreenPrefab;

        [Header("Reward Card Prefabs")]
        [SerializeField] private GameObject BBRewardPrefab;
        [SerializeField] private GameObject CTRewardPrefab;
        [SerializeField] private GameObject LARewardPrefab;
        [SerializeField] private GameObject QBRewardPrefab;
        [SerializeField] private GameObject QURewardPrefab;

        public readonly string rewardsPath = "_Wrapper/Rewards/RewardAssets/";
        public RewardAsset[] rewardAssets;
        public Dictionary<CardType, Color> colorDict;
        public Dictionary<Game, GameObject> prefabDict;

        #region Unity Functions

        private void Awake()
        {
            InitSingleton();
            InitColorDict();
            InitPrefabDict();
            InitRewardAssetArray();
            Routine.Start(IntroDialogueRoutine()); //todo: also wait for loadingScreenGO to be null?
            debugButton.onClick.AddListener(() => debugScreen.SetActive(!(debugScreen.activeInHierarchy))); //todo: debug, delete later
            Input.multiTouchEnabled = false;
        }

        private void Start()
        {
            if (!(saveManager.isUserLoggedIn))
            {
                loginScreen.SetActive(true);
                Events.OpenLoginScreen?.Invoke();
            }

            Events.PlayMusic?.Invoke("WrapperTheme");
        }

        private void OnEnable()
        {
            Events.OpenMinigame += OpenMinigame;
            Events.CreatRewardCard += CreateCard;
            Events.ShowCardPopup += ShowCardPopup; // Debug
            Events.ToggleLoadingScreen += ToggleLoadingScreen;
            Events.CollectAndDisplayReward += CollectAndDisplayReward;
        }

        private void OnDisable()
        {
            Events.OpenMinigame -= OpenMinigame;
            Events.CreatRewardCard -= CreateCard;
            Events.ShowCardPopup -= ShowCardPopup; // Debug
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

            bool rewardAdded = Events.AddReward?.Invoke(levelReward.rewardID) ?? false;

            if (rewardAdded)
                Routine.Start(cardPopup.DisplayCard(CreateCard(levelReward.rewardID, cardPopup.GetContainerMount(), DisplayType.CardPopup)));

            ////todo: call a function that creates the card and displays it in the reward card panel
            //Debug.LogFormat("Won Reward {0} in game {1} at level {2}", levelReward.rewardID, game, level);
            //    Routine.Start(DestroyLoadingScreen()); //todo: debug, delete later
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

        private void InitColorDict()
        {
            ColorUtility.TryParseHtmlString("#89d7ff", out Color visualColor);
            ColorUtility.TryParseHtmlString("#ffe698", out Color charColor);
            ColorUtility.TryParseHtmlString("#ff8062", out Color conceptColor);
            ColorUtility.TryParseHtmlString("#97fb9b", out Color compPartColor);

            colorDict = new Dictionary<CardType, Color>
            {
                { CardType.Visual, visualColor },
                { CardType.Character, charColor },
                { CardType.Concept, conceptColor },
                { CardType.Computer_Part, compPartColor }
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

        private IEnumerator IntroDialogueRoutine()
        {
            while (!(saveManager.isUserLoggedIn) || !(saveManager.currentUserSave != null))
                yield return null;

            if (saveManager.HasPlayerSeenIntroDialogue())
                yield break;

            Events.StartDialogueSequence?.Invoke(introSequenceID);
            saveManager.ToggleIntroDialogueSeen(true);
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
        }

        //todo: Utils class?
        private void ClearChildren(GameObject mount)
        {
            foreach (Transform child in mount.transform)
                Destroy(child.gameObject);
        }

        #endregion
    }
}