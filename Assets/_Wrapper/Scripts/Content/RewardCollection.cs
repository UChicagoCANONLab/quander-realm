using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wrapper
{
    public class RewardCollection : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject visibleRewardsMount;
        [SerializeField] private GameObject hiddenRewardsMount;

        [Header("Card Prefabs")]
        [SerializeField] private GameObject BBRewardPrefab;
        [SerializeField] private GameObject CTRewardPrefab;
        [SerializeField] private GameObject LARewardPrefab;
        [SerializeField] private GameObject QBRewardPrefab;
        [SerializeField] private GameObject QURewardPrefab;

        private const string rewardsPath = "_Wrapper/Rewards";

        private Dictionary<CardType, Color> colorDict;
        private Dictionary<Game, GameObject> prefabDict;
        private Dictionary<Game, List<JournalPage>> journal;

        #endregion

        void Awake()
        {
            InitColorDict();
            InitPrefabDict();
            InitJournalDict();

            PopulateJournal();
            SwitchPage(journal.First().Value.First());
        }

        private void SwitchPage(JournalPage journalPage)
        {
            // Move visible cards to the background 
            foreach(GameObject rewardGO in visibleRewardsMount.transform)
                rewardGO.transform.SetParent(hiddenRewardsMount.transform);

            // Bring current page's cards into the foreground
            foreach(GameObject rewardGO in journalPage.cardList)
                rewardGO.transform.SetParent(visibleRewardsMount.transform);
        }

        private void PopulateJournal()
        {
            RewardAsset[] rewardAssetArray = Resources.LoadAll<RewardAsset>(rewardsPath);

            foreach (RewardAsset rAsset in rewardAssetArray)
            {
                GameObject rewardGO = Instantiate(prefabDict[rAsset.game], hiddenRewardsMount.transform);
                Color typeColor = colorDict[rAsset.cardType];

                rewardGO.GetComponent<Reward>().SetContent(rAsset, typeColor);
                AddCard(rAsset.game, rewardGO);
            }
        }

        private void AddCard(Game game, GameObject rewardGO)
        {
            AddPageIfNeeded(game);
            journal[game].Last().Add(rewardGO);
        }

        private void AddPageIfNeeded(Game game)
        {
            if (journal[game].Count == 0 || journal[game].Last().IsFull())
                journal[game].Add(new JournalPage());
        }

        #region Initialize Dictionaries

        private void InitColorDict()
        {
            ColorUtility.TryParseHtmlString("89d7ff", out Color visualColor);
            ColorUtility.TryParseHtmlString("ffe698", out Color charColor);
            ColorUtility.TryParseHtmlString("ff8062", out Color conceptColor);
            ColorUtility.TryParseHtmlString("97fb9b", out Color compPartColor);

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

        private void InitJournalDict()
        {
            journal = new Dictionary<Game, List<JournalPage>>
            {
                { Game.BlackBox,  new List<JournalPage>() },
                { Game.Circuits,  new List<JournalPage>() },
                { Game.Labyrinth, new List<JournalPage>() },
                { Game.QueueBits, new List<JournalPage>() },
                { Game.Qupcakes,  new List<JournalPage>() }
            };
        }

        #endregion
    }
}
