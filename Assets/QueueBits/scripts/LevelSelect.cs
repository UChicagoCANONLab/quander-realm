using System;
//using Wrapper;
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
        QBSaveData saveData;

        // Start is called before the first frame update
        void Start()
        {
            // InitQueueBitsSaveData();
            // star system
            if (StarSystem.levelStarCount == null)
            {
                Debug.Log("initialize star system");
                StarSystem.levelStarCount = new int[15+1];
                for (int i = 0; i < StarSystem.levelStarCount.Length; i++)
                {
                    StarSystem.levelStarCount[i] = 0;
                }
            }
            // dialogue
            if (DialogueManager.playDialogue == null)
            {
                DialogueManager.playDialogue = new bool[]
                {
                    true, // level select
                    true, // level 1
                    true, // level 2
                    true, // level 3
                    true, // level 4
                    true, // level 5
                    true, // level 6
                    true, // level 7
                    true, // level 8
                    true, // level 9
                    true, // level 10
                    true, // level 11
                    true, // level 12
                    true, // level 13
                    true, // level 14
                    true, // level 15
                };
            }
            if (DialogueManager.playDialogue[0])
            {
                Wrapper.Events.StartDialogueSequence?.Invoke("QB_Intro");
                DialogueManager.playDialogue[0] = false;
            }

            if (DialogueManager.playDialogue[0] == true)
            {
                Wrapper.Events.StartDialogueSequence?.Invoke("QB_Intro");
                // saveData.playDialogue[0] = false;
                DialogueManager.playDialogue[0] = false;
                // Wrapper.Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.QueueBits, saveData);
            }

            panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
            iconDimensions = iconStar3.GetComponent<RectTransform>().rect;
            int maxInARow = Mathf.FloorToInt((panelDimensions.width + iconSpacing.x) / (iconDimensions.width + iconSpacing.x));
            int maxInACol = Mathf.FloorToInt((panelDimensions.height + iconSpacing.y) / (iconDimensions.height + iconSpacing.y));
            amountPerPage = maxInARow * maxInACol;
            int totalPages = Mathf.CeilToInt((float)(StarSystem.levelStarCount.Length - 1)/ amountPerPage);
            LoadPanels(totalPages);
        }

        void InitQueueBitsSaveData()
        {
            try
            {
                string saveString = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.QueueBits);
                saveData = JsonUtility.FromJson<QBSaveData>(saveString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (saveData == null)
                saveData = new QBSaveData();
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
                int numberOfIcons = i == numberOfPanels ? (StarSystem.levelStarCount.Length - 1) - currentLevelCount : amountPerPage;
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
                int currentLevelStar = StarSystem.levelStarCount[currentLevelCount];
                GameObject icon;
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

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}