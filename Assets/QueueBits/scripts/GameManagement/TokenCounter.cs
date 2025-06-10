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
        public void HighlightToken(int probability)
        {
            // Cancel any existing highlight
            if (highlightCoroutine != null)
            {
                StopCoroutine(highlightCoroutine);
                if (activeHighlight != null)
                    Destroy(activeHighlight);
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
                return;

            // Create an arrow pointing to the token
            activeHighlight = new GameObject("TokenHighlightArrow");
            activeHighlight.transform.SetParent(transform);

            // Create a sprite renderer for the arrow
            SpriteRenderer sr = activeHighlight.AddComponent<SpriteRenderer>();

            // That won't ever work, so we create a custom arrow.
            if (sr.sprite == null) {
                // Create a custom arrow using line renderer
                LineRenderer lineRenderer = activeHighlight.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = new Color(0.7f, 0.3f, 1.0f);
                lineRenderer.endColor = new Color(0.7f, 0.3f, 1.0f);

                // Define points for a left-pointing arrow
                Vector3[] arrowPoints = new Vector3[7];
                // Define points for a right-pointing arrow
                arrowPoints[0] = new Vector3(0.3f, 0f, 0f);      // Arrow tip (now on the right)
                arrowPoints[1] = new Vector3(0f, 0.2f, 0f);      // Top left corner
                arrowPoints[2] = new Vector3(0f, 0.1f, 0f);      // Left top inner
                arrowPoints[3] = new Vector3(-0.3f, 0.1f, 0f);   // Shaft top
                arrowPoints[4] = new Vector3(-0.3f, -0.1f, 0f);  // Shaft bottom
                arrowPoints[5] = new Vector3(0f, -0.1f, 0f);     // Left bottom inner
                arrowPoints[6] = new Vector3(0f, -0.2f, 0f);     // Bottom left corner

                lineRenderer.positionCount = arrowPoints.Length + 1;

                // Complete the arrow (connect back to the tip)
                for (int i = 0; i < arrowPoints.Length; i++) {
                    lineRenderer.SetPosition(i, arrowPoints[i]);
                }

                // Close the shape
                lineRenderer.SetPosition(arrowPoints.Length, arrowPoints[0]);
                lineRenderer.enabled = false; // Disable the line renderer to avoid drawing lines

                // Create a simple arrowhead to fill in the shape
                GameObject arrowFill = new GameObject("ArrowFill");
                arrowFill.transform.parent = activeHighlight.transform;
                arrowFill.transform.localPosition = Vector3.zero;

                // Use a mesh to create a filled arrow
                MeshFilter meshFilter = arrowFill.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = arrowFill.AddComponent<MeshRenderer>();

                Mesh mesh = new Mesh();
                Vector3[] vertices = arrowPoints;
                int[] triangles = new int[] {
                    0, 1, 2,  // Top triangle of arrowhead
                    0, 2, 5,  // Middle of arrowhead
                    0, 5, 6,  // Bottom triangle of arrowhead
                    2, 3, 4,  // Rectangle of shaft
                    2, 4, 5   // Rectangle of shaft
                };

                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.RecalculateNormals();

                meshFilter.mesh = mesh;
                meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
                meshRenderer.material.color = new Color(0.7f, 0.3f, 1.0f);

                // Disable the sprite renderer since we're using line renderer and mesh
                sr.enabled = false;
            }

            // Position the arrow to the right of the target counter (so it points left)
            activeHighlight.transform.position = targetCounter.transform.position + new Vector3(1.5f, 0f, 0.1f);
            // Set color and initial properties
            sr.color = new Color(0.7f, 0.3f, 1f, 0.8f); // Yellow with slight transparency

            // Rotate the arrow to point left (assuming the default arrow points right)
            activeHighlight.transform.rotation = Quaternion.Euler(0, 0, 180);

            // Start animation coroutine
            highlightCoroutine = StartCoroutine(AnimateTokenHighlight(activeHighlight));
        }

        private IEnumerator AnimateTokenHighlight(GameObject highlight)
        {
            SpriteRenderer sr = highlight.GetComponent<SpriteRenderer>();
            if (sr == null)
                sr = highlight.GetComponentInChildren<SpriteRenderer>();

            float duration = 7.0f;
            float elapsed = 0f;
            Vector3 originalPosition = highlight.transform.position;
            Vector3 targetPosition = originalPosition + new Vector3(0.3f, 0f, 0); // Move right instead of left
            while (elapsed < duration)
            {
                // Pulse alpha
                if (sr != null) {
                    float alpha = Mathf.PingPong(elapsed * 2f, 0.8f) + 0.2f;
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
                }

                // Move left and right slightly
                float t = Mathf.PingPong(elapsed, 1f);
                highlight.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(highlight);
            activeHighlight = null;
            highlightCoroutine = null;
        }
        public void CancelHighlight()
        {
            if (highlightCoroutine != null)
            {
                StopCoroutine(highlightCoroutine);
                if (activeHighlight != null)
                    Destroy(activeHighlight);
                activeHighlight = null;
                highlightCoroutine = null;
            }
        }
    }
}