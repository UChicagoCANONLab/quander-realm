using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Qupcakery
{
    public class UILevelPanelController : MonoBehaviour
    {
        // public Text t;
        // public Sprite[] levelNumber = new Sprite[27];
        public TextMeshProUGUI num;

        private void Start()
        {
            // t.text = "LEVEL " + GameManagement.Instance.GetCurrentLevel().LevelInd;
            // Image levelNum = gameObject.transform.Find("LevelNumber").GetComponent<Image>();
            // levelNum.sprite = levelNumber[GameManagement.Instance.GetCurrentLevel().LevelInd - 1];

            num.text = $"{GameManagement.Instance.GetCurrentLevel().LevelInd}";
        }
    }
}

