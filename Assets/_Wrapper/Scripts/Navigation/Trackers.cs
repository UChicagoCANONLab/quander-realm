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
        // [SerializeField] private TMP_Text totalStarsTMP;
        // private int totalStars;
        // [SerializeField] private TMP_Text totalCoinsTMP;
        // private int totalCoins;
        // [SerializeField] private TMP_Text streakLengthTMP;
        // private int streakLength;
        // [SerializeField] private GameObject fire;
        // [SerializeField] private Image lanternFront;


        [Header("Animator")]
        [SerializeField] private Animator trackerAnimator;

        private bool panelVisible = false;
        private Game[] gamesArray = new Game[] {
            Game.Qupcakes, Game.Labyrinth, Game.Circuits, Game.QueueBits, Game.BlackBox
        };
        private bool active = true;



        private void OnEnable()
        {
            Events.InitializeStarTracker += DelayInitTrackers;
            Events.UpdateStreakLength += OnUpdateStreakLength;
            Events.ResetStarCounts += ResetStarCounts;
        }

        private void OnDisable()
        {
            Events.InitializeStarTracker -= DelayInitTrackers;
            Events.UpdateStreakLength -= OnUpdateStreakLength;
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
            RecountTotal();
            UpdateCoinTracker();
#endif
            Events.InitializeMap?.Invoke();
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

            trackerPanel.SetStars(0);
            // totalStars = 0;
            // totalStarsTMP.text = "0";

            trackerPanel.SetCoins(0);
            // totalCoins = 0;
            // totalCoinsTMP.text = "0";
        }

        public void UpdateCoinTracker()
        {
            int totalCoins = Events.GetUserSaveTotalCoins.Invoke();
            trackerPanel.SetCoins(totalCoins);
            // totalCoinsTMP.text = $"{totalCoins}";
        }

        public void OnUpdateStreakLength(long streak)
        {
            trackerPanel.SetStreak((int)streak);
            //Debug.Log("Updating streak length");
            /* bool active = streak > 0;
            if(active){
                lanternFront.color = new Color(1f, 1f, 1f);
                fire.SetActive(true);
            }
            else{
                lanternFront.color = new Color(0.3f, 0.3f, 0.3f);
                fire.SetActive(false);
            }
            if (active) {
                streakLengthTMP.text = streak.ToString();
            } */
        }

        // Updates the total star count on panel and main screen
        public void RecountTotal() {
            tracker_Challenge.SetStarDisplay(Events.GetMinigameStarCount.Invoke(Game.Rewards));
            int totalStars = Events.GetOverallTotalStars.Invoke();
            tracker_Overall.SetStarDisplay(totalStars);
            trackerPanel.SetStars(totalStars);
            // totalStarsTMP.text = $"{totalStars}";
        }


        // Toggles the panel with star counts
        public void ToggleStarPanel() {
            panelVisible = !panelVisible;
            trackerAnimator.SetBool("ShowPanel", panelVisible);
        }

        // Toggles all tracker visibility
        public void ToggleTrackers(bool isOn) {
            trackerAnimator.SetBool("IsOn", isOn);
            active = isOn;
        }

    }
}