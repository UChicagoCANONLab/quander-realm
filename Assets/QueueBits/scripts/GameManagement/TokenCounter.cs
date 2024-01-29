using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QueueBits;

namespace QueueBits {
    public class TokenCounter : MonoBehaviour
    {
        public TMP_Text counter100;
        public TMP_Text counter75;
        public TMP_Text counter50;

        public bool isPlayer;

        public GameObject[] counterObjects50;

        public TokenSelector TS;

        public int[][] tokenCountsPerLevel = new int[][] { // {100%, 75%, 50%}
            new int[] {0, 0, 0},  // Buffer so index == level number
            new int[] {21, 0, 0}, // Level 1, no prefilled
            new int[] {21, 0, 0},
            new int[] {7, 7, 0},  // Level 3, measured upon drop
            new int[] {5, 5, 4},
            new int[] {4, 4, 6},
            new int[] {7, 7, 0},  // Level 6, measured in order at end
            new int[] {5, 5, 4},
            new int[] {4, 6, 4},
            new int[] {4, 4, 6},
            new int[] {2, 6, 6},
            new int[] {7, 7, 0},  // Level 11, measured by choice at end
            new int[] {5, 5, 4},
            new int[] {4, 6, 4},
            new int[] {4, 4, 6},
            new int[] {2, 6, 6}
        };

        public void initCounter(int level) {
            counter100.text = tokenCountsPerLevel[level][0].ToString();
            counter75.text = tokenCountsPerLevel[level][1].ToString();
            counter50.text = tokenCountsPerLevel[level][2].ToString();
        }

        public int getCounter(int prob) {
            if (prob == 100) { return int.Parse(counter100.text);}
            else if (prob == 75) { return int.Parse(counter75.text);}
            else if (prob == 50) { return int.Parse(counter50.text);}
            return 0;
        }

        public void setCounter(int prob, int value) {
            if (prob == 100) { counter100.text = value.ToString();}
            else if (prob == 75) { counter75.text = value.ToString();}
            else if (prob == 50) { counter50.text = value.ToString();}

            if (isPlayer) { TS.updateSelectorDisplay(prob, value); }
        }

        public void disable50() {
            foreach (GameObject i in counterObjects50) {
                i.SetActive(false);
            }
            if (isPlayer) { TS.updateSelectorDisplay(50, 0); }
        }
        
    }
}