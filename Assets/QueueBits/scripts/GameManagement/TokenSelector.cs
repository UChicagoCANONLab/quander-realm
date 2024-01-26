using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;
using UnityEngine.UI;

namespace QueueBits {
    public class TokenSelector : MonoBehaviour
    {        
        public GameObject select100;
        public GameObject select75;
        public GameObject select50;

        public TokenCounter TC;

        public bool visible;

        public Level3 GM;


        /* public void updateSelectorDisplay() {
            if (visible) { 
                this.gameObject.SetActive(true); 
            }

            if (TC.counter100.text == "0") {
                select100.SetActive(false);
            } if (TC.counter75.text == "0") {
                select75.SetActive(false);
            } if (TC.counter50.text == "0") {
                select50.SetActive(false);
            }
        } */
        
        public void updateSelectorDisplay(int prob, int value) {
            if (value == 0) {
                if (prob == 100) { select100.SetActive(false); }
                else if (prob == 75) { select75.SetActive(false); }
                else if (prob == 50) { select50.SetActive(false); }
            }
        }

        public void toggleDisplay() {
            this.gameObject.SetActive(!visible);
            visible = !visible;
        }

        public void SelectToken(int prob) {
            if (prob == 100 && TC.counter100.text != "0") {
                Debug.Log("Selected 100!");
                // GM.SpawnPiece(100);
            }
            if (prob == 75 && TC.counter75.text != "0") {
                Debug.Log("Selected 75!");
            }
            if (prob == 50 && TC.counter50.text != "0") {
                Debug.Log("Selected 50!");
            }
        }



    }
}