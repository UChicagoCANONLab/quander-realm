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

        // public TokenCounter tokenCounterPlayer;
        // public TokenCounter tokenCounterCPU;

        public GameOverScreen resultDisplay;
        public TokenSelector tokenSelector;
        public LevelNumberDisplay numDisplay;
        public LoadLevel menu;
        // public StarDisplay starDisplay;


        public void SwitchPlayer(bool isPlayer) {
            turnSign.SetActive(isPlayer);
            tokenSelector.switchTurns(isPlayer);
        }

        public void GameOver(Results result) {
            displayHolder.SetActive(false);
            resultDisplay.gameObject.SetActive(true);
            resultDisplay.GameOver(result, LEVEL_NUMBER);
            numDisplay.resetLevelNumber();
        }

        /* public void initTokenCounters(int level) {
            tokenCounterPlayer.initCounter(level);
            tokenCounterCPU.initCounter(level);
        } */

        public void initDisplay(int level) {
            displayHolder.SetActive(true);
            resultDisplay.gameObject.SetActive(false);
            numDisplay.initLevelNumber(level);
            menu.currentLevel = level;

            LEVEL_NUMBER = level;
        }

    }
}