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
            SceneManager.LoadScene("LevelSelection");
        }

        public static void LoadExperimentMode()
        {
            SceneManager.LoadScene("ExperimentMode");
        }

        public static void LoadLevelSelectionMenu()
        {
            if (GameManagement.Instance.game.gameStat.MaxLevelCompleted
                == GameManagement.Instance.GetTotalLevelCnt())
                Wrapper.Events.StartDialogueSequence?.Invoke("QU_End");

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
