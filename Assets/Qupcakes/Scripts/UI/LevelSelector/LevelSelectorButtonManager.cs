using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Costume = AssetCostumeUtilities;
using TMPro;

/* Manages selection button appearance and action */

public class LevelSelectorButtonManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] stars = new Sprite[4];

    // On-click load target level
    public void LoadLevel()
    {
        string iconName = GetComponentInChildren<TextMeshProUGUI>().text;
        int levelNum = System.Convert.ToInt32(iconName.Split(' ')[1]);
        GameManagement.Instance.SetCurrentLevel(levelNum);

        if (GameManagement.Instance.gameMode == GameManagement.GameMode.Regular)
            SceneManagementUtilities.LoadGameScene();
        else
            SceneManagementUtilities.LoadExperimentMode();
    }

    // Set star
    public void SetStar(int starCnt)
    {
        GameObject starIcon = gameObject.transform.Find("RegularIcon").gameObject;
        starIcon.GetComponent<Image>().sprite = stars[starCnt];
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
