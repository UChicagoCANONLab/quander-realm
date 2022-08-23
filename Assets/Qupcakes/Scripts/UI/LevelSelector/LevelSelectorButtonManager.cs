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

        // On-click load target level
        public void LoadLevel()
        {
            if (levelAvailable)
            {
                string iconName = GetComponentInChildren<TextMeshProUGUI>().text;
                int levelNum = System.Convert.ToInt32(iconName.Split(' ')[1]);
                GameManagement.Instance.SetCurrentLevel(levelNum);

                if (GameManagement.Instance.gameMode == GameManagement.GameMode.Regular)
                    SceneManagementUtilities.LoadGameScene();
                else
                    SceneManagementUtilities.LoadExperimentMode();
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

                TextMeshProUGUI text = gameObject.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
                Color32 textColor = text.color;
                text.color = new Color(textColor.r, textColor.g, textColor.b, 0.4f);
            }
        }

        // Set icon sprite for given game mode
        public void SetMode(GameManagement.GameMode mode)
        {
            GameObject regIcon = gameObject.transform.Find("RegularIcon").gameObject;
            GameObject expIcon = gameObject.transform.Find("ExperimentIcon").gameObject;

            switch (mode)
            {
                case GameManagement.GameMode.Regular:
                    regIcon.SetActive(true);
                    expIcon.SetActive(false);
                    break;
                case GameManagement.GameMode.Experiment:
                    regIcon.SetActive(false);
                    expIcon.SetActive(true);
                    break;
                default:
                    throw new System.ArgumentException("Unrecognized mode: " + mode);
            }
        }
    }
}
