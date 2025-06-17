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
        [SerializeField] public Image icons;
        [SerializeField] private Image badgeGraphic;
        [SerializeField] private Image panelBackground;
        [SerializeField] public TextMeshProUGUI titleText;
        [SerializeField] public TextMeshProUGUI descriptionText;
        [SerializeField] public Star[] stars;

        private string iconPrefix = "_Wrapper/Incentives/BadgeIcons";
        
        private string descriptionTemp;
        
        // private bool UNLOCKED = false;
        // private bool MINI = true;

        // BB, CT, LA, QB, QC, RW, None
        // private string[] gameHexOrig = {"#BF90F1", "#71B0A6", "#D38B97", "#72D0DC", "#F1A7C7", "#000000", "#000000"};
        // private string[] gameHexText = {"#8574B3", "#4A6E76", "#853D5A", "#417284", "#7B5677", "#000000", "#000000"};
        private string[] gameHexPanel = {"#8574B3", "#71B0A6", "#C75675", "#72D0DC", "#A87FA9", "#000000", "#000000"};
        [SerializeField] private Sprite[] gameBadges;


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
            // descriptionText.text = bAsset.description;
            descriptionTemp = bAsset.description;
            icons.sprite = Resources.Load<Sprite>($"{iconPrefix}/{bAsset.iconName}");

            // Reload saves before checking criteria
            Events.LoadMinigameSave?.Invoke(game);

            // Determine star level based on criteria and set Star GameObjects
            starStatus = 0;
            for (int i=0; i<starLevel; i++) 
            {
                if (CheckCriteria(game, type, criteria[i])) starStatus++;
            }
            for (int i=0; i<5; i++) {
                stars[i].SetStar( (i <= starLevel - 1), (i <= starStatus - 1) );
            }

            // Change description based on starStatus and display
            if (starStatus == 0) {
                // descriptionText.text = "Play more to unlock reward...";
                titleText.text = "???";
                this.gameObject.GetComponent<Button>().interactable = false;
            } else {
                if (descriptionTemp.Contains("[temp]")) {
                    descriptionText.text = descriptionTemp.Replace("[temp]", criteriaDescription[starStatus-1]);
                } else {
                    descriptionText.text = descriptionTemp;
                }
            }

            // Set badge graphic and text colors based on game
            badgeGraphic.sprite = gameBadges[(int)game];
            ColorUtility.TryParseHtmlString(gameHexPanel[(int)game], out Color tempColor);
            panelBackground.color = tempColor;
            // titleText.color = tempColor;
            // descriptionText.color = tempColor;
        }

        public void OnEnable()
        {
            // SetAnimator();
            badgeAnimator.SetInteger("Game", (int)game);
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
    }
}