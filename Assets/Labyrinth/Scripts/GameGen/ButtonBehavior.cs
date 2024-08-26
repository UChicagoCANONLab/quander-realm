using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Labyrinth 
{ 
    public class ButtonBehavior : MonoBehaviour
    {
        public GameBehavior GB;
        public PlayerMovement PM;
        public InfoPopup IP;

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
                    GameObject.Find($"{prefix}/{i}/StarMessage{i}").GetComponent<StarMessage>().displayStars();
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

        
        // ~~~~~~~~~~~~~~~ Scene Loading Functions ~~~~~~~~~~~~~~~

        public void LoadLevelSelectMenu() {
            /* if (DialogueAndRewards.Instance.levelDialogue[0] == false) {
                LevelSelect(0);
            }
            else {
                SceneManager.LoadScene("LA_LevelSelect");
            } */
            SceneManager.LoadScene("LA_LevelSelect");
        }

        public void LoadMainMenu() {
            Load.LoadGame();
            SceneManager.LoadScene("LA_MainMenu");
        }

        public void Exit() {
            Application.Quit();
        }

        public void NextLevel() {
            SaveData.Instance.CurrentLevel += 1;
            LevelSelect(SaveData.Instance.CurrentLevel);
        }

        public void LevelSelect(int sel) {
            string currScene;
            SaveData.Instance.CurrentLevel = sel;

            switch(sel) {
                case 0:
                    // DialogueAndRewards.Instance.doDialogue(sel);
                    // DialogueAndRewards.Instance.levelDialogue[0] = true;
                    // SaveData.Instance.Degree = 0;
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

        // ~~~~~~~~~~~~~~~ Calling Button Functions from Other Scripts ~~~~~~~~~~~~~~~

        public void HintButton() {
            GB.GiveHint();
        }

        public void RestartLevel() {
            GB.Restart();
        }

        public void MoveButton(string move) {
            Vector3 press = PM.getButtonPress(move);
        }

        public void SwitchButton() {
            PM.SwitchPlayer();
        }

        public void InfoButton() {
            IP.gameObject.SetActive(true);
            IP.showInfoMessage();
        }


        // ~~~~~~~~~~~~~~~ Enabled in Filament Environment ~~~~~~~~~~~~~~~

        private void OnEnable() {
            Wrapper.Events.MinigameClosed += DestroyDataObject;
        }
        private void OnDisable(){
            Wrapper.Events.MinigameClosed -= DestroyDataObject;
        }
        private void DestroyDataObject() {
            // Time.timeScale = 1f;
            Destroy(GameObject.Find("ProfileData"));
        }

    }
}