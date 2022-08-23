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
        public GameObject gameplayButtons;
        public GameObject gameplayObjects;
        public GameObject[] starsWon;

        public Button[] buttons;

        void Start() {
            if (SaveData.Instance.levelDialogue[-1] == true) {
                Wrapper.Events.StartDialogueSequence?.Invoke("LA_Intro");
                SaveData.Instance.levelDialogue[-1] = false;
            }

            if (buttons == null) {
                return;
            }
            /* else if (buttons.Length > 0) {
                for (int i=0; i<15; i++) {
                    if (i < FindObjectOfType<SaveData>().levelUnlocked) {
                        buttons[i].interactable = true;
                    }
                    else {
                        buttons[i].interactable = false;
                    }
                }
            } */
            else if (buttons.Length > 0) {
                for (int i=1; i<=15; i++) {
                    GameObject.Find($"StarMessage{i}").GetComponent<StarMessage>().displayStars();
                }
            }
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                LoadMainMenu();
                // LoadLevelSelectMenu();
            }
        }

        public void LoadLevelSelectMenu() {
            Time.timeScale = 1f;
            Load.LoadGame();

            if (SaveData.Instance.levelDialogue[0] == true) {
                LevelSelect(0);
            }
            else {
                SceneManager.LoadScene("LA_LevelSelect");
            }
        }

        /* public void InitialLoadLevelSelectMenu() {
            Time.timeScale = 1f;
            Load.LoadGame();
            SceneManager.LoadScene("LevelSelect");
        } */

        public void LoadMainMenu() {
            Time.timeScale = 1f;
            SceneManager.LoadScene("LA_MainMenu");
        }

        public void Exit() {
            // Save.SaveGame();
            
            Application.Quit();
        }

        public void Win(int goalsCollected) {      
            SaveData.Instance.winner = true;
            if (SaveData.Instance.CurrentLevel > 0) {
                Save.SaveGame();
            }
    
            winScreen.SetActive(true);
            gameplayButtons.SetActive(false);
            gameplayObjects.SetActive(false);
            starsWon[goalsCollected].SetActive(true);
            // Time.timeScale = 0f;

            if ((SaveData.Instance.CurrentLevel % 5 == 0) 
            && (SaveData.Instance.CurrentLevel != 0)) {
                doDialogue(SaveData.Instance.CurrentLevel);
            }
        }

        public void UndoWin(int goalsCollected) {
            SaveData.Instance.winner = false;

            winScreen.SetActive(false);
            gameplayButtons.SetActive(true);
            gameplayObjects.SetActive(true);
            starsWon[goalsCollected].SetActive(false);
            Time.timeScale = 1f;
        }

        public void LevelSelect(int sel) {
            string currScene;
            SaveData.Instance.CurrentLevel = sel;

            switch(sel) {
                case 0:
                    doDialogue(sel);
                    SaveData.Instance.Degree = 0;
                    currScene = "LA_Tutorial";
                    break;
                case < 3:
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
                    doDialogue(sel);
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
                    doDialogue(sel);
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

        public void doDialogue(int level) {
            Debug.Log("Outside");
            if (SaveData.Instance.levelDialogue.ContainsKey(level) &&
            SaveData.Instance.levelDialogue[level] == true) {
                Debug.Log("Inside");
                Wrapper.Events.StartDialogueSequence?.Invoke("LA_Level"+level);
                SaveData.Instance.levelDialogue[level] = false;
            }
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