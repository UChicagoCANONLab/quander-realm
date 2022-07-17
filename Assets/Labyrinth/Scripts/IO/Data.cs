using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth 
{ 
        
    public class Data {

        public int[] MaxStarsPerLevel;
        // public float[] TimePlayed;
        public int Level;
        public int NumStars;
        public float Time;
        public int HintsUsed;
        // public object[] PreviousSave; // {level #, stars, time, hints used #}
        
        public Data(SaveData save) {
            MaxStarsPerLevel = save.starsPerLevel;
            // TimePlayed = save.timePerLevel;
            //PreviousSave = save.previousSave;
            Level = save.level;
            NumStars = save.numStars;
            Time = save.time;
            HintsUsed = save.hintsUsed;

        }
    }
}