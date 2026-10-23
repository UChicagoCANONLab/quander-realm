using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Loads non-research game data from file/database:
//     Dialogue System
//     Star System

namespace QueueBits
{
    public class GameManager : MonoBehaviour
    {
        public static QBSaveData saveData;
        public static Data researchData;

        // rewardSystem NO LONGER USED -- kept uncommented for compilation reasons
        public static bool[] rewardSystem = { false, false, false, true, false, true, true, false, true, true, false, true, false, true, false, true };
        public static int LEVEL = 0;

        public static void Load()
        {
            try {
                string saveString = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.QueueBits);
                saveData = JsonUtility.FromJson<QBSaveData>(saveString);
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }

            if (saveData == null) {
                saveData = new QBSaveData();
            }

            if (saveData.maxLevelUnlocked == 0) {
                saveData.maxLevelUnlocked = 1;
            }

            Save();
        }

        public static void Save()
        {
            UpdateTotalStars();
            Wrapper.Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.QueueBits, saveData);
            Debug.Log("DataSaved!");
        }

        public static void UpdateTotalStars() {
            int temp = 0;
            foreach (int i in saveData.starSystem){
                temp += i;
            }
            saveData.totalStars = temp;
        }
    }
}
