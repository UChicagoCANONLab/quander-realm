using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Restart level during gameplay

namespace Qupcakery
{
    public class RestartButton : MonoBehaviour
    {
        public void RestartGame()
        {
            GameManagement.Instance.game.gameStat.SetLevelResultAndSave(GameStat.LevelResult.QUIT);

            if (GameObject.FindGameObjectsWithTag("InfoPanel").Length > 0)
                return;

            if (GameUtilities.gameIsPaused) GameUtilities.UnpauseGame();

            if (GameManagement.Instance.InTutorial) GameObject.Find("TutorialItems").
                    GetComponent<QCTutorialLevel>().EndTutorial();

            GameObjectsManagement.ResetAllGameObjects();
            SceneManagementUtilities.LoadGameScene();
        }
    }
}
