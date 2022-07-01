using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Utilities functions for LevelManager */

public static class GameUtilities
{
    public static bool gameIsPaused { get; set; } = false;

    // Called to erase all previous data and start game anew, keep previously loaded resources (sprite/prefab)
    public static void CreateNewGame()
    {
        //Utilities.tutorialSprites.Clear();
        Utilities.InitializeTutorialAndHelpMenu();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Earning", 0);

        GameManagement.Instance.CreateNewGame();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public static void UnpauseGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public static int CalculateLevelResult(float completionRate)
    {
        int starCnt = 0;
        if (completionRate > 0.99f)
            starCnt = 3;
        else if (completionRate > 0.8f)
            starCnt = 2;
        else if (completionRate > 0.6f)
            starCnt = 1;

        return starCnt;
    }

    public static void SaveLevelResult(int levelInd, int starCnt)
    {
        string level = "Level" + (levelInd).ToString();
        if (PlayerPrefs.HasKey(level))
        {
            int prevCnt = PlayerPrefs.GetInt(level);
            if (prevCnt < starCnt)
            {
                PlayerPrefs.SetInt(level, (int)starCnt);
                GameManagement.Instance.game.gameStat.
                    SetLevelPerformance((int)levelInd, starCnt);
            }
        }
        else
        {
            PlayerPrefs.SetInt(level, (int)starCnt);
            GameManagement.Instance.game.gameStat.
                    SetLevelPerformance((int)levelInd, starCnt);
        }
    }

    // At the end of the level, update total earning tracker
    public static void UpdateTotalEarning(int currentLevelEarning)
    {
        int BaseEarning = PlayerPrefs.GetInt("Earning");
        int newTotalEarning = BaseEarning + currentLevelEarning;
        PlayerPrefs.SetInt("Earning", newTotalEarning);

        GameManagement.Instance.game.gameStat.UpdateTotalEarning(newTotalEarning);
    }

    public static int GetLevelResult(string level)
    {
        int starCnt = 0;

        if (PlayerPrefs.HasKey(level))
            starCnt = PlayerPrefs.GetInt(level);
        else
            throw new System.ArgumentException("WinMenu load unsuccessful: " +
                                                level + " result has not been" +
                                                " recorded.");
        return starCnt;
    }

    // Check win/lose condition and load corresponding transition scene
    public static void LoadMenuBasedOnEndResult(int starCnt)
    {
        if (starCnt > 0)
        {
            SceneManager.LoadScene("WinMenu");
        }
        else
            SceneManager.LoadScene("LoseMenu");
    }

    public static int GetTotalEarning()
    {
        return PlayerPrefs.GetInt("Earning");
    }

    public static Vector2 GetCustomerStartPosition(int beltIndex, int beltCnt)
    {
        Vector2 startPosition = SetupUtilities.BeltPosition(beltCnt, beltIndex) + new Vector2(10f, 0.05f);
        return new Vector2(startPosition.x, startPosition.y);
    }

    public static Vector2 GetCakeStartPosition(int beltIndex, int beltCnt)
    {
        float beltY = SetupUtilities.BeltPosition(beltCnt, beltIndex).y;
        return new Vector2(-4f, beltY+0.2f);
    }

}
