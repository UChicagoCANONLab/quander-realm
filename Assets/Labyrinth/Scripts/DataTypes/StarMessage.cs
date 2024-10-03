using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth 
{ 
    public class StarMessage : MonoBehaviour
    {
        public int levelNum;
        public GameObject[] stars;

        public void displayStars() {
            int numStars = SaveData.Instance.starsPerLevel[levelNum-1];
            
            for (int i=0; i < numStars; i++) {
                stars[i].SetActive(true);
            }
        }

        public void showStars(int numStars) {
            for (int i=0; i < numStars; i++) {
                stars[i].SetActive(true);
            }
        }

        public void resetStars() {
            for (int i=0; i < 3; i++) {
                stars[i].SetActive(false);
            }
        }
    }
}