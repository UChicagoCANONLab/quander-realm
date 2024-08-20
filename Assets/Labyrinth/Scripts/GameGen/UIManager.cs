using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth 
{
    public class UIManager : MonoBehaviour
    {
        public GameObject winScreen;
        public GameObject loseScreen;

        public GameObject gameplayButtons;
        public GameObject gameplayObjects;
        public GameObject progressBar;


        public void Reset() {
            winScreen.SetActive(false);
            loseScreen.SetActive(false);

            gameplayButtons.SetActive(true);
            gameplayObjects.SetActive(true);
            progressBar.SetActive(true);
        }

        public void LevelComplete(bool winStatus) {
            if (!winStatus) {
                loseScreen.SetActive(true);
                // loseScreen.GetComponent<Animator>.Play("Popups");
            }
            else {
                winScreen.SetActive(true);
                winScreen.GetComponent<Animator>().Play("Popups");
            }
            gameplayButtons.SetActive(false);
            gameplayObjects.SetActive(false);
            progressBar.SetActive(false);
        }
    }
}