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
        //[DllImport("__Internal")]
        //private static extern void QupcakesGameSaved(string data);

        public static int Save()
        {
            // Save research data
            //string dataJson = JsonUtility.ToJson(GameManagement.Instance.game.gameStat);
            //QupcakesGameSaved(dataJson);

            // Save game data
            LoadGame.saveData.UpdateGameData(GameManagement.Instance.game.gameStat);
            //Debug.Log("Saving Data " + JsonUtility.ToJson(LoadGame.saveData));

            Wrapper.Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.Qupcakes,
                LoadGame.saveData); ;
            return 0;
        }
    }
}
