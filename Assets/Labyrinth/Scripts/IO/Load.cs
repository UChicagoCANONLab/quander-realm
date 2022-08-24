using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using System.IO;
using System.Runtime.InteropServices;

namespace Labyrinth 
{ 
    public class Load : MonoBehaviour
    {
        // [DllImport("__Internal")]
        // private static extern void TwinTanglementLoad(string callback);

        /* private void Start() {
            #if UNITY_WEBGL == true && UNITY_EDITOR == false
                TwinTanglementLoad("loadData");
            #endif
        } */

        /* public static void LoadGame() {
            #if UNITY_WEBGL == true && UNITY_EDITOR == false
                TwinTanglementLoad ("loadData");
            #endif
        }

        public void loadData(string data) {
            Debug.Log("I just got this data:");
            Debug.Log(data);
            //TODO Load Data into memory

            if (data.Length > 0) {
                Data tempData = JsonUtility.FromJson<Data>(data);
                
                for (int i=0; i<15; i++) {
                    SaveData.Instance.starsPerLevel[i] = tempData.MaxStarsPerLevel[i];
                }
            }
        } */

        TTSaveData saveData;

        public void InitTwinTanglementSaveData() {
            try {
                string saveString = Events.GetMinigameSaveData?.Invoke(Game.Labyrinth);
                saveData = JsonUtility.FromJson<TTSaveData>(saveString);
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }

            if (saveData == null) {
                saveData = new TTSaveData();
            }
        }

    }
}