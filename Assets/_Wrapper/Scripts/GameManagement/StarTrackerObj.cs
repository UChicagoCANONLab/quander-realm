using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Wrapper
{
    public class StarTrackerObj : MonoBehaviour
    {
        [SerializeField] public TMP_Text starDisplay;
        [SerializeField] public int starsWon;

        [SerializeField] private int totalStars;
        [SerializeField] private Game minigame;
        // [SerializeField] private bool gameUnlocked = true;

        // add animator?

        
        public void SetStarDisplay(int num) {
            starsWon = num;
            starDisplay.text = $"{num}";
        }

    }
}