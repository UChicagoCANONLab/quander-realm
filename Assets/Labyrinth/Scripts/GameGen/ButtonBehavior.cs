using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Labyrinth 
{ 
    public class ButtonBehavior : MonoBehaviour
    {
        public GameObject winScreen;
        public GameObject loseScreen;
        public GameObject gameplayButtons;
        public GameObject gameplayObjects;
        public GameObject progressBar;
        public GameObject[] starsWonArr;
        public GameObject litePanel;

        public Button[] levelButtons;

        void Start() {
            if (SceneManager.GetActiveScene().name == "LA_MainMenu") {
                Load.LoadGame();
                DialogueAndRewards.Instance.updateDialogueDict();
            }
            if ((SceneManager.GetActiveScene().name == "LA_MainMenu") 
            && (DialogueAndRewards.Instance.levelDialogue[-1] == false)) {
                Wrapper.Events.StartDialogueSequence?.Invoke("LA_Intro");
                DialogueAndRewards.Instance.levelDialogue[-1] = true;
            }

            if (levelButtons == null) {
                return;
            }

            else if (levelButtons.Length > 0) {
                string prefix = "Canvas/LevelButtons-New/Container";
                for (int i=1; i<=15; i++) {
                    GameObject.Find($"StarMessage{i}").GetComponent<StarMessage>().displayStars();
                    if (i > SaveData.Instance.MaxLevelUnlocked) {
                        levelButtons[i-1].enabled = false;
                        GameObject.Find($"{prefix}/{i}/Locked{i}").SetActive(true);
                    }
                }
            }
#if LITE_VERSION
            if (litePanel != null) {
                litePanel.SetActive(true);
            }
#endif
        }


        public void LoadLevelSelectMenu() {
            Time.timeScale = 1f;
            // Load.LoadGame();
            // DialogueAndRewards.Instance.updateDialogueDict();

            /* if (DialogueAndRewards.Instance.levelDialogue[0] == false) {
                LevelSelect(0);
            }
            else {
                SceneManager.LoadScene("LA_LevelSelect");
            } */
            SceneManager.LoadScene("LA_LevelSelect");
        }

        public void LoadMainMenu() {
            Time.timeScale = 1f;
            // Load.LoadTTSaveData();
            Load.LoadGame();
            SceneManager.LoadScene("LA_MainMenu");
        }

        public void Exit() {
            // Save.SaveGame();
            Application.Quit();
        }

        public void Win(int starsWon) {      
            if (starsWon == 0) {
                SaveData.Instance.winner = false;
                loseScreen.SetActive(true);
            }
            else {
                SaveData.Instance.winner = true;
                winScreen.SetActive(true);
                
                for (int i=0; i<starsWon; i++) { 
                    starsWonArr[i].SetActive(true); 
                }

                if (SaveData.Instance.CurrentLevel == SaveData.Instance.MaxLevelUnlocked) {
                    SaveData.Instance.MaxLevelUnlocked += 1;
                }
            }
            
            Save.Instance.SaveGame();
            
            gameplayButtons.SetActive(false);
            gameplayObjects.SetActive(false);
            progressBar.SetActive(false);

            // Time.timeScale = 0f;

            if ((SaveData.Instance.CurrentLevel % 5 == 0) 
            && (SaveData.Instance.CurrentLevel != 0)) {
                DialogueAndRewards.Instance.doDialogue(SaveData.Instance.CurrentLevel);
            }
            DialogueAndRewards.Instance.giveReward(SaveData.Instance.CurrentLevel);
            // DialogueAndRewards.Instance.updateDialogueDict();
        }

        public void UndoWin(int starsWon) {
            SaveData.Instance.winner = false;

            winScreen.SetActive(false);
            loseScreen.SetActive(false);
            gameplayButtons.SetActive(true);
            gameplayObjects.SetActive(true);
            progressBar.SetActive(true);

            for (int i=0; i<starsWon; i++) {
                starsWonArr[i].SetActive(false);
            }
            // Time.timeScale = 1f;
        }

        public void LevelSelect(int sel) {
            string currScene;
            SaveData.Instance.CurrentLevel = sel;

            switch(sel) {
                case 0:
                    DialogueAndRewards.Instance.doDialogue(sel);
                    SaveData.Instance.Degree = 0;
                    currScene = "LA_Tutorial";
                    break;
                case < 3:
                    DialogueAndRewards.Instance.doDialogue(sel);
                    SaveData.Instance.Degree = 0;
                    currScene = "LA_4x4";
                    break;
                case < 5:
                    SaveData.Instance.Degree = 0;
                    currScene = "LA_5x5";
                    break;
                case 5:
                    SaveData.Instance.Degree = 0;
                    currScene = "LA_6x6";
                    break;


                case < 8:
                    DialogueAndRewards.Instance.doDialogue(sel);
                    SaveData.Instance.Degree = 180;
                    currScene = "LA_4x4";
                    break;
                case < 10:
                    SaveData.Instance.Degree = 180;
                    currScene = "LA_5x5";
                    break;
                case 10:
                    SaveData.Instance.Degree = 180;
                    currScene = "LA_6x6";
                    break;


                case < 13:
                    DialogueAndRewards.Instance.doDialogue(sel);
                    SaveData.Instance.Degree = 90;
                    currScene = "LA_4x4";
                    break;
                case < 15:
                    SaveData.Instance.Degree = 90;
                    currScene = "LA_5x5";
                    break;
                case 15:
                    SaveData.Instance.Degree = 90;
                    currScene = "LA_6x6";
                    break;
                

                default:
                    currScene = "LA_LevelSelect";
                    break;
            }
            SceneManager.LoadScene(currScene);
        }



        // ~~~~~~~~~~~~~~~ Enabled in Filament Environment ~~~~~~~~~~~~~~~

        private void OnEnable() {
            Wrapper.Events.MinigameClosed += DestroyDataObject;
        }
        private void OnDisable(){
            Wrapper.Events.MinigameClosed -= DestroyDataObject;
        }
        private void DestroyDataObject() {
            Time.timeScale = 1f;
            Destroy(GameObject.Find("ProfileData"));
        }

    }
}