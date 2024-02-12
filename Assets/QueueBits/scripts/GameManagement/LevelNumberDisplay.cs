using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QueueBits 
{
    // Display in QU_Level that has level number on it, set in DisplayManager.cs
    public class LevelNumberDisplay : MonoBehaviour
    {
        public GameObject evenNumberToken;
        public TMP_Text levelNum;

        // Initializes display
        public void initLevelNumber(int level) {
            if (level % 2 == 0) {
                evenNumberToken.SetActive(true);
            }
            levelNum.text = level.ToString();
        }

        // Resets display
        public void resetLevelNumber() {
            evenNumberToken.SetActive(false);
            levelNum.text = "0";
        }

    }
}
