using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

// Firebase non-research load/save data

namespace Labyrinth 
{ 
    [System.Serializable]
    public class TTSaveData {

        public Wrapper.Game gameID = Wrapper.Game.Labyrinth;

        public int[] MaxStarsPerLevel = new int[15];
        public bool[] DialogueSeen = new bool[7];
        public int TotalStars = 0;
        public int MaxLevelUnlocked = 1;

        public void UpdateTTSaveData(SaveData save) {
            MaxStarsPerLevel = save.starsPerLevel;
            DialogueSeen = save.dialogueSeen;
            TotalStars = save.totalStars;
            MaxLevelUnlocked = save.MaxLevelUnlocked;
            // Debug.Log("TTSD: " + TTSDtoString());
        }

        public string TTSDtoString() {
            string jsonData = JsonUtility.ToJson(this);
            return jsonData;
        }
    }
}