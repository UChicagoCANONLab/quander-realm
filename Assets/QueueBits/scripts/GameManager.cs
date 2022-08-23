using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Loads game data from file/database:
//     Dialogue System
//     Star System

namespace QueueBits
{
    public class GameManager : MonoBehaviour
    {
        public static QBSaveData saveData;

        public static void Load()
        {
            try
            {
                string saveString = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.QueueBits);
                saveData = JsonUtility.FromJson<QBSaveData>(saveString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (saveData == null)
            {
                saveData = new QBSaveData();
                Save();
            }
        }

        public static void Save()
        {
            Wrapper.Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.QueueBits, saveData);
            Debug.Log("DataSaved!");
        }
    }
}
