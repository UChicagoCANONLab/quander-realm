using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BeauRoutine;
using System.IO;

namespace Wrapper
{
    public class RewardJournal : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Animator animator;

        [Header("Card Mounts")]
        [SerializeField] private GameObject visibleRewardsMount;
        [SerializeField] private GameObject hiddenRewardsMount;
        [SerializeField] private GameObject featuredCardMount;

        [Header("Card Prefabs")]
        [SerializeField] private GameObject BBRewardPrefab;
        [SerializeField] private GameObject CTRewardPrefab;
        [SerializeField] private GameObject LARewardPrefab;
        [SerializeField] private GameObject QBRewardPrefab;
        [SerializeField] private GameObject QURewardPrefab;

        [Header("Section Tabs")]
        [SerializeField] private GameObject BBTab;
        [SerializeField] private GameObject CTTab;
        [SerializeField] private GameObject LATab;
        [SerializeField] private GameObject QBTab;
        [SerializeField] private GameObject QUTab;

        [Header("Navigation")]
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Toggle[] navDots;

        private Dictionary<CardType, Color> colorDict;
        private Dictionary<Game, GameObject> prefabDict;
        private Dictionary<Game, JournalSection> journal;

        private const string rewardsPath = "_Wrapper/Rewards";
        private Reward currentReward;
        private JournalPage currentPage;

        #endregion

        void Awake()
        {
            Events.ToggleLoadingScreen?.Invoke();
            Debug.Log("Awake");
            InitColorDict();
            InitPrefabDict();
            InitJournal();
            InitPageNavigation();
        }

        private void Start()
        {
            Debug.Log("Start");
            PopulateJournal();
            OpenFirstPage();
            Events.ToggleLoadingScreen?.Invoke();
        }

        private void OnEnable()
        {
            Events.FeatureCard += FeatureCard;
            Events.OpenJournalPage += SwitchPageVisual;
        }

        private void OnDisable()
        {
            Events.FeatureCard -= FeatureCard;
            Events.OpenJournalPage -= SwitchPageVisual;
        }
        private void FeatureCard(string id)
        {
            animator.SetBool("FeatureCard", false);

            foreach (Transform transform in featuredCardMount.transform)
                Destroy(transform.gameObject);

            RewardAsset rAsset = Resources.Load<RewardAsset>(Path.Combine(rewardsPath, id));
            GameObject rewardGO = CreateCard(rAsset, featuredCardMount);

            currentReward = rewardGO.GetComponent<Reward>();
            animator.SetBool("FeatureCard", true);
        }

        private GameObject CreateCard(RewardAsset rAsset, GameObject mount)
        {
            GameObject rewardGO = Instantiate(prefabDict[rAsset.game], mount.transform);
            rewardGO.GetComponent<Reward>().SetContent(rAsset, colorDict[rAsset.cardType]);

            return rewardGO;
        }

        private void SwitchPage(int pageNumber)
        {
            Debug.Log("SwitchPageNum");
            bool pageFound = false;

            foreach(JournalSection section in journal.Values)
            {
                foreach(JournalPage page in section.pages)
                {
                    if (page.pageNumber == pageNumber)
                    {
                        SwitchPageVisual(page);
                        pageFound = true;
                        break;
                    }
                }

                if (pageFound)
                    break;
            }
        }

        /// <summary>
        /// Uses the navDots' OnValueChangedEvent to trigger a page switch. The function will add the value of "step"
        /// to the current page's index and trigger the navDot at that index.
        /// </summary>
        private void SwitchPage(Step step)
        {
            int newIndex = currentPage.pageNumber + (int)step;
            if (newIndex < 0 || newIndex > navDots.Length - 1)
                return;

            navDots[newIndex].isOn = true;
        }

        /// <summary>
        /// Move visible cards to the hidden cards mount, bring the requested page's cards onto the visible cards mount
        /// </summary>
        private void SwitchPageVisual(JournalPage page)
        {
            Debug.Log("SwitchActual");
            foreach (Transform rewardTransform in GetVisibleCards())
                rewardTransform.SetParent(hiddenRewardsMount.transform);

            foreach (GameObject rewardGO in page.cardList)
            {
                rewardGO.transform.SetParent(visibleRewardsMount.transform);
                Routine.Start(rewardGO.GetComponent<Reward>().UpdateAnimationState());
            }

            //SelectFirstAvailableCard();
            //ManageSectionTabAnim(page);
            currentPage = page;
        }

        private void ManageSectionTabAnim(JournalPage page)
        {
            Game currentGame = currentPage.cardList.First().GetComponent<Reward>().game;
            Game newGame = page.cardList.First().GetComponent<Reward>().game;

            if (currentGame != newGame)
                journal[currentGame].ToggleTabAnim(true);
        }

        private List<Transform> GetVisibleCards()
        {
            Debug.Log("GetVisibleCards");
            List<Transform> visibleCards = new List<Transform>();

            foreach (Transform transform in visibleRewardsMount.transform)
                visibleCards.Add(transform);

            return visibleCards;
        }

        private void SelectFirstAvailableCard()
        {
            Debug.Log("SelectFirstAvail");
            foreach (Transform rewardTransform in GetVisibleCards())
            {
                Reward reward = rewardTransform.GetComponent<Reward>();
                if (reward.IsUnlocked())
                {
                    Routine.Start(reward.SelectCard());
                    break;
                }
            }
        }

        #region Initialize

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
                { CardType.ComputerPart, compPartColor }
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

        private void InitJournal()
        {
            Debug.Log("InitJournal");
            journal = new Dictionary<Game, JournalSection>
            {
                { Game.BlackBox,  new JournalSection(BBTab) },
                { Game.Circuits,  new JournalSection(CTTab) },
                { Game.Labyrinth, new JournalSection(LATab) },
                { Game.QueueBits, new JournalSection(QBTab) },
                { Game.Qupcakes,  new JournalSection(QUTab) }
            };

            Events.ResetPageNumbers?.Invoke();
        }

        private void OpenFirstPage()
        {
            animator.SetBool("On", true);
            currentPage = journal.First().Value.pages.First();
            journal.First().Value.ToggleTabAnim(true);
            SwitchPage(0);
        }

        private void InitPageNavigation()
        {
            Debug.Log("InitPageNav");
            foreach (Toggle dot in navDots)
            {
                dot.onValueChanged.AddListener((isOn) => 
                {
                    if (currentPage.pageNumber == Array.IndexOf(navDots, dot))
                        return;

                    if (isOn)
                        SwitchPage(Array.IndexOf(navDots, dot)); 
                });
            }

            previousButton.onClick.AddListener(() => SwitchPage(Step.Backward));
            nextButton.onClick.AddListener(() => SwitchPage(Step.Forward));
        }

        private void PopulateJournal()
        {
            RewardAsset[] rewardAssetArray = Resources.LoadAll<RewardAsset>(rewardsPath);

            foreach (RewardAsset rAsset in rewardAssetArray)
            {
                GameObject rewardGO = CreateCard(rAsset, hiddenRewardsMount);
                journal[rAsset.game].AddCard(rewardGO);
            }
        }

        #endregion
    }
}
