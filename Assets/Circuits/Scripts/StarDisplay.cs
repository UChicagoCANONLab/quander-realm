using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits 
{
    public class StarDisplay : MonoBehaviour 
    {
        public GameObject[] stars = new GameObject[3];
        public int numStars = 3;
        public int starPenalties = 0;

        // Defining static StarDisplay Singleton
        public static StarDisplay SD;
        void Awake() {
            if (SD != null) Debug.LogError("StarDisplay Singleton already exists!");
            SD = this;
        }

        public void AddPenalty() {
            starPenalties += 1;
            if (starPenalties % 2 == 0 && starPenalties > 0) {
                LoseStar();
            }
        }

        // Called when a hint is used -- lose one star
        public void LoseStar() {
            if (numStars == 0) { return; }
            switch (numStars) {
                case 3:
                    stars[0].SetActive(false);
                    break;
                case 2:
                    stars[1].SetActive(false);
                    break;
                case 1:
                    stars[2].SetActive(false);
                    break;
            }
            numStars -= 1;
        }

        // Resets stars to be back to all 3 active
        public void ResetStars() {
            foreach (GameObject i in stars) {
                i.SetActive(true);
            }
            numStars = 3;
            starPenalties = 0;
        }

    }
}