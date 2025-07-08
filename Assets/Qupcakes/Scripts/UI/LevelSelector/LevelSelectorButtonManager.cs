using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Costume = Qupcakery.AssetCostumeUtilities;
using TMPro;

/* Manages selection button appearance and action */
namespace Qupcakery
{
    public class LevelSelectorButtonManager : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] stars = new Sprite[4];
        private bool levelAvailable = true; 
        private int level;
        public GameObject lockIcon;


        public void InitButton(int level)
        {
            this.level = level;
            // If player has completed this level
            if (level <= GameManagement.Instance.game.gameStat.MaxLevelCompleted)
            {
                int starCnt = GameManagement.Instance.game.gameStat.GetLevelPerformance(level);
                SetStar(starCnt);
            } else // If player has not completed this level
            {
                if (level != GameManagement.Instance.game.gameStat.MaxLevelCompleted + 1)
                    SetAvailability(false);
            }
           this.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText($"{level}");
        }

        // On-click load target level
        public void LoadLevel()
        {
            if (levelAvailable)
            {
                string iconName = GetComponentInChildren<TextMeshProUGUI>().text;
                // int levelNum = System.Convert.ToInt32(iconName.Split(' ')[1]);
                GameManagement.Instance.SetCurrentLevel(level);
                SceneManagementUtilities.LoadGameScene();
            }                   
        }

        // Set star
        public void SetStar(int starCnt)
        {
            GameObject starIcon = gameObject.transform.Find("RegularIcon").gameObject;
            starIcon.GetComponent<Image>().sprite = stars[starCnt];
        }

        // Set level accessibility
        public void SetAvailability(bool available)
        {
            if (!available)
            {
                levelAvailable = false;
                Image iconImage = gameObject.transform.Find("RegularIcon").gameObject.GetComponent<Image>();
                Color imageColor = iconImage.color;
                iconImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0.4f);

                TextMeshProUGUI text = gameObject.transform.Find("NumberText").gameObject.GetComponent<TextMeshProUGUI>();
                Color32 textColor = text.color;
                text.color = new Color(textColor.r, textColor.g, textColor.b, 0.4f);

                // comment this out if you want the original button prefab
                Image iconNumber = gameObject.transform.Find("Number").gameObject.GetComponent<Image>();
                Color imageColor2 = iconNumber.color;
                iconNumber.color = new Color(imageColor2.r, imageColor2.g, imageColor2.b, 0.6f);

                lockIcon.SetActive(true);
            }
        }

    }
}
