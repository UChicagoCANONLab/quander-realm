using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Wrapper 
{
    public class Trackers : MonoBehaviour 
    {

        [Header("StarTrackers on Panel")]
        [SerializeField] private StarTracker tracker_Overall;        
        
        [SerializeField] private StarTracker tracker_Qupcakery;
        [SerializeField] private StarTracker tracker_Twintanglement;
        [SerializeField] private StarTracker tracker_TanglesLair;
        [SerializeField] private StarTracker tracker_Queuebits;
        [SerializeField] private StarTracker tracker_BuriedTreasure;
        
        [SerializeField] private StarTracker tracker_Challenge;

        
        [Header("General Trackers at top of screen")]
        [SerializeField] private TMP_Text totalStarsTMP; 
        private int totalStars; 
        [SerializeField] private TMP_Text totalCoinsTMP;
        private int totalCoins;
        [SerializeField] private TMP_Text streakLengthTMP;
        private int streakLength;

        
        [Header("Animator")]
        [SerializeField] private Animator trackerAnimator;
        
        private bool panelVisible = false;
        private Game[] gamesArray = new Game[] {
            Game.Qupcakes, Game.Labyrinth, Game.Circuits, Game.QueueBits, Game.BlackBox
        };



        private void OnEnable()
        {
            // Events.GetGameUnlocked += CheckUnlocked;
            // Events.InitializeStarTracker += InitStarTracker;
            Events.InitializeStarTracker += DelayInitTrackers;
            Events.ResetStarCounts += ResetStarCounts;
        }

        private void OnDisable()
        {
            // Events.GetGameUnlocked -= CheckUnlocked;
            // Events.InitializeStarTracker -= InitStarTracker;
            Events.InitializeStarTracker -= DelayInitTrackers;
            Events.ResetStarCounts -= ResetStarCounts;
        }


        // Initializes or updates trackers to reflect number of stars in data
        // Needs a slight delay to allow the user save to load when logging in
        public void DelayInitTrackers() {
            Invoke("InitStarTracker", 0.1f);
        }
        public void InitStarTracker() {
            Events.LoadAllMinigameSaves?.Invoke();
#if LITE_VERSION
            ResetStarCounts();
#else
            foreach (Game minigame in gamesArray) {
                initMinigameStarDisplay(minigame);
            }
            RecountTotal();
#endif
            Events.InitializeMap?.Invoke();
        }


        private void initMinigameStarDisplay(Game game) 
        {
            int totalStars = Events.GetMinigameTotalStars.Invoke(game); 
            bool gameUnlocked = Events.GetGameUnlocked.Invoke(game);

            switch(game) 
            {
                case Game.Qupcakes:
                    tracker_Qupcakery.SetStarDisplay(totalStars);
                    // Unlocked by default
                    break;
                case Game.Labyrinth:
                    tracker_Twintanglement.SetStarDisplay(totalStars);
                    // Unlocked by default
                    break;
                case Game.Circuits:
                    tracker_TanglesLair.SetStarDisplay(totalStars);
                    tracker_TanglesLair.SetGameUnlocked(gameUnlocked);
                    break;
                case Game.QueueBits:
                    tracker_Queuebits.SetStarDisplay(totalStars);
                    tracker_Queuebits.SetGameUnlocked(gameUnlocked);
                    break;
                case Game.BlackBox:
                    tracker_BuriedTreasure.SetStarDisplay(totalStars);
                    tracker_BuriedTreasure.SetGameUnlocked(gameUnlocked);
                    break;
                default: return;
            }
        }


        // Resets all the counts to be zero for when NewGame button pressed
        public void ResetStarCounts() {
            tracker_Qupcakery.ResetStarDisplay();
            tracker_Twintanglement.ResetStarDisplay();
            tracker_TanglesLair.ResetStarDisplay();
            tracker_Queuebits.ResetStarDisplay();
            tracker_BuriedTreasure.ResetStarDisplay();
            
            tracker_Overall.ResetStarDisplay();
            totalStars = 0;
            totalStarsTMP.text = "0";
        }


        // Updates the total star count on panel and main screen
        public void RecountTotal() {
            totalStars = Events.GetOverallTotalStars.Invoke();
            tracker_Overall.SetStarDisplay(totalStars);
            totalStarsTMP.text = $"{totalStars}";
        }


        // Toggles the panel with star counts
        public void ToggleStarPanel() {
            panelVisible = !panelVisible;
            trackerAnimator.SetBool("ShowPanel", panelVisible);
        }

    }    
}