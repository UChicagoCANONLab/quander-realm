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
        [SerializeField] public string[] criteriaDescription;
        
        [SerializeField] public Image[] icons; // two icons; one is a shadow
        [SerializeField] public TextMeshProUGUI titleText;
        [SerializeField] public TextMeshProUGUI miniTitleText;
        [SerializeField] public TextMeshProUGUI descriptionText;

        private string iconPrefix = "_Wrapper/Incentives/BadgeIcons";
        private int starStatus = 0;
        private string descriptionTemp;
        
        private bool UNLOCKED = false;
        private bool MINI = true;


        public void InitBadge(BadgeAsset bAsset) 
        {
            game = bAsset.game;
            starLevel = bAsset.starLevels;
            type = bAsset.criteriaType;
            criteria = bAsset.criteria;
            criteriaDescription = bAsset.criteriaDescription;

            titleText.text = bAsset.title;
            miniTitleText.text = bAsset.title;
            // descriptionText.text = bAsset.description;
            descriptionTemp = bAsset.description;
            icons[0].sprite = Resources.Load<Sprite>($"{iconPrefix}/{bAsset.iconName}");
            icons[1].sprite = Resources.Load<Sprite>($"{iconPrefix}/{bAsset.iconName}");

            // SetAnimator();
            // this.gameObject.onClick.AddListener(ToggleMini);
        }

        public void OnEnable()
        {
            SetAnimator();
        }

        public void SetAnimator()
        {
            Events.LoadMinigameSave?.Invoke(game);

            badgeAnimator.SetInteger("Game", (int)game);
            badgeAnimator.SetInteger("Star Level", starLevel);

            starStatus = 0;
            for (int i=0; i<starLevel; i++) 
            {
                if (CheckCriteria(game, type, criteria[i])) {
                    starStatus++;
                }
            }
            badgeAnimator.SetInteger("Star Status", starStatus);

            if (starStatus == 0) {
                // set badge to locked
            } else {
                if (descriptionTemp.Contains("[temp]")) {
                    descriptionText.text = descriptionTemp.Replace("[temp]", criteriaDescription[starStatus-1]);
                }
            }
        }


        public bool CheckCriteria(Game game, CriteriaType criteriaType, int criteria)
        {
            switch(criteriaType)
            {
                case CriteriaType.Level:
                    if (Events.GetMinigameMaxLevel.Invoke(game) >= criteria) {
                        Events.AddBadge.Invoke($"{this.gameObject.name}_{criteria}");
                        return true;
                    } break;

                case CriteriaType.Star:
                    if (Events.GetMinigameTotalStars.Invoke(game) >= criteria) {
                        Events.AddBadge.Invoke($"{this.gameObject.name}_{criteria}");
                        return true;
                    } break;

                case CriteriaType.Card:
                    if (Events.HasRewardsFromGame.Invoke(game) >= criteria) {
                        Events.AddBadge.Invoke($"{this.gameObject.name}_{criteria}");
                        return true;
                    } break;

                case CriteriaType.Unlocked:
                    if (Events.GetGameUnlocked.Invoke((Game)criteria)) {
                        Events.AddBadge.Invoke($"{this.gameObject.name}_{criteria}");
                        return true;
                    } break;
            }
            return false;
        }

        public void ToggleMini()
        {
            badgeAnimator.SetBool("Mini", !MINI);
            MINI = !MINI;
        }

        public int GetStarStatus()
        {
            return starStatus;
        }
    
    }
}