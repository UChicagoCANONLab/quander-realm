using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;

namespace QueueBits {
    public class StarDisplay : MonoBehaviour
    {
        public GameObject[] stars;

        public void setDisplay(int numStars) 
        {
            if (numStars == 0) {
                resetStars();
            } else {
                for (int i=0; i<numStars; i++) {
                    stars[i].SetActive(true);
                }
            }
        }

        public void resetStars() 
        {
            foreach (GameObject s in stars) {
                s.SetActive(false);
            }
        }

        public void setResults(Results winner) 
        {
            if (winner == Results.Draw) {
                setDisplay(2);
            } else if (winner == Results.Win) {
                setDisplay(3);
            } else if (winner == Results.Lose) {
                setDisplay(1);
            } else {
                setDisplay(0);
            }
        }
    }
}