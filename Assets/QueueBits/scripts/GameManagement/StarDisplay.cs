using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;
using System;

namespace QueueBits 
{
    public class StarDisplay : MonoBehaviour
    {
        public GameObject[] stars;

        // Sets display to show number of stars inputted
        public void setDisplay(int numStars) {
            if (numStars == 0) {
                resetStars();
            } else {
                for (int i=0; i<numStars; i++) {
                    stars[i].SetActive(true);
                }
            }
        }

        // Resets display to show 0 stars
        public void resetStars()  {
            foreach (GameObject s in stars) {
                s.SetActive(false);
            }
        }

        // Sets display to show proper number of stars per Result
        public void setResults(Results winner) {
            int stars;
            // If player ties, they get 2 stars
            if (winner == Results.Draw) {
                stars = 2;
            } // If they win, they get 3 stars
            else if (winner == Results.Win) {
                stars = 3;
            } // If they lose, they get 1 star 
            else if (winner == Results.Lose) {
                stars = 1;
            } // If anything weird happens, they get 0
            else {
                stars = 0;
            }
            setDisplay(stars);
        }

        // Returns number of stars per Result
        public int getResults(Results winner) {
            if (winner == Results.Draw) {
                return 2;
            } else if (winner == Results.Win) {
                return 3;
            } else if (winner == Results.Lose) {
                return 1;
            } else {
                return 0;
            }
        }
    }
}