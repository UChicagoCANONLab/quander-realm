using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Wrapper 
{
    public class StarTracker : MonoBehaviour {

        private static StarTracker ST;

        public Dictionary<Game, int> starsPerGame = new Dictionary<Game, int>() {
            {Game.BlackBox, 0},     // max 0? maybe make 15 or 45
            {Game.Circuits, 0},     // max 25 now, soon 75
            {Game.Labyrinth, 0},    // max 45
            {Game.QueueBits, 0},    // max 45
            {Game.Qupcakes, 0}      // max 81
        };

        private TMP_Text scoreNumber; 

        public void ResetStarCounter() {
            int i = 0;
            foreach (Game g in starsPerGame.Keys) {
                i += starsPerGame[g];
            }
            scoreNumber.text = $"{i}";
        }

        public void updateScoreTracker(Game game, int newStars) {
            starsPerGame[game] += newStars;
        }
        
    }
}