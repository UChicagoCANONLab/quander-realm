using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        private Coroutine highlightCoroutine;
        private GameObject activeHighlight;
        private static GameObject hintArrowPrefab;

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
                ///Debug.Log(prob);
                //Debug.Log(value);
                //Debug.Log("-----");
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


        // Make buttons not interactable when not turn/already picked one
        public void toggleButtons(bool enabled) {
            foreach(GameObject holder in counterObjects) {
                foreach(Transform item in holder.transform) {
                    item.gameObject.GetComponent<Button>().interactable = enabled;
                }
            }
        }
        public void HighlightToken(int probability, GameObject hintArrowObj = null)
        {
            Debug.Log("hintArrowObj: " + hintArrowObj);

            // Cancel any existing highlight
            if (highlightCoroutine != null)
            {
                StopCoroutine(highlightCoroutine);
                // Don't destroy the arrow, just deactivate if needed
                if (activeHighlight != null)
                    activeHighlight.SetActive(false);
            }

            // Get the appropriate token counter GameObject based on probability
            GameObject targetCounter = null;
            if (probability == 100 && counts[0] > 0)
                targetCounter = counterObjects[0];
            else if (probability == 75 && counts[1] > 0)
                targetCounter = counterObjects[1];
            else if (probability == 50 && counts[2] > 0)
                targetCounter = counterObjects[2];

            // If no valid token or no tokens of that probability left, exit
            if (targetCounter == null)
            {
                Debug.LogWarning($"No valid token counter found for probability {probability}");
                return;
            }

            Debug.Log($"Creating highlight for {probability}% token at {targetCounter.name}");

            // Update the static reference if we get a new one
            if (hintArrowObj != null)
            {
                hintArrowPrefab = hintArrowObj;
            }

            // Use the provided hint arrow
            if (hintArrowPrefab != null)
            {
                activeHighlight = hintArrowPrefab;
                activeHighlight.SetActive(true);

                Vector3 counterPos = targetCounter.transform.position;
                activeHighlight.transform.position = counterPos + new Vector3(4.1f, 1.6f, 0f);


                // Start animation coroutine
                highlightCoroutine = StartCoroutine(AnimateTokenHighlight(activeHighlight));
            }
            else
            {
                Debug.LogError("No hint arrow available to use!");
            }
        }

        private IEnumerator AnimateTokenHighlight(GameObject highlight)
        {
            // Get the sprite renderer (either directly or from children)
            SpriteRenderer sr = highlight.GetComponent<SpriteRenderer>();
            if (sr == null)
                sr = highlight.GetComponentInChildren<SpriteRenderer>();

            // Animation settings
            float duration = 7.0f;
            float elapsed = 0f;
            Vector3 originalPosition = highlight.transform.position;

            //Start at the RIGHT side, move to the left and back
            Vector3 rightPosition = originalPosition + new Vector3(0.3f, 0f, 0); // Start on right side

            float animationSpeed = 1.1f;

            // Move to starting position immediately
            highlight.transform.position = rightPosition;

            while (elapsed < duration)
            {
                // Calculate a value that goes 0->1->0 smoothly for the ping-pong effect
                float t = Mathf.PingPong(elapsed * animationSpeed, 1f);

                // Move between right position and original position
                highlight.transform.position = Vector3.Lerp(rightPosition, originalPosition, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Reset to original position at the end
            highlight.transform.position = originalPosition;

            // Cleanup Cleanup
            highlight.SetActive(false);


            activeHighlight = null;
            highlightCoroutine = null;
        }
        public void CancelHighlight()
        {
            if (highlightCoroutine != null)
            {
                StopCoroutine(highlightCoroutine);

                if (activeHighlight != null)
                    activeHighlight.SetActive(false);
                    // Destroy(activeHighlight);
                
                activeHighlight = null;
                highlightCoroutine = null;
            }
        }
    }
}