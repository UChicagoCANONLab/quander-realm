using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

/*
 * Loads game from file/database
 */

namespace Qupcakery
{
    public class LoadGame : MonoBehaviour
    {
        public GameObject ResumeButton;

        [DllImport("__Internal")]
        private static extern void QupcakesGameLoaded(string callback);

        public static int Load()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                QupcakesGameLoaded ("QupcakesLoadData");
#endif
            //string data = File.ReadAllText(Application.dataPath + "/test.txt");
            //QupcakesloadData(data);
            return 0;
        }

        public void QupcakesLoadData(string data)
        {
            //Debug.Log("I just got this data:");
            //Debug.Log(data);

            //if (data.Length > 0)
            //{
            //    GameUtilities.CreateNewGame();

            //    // If there is data
            //    GameStat gameStat = JsonUtility.FromJson<GameStat>(data);
            //    GameManagement.Instance.game.gameStat.TotalEarning = gameStat.TotalEarning;
            //    GameManagement.Instance.game.gameStat.MaxLevelCompleted = gameStat.MaxLevelCompleted;
            //    Debug.Log("(data received) Max level completed is : " + gameStat.MaxLevelCompleted);

            //    for (int i = 1; i <= gameStat.MaxLevelCompleted; i++)
            //    {
            //        int starCnt = (int)gameStat.GetLevelPerformance(i);
            //        string level = "Level" + i;
            //        GameManagement.Instance.game.gameStat.SetLevelPerformance((int)i, (int)starCnt);
            //    }
            //}
        }
    }
}
