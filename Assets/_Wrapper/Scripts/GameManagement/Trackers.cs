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
        [SerializeField] private StarTracker starTotal;        
        
        [SerializeField] private StarTracker starQupcakery;
        [SerializeField] private StarTracker starTwinTanglement;
        [SerializeField] private StarTracker starTanglesLair;
        [SerializeField] private StarTracker starQueueBits;
        [SerializeField] private StarTracker starBuriedTreasure;
        
        [SerializeField] private StarTracker starChallenge;

        
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
        private Game[] games = new Game[] {
            Game.Qupcakes, Game.Labyrinth, Game.Circuits, Game.QueueBits, Game.BlackBox
        };



        private void OnEnable()
        {
            Events.GetGameUnlocked += CheckUnlocked;
            // Events.InitializeStarTracker += InitStarTracker;
            Events.InitializeStarTracker += DelayInitTrackers;
            Events.ResetStarCounts += ResetStarCounts;
        }

        private void OnDisable()
        {
            Events.GetGameUnlocked -= CheckUnlocked;
            // Events.InitializeStarTracker -= InitStarTracker;
            Events.InitializeStarTracker -= DelayInitTrackers;
            Events.ResetStarCounts -= ResetStarCounts;
        }


        // Checks if game is unlocked, and if it is, changes display on panel
        public bool CheckUnlocked(Game game) {
#if LITE_VERSION
            switch(game) {
                case Game.Qupcakes: return true;
                case Game.Labyrinth: return true;
                case Game.Circuits:
                    if (starQupcakery.starsWon >= 12) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                        starTanglesLair.SetGameUnlocked(true);
                        return true;
                    } break;

                case Game.QueueBits:
                    // No pop-up
                    starQueueBits.SetGameUnlocked(true);
                    return true;
                    break;

                case Game.BlackBox:
                    RecountTotal();
                    if (starTotal.starsWon >= 50) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                        starBuriedTreasure.SetGameUnlocked(true);
                        return true;
                    } break;

                default:
                    return false;
            }
#else
            switch(game) {
                case Game.Qupcakes: return true;
                case Game.Labyrinth: return true;
                case Game.Circuits:
                    if (starQupcakery.starsWon >= 27) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                        starTanglesLair.SetGameUnlocked(true);
                        return true;
                    } break;

                case Game.QueueBits:
                    if (starQupcakery.starsWon >= 10 
                    && starTwinTanglement.starsWon >= 10) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.QueueBits);
                        starQueueBits.SetGameUnlocked(true);
                        return true;
                    } break;

                case Game.BlackBox:
                    RecountTotal();
                    if (starTotal.starsWon >= 120) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                        starBuriedTreasure.SetGameUnlocked(true);
                        return true;
                    } break;

                default:
                    return false;
            }
#endif
            return false;
        }



        // Initializes or updates trackers to reflect number of stars in data
        // Needs a slight delay to allow the user save to load when logging in
        public void DelayInitTrackers() {
            Invoke("InitStarTracker", 0.1f);
        }

        public void InitStarTracker() {
#if LITE_VERSION
            ResetStarCounts();
#else
            foreach (Game minigame in games) {
                initMinigameStars(minigame);
            }
            RecountTotal();
#endif
            Events.InitializeMap?.Invoke();
        }


        // Loads user save to retrieve total stars from each game
        // Each game has slightly different notation for their saves
        private void initMinigameStars(Game game) {
            switch(game) {
                case Game.Qupcakes:
                    string data_QC = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Qupcakes);
                    Qupcakery.GameData data2_QC = JsonUtility.FromJson<Qupcakery.GameData>(data_QC);
                    if (data2_QC != null) {
                        starQupcakery.SetStarDisplay(data2_QC.TotalStars);
                    } return;

                case Game.Labyrinth:
                    string data_TT = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Labyrinth);
                    Labyrinth.TTSaveData data2_TT = JsonUtility.FromJson<Labyrinth.TTSaveData>(data_TT);
                    if (data2_TT != null) {
                        starTwinTanglement.SetStarDisplay(data2_TT.TotalStars);
                    } return;
                
                case Game.Circuits:
                    string data_TL = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Circuits);
                    Circuits.Circuits_SaveData data2_TL = JsonUtility.FromJson<Circuits.Circuits_SaveData>(data_TL);
                    if (data2_TL != null) {
                        CheckUnlocked(Game.Circuits);
                        starTanglesLair.SetStarDisplay(data2_TL.totalStars);
                    } return;
                
                case Game.QueueBits:
                    string data_QB = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.QueueBits);
                    QueueBits.QBSaveData data2_QB = JsonUtility.FromJson<QueueBits.QBSaveData>(data_QB);
                    if (data2_QB != null) {
                        CheckUnlocked(Game.QueueBits);
                        starQueueBits.SetStarDisplay(data2_QB.totalStars);
                    } return;

                case Game.BlackBox:
                    string data_BT = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.BlackBox);
                    BlackBox.BBSaveData data2_BT = JsonUtility.FromJson<BlackBox.BBSaveData>(data_BT);
                    if (data2_BT != null) {
                        CheckUnlocked(Game.BlackBox);
                        starBuriedTreasure.SetStarDisplay(data2_BT.totalStars);
                    } return;

                default: return;
            }
        }


        // Resets all the counts to be zero for when NewGame button pressed
        public void ResetStarCounts() {
            starQupcakery.ResetStarDisplay();
            starTwinTanglement.ResetStarDisplay();
            starTanglesLair.ResetStarDisplay();
            starQueueBits.ResetStarDisplay();
            starBuriedTreasure.ResetStarDisplay();
            
            starTotal.ResetStarDisplay();
            totalStars = 0;
            totalStarsTMP.text = "0";
        }


        // Updates the total star count on panel and main screen
        public void RecountTotal() {
            int i = 0;
            i += starQupcakery.starsWon;
            i += starTwinTanglement.starsWon;
            i += starTanglesLair.starsWon;
            i += starQueueBits.starsWon;
            i += starBuriedTreasure.starsWon;
            
            starTotal.SetStarDisplay(i);
            totalStars = i;
            totalStarsTMP.text = $"{i}";
        }


        // Toggles the panel with star counts
        public void ToggleStarPanel() {
            panelVisible = !panelVisible;
            trackerAnimator.SetBool("ShowPanel", panelVisible);
        }

    }    
}