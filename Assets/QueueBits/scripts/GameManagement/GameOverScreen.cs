using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QueueBits;

namespace QueueBits 
{
    public class GameOverScreen : MonoBehaviour
    {
        // Text object and result strings for when Result determined
        public TMP_Text message;
        public string[] textResults = {"You Tied", "You Won!", "You Lost..."};
        
        // Shows proper number of stars when Result determined
        public StarDisplay starsWon;
        
        // Character graphics to display when Result determined
        public GameObject[] characterResults;
        
        
        // Called to initialize GameOver display
        public void GameOver(Results result, int level) 
        {
            characterResults[(int)result].SetActive(true);
            message.text = textResults[(int)result];
            starsWon.setResults(result);
        }

    }
}