using System;
//using Wrapper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


namespace QueueBits
{
    public class LevelSelect : MonoBehaviour
    {
        // Holder object for level buttons
        public GameObject levelHolder;
        // Icon Prefab, has star display and button 
        public LevelSelectIcon iconStar;
        // Panel displayed in LITE_VERSION
        public GameObject litePanel;
        // LoadLevel for buttons
        public LoadLevel levelLoader;


        // Start is called before the first frame update
        void Start()
        {
            // Load data, try to do dialogue, load to display stars
            GameManager.Load();
            if (GameManager.saveData.dialogueSystem[0])
            {
                Wrapper.Events.StartDialogueSequence?.Invoke("QB_Intro");
                GameManager.saveData.dialogueSystem[0] = false;
                GameManager.Save();
            }

            // Init for LITE_VERSION
#if LITE_VERSION
            int numberOfIcons = 8;
            litePanel.SetActive(true);
#else
            int numberOfIcons = 15;
#endif

            // Instantiate Level Buttons
            for (int i = 1; i <= numberOfIcons; i++)
            {
                int currentLevelStar = GameManager.saveData.starSystem[i];
                LevelSelectIcon icon = Instantiate(iconStar, levelHolder.transform);
                icon.initIcon(i, currentLevelStar);
                icon.name = "Level" + i;
                
                // Set button to load level displayed on it
                int chosenLevel = i;
                icon.GetComponent<Button>().onClick.AddListener(delegate { levelLoader.loadLevel(chosenLevel); });
            }
        }
    }
}