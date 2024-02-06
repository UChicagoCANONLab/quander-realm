using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QueueBits;

namespace QueueBits {
    public class TokenCounter : MonoBehaviour
    {
        public TMP_Text[] counterText;
        public GameObject[] counterObjects;

        // Boolean for if this display is for the player or the CPU
        public bool isPlayer;

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

        // 0 = 100%, 1 = 75%, 2 = 50%
        private Dictionary<int, int> indexToProb = new Dictionary<int, int>() {
            {0, 100}, {1, 75}, {2, 50}
        }; 
        // 100% = 0, 75% = 1, 50% = 2
        private Dictionary<int, int> probToIndex = new Dictionary<int, int>() {
            {100, 0}, {75, 1}, {50, 2}
        };

        public void initCounter(int level) {
            for (int i=0; i<3; i++) {
                counterText[i].text = tokenCountsPerLevel[level][i].ToString();
                if (tokenCountsPerLevel[level][i] == 0) {
                    disableCounter(indexToProb[i]);
                    if (isPlayer) { TS.updateSelectorDisplay(indexToProb[i], 0); }
                }
            }
        }

        public Dictionary<int, int> getCounterDict(int level) {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i=0; i<3; i++) {
                if (tokenCountsPerLevel[level][i] != 0) {
                    dict.Add(indexToProb[i], tokenCountsPerLevel[level][i]);
                }
            }
            return dict;
        }

        public int getCounter(int prob) {
            if (probToIndex.ContainsKey(prob)) {
                return int.Parse(counterText[probToIndex[prob]].text);
            }
            return 0;
        }

        public void setCounter(int prob, int value) {
            if (probToIndex.ContainsKey(prob)) {
                counterText[probToIndex[prob]].text = value.ToString();
            }
            if (isPlayer) { TS.updateSelectorDisplay(prob, value); }
        }

        public void disableCounter(int prob) {
            if (probToIndex.ContainsKey(prob)) {
                counterObjects[probToIndex[prob]].SetActive(false);
            }
        }
        
    }
}