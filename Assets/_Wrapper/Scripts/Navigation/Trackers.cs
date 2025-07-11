using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Wrapper
{
    public class Trackers : MonoBehaviour
    {

        [Header("StarTrackers on Panel")]
        [SerializeField] private StarTracker tracker_Overall;
        // In order: {Blackbox, Circuits, Labyrinth, Queuebits, Qupcakes}
        [SerializeField] private StarTracker[] tracker_Minigames;
        [SerializeField] private StarTracker tracker_Challenge;

        [Header("General Trackers at top of screen")]
        [SerializeField] private GeneralTrackers trackerPanel;

        [Header("Animator")]
        [SerializeField] private Animator trackerAnimator;


        private bool panelVisible = false;
        private Game[] gamesArray = new Game[] {
            Game.Qupcakes, Game.Labyrinth, Game.Circuits, Game.QueueBits, Game.BlackBox
        };
        public bool active = true;



        private void OnEnable()
        {
            Events.InitializeStarTracker += DelayInitTrackers;
            // Events.UpdateStreakLength += OnUpdateStreakLength;
            Events.ResetStarCounts += ResetStarCounts;
        }

        private void OnDisable()
        {
            Events.InitializeStarTracker -= DelayInitTrackers;
            // Events.UpdateStreakLength -= OnUpdateStreakLength;
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
            // for (int i=0; i<5; i++) {
            //     initMinigameStarDisplay((Game)i);
            // }
            foreach (Game minigame in gamesArray) {
                initMinigameStarDisplay(minigame);
            }
            trackerPanel.UpdateDisplay();
            RecountTotal();
            // UpdateCoinTracker();
#endif
            Events.InitializeMap?.Invoke();
        }

        public void UpdateTrackers()
        {
            Events.LoadAllMinigameSaves?.Invoke();

            foreach (Game minigame in gamesArray) {
                initMinigameStarDisplay(minigame);
            }
            trackerPanel.UpdateDisplay();
            RecountTotal();
        }


        private void initMinigameStarDisplay(Game game)
        {
            int totalStars = Events.GetMinigameTotalStars.Invoke(game);
            bool gameUnlocked = Events.GetGameUnlocked.Invoke(game);

            tracker_Minigames[(int)game].SetStarDisplay(totalStars);
            tracker_Minigames[(int)game].SetGameUnlocked(gameUnlocked);
        }


        // Resets all the counts to be zero for when NewGame button pressed
        public void ResetStarCounts() {
            foreach(StarTracker tracker in tracker_Minigames) {
                tracker.ResetStarDisplay();
            }
            tracker_Overall.ResetStarDisplay();
            tracker_Challenge.ResetStarDisplay();

            trackerPanel.ResetDisplay();
        }

        /* public void UpdateCoinTracker()
        {
            int totalCoins = Events.GetUserSaveTotalCoins.Invoke();
            trackerPanel.SetCoins(totalCoins);
        } */

        /* public void OnUpdateStreakLength(long streak)
        {
            trackerPanel.SetStreak((int)streak);
            //Debug.Log("Updating streak length");
            // MOVED TO trackerPanel
            // bool active = streak > 0;
            // if(active){
            //     lanternFront.color = new Color(1f, 1f, 1f);
            //     fire.SetActive(true);
            // }
            // else{
            //     lanternFront.color = new Color(0.3f, 0.3f, 0.3f);
            //     fire.SetActive(false);
            // }
            // if (active) {
            //     streakLengthTMP.text = streak.ToString();
            // }
        } */

        // Updates the total star count on panel and main screen
        public void RecountTotal() {
            tracker_Challenge.SetStarDisplay(Events.GetMinigameStarCount.Invoke(Game.Rewards));
            int totalStars = Events.GetOverallTotalStars.Invoke();
            tracker_Overall.SetStarDisplay(totalStars);
            // trackerPanel.SetStars(totalStars); 
        }


        // Toggles the panel with star counts
        public void ToggleStarPanel() {
            panelVisible = !panelVisible;
            trackerAnimator.SetBool("ShowPanel", panelVisible);
        }

        // Toggles all tracker visibility
        public void ToggleTrackers(bool isOn) {
            if (isOn) UpdateTrackers();
            else {
                if (trackerAnimator.GetBool("ShowPanel")) ToggleStarPanel();
            }
            trackerAnimator.SetBool("IsOn", isOn);
            active = isOn;
        }

    }
}