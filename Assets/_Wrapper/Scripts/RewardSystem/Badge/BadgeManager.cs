using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BeauRoutine;

namespace Wrapper
{
    public class BadgeManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] private BadgeHolder[] badgeHolders;
        [SerializeField] private TextMeshProUGUI starCount;

        private string prefix = "_Wrapper/Incentives/Badges";

        private string[] badgeIDs = {
            "Leveler_QU", "Leveler_QB", "Leveler_BB", "Leveler_CT", "Leveler_LA", 
            "Reward_all", "Reward_first", "Unlocked",
            "Reward_QU", "Reward_QB", "Reward_BB", "Reward_CT", "Reward_LA", 
            "Stars_QU", "Stars_QB", "Stars_BB", "Stars_CT", "Stars_LA"
        };
        
        private string[] badgeIDs2 = {
            "Leveler_QU", "Reward_QU", "Stars_QU",
            "Leveler_QB", "Reward_QB", "Stars_QB",
            "Leveler_BB", "Reward_BB", "Stars_BB",
            "Leveler_CT", "Reward_CT", "Stars_CT",
            "Leveler_LA", "Reward_LA", "Stars_LA",
            "Reward_all", "Reward_first", "Unlocked"
        };

        void Start()
        {
            LoadBadges();
        }

        private void LoadBadges()
        {
            foreach (string ID in badgeIDs2)
            {
                BadgeAsset bAsset = Resources.Load<BadgeAsset>($"{prefix}/{ID}");
                badgeHolders[(int)bAsset.criteriaType].AddBadge(bAsset, ID);
            }
        }


        public void DelayGetStars()
        {
            Invoke("GetBonusStars", 0.5f);
        }
        public void GetBonusStars()
        {
            int stars = 0;
            foreach(BadgeHolder holder in badgeHolders)
            {
                foreach(Badge badge in holder.badges)
                {
                    stars += badge.GetStarStatus();
                    // Debug.Log(badge.GetStarStatus());
                }
            }
            starCount.text = $"{stars}";
            Events.UpdateMinigameStarCount(Game.Rewards, stars);
        }


    }
}