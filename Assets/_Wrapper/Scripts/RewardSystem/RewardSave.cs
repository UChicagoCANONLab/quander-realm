using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class RewardSave : MonoBehaviour
    {
        public static RewardSave Instance;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public int SaveRewardResearchData() {
            // Save research data
            RewardResearchData.Instance.RewardRDtoString();
            Debug.Log(RewardResearchData.Instance.rewardResearchData);
            Wrapper.Events.SaveMinigameResearchData?.Invoke(Wrapper.Game.Rewards, RewardResearchData.Instance.rewardResearchData);
            return 0;
        }
        
    }
}