using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QueueBits;

namespace QueueBits 
{
    public class TokenCounter : MonoBehaviour
    {
        // public TMP_Text[] counterText;
        public int[] counts = new int[]{0,0,0};
        public GameObject[] counterObjects;

        // Boolean for if this display is for the player or the CPU
        public bool isPlayer;

        // TokenSelector object
        public TokenSelector TS;

        // Number of tokens available by level
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

        // TO DO: Phase out below dictionaries, implement via Enums.cs Prob
        // Prob.Pr100 = 0, Prob.Pr75 = 1, Prob.Pr50 = 2
        // 0 = 100%, 1 = 75%, 2 = 50%
        private Dictionary<int, int> indexToProb = new Dictionary<int, int>() {
            {0, 100}, {1, 75}, {2, 50}
        }; 
        // 100% = 0, 75% = 1, 50% = 2
        private Dictionary<int, int> probToIndex = new Dictionary<int, int>() {
            {100, 0}, {75, 1}, {50, 2}
        };

        // Initialize counter display, set texts and disable inactive tokens
        public void initCounter(int level) {
            for (int i=0; i<3; i++) {
                // counterText[i].text = tokenCountsPerLevel[level][i].ToString();
                // counts[i] = tokenCountsPerLevel[level][i];
                if (tokenCountsPerLevel[level][i] == 0) {
                    disableCounter(indexToProb[i]);
                    if (isPlayer) { TS.updateSelectorDisplay(indexToProb[i], 0); }
                }else{
                    setCounter(indexToProb[i], tokenCountsPerLevel[level][i]);
                }
            }
        }

        // Return dictionary of counter values
        public Dictionary<int, int> getCounterDict(int level) {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i=0; i<3; i++) {
                if (tokenCountsPerLevel[level][i] != 0) {
                    dict.Add(indexToProb[i], tokenCountsPerLevel[level][i]);
                }
            }
            return dict;
        }

        // Get the number of tokens available by probability
        public int getCounter(int prob) {
            if (probToIndex.ContainsKey(prob)) {
                return counts[probToIndex[prob]];
            }
            return 0;
        }

        // Set the counter of a certian probability to a value
        public void setCounter(int prob, int value) {
            if (probToIndex.ContainsKey(prob)) {
                // Debug.Log(prob, value)
                counts[probToIndex[prob]] = value;
                if (value < 5)
                {
                    int nToDisable = 5 - value;
                    foreach (Transform child in counterObjects[probToIndex[prob]].transform)
                    {
                        nToDisable -= 1;
                        child.gameObject.SetActive(false);
                        if(nToDisable == 0){
                            break;
                        }
                    }
                    
                }
                Debug.Log(prob);
                Debug.Log(value);
                Debug.Log("-----");
                                // counterText[probToIndex[prob]].text = value.ToString();
            }
            if (isPlayer) { TS.updateSelectorDisplay(prob, value); }
        }

        // Disable counter icon, when zero available
        public void disableCounter(int prob) {
            if (probToIndex.ContainsKey(prob)) {
                counterObjects[probToIndex[prob]].SetActive(false);
            }
        }
        
    }
}