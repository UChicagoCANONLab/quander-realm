using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data {

    public int[] StarsWon;
    // public float[] TimePlayed;
    public object[] PreviousSave; // {level #, stars, time, hints used #}
    
    public Data(SaveData save) {
        StarsWon = save.starsPerLevel;
        // TimePlayed = save.timePerLevel;
        PreviousSave = save.previousSave;
    }
}
