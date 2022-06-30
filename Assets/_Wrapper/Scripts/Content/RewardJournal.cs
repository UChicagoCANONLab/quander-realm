using BeauRoutine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

        private const string featuredCardParam = "FeaturedCard";
        private GameObject featuredCardGO;
        private string featuredCardID;
        private JournalPage currentPage;

        #endregion

        #region Unity Functions

        void Awake()
        {
            //Events.ToggleLoadingScreen?.Invoke();
            InitColorDict();
            InitPrefabDict();
            InitJournal();
            InitPrevNextButtons();
        }

        private void Start()
        {
            PopulateJournal();
            //Events.ToggleLoadingScreen?.Invoke();
            animator.SetBool("On", true);
            InitFirstPage();
        }

        private void OnEnable()
        {
            Events.GetNavDot += GetNavDot;
            Events.SwitchPage += SwitchCardsOnPage;
            Events.FeatureCard += StartFeaturedCardSwap;
        }

        private void OnDisable()
        {
            Events.GetNavDot -= GetNavDot;
            Events.SwitchPage -= SwitchCardsOnPage;
            Events.FeatureCard -= StartFeaturedCardSwap;
        }

        #endregion

        #region Featured Card

        private void StartFeaturedCardSwap(string id)
        {
            featuredCardID = id;

            if (featuredCardMount.transform.childCount == 0)
            {
                InstantiateFeaturedCard();
                return;
            }

            animator.SetBool(featuredCardParam, false);
        }

        private void InstantiateFeaturedCard()
        {
            foreach (Transform transform in featuredCardMount.transform)
                Destroy(transform.gameObject);

            RewardAsset rAsset = Resources.Load<RewardAsset>(Path.Combine(GameManager.Instance.rewardsPath, featuredCardID));
            if (rAsset == null)
            {
                Debug.LogErrorFormat("Could not find card {0} to feature", featuredCardID);
                return;
            }

            featuredCardGO = CreateCard(rAsset, featuredCardMount, DisplayType.Featured);
            DisplayNewFeaturedCard();
        }

        private void DisplayNewFeaturedCard()
        {
            animator.SetBool(featuredCardParam, true);
            Routine.Start(featuredCardGO.GetComponent<Reward>().UpdateAnimationState());
        }

        #endregion

        #region Page Switching

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

        private void SwitchCardsOnPage(JournalPage newPage)
        {
            Routine.Start(SwitchCardsRoutine(newPage));
        }

        /// <summary>
        /// Move visible cards to the hidden cards mount, bring the requested page's cards onto the visible cards mount
        /// </summary>
        private IEnumerator SwitchCardsRoutine(JournalPage newPage)
        {
            animator.SetTrigger(GetPageFlipTrigger(newPage));
            yield return animator.WaitToCompleteAnimation(3);

            foreach (Transform rewardTransform in GetVisibleCards())
                rewardTransform.SetParent(hiddenRewardsMount.transform);

            foreach (GameObject rewardGO in newPage.cardList)
            {
                rewardGO.transform.SetParent(visibleRewardsMount.transform);
                Routine.Start(rewardGO.GetComponent<Reward>().UpdateAnimationState());
            }

            Events.UpdateTab?.Invoke(newPage);
            animator.SetTrigger(GetPageFlipTrigger(newPage));
            currentPage = newPage;
        }

        private string GetPageFlipTrigger(JournalPage newPage)
        {
            string animTrigger = "Next";
            if (currentPage.pageNumber > newPage.pageNumber)
                animTrigger = "Previous";

            return animTrigger;
        }

        private List<Transform> GetVisibleCards()
        {
            List<Transform> visibleCards = new List<Transform>();

            foreach (Transform transform in visibleRewardsMount.transform)
                visibleCards.Add(transform);

            return visibleCards;
        }

        #endregion

        private GameObject CreateCard(RewardAsset rAsset, GameObject mount, DisplayType displayType)
        {
            GameObject rewardGO = Instantiate(prefabDict[rAsset.game], mount.transform);
            rewardGO.GetComponent<Reward>().SetContent(rAsset, colorDict[rAsset.cardType], displayType);

            return rewardGO;
        }

        private Toggle GetNavDot(int pageNumber)
        {
            return navDots[pageNumber];
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

        private void InitJournal()
        {
            journal = new Dictionary<Game, JournalSection>
            {
                { Game.BlackBox,  new JournalSection(BBTab, true) },
                { Game.Circuits,  new JournalSection(CTTab) },
                { Game.Labyrinth, new JournalSection(LATab) },
                { Game.QueueBits, new JournalSection(QBTab) },
                { Game.Qupcakes,  new JournalSection(QUTab) }
            };

            Events.ResetPageNumbers?.Invoke();
        }

        private void InitFirstPage()
        {
            currentPage = journal.First().Value.pages.First();
            currentPage.ClickNavDot();
        }

        private void InitPrevNextButtons()
        {
            previousButton.onClick.AddListener(() => SwitchPage(Step.Backward));
            nextButton.onClick.AddListener(() => SwitchPage(Step.Forward));
        }

        private void PopulateJournal()
        {
            foreach (RewardAsset rAsset in GameManager.Instance.rewardAssets)
            {
                GameObject rewardGO = CreateCard(rAsset, hiddenRewardsMount, DisplayType.InJournal);
                journal[rAsset.game].AddCard(rewardGO);
            }
        }

        #endregion
    }
}
