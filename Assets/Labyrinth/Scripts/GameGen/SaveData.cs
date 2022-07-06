using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


// [System.Serializable]
public class SaveData : MonoBehaviour {
    
    // public int levelUnlocked; //furthest level the player got to (init 1)

    // Helps the game run
    public int Degree;
    public int CurrentLevel; //current level the user is playing
    
    // What we're actually saving as research data
    public int[] starsPerLevel; //15 levels, all w 0-3 stars
    // public float[] timePerLevel; //how long each level took in seconds

    public object[] previousSave; // {level #, stars, time, hints used #}
    
    public static SaveData Instance;

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /* public void init() {
        levelUnlocked = 1;

        for (int i=0; i<15; i++) {
            starsPerLevel[i] = 0;
            timePerLevel[i] = 0f;
        }
    } */

    public void updateSave(GameBehavior gb) {
        /* if (CurrentLevel == levelUnlocked) {
            levelUnlocked = CurrentLevel + 1;
        } */
        if (gb.numStars > starsPerLevel[CurrentLevel - 1]) {
            starsPerLevel[CurrentLevel - 1] = gb.numStars;
        }
        /* if (gb.timePlayed > timePerLevel[CurrentLevel - 1]) {
            timePerLevel[CurrentLevel - 1] = gb.timePlayed;
        } */
        previousSave = new object[] {CurrentLevel, gb.numStars, gb.timePlayed, gb.hintsUsed};
    }

}
