using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Wrapper 
{
    public class StarTracker : MonoBehaviour {

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

        // Total stars per each game
        private Dictionary<Game, int> starsPerGame = new Dictionary<Game, int>() {
            {Game.BlackBox, 0},     // max 0? maybe make 15 or 45
            {Game.Circuits, 0},     // max 25 now, soon 75
            {Game.Labyrinth, 0},    // max 45
            {Game.QueueBits, 0},    // max 45
            {Game.Qupcakes, 0}      // max 81
        };
        private Game[] games = new Game[] {
            Game.BlackBox, Game.Circuits, Game.Labyrinth, Game.QueueBits, Game.Qupcakes
        };

        public void PrintDict() {
            foreach (Game game in starsPerGame.Keys) {
                Debug.Log($"{game}: {starsPerGame[game]}");
            }
        }

        // GameObject that displays the star count
        public TMP_Text scoreNumber; 

        // Function to Initialize the StarTracker, called during LoginRoutine in SaveManager
        public void InitStarTracker() {
            InitTTStars();
            InitQCStars();

            ResetStarDisplay();
            // PrintDict();
        }

        // Reset the TMPro Asset that displays the count
        public void ResetStarDisplay() {
            int i = 0;
            foreach (Game g in starsPerGame.Keys) {
                i += starsPerGame[g];
            }
            scoreNumber.text = $"{i}";
        }

        // Resets all the counts to be zero for when NewGame button pressed
        public void ResetStarCounts() {
            foreach (Game g in games) {
                starsPerGame[g] = 0;
            }
            scoreNumber.text = "0";
        }

        // Sets private Dictionary values and updates display
        public void UpdateStarTracker(Game game, int i) {
            starsPerGame[game] = i;
            ResetStarDisplay();
        }


        /* Getting TotalStars from each game */

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
            // Debug.Log($"QC: {string.Join(",", data2.levelPerformance)}");
            // Debug.Log($"QC: {data2.TotalStars}");
        }
        
    }
}