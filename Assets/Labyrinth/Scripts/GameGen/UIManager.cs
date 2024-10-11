using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Labyrinth 
{
    public class UIManager : MonoBehaviour
    {
        public TMP_Text levelNumber;

        public Animator winScreen;
        public Animator loseScreen;

        public GameObject gameplayButtons;
        public GameObject gameplayObjects;
        public GameObject progressBar;


        private void OnEnable() 
        {
            TTEvents.SetLevelNumber += SetLevelNumber;
            TTEvents.SetProgressBar += SetProgressBar;
            TTEvents.UpdateProgressBar += UpdateProgressBar;
            TTEvents.ResetUI += Reset;
            TTEvents.LevelComplete += LevelComplete;
        }
        private void OnDisable() 
        {
            TTEvents.SetLevelNumber -= SetLevelNumber;
            TTEvents.SetProgressBar -= SetProgressBar;
            TTEvents.UpdateProgressBar -= UpdateProgressBar;
            TTEvents.ResetUI -= Reset;
            TTEvents.LevelComplete -= LevelComplete;
        }


        public void SetLevelNumber(string num) {
            if (num == "0") { return; }
            levelNumber.text = num;
        }

        public void SetProgressBar(int pathLength) {
            progressBar.GetComponent<ProgressBar>().resetBar();
            progressBar.GetComponent<ProgressBar>().initializeBar(pathLength);
        }

        public void UpdateProgressBar(int numSteps) {
            progressBar.GetComponent<ProgressBar>().detractBar(numSteps);
        }

        public void Reset() {
            winScreen.SetBool("IsOn", false);
            winScreen.SetInteger("NumStars", 0);
            loseScreen.SetBool("IsOn", false);

            gameplayButtons.SetActive(true);
            gameplayObjects.SetActive(true);
            progressBar.SetActive(true);
        }

        public void LevelComplete(int starsWon) {
            if (starsWon == 0) {
                loseScreen.SetBool("IsOn", true);
            }
            else {
                winScreen.SetBool("IsOn", true);
                winScreen.SetInteger("NumStars", starsWon);

                if (SaveData.Instance.CurrentLevel == 0) { return; }

                if (SaveData.Instance.CurrentLevel == SaveData.Instance.MaxLevelUnlocked) {
                    SaveData.Instance.MaxLevelUnlocked += 1;
                }
            }
            Save.Instance.SaveGame();

            gameplayButtons.SetActive(false);
            gameplayObjects.SetActive(false);
            progressBar.SetActive(false);
        }

    }
}