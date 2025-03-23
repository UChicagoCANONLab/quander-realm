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
        [SerializeField] public int starStatus = 0;
        [SerializeField] public CriteriaType type;
        [SerializeField] public int[] criteria;
        [SerializeField] public string[] criteriaDescription;
        
        [Header("Badge Display GameObjects")]
        [SerializeField] public Image[] icons; // two icons; one is a shadow
        [SerializeField] public TextMeshProUGUI titleText;
        [SerializeField] public TextMeshProUGUI miniTitleText;
        [SerializeField] public TextMeshProUGUI descriptionText;
        [SerializeField] public Star[] stars;

        private string iconPrefix = "_Wrapper/Incentives/BadgeIcons";
        
        private string descriptionTemp;
        
        private bool UNLOCKED = false;
        private bool MINI = true;


        public void InitBadge(BadgeAsset bAsset) 
        {
            // Set values from BadgeAsset
            game = bAsset.game;
            starLevel = bAsset.starLevels;
            type = bAsset.criteriaType;
            criteria = bAsset.criteria;
            criteriaDescription = bAsset.criteriaDescription;

            // Set texts and icons
            titleText.text = bAsset.title;
            miniTitleText.text = bAsset.title;
            // descriptionText.text = bAsset.description;
            descriptionTemp = bAsset.description;
            icons[0].sprite = Resources.Load<Sprite>($"{iconPrefix}/{bAsset.iconName}");
            icons[1].sprite = Resources.Load<Sprite>($"{iconPrefix}/{bAsset.iconName}");

            // SetAnimator();
            // this.gameObject.onClick.AddListener(ToggleMini);

            Events.LoadMinigameSave?.Invoke(game);

            // Determine star level and set Star GameObjects
            starStatus = 0;
            for (int i=0; i<starLevel; i++) 
            {
                if (CheckCriteria(game, type, criteria[i])) {
                    starStatus++;
                }
            }
            for (int i=0; i<5; i++) {
                stars[i].SetStar( (i <= starLevel - 1), (i <= starStatus - 1) );
            }

            // Set description based on starStatus
            if (starStatus == 0) {
                descriptionText.text = "Play more to unlock reward...";
                // set badge to locked
            } else {
                if (descriptionTemp.Contains("[temp]")) {
                    descriptionText.text = descriptionTemp.Replace("[temp]", criteriaDescription[starStatus-1]);
                } else {
                    descriptionText.text = descriptionTemp;
                }
            }
        }

        public void OnEnable()
        {
            // SetAnimator();
            badgeAnimator.SetInteger("Game", (int)game);
        }

        /* public void SetAnimator()
        {
            Events.LoadMinigameSave?.Invoke(game);

            badgeAnimator.SetInteger("Game", (int)game);
            // badgeAnimator.SetInteger("Star Level", starLevel);

            starStatus = 0;
            for (int i=0; i<starLevel; i++) 
            {
                if (CheckCriteria(game, type, criteria[i])) {
                    starStatus++;
                }
            }
            // badgeAnimator.SetInteger("Star Status", starStatus);

            for (int i=0; i<5; i++) {
                stars[i].SetStar( (i <= starLevel - 1), (i <= starStatus - 1) );
            }

            if (starStatus == 0) {
                descriptionText.text = "Play more to unlock reward...";
                // set badge to locked
            } else {
                if (descriptionTemp.Contains("[temp]")) {
                    descriptionText.text = descriptionTemp.Replace("[temp]", criteriaDescription[starStatus-1]);
                } else {
                    descriptionText.text = descriptionTemp;
                }
            }
        } */


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