using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Utility functions that facilitate scene change
 */

namespace Qupcakery
{
    public static class SceneManagementUtilities
    {

        public static void LoadGameScene()
        {
            SceneManager.LoadScene("Level");
            // If tutorial available, start tutorial sequence
            int levelInd = GameManagement.Instance.game.CurrLevelInd;
            if (TutorialManager.tutorialAvailable[levelInd])
            {
                Wrapper.Events.StartDialogueSequence?.Invoke("QU_Level"+levelInd.ToString());
                TutorialManager.UpdateAvailability(levelInd);
            }
        }

        public static void LoadHomePage()
        {
            SceneManager.LoadScene("QupcakesStartScene");
        }

        public static void LoadModeSelectionScene()
        {
            // SceneManager.LoadScene("ModeSelection");
            SceneManager.LoadScene("LevelSelection");
        }

        public static void LoadExperimentMode()
        {
            SceneManager.LoadScene("ExperimentMode");
        }

        public static void LoadLevelSelectionMenu()
        {
            SceneManager.LoadScene("LevelSelection");
        }

        public static void LoadLoseMenu()
        {
            SceneManager.LoadScene("LoseMenu");
        }

        public static void LoadWinMenu()
        {
            SceneManager.LoadScene("WinMenu");
        }
    }
}
