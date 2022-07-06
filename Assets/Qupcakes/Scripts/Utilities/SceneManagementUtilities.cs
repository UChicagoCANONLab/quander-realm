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
        }

        public static void LoadHomePage()
        {
            SceneManager.LoadScene("QupcakesStartScene");
        }

        public static void LoadModeSelectionScene()
        {
            // SceneManager.LoadScene("ModeSelection");
            // #IMPORTANT: temporary change for quantime
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
