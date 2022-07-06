using System;
using System.Collections.Generic;
using UnityEngine;

namespace Qupcakery
{
    public class TutorialCloseButton : MonoBehaviour
    {
        public GameObject tutorialPanel;
        public GameObject pauseButton;

        public void CloseTutorialPanel()
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(false);
                GameUtilities.UnpauseGame();
                pauseButton.GetComponent<PauseButton>().SetPauseSprite();
            }
        }
    }
}
