using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

/*
 * Saves a game to database
 */
namespace Qupcakery
{
    public class SaveGame : MonoBehaviour
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        [DllImport("__Internal")]
        private static extern void QupcakesGameSaved(string data);
#endif
        public static int Save()
        {
            Debug.Log("Saving game");
            string dataJson = JsonUtility.ToJson(GameManagement.Instance.game.gameStat);
            Debug.Log(dataJson);
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                QupcakesGameSaved(dataJson);
#endif
            return 0;
        }
    }
}
