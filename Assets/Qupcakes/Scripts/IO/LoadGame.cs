using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

/*
 * Loads game data from file/database
 */

namespace Qupcakery
{
    public static class LoadGame
    {
        public static GameData saveData;

        public static void Load()
        {
            GameUtilities.CreateNewGame();

            try
            {
                string saveString = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Qupcakes);
                saveData = JsonUtility.FromJson<GameData>(saveString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (saveData == null)
                saveData = new GameData();
            else // Update gamestat accordingly
            {
                GameManagement.Instance.game.gameStat.TotalEarning = saveData.TotalEarning;
                GameManagement.Instance.game.gameStat.MaxLevelCompleted = saveData.MaxLevelCompleted;
                // Debug.Log("(data received) Max level completed is : " + saveData.MaxLevelCompleted);

                for (int i = 0; i < saveData.MaxLevelCompleted; i++)
                {
                    int starCnt = (int)saveData.levelPerformance[i];
                    GameManagement.Instance.game.gameStat.SetLevelPerformance((int)i+1, (int)starCnt);
                }
            }

            Utilities.InitializeTutorialAndHelpMenu();
        }
    }
}
