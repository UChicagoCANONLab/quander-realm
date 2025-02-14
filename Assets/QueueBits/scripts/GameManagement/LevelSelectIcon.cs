using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QueueBits 
{
    // Icon that appears in QU_LevelSelect, has button component set in LevelSelect.cs
    public class LevelSelectIcon : MonoBehaviour
    {
        public TMP_Text levelNum;
        public StarDisplay starIcons;
        public GameObject evenNumberToken;

        public void initIcon(int level, int stars)
        {
            levelNum.text = $"{level}";
            starIcons.setDisplay(stars);

            if (level % 2 == 0) {
                evenNumberToken.SetActive(true);
            }
        }
    }
}