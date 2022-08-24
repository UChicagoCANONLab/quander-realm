using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Labyrinth 
{ 
    // [System.Serializable]
    public class SaveData : MonoBehaviour {
        
        // public int levelUnlocked; //furthest level the player got to (init 1)

        // Helps the game run
        public int Degree;
        public int CurrentLevel; //current level the user is playing
        
        // Dialogue needed per level offered
        public Dictionary<int, bool> levelDialogue = new Dictionary<int, bool>()
        { {-1, true}, {0, true}, {5, true}, {6, true}, {10, true}, {11, true}, {15, true} };
        // public bool NeedTutorial1 = false; //turns off once they receive intro
        // public bool NeedTutorial2 = false; //turns off once they receive tutorial

        // What we're actually saving as research data
        public int[] starsPerLevel; //15 levels, all w 0-3 stars
        // public float[] timePerLevel; //how long each level took in seconds

        public int level; 
        public int numStars;
        public float time;
        public int hintsUsed;
        public bool winner = false;
        // public object[] previousSave; // {level #, stars, time, hints used #}
        
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
            if (CurrentLevel == 0) {
                return;
            }
            else if (gb.numStars > starsPerLevel[CurrentLevel - 1]) {
                starsPerLevel[CurrentLevel - 1] = gb.numStars;
            }
            /* if (gb.timePlayed > timePerLevel[CurrentLevel - 1]) {
                timePerLevel[CurrentLevel - 1] = gb.timePlayed;
            } */
            level = CurrentLevel;
            numStars = gb.numStars;
            time = gb.timePlayed;
            hintsUsed = gb.hintsUsed;
            
            //previousSave = new object[] {CurrentLevel, gb.numStars, gb.timePlayed, gb.hintsUsed};
        }

    }
}