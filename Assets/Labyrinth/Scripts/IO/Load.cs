using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.InteropServices;


namespace Labyrinth 
{ 
    public class Load : MonoBehaviour
    {

        public static void LoadGame() {

            try {
                string saveString = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Labyrinth);
                Data.Instance.ttSaveData = JsonUtility.FromJson<TTSaveData>(saveString);
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }

            if (Data.Instance.ttSaveData == null) {
                Data.Instance.ttSaveData = new TTSaveData();
            }
            else {
                SaveData.Instance.totalStars = 0;
                SaveData.Instance.MaxLevelUnlocked = Data.Instance.ttSaveData.MaxLevelUnlocked;
                for (int i=0; i<15; i++) {
                    SaveData.Instance.starsPerLevel[i] = Data.Instance.ttSaveData.MaxStarsPerLevel[i];
                    SaveData.Instance.totalStars += Data.Instance.ttSaveData.MaxStarsPerLevel[i];
                }
                for (int i=0; i<5; i++) {
                    SaveData.Instance.dialogueSeen[i] = Data.Instance.ttSaveData.DialogueSeen[i];
                }
            }
            Data.Instance.researchData = new ResearchData();
        }
    }
}