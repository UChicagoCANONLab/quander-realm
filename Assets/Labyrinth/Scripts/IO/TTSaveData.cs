using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth 
{ 
    [System.Serializable]
    public class TTSaveData {

        public int[] MaxStarsPerLevel;
        public int Level;
        public int NumStars;
        public float Time;
        public int HintsUsed;
        public bool Winner;
        
        public TTSaveData(SaveData save) {
            MaxStarsPerLevel = save.starsPerLevel;
            // TimePlayed = save.timePerLevel;
            //PreviousSave = save.previousSave;
            Level = save.level;
            NumStars = save.numStars;
            Time = save.time;
            HintsUsed = save.hintsUsed;
            Winner = save.winner;

        }

        public Wrapper.Game gameID = Wrapper.Game.Labyrinth;
        public string currentLevelID = string.Empty;
        // public bool[] tutorialsSeen = new bool[] { false, false, false, false, false };

    }
}