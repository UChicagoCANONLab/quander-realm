using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class RewardJournal : MonoBehaviour
    {
        #region Variables

        [Header("Card Mounts")]
        [SerializeField] private GameObject visibleRewardsMount;
        [SerializeField] private GameObject hiddenRewardsMount;

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

        private Dictionary<CardType, Color> colorDict;
        private Dictionary<Game, GameObject> prefabDict;
        private Dictionary<Game, JournalSection> journal;

        private const string rewardsPath = "_Wrapper/Rewards";
        private Reward currentReward;

        #endregion

        void Awake()
        {
            InitColorDict();
            InitPrefabDict();
            InitJournal();
            PopulateJournal();
            OpenFirstPage();
        }

        private void OnEnable()
        {
            Events.OpenJournalPage += SwitchPage;
        }

        private void OnDisable()
        {
            Events.OpenJournalPage -= SwitchPage;
        }

        /// <summary>
        /// Move visible cards to the hidden cards mount, bring the requested page's cards onto the visible cards mount
        /// </summary>
        private void SwitchPage(JournalPage journalPage)
        {
            foreach (Transform rewardTransform in GetVisibleCards())
                rewardTransform.SetParent(hiddenRewardsMount.transform);

            foreach (GameObject rewardGO in journalPage.cardList)
                rewardGO.transform.SetParent(visibleRewardsMount.transform);

            //todo: find first available reward rather than first
            //visibleRewardsMount.transform.GetChild(0).GetComponent<Reward>().ToggleSelected(true);
        }

        private List<Transform> GetVisibleCards()
        {
            List<Transform> visibleCards = new List<Transform>();

            foreach (Transform transform in visibleRewardsMount.transform)
                visibleCards.Add(transform);

            return visibleCards;
        }

        private void OpenFirstPage()
        {
            JournalSection firstSection = journal.First().Value;
            SwitchPage(firstSection.GetFirstPage());
            firstSection.ToggleTab(true);
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
            journal = new Dictionary<Game, JournalSection>
            {
                { Game.BlackBox,  new JournalSection(BBTab) },
                { Game.Circuits,  new JournalSection(CTTab) },
                { Game.Labyrinth, new JournalSection(LATab) },
                { Game.QueueBits, new JournalSection(QBTab) },
                { Game.Qupcakes,  new JournalSection(QUTab) }
            };
        }

        private void PopulateJournal()
        {
            RewardAsset[] rewardAssetArray = Resources.LoadAll<RewardAsset>(rewardsPath);

            foreach (RewardAsset rAsset in rewardAssetArray)
            {
                GameObject rewardGO = Instantiate(prefabDict[rAsset.game], hiddenRewardsMount.transform);
                rewardGO.GetComponent<Reward>().SetContent(rAsset, colorDict[rAsset.cardType]);

                journal[rAsset.game].AddCard(rewardGO);
            }
        }

        #endregion
    }
}
