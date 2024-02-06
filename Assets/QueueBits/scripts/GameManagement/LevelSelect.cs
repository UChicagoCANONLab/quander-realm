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
        public LevelSelectIcon iconStar;
        public GameObject litePanel;
        public LoadLevel levelLoader;


        // Start is called before the first frame update
        void Start()
        {
            GameManager.Load();

            if (GameManager.saveData.dialogueSystem[0])
            {
                Wrapper.Events.StartDialogueSequence?.Invoke("QB_Intro");
                GameManager.saveData.dialogueSystem[0] = false;
                GameManager.Save();
            }

#if LITE_VERSION
            int numberOfIcons = 8;
            litePanel.SetActive(true);
#else
            int numberOfIcons = 15;
#endif

            for (int i = 1; i <= numberOfIcons; i++)
            {
                int currentLevelStar = GameManager.saveData.starSystem[i];
                LevelSelectIcon icon = Instantiate(iconStar, levelHolder.transform);
                icon.initIcon(i, currentLevelStar);
                icon.name = "Level" + i;
                
                int chosenLevel = i;
                icon.GetComponent<Button>().onClick.AddListener(delegate { levelLoader.loadLevel(chosenLevel); });
            }
        }
    }
}