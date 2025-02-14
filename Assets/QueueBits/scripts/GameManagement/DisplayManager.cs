using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;

namespace QueueBits {
    public class DisplayManager : MonoBehaviour
    {
        private int LEVEL_NUMBER;

        public GameObject displayHolder;
        public GameObject turnSign;

        // TO DO: Implement TokenCounters here instead of in GameMode#
        // public TokenCounter tokenCounterPlayer;
        // public TokenCounter tokenCounterCPU;

        public GameOverScreen resultDisplay;
        public TokenSelector tokenSelector;
        public LevelNumberDisplay numDisplay;
        
        // TO DO: Use StarDisplay in QB_Level scene
        // public StarDisplay starDisplay;


        // Initialize display at beginning of level
        public void initDisplay(int level) {
            LEVEL_NUMBER = level;

            displayHolder.SetActive(true);
            resultDisplay.gameObject.SetActive(false);
            numDisplay.initLevelNumber(level);
        }

        // Switch display when player switches turns
        public void SwitchPlayer(bool isPlayer) {
            turnSign.SetActive(isPlayer);
            tokenSelector.switchTurns(isPlayer);
        }

        // Display when the game is over
        public void GameOver(Results result) {
            displayHolder.SetActive(false);
            resultDisplay.gameObject.SetActive(true);
            resultDisplay.GameOver(result, LEVEL_NUMBER);
            numDisplay.resetLevelNumber();
        }
    }
}