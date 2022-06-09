using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class RewardJournal : MonoBehaviour
    {
        private const string rewardsPath = "_Wrapper/Rewards";
        private Dictionary<Game, JournalSection> journal;

        [SerializeField] private GameObject BBRewardPrefab;
        [SerializeField] private GameObject CTRewardPrefab;
        [SerializeField] private GameObject LARewardPrefab;
        [SerializeField] private GameObject QBRewardPrefab;
        [SerializeField] private GameObject QURewardPrefab;

        void Start()
        {
            InitJournalDict();
            PopulateJournal();
        }

        private void PopulateJournal()
        {
            RewardAsset[] rewardAssetArray = Resources.LoadAll<RewardAsset>(rewardsPath);
            foreach (RewardAsset rAsset in rewardAssetArray)
                journal[rAsset.game].AddReward(rAsset);
        }

        private void InitJournalDict()
        {
            journal = new Dictionary<Game, JournalSection>
            {
                { Game.BlackBox, new JournalSection(BBRewardPrefab) },
                { Game.Circuits, new JournalSection(CTRewardPrefab) },
                { Game.Labyrinth, new JournalSection(LARewardPrefab) },
                { Game.QueueBits, new JournalSection(QBRewardPrefab) },
                { Game.Qupcakes, new JournalSection(QURewardPrefab) }
            };
        }
    }
}
