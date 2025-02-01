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
        [SerializeField] private BadgeAsset currBadge;
        [SerializeField] private Animator animator;
        
        [SerializeField] private GameObject badgeHolder;
        [SerializeField] private GameObject badgePrefab;

        private string prefix = "_Wrapper/Incentives/Badges";

        private string[] badgeIDs = {
            "Leveler_QU", "Leveler_QB", "Leveler_BB", "Leveler_CT", "Leveler_LA", 
            "Reward_all", "Reward_first", "Unlocked",
            "Reward_QU", "Reward_QB", "Reward_BB", "Reward_CT", "Reward_LA", 
            "Stars_QU", "Stars_QB", "Stars_BB", "Stars_CT", "Stars_LA"
        };

        void Awake()
        {
            LoadBadges();
        }

        private void LoadBadges()
        {
            foreach (string ID in badgeIDs)
            {
                BadgeAsset bAsset = Resources.Load<BadgeAsset>($"{prefix}/{ID}");
                GameObject bObject = Instantiate(badgePrefab, badgeHolder.transform);
                bObject.name = ID;
                bObject.GetComponent<Badge>().InitBadge(bAsset);
            }
        }


    }
}