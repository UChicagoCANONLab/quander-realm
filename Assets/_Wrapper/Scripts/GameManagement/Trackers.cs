using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        [SerializeField] private TMP_Text totalStarsTMP;
        private int totalStars;
        [SerializeField] private TMP_Text totalCoinsTMP;
        private int totalCoins;
        [SerializeField] private TMP_Text streakLengthTMP;
        private int streakLength;
        [SerializeField] private GameObject fire;
        [SerializeField] private GameObject lanternFront;


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
            totalStars = 0;
            totalStarsTMP.text = "0";
        }

        public void OnUpdateStreakLength(long streak)
        {
            //Debug.Log("Updating streak length");
            bool active = streak > 0;
            if(active){
                lanternFront.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                fire.SetActive(true);
            }
            else{
                lanternFront.GetComponent<SpriteRenderer>().color = new Color(77, 77, 77);
                fire.SetActive(false);
            }
            Debug.Log(fire.activeSelf);
            if (active) {
                streakLengthTMP.text = streak.ToString();
            }
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

        // Toggles all tracker visibility
        public void ToggleTrackers(bool isOn) {
            trackerAnimator.SetBool("IsOn", isOn);
            active = isOn;
        }

    }
}