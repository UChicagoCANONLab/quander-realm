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
        
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        [DllImport("__Internal")]
        private static extern void QupcakesGameLoaded(string callback);
#endif
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
            Debug.Log("I just got this data:");
            Debug.Log(data);

            if (data.Length > 0)
            {
                GameManagement.Instance.CreateNewGame();

                // If there is data
                GameStat gameStat = JsonUtility.FromJson<GameStat>(data);
                GameManagement.Instance.game.gameStat.TotalEarning = gameStat.TotalEarning;
                GameManagement.Instance.game.gameStat.MaxLevelCompleted = gameStat.MaxLevelCompleted;
                PlayerPrefs.SetInt("Earning", (int)gameStat.TotalEarning);

                for (int i = 1; i <= GameManagement.Instance.GetTotalLevelCnt(); i++)
                {
                    int starCnt = (int)gameStat.GetLevelPerformance(i);
                    string level = "Level" + i;
                    PlayerPrefs.SetInt(level, starCnt);

                    GameManagement.Instance.game.gameStat.SetLevelPerformance((int)i, (int)starCnt);
                }
            }
        }
    }
}
