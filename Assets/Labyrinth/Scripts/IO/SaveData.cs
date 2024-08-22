using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Labyrinth 
{ 
    // [System.Serializable]
    public class SaveData : MonoBehaviour {
        
        // Helps the game run
        public int Degree;
        public int CurrentLevel; //current level the user is playing

        [Header("Non-Research Data")]
        public int[] starsPerLevel; //15 levels, all w 0-3 stars
        public bool[] dialogueSeen;
        public int totalStars;
        public int MaxLevelUnlocked = 1;

        [Header("Research Data")]
        public int level; 
        public int numStars;
        public float time;
        public int hintsUsed;
        public bool winner = false;

        // Instance        
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

        public void updateSave(GameBehavior GB) {
            /* if (CurrentLevel == levelUnlocked) {
                levelUnlocked = CurrentLevel + 1;
            } */
            /* if (CurrentLevel == 0) {
                // DialogueAndRewards.Instance.updateDialogueDict();
                return;
            }
            else  */
            if (CurrentLevel == 0) { return; }

            if (GB.numStars > starsPerLevel[CurrentLevel - 1]) {
                totalStars += (GB.numStars - starsPerLevel[CurrentLevel-1]);
                starsPerLevel[CurrentLevel - 1] = GB.numStars;
            }

            int i=0;
            foreach(bool item in DialogueAndRewards.Instance.levelDialogue.Values) {
                dialogueSeen[i] = item;
                i++;
            }
            // MaxLevelUnlocked incremented at ButtonBehavior line 107

            level = CurrentLevel;
            numStars = GB.numStars;
            time = GB.timePlayed;
            hintsUsed = GB.hintsUsed;
        }

        public string SDtoString() {
            string jsonData = JsonUtility.ToJson(this);
            return jsonData;
        }

    }
}