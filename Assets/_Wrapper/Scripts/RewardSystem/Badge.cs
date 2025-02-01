using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wrapper
{
    public class Badge : MonoBehaviour
    {
        [SerializeField] public Animator badgeAnimator;
        [SerializeField] public Game game;
        [SerializeField] public int starLevel;
        [SerializeField] public CriteriaType type;
        [SerializeField] public int[] criteria;
        
        [SerializeField] public Image icon;
        [SerializeField] public TextMeshProUGUI titleText;
        [SerializeField] public TextMeshProUGUI descriptionText;

        private string iconPrefix = "_Wrapper/Incentives/BadgeIcons";
        private int starStatus = 0;
        private bool UNLOCKED = false;

        public void InitBadge(BadgeAsset bAsset) 
        {
            game = bAsset.game;
            starLevel = bAsset.starLevels;
            type = bAsset.criteriaType;
            criteria = bAsset.criteria;

            titleText.text = bAsset.title;
            descriptionText.text = bAsset.description;
            icon.sprite = Resources.Load<Sprite>($"{iconPrefix}/{bAsset.iconName}");

            // badgeAnimator.SetInteger("Game", (int)bAsset.game);
            badgeAnimator.SetInteger("Game", 5);
            badgeAnimator.SetInteger("Star Level", bAsset.starLevels);

            starStatus = 0;
            for (int i=0; i<starLevel; i++) 
            {
                if (CheckCriteria(game, type, criteria[i])) {
                    starStatus++;
                }
            }
            badgeAnimator.SetInteger("Star Status", starStatus);
        }


        public bool CheckCriteria(Game game, CriteriaType criteriaType, int criteria)
        {
            return false;
        }
    
    }
}