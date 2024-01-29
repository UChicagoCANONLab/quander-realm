using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;

namespace QueueBits {
    public class DisplayManager : MonoBehaviour
    {
        public GameObject displayHolder;
        public GameObject turnSign;

        // public TokenCounter tokenCounterPlayer;
        // public TokenCounter tokenCounterCPU;

        public GameOverScreen resultDisplay;
        public TokenSelector tokenSelector;
        // public StarDisplay starDisplay;


        public void SwitchPlayer(bool isPlayer) {
            turnSign.SetActive(isPlayer);
            tokenSelector.switchTurns(isPlayer);
        }

        public void GameOver(Results result) {
            displayHolder.SetActive(false);
            resultDisplay.gameObject.SetActive(true);
            resultDisplay.GameOver(result);
        }

        /* public void initTokenCounters(int level) {
            tokenCounterPlayer.initCounter(level);
            tokenCounterCPU.initCounter(level);
        } */
    }
}