using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Wrapper 
{
    public class StarTracker : MonoBehaviour 
    {
        public int TotalStars; // total possible is 291

        // Set up Singleton for StarTracker
        public static StarTracker ST;
        private void Awake() {
            if (ST != null) {
                Destroy(gameObject);
                return;
            }
            ST = this;
            DontDestroyOnLoad(gameObject);
        }

        /* 
        Dictionary and helper methods 
        */

        // Total stars per each game
        public Dictionary<Game, int> starsPerGame = new Dictionary<Game, int>() {
            {Game.BlackBox, 0},     // max 45
            {Game.Circuits, 0},     // max 75
            {Game.Labyrinth, 0},    // max 45
            {Game.QueueBits, 0},    // max 45
            {Game.Qupcakes, 0}      // max 81
        };
        private Game[] games = new Game[] {
            Game.BlackBox, Game.Circuits, Game.Labyrinth, Game.QueueBits, Game.Qupcakes
        };
        public void PrintDict() {
            foreach (Game game in games) {
                Debug.Log($"{game}: {starsPerGame[game]}; Unlocked: {gameUnlocked[game]}");
            }
            Debug.Log("-----");
        }

        /* 
        Game unlock status 
        */
        public Dictionary<Game, bool> gameUnlocked = new Dictionary<Game, bool>() {
            {Game.BlackBox, false},     // max 45
            {Game.Circuits, false},     // max 75
            {Game.Labyrinth, true},    // max 45
            {Game.QueueBits, false},    // max 45
            {Game.Qupcakes, true}      // max 81
        };

        /* 
        Function to check if game meets unlocked status
        Different for LITE_VERSION and full version
        */

#if LITE_VERSION
        public bool CheckUnlocked(Game game) {
            if (gameUnlocked[game] == true) {
                return true;
            }
            else if (game == Game.Circuits 
            && starsPerGame[Game.Qupcakes] >= 12) { // NORMAL SETTING
                Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                gameUnlocked[Game.Circuits] = true;
                return true;
            }
            else if (game == Game.QueueBits) { // No criteria for this one
                // No pop-up
                gameUnlocked[Game.QueueBits] = true;
                return true;
            }
            // else if (game == Game.BlackBox
            // && TotalStars >= 50) { // NORMAL SETTING
            else if (game == Game.BlackBox) { // Temporary Setting for quantum class
                Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                gameUnlocked[Game.BlackBox] = true;
                return true;
            }
            else { return false; }
        }
#else
        public bool CheckUnlocked(Game game) {
            if (gameUnlocked[game] == true) {
                return true;
            }
            else if (game == Game.Circuits 
            && starsPerGame[Game.Qupcakes] >= 27) { // NORMAL SETTING
                Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                gameUnlocked[Game.Circuits] = true;
                return true;
            }
            else if (game == Game.QueueBits
            && starsPerGame[Game.Qupcakes] >= 10
            && starsPerGame[Game.Labyrinth] >= 10) {
                Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.QueueBits);
                gameUnlocked[Game.QueueBits] = true;
                return true;
            }
            else if (game == Game.BlackBox
            && TotalStars >= 120) {
                Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                gameUnlocked[Game.BlackBox] = true;
                return true;
            }
            else { return false; }
        }
#endif

        // GameObject that displays the star count
        public TMP_Text scoreNumber; 

        // Function to Initialize the StarTracker, called during LoginRoutine in SaveManager

        public void InitStarTracker_Lite() {
            ResetStarCounts();
            GameObject.Find("MapCanvas/MapPanel").GetComponent<MapManager>().InitMap();
        }

        public void InitStarTracker() {
            InitTTStars();
            InitQCStars();
            InitTLStars();
            InitQBStars();
            InitBTStars();

            ResetStarDisplay();
            GameObject.Find("MapCanvas/MapPanel").GetComponent<MapManager>().InitMap();
            // PrintDict();
        }


        /* 
        Updating star display and dictionary 
        */

        // Reset the TMPro Asset that displays the count
        public void ResetStarDisplay() {
            int i = 0;
            foreach (Game g in starsPerGame.Keys) {
                i += starsPerGame[g];
            }
            TotalStars = i;
            scoreNumber.text = $"{i}";
        }

        // Resets all the counts to be zero for when NewGame button pressed
        public void ResetStarCounts() {
            foreach (Game g in games) {
                starsPerGame[g] = 0;
            }
            TotalStars = 0;
            scoreNumber.text = "0";

            gameUnlocked[Game.BlackBox] = false;
            gameUnlocked[Game.Circuits] = false;
            gameUnlocked[Game.QueueBits] = false;
        }

        // Sets private Dictionary values and updates display
        public void UpdateStarTracker(Game game, int i) {
            starsPerGame[game] = i;
            ResetStarDisplay();
        }

        /* 
        Loading TotalStars from each game 
        */

        // TwinTanglement
        private void InitTTStars() {
            string data = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Labyrinth);
            Labyrinth.TTSaveData data2 = JsonUtility.FromJson<Labyrinth.TTSaveData>(data);
            if (data2 != null) {
                UpdateStarTracker(Game.Labyrinth, data2.TotalStars);
            }
        }
        
        // QupCakery
        private void InitQCStars() {
            string data = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Qupcakes);
            Qupcakery.GameData data2 = JsonUtility.FromJson<Qupcakery.GameData>(data);
            if (data2 != null) {
                UpdateStarTracker(Game.Qupcakes, data2.TotalStars);   
            }
        }

        // Tangle's Lair
        private void InitTLStars() {
            string data = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Circuits);
            Circuits.Circuits_SaveData data2 = JsonUtility.FromJson<Circuits.Circuits_SaveData>(data);
            if (data2 != null) {
                UpdateStarTracker(Game.Circuits, data2.totalStars);
                gameUnlocked[Game.Circuits] = CheckUnlocked(Game.Circuits);
            }
        }

        // QueueBits
        private void InitQBStars() {
            string data = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.QueueBits);
            QueueBits.QBSaveData data2 = JsonUtility.FromJson<QueueBits.QBSaveData>(data);
            if (data2 != null) {
                UpdateStarTracker(Game.QueueBits, data2.totalStars);
                gameUnlocked[Game.QueueBits] = CheckUnlocked(Game.QueueBits);
            }
        }

        // Buried Treasure
        private void InitBTStars() {
            string data = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.BlackBox);
            BlackBox.BBSaveData data2 = JsonUtility.FromJson<BlackBox.BBSaveData>(data);
            if (data2 != null) {
                UpdateStarTracker(Game.BlackBox, data2.totalStars);
                gameUnlocked[Game.BlackBox] = CheckUnlocked(Game.BlackBox);
            }
        }
        
    }
}