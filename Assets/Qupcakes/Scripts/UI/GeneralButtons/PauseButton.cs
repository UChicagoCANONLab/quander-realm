using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qupcakery
{
    public class PauseButton : MonoBehaviour
    {
        public Sprite pauseSprite;
        public Sprite unpauseSprite;
        public GameObject icon;
        public GameObject overlay;

        // Image image;

        private void Awake()
        {
            // image = GetComponent<Image>();
            SetPauseSprite();
        }

        public void PauseGame()
        {
            if (GameObject.FindGameObjectsWithTag("InfoPanel").Length > 0)
                return;

            if (GameUtilities.gameIsPaused)
            {
                GameUtilities.UnpauseGame();
                SetPauseSprite();
                //Utilities.DeactiveGamePanel();
            }
            else
            {
                GameUtilities.PauseGame();
                SetUnpauseSprite();
                //Utilities.ActiveGamePanel();
            }
        }

        public void SetPauseSprite()
        {
            icon.GetComponent<Image>().sprite = pauseSprite;
            overlay.SetActive(false);
            // image.sprite = pauseSprite;
        }

        public void SetUnpauseSprite()
        {
            icon.GetComponent<Image>().sprite = unpauseSprite;
            overlay.SetActive(true);
            // image.sprite = unpauseSprite;
        }
    }
}
