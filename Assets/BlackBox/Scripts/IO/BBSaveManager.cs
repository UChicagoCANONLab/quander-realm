using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeauRoutine;
using Wrapper;

namespace BlackBox 
{
    public class BBSaveManager : MonoBehaviour
    {

        public BBSaveData saveData;
        public BBResearchData researchData;
        
        public static BBSaveManager Instance;

        public void LoadGame() 
        {
            try
            {
                string saveString = Events.GetMinigameSaveData?.Invoke(Game.BlackBox);
                saveData = JsonUtility.FromJson<BBSaveData>(saveString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if ((saveData == null) || saveData.starsPerLevel.Length < 24) {
                saveData = new BBSaveData();
            }
            int temp = 0;
            foreach (int i in saveData.starsPerLevel) { temp += i; }
            saveData.totalStars = temp;

            researchData = new BBResearchData();
        }



        public void SaveGame()
        {
            researchData.BBSaveDataString = JsonUtility.ToJson(saveData);

            Events.UpdateMinigameSaveData?.Invoke(Game.BlackBox, saveData);
            Events.SaveMinigameResearchData?.Invoke(Game.BlackBox, researchData);
        }

    }
}