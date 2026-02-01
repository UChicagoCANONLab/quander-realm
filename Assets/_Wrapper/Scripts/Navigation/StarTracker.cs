using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Wrapper
{
    public class StarTracker : MonoBehaviour
    {
        [SerializeField] public TMP_Text starDisplay;
        [SerializeField] public int starsWon;
        [SerializeField] public bool gameUnlocked = true;

        [SerializeField] private int totalStars;
        [SerializeField] private Game minigame;

        [SerializeField] private Animator animator;

        
        public void SetStarDisplay(int num) {
            starsWon = num;
            starDisplay.text = $"{num}";

            if (animator != null) {
                if (starsWon == totalStars) { 
                    animator.SetBool("Completed", true);
                } else {
                    animator.SetBool("Completed", false);
                }
            }
        }

        public void OnEnable()
        {
            if (animator != null) {
                animator.SetBool("Unlocked", gameUnlocked);
                animator.SetBool("Completed", (starsWon == totalStars));
            }
        }


        public void ResetStarDisplay() {
            if (minigame == Game.Circuits 
            || minigame == Game.QueueBits
            || minigame == Game.BlackBox) {
                gameUnlocked = false;
            }
            SetStarDisplay(0);
        }


        public void SetGameUnlocked(bool isGameUnlocked) {
            if (animator != null)
            {
                animator.SetBool("Unlocked", isGameUnlocked);
                if (gameUnlocked == isGameUnlocked) {
                    return;
                } else {
                    bool wasLocked = !gameUnlocked; // Track if it was previously locked
                    gameUnlocked = isGameUnlocked;
                    // Only fire event if game was locked and is now unlocked (actual unlock event)
                    if (wasLocked && isGameUnlocked) {
                        Events.UnlockAndDisplayGame?.Invoke(minigame);
                    }
                }
            }
        }
        public bool IsGameUnlocked() 
        {
            return gameUnlocked;
        }

    }
}