using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;
using UnityEngine.UI;

namespace QueueBits {
    public class TokenSelector : MonoBehaviour
    {        
        // GameObjects of each token icon and percentage text
        public GameObject select100;
        public GameObject select75;
        public GameObject select50;

        // Holds all the objects for the player's display
        // When inactive, shows CPU's display (i.e. "Waiting...")
        public GameObject playerDisplay;

        // Container for all the token buttons
        // Goes inactive after player selects token
        public GameObject pieceContainer;

        // TokenCounter and GameController references
        public TokenCounter TC;
        public GameController GC;


        // Updates display to exclude unavailable tokens
        public void updateSelectorDisplay(int prob, int value) {
            if (value == 0) {
                if (prob == 100) { select100.SetActive(false); }
                else if (prob == 75) { select75.SetActive(false); }
                else if (prob == 50) { select50.SetActive(false); }
            }
        }

        // Toggles display between Player and CPU
        public void switchTurns(bool showing) {
            playerDisplay.SetActive(showing);
            pieceContainer.SetActive(showing);
        }

        // Function called by token buttons to select token
        // TokenSelector -> GameController -> GameMode#
        public void SelectToken(int prob) {
            if (prob == 100 && TC.counterText[0].text != "0") {
                GC.tokenSelectedByButton(100);
                pieceContainer.SetActive(false);
            }
            if (prob == 75 && TC.counterText[1].text != "0") {
                GC.tokenSelectedByButton(75);
                pieceContainer.SetActive(false);
            }
            if (prob == 50 && TC.counterText[2].text != "0") {
                GC.tokenSelectedByButton(50);
                pieceContainer.SetActive(false);
            }
        }



    }
}