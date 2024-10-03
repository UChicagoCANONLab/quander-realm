using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Labyrinth 
{ 
    public class ButtonBehavior : MonoBehaviour
    {
        public GameObject litePanel;
        public Button[] levelButtons;


        private void OnEnable() 
        {
            Wrapper.Events.MinigameClosed += DestroyDataObject;
            TTEvents.SelectLevel += LevelSelect;
        }
        private void OnDisable()
        {
            Wrapper.Events.MinigameClosed -= DestroyDataObject;
            TTEvents.SelectLevel += LevelSelect;
        }
        
        private void DestroyDataObject() 
        {
            Destroy(GameObject.Find("ProfileData"));
        }


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
            SceneManager.LoadScene("LA_LevelSelect");
        }

        public void LoadMainMenu() {
            Load.LoadGame();
            SceneManager.LoadScene("LA_MainMenu");
        }

        public void NextLevel() {
            SaveData.Instance.CurrentLevel += 1;
            LevelSelect(SaveData.Instance.CurrentLevel);
        }

        public void LevelSelect(int sel) {
            string currScene;
            SaveData.Instance.CurrentLevel = sel;

            switch(sel) {
                // 0 Degree Levels
                case 0:
                    // DialogueAndRewards.Instance.doDialogue(sel);
                    // SaveData.Instance.Degree = 0;
                    currScene = "LA_Tutorial";
                    break;
                case < 3:
                    // DialogueAndRewards.Instance.doDialogue(sel);
                    SaveData.Instance.Degree = 0;
                    if (!DialogueAndRewards.Instance.tutorialSeen[0]) {
                        SaveData.Instance.CurrentLevel = 0;
                        currScene = "LA_Tutorial";
                        break;
                    }
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

                // 180 Degree Levels
                case < 8:
                    // DialogueAndRewards.Instance.doDialogue(sel);
                    SaveData.Instance.Degree = 180;
                    if (!DialogueAndRewards.Instance.tutorialSeen[1]) {
                        SaveData.Instance.CurrentLevel = 0;
                        currScene = "LA_Tutorial";
                        break;
                    }
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

                // 90 Degree Levels
                case < 13:
                    // DialogueAndRewards.Instance.doDialogue(sel);
                    SaveData.Instance.Degree = 90;
                    if (!DialogueAndRewards.Instance.tutorialSeen[2]) {
                        SaveData.Instance.CurrentLevel = 0;
                        currScene = "LA_Tutorial";
                        break;
                    }
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
            TTEvents.GiveHint?.Invoke();
        }

        public void RestartLevel() {
            TTEvents.RestartLevel?.Invoke();
        }

        public void MoveButton(string move) {
            Vector3 press = TTEvents.GetButtonPress.Invoke(move);
        }

        public void SwitchButton() {
            TTEvents.SwitchPlayer?.Invoke();
        }

        public void InfoButton() {
            TTEvents.ShowInfoMessage?.Invoke();
        }

    }
}