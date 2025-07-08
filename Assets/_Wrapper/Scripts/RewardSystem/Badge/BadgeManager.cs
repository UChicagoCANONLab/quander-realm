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

        [SerializeField] private GameObject mainBadgeHolder;
        // [SerializeField] private BadgeHolder[] badgeHolders;
        [SerializeField] private TextMeshProUGUI starCount;

        [SerializeField] public GameObject badgePrefab;
        [SerializeField] public List<Badge> badges; 

        private string prefix = "_Wrapper/Incentives/Badges";

        private string[] badgeIDs = {
            "Leveler_QU", "Leveler_QB", "Leveler_BB", "Leveler_CT", "Leveler_LA", 
            // "Reward_all", "Reward_first", "Unlocked",
            "Reward_QU", "Reward_QB", "Reward_BB", "Reward_CT", "Reward_LA", 
            "Stars_QU", "Stars_QB", "Stars_BB", "Stars_CT", "Stars_LA"
        };
        
        private string[] badgeIDs2 = {
            "Leveler_QU", "Reward_QU", "Stars_QU",
            "Leveler_QB", "Reward_QB", "Stars_QB",
            "Leveler_BB", "Reward_BB", "Stars_BB",
            "Leveler_CT", "Reward_CT", "Stars_CT",
            "Leveler_LA", "Reward_LA", "Stars_LA"
            // "Reward_all", "Reward_first", "Unlocked"
        };

        void Start()
        {
            LoadBadges();
        }

        private void LoadBadges()
        {
            /* foreach (string ID in badgeIDs2)
            {
                BadgeAsset bAsset = Resources.Load<BadgeAsset>($"{prefix}/{ID}");
                badgeHolders[(int)bAsset.criteriaType].AddBadge(bAsset, ID);
            } */

            foreach (string ID in badgeIDs2)
            {
                BadgeAsset bAsset = Resources.Load<BadgeAsset>($"{prefix}/{ID}");
                // badgeHolders[(int)bAsset.criteriaType].AddBadge(bAsset, ID);

                GameObject bObject = Instantiate(badgePrefab, mainBadgeHolder.transform);
                bObject.name = ID;
                bObject.GetComponent<Badge>().InitBadge(bAsset);

                badges.Add(bObject.GetComponent<Badge>());
            }
        }


        public void DelayGetStars()
        {
            Invoke("GetBonusStars", 0.5f);
        }
        public void GetBonusStars()
        {
            int stars = 0;
            /* foreach(BadgeHolder holder in badgeHolders)
            {
                foreach(Badge badge in holder.badges)
                {
                    stars += badge.starStatus;
                }
            } */
            foreach(Badge b in badges)
            {
                stars += b.starStatus;
            }
            starCount.text = $"{stars}";
            Events.UpdateMinigameStarCount(Game.Rewards, stars);
        }


    }
}