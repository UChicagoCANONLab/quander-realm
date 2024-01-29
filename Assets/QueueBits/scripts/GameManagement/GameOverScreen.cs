using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QueueBits;

namespace QueueBits {
    public class GameOverScreen : MonoBehaviour
    {
        public TMP_Text message;
        public StarDisplay starsWon;
        
        public GameObject[] characterResults;
        public string[] textResults = {"You Tied", "You Won!", "You Lost..."};
        
        public void GameOver(Results result) 
        {
            characterResults[(int)result].SetActive(true);
            message.text = textResults[(int)result];
            starsWon.setResults(result);
        }

        public void reset() 
        {
            message.text = "";
            starsWon.resetStars();
            foreach (GameObject icon in characterResults) {
                icon.SetActive(false);
            }
        }




    }
}