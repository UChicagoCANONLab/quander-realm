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

        public GameObject winScreen;
        public StarMessage starDisplay;
        public GameObject loseScreen;

        public GameObject gameplayButtons;
        public GameObject gameplayObjects;
        public GameObject progressBar;


        public void SetLevelNumber(string num) {
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
            winScreen.SetActive(false);
            loseScreen.SetActive(false);

            gameplayButtons.SetActive(true);
            gameplayObjects.SetActive(true);
            progressBar.SetActive(true);

            starDisplay.resetStars();
        }

        public void LevelComplete(int starsWon) {
            if (starsWon == 0) {
                loseScreen.SetActive(true);
            }
            else {
                winScreen.SetActive(true);
                starDisplay.showStars(starsWon);

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