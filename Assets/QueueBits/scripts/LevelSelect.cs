using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Script adapted from: https://pressstart.vip/tutorials/2019/06/1/96/level-selector.html

namespace QueueBits
{
    public class LevelSelect : MonoBehaviour
    {
        public GameObject levelHolder;
        public GameObject iconStar0;
        public GameObject iconStar1;
        public GameObject iconStar2;
        public GameObject iconStar3;
        public GameObject thisCanvas;
        public Vector2 iconSpacing;
        private Rect panelDimensions;
        private Rect iconDimensions;
        private int amountPerPage;
        private int currentLevelCount;

        // Start is called before the first frame update
        void Start()
        {
            // star system
            if (Object.Equals(StarSystem.levelStarCount, default(Dictionary<int, int>)))
            {
                Debug.Log("initialize star system");
                StarSystem.levelStarCount = new Dictionary<int, int>
                {
                    {1,0},
                    {2,0},
                    {3,0},
                    {4,0},
                    {5,0},
                    {6,0},
                    {7,0},
                    {8,0},
                    {9,0},
                    {10,0},
                    {11,0},
                    {12,0},
                    {13,0},
                    {14,0},
                    {15,0}
                };
            }
            // dialogue
            if (Object.Equals(DialogueManager.playDialogue, default(Dictionary<int, bool>)))
            {
                DialogueManager.playDialogue = new Dictionary<int, bool>
                {
                    {0,true},
                    {1,true},
                    {2,true},
                    {3,true},
                    {4,true},
                    {5,true},
                    {6,true},
                    {7,true},
                    {8,true},
                    {9,true},
                    {10,true},
                    {11,true},
                    {12,true},
                    {13,true},
                    {14,true},
                    {15,true}
                };
            }
            if (DialogueManager.playDialogue[0])
            {
                Wrapper.Events.StartDialogueSequence?.Invoke("QB_Intro");
                DialogueManager.playDialogue[0] = false;
            }

            panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
            iconDimensions = iconStar3.GetComponent<RectTransform>().rect;
            int maxInARow = Mathf.FloorToInt((panelDimensions.width + iconSpacing.x) / (iconDimensions.width + iconSpacing.x));
            int maxInACol = Mathf.FloorToInt((panelDimensions.height + iconSpacing.y) / (iconDimensions.height + iconSpacing.y));
            amountPerPage = maxInARow * maxInACol;
            int totalPages = Mathf.CeilToInt((float)StarSystem.levelStarCount.Keys.Count / amountPerPage);
            LoadPanels(totalPages);
        }
        void LoadPanels(int numberOfPanels)
        {
            GameObject panelClone = Instantiate(levelHolder) as GameObject;

            for (int i = 1; i <= numberOfPanels; i++)
            {
                GameObject panel = Instantiate(panelClone) as GameObject;
                panel.transform.SetParent(thisCanvas.transform, false);
                panel.transform.SetParent(levelHolder.transform);
                panel.name = "Page-" + i;
                panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i - 1), 0);
                SetUpGrid(panel);
                int numberOfIcons = i == numberOfPanels ? StarSystem.levelStarCount.Keys.Count - currentLevelCount : amountPerPage;
                LoadIcons(numberOfIcons, panel);
            }
            Destroy(panelClone);
        }
        void SetUpGrid(GameObject panel)
        {
            GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(iconDimensions.width, iconDimensions.height);
            grid.childAlignment = TextAnchor.MiddleCenter;
            grid.spacing = iconSpacing;
        }
        void LoadIcons(int numberOfIcons, GameObject parentObject)
        {
            for (int i = 1; i <= numberOfIcons; i++)
            {
                currentLevelCount++;
                int currentLevelStar;
                GameObject icon;
                if (StarSystem.levelStarCount.TryGetValue(currentLevelCount, out currentLevelStar))
                {
                    if (currentLevelStar == 3)
                    {
                        icon = Instantiate(iconStar3) as GameObject;
                    }
                    else if (currentLevelStar == 2)
                    {
                        icon = Instantiate(iconStar2) as GameObject;
                    }
                    else if (currentLevelStar == 1)
                    {
                        icon = Instantiate(iconStar1) as GameObject;
                    }
                    else
                    {
                        icon = Instantiate(iconStar0) as GameObject;
                    }
                    icon.transform.SetParent(thisCanvas.transform, false);
                    icon.transform.SetParent(parentObject.transform);
                    icon.name = "Level" + i;
                    icon.GetComponentInChildren<TextMeshProUGUI>().SetText("Level " + currentLevelCount);
                    string chosenLevel = "Level"+i;
                    icon.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(chosenLevel); });
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}