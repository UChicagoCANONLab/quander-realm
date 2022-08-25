using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Saves a game to database
 */
namespace Qupcakery
{
    public class SaveGame : MonoBehaviour
    {
        public static SaveGame Instance;

        void Awake()
        {
            Instance = this;
        }

        public int Save()
        {
            // Save research data
            Data.researchData.UpdateResearchData(GameManagement.Instance.game.gameStat);
            string dataJson = JsonUtility.ToJson(Data.researchData);
            StartCoroutine(SaveResearchData(dataJson));

            // Save game data
            Data.gameData.UpdateGameData(GameManagement.Instance.game.gameStat);
            Wrapper.Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.Qupcakes,
                Data.gameData);
            return 0;
        }

        IEnumerator SaveResearchData(string dataJson)
        {
            string username = "test"; // #TODO

            //WWWForm form = new WWWForm();
            //form.AddField("username", username);
            //form.AddField("saveData", dataJson);

            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(dataJson);
            string url = "http://127.0.0.1:5000/qupcakes_save";
            //UnityWebRequest www = UnityWebRequest.Post(url, form);

            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
            }
        }
    }
}
