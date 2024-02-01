using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QueueBits 
{
    public class LevelNumberDisplay : MonoBehaviour
    {
        public GameObject evenNumberToken;
        public TMP_Text levelNum;

        public void initLevelNumber(int level) {
            if (level % 2 == 0) {
                evenNumberToken.SetActive(true);
            }
            levelNum.text = level.ToString();
        }

        public void resetLevelNumber() {
            evenNumberToken.SetActive(false);
            levelNum.text = "0";
        }

    }
}
