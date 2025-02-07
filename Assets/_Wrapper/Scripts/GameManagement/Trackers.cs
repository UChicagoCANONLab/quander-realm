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


        /* // Checks if game is unlocked, and if it is, changes display on panel
        public bool CheckUnlocked(Game game) {
#if LITE_VERSION
            switch(game) {
                case Game.Qupcakes: return true;
                case Game.Labyrinth: return true;
                case Game.Circuits:
                    if (tracker_Qupcakery.starsWon >= 12) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                        tracker_TanglesLair.SetGameUnlocked(true);
                        return true;
                    } break;

                case Game.QueueBits:
                    // No pop-up
                    tracker_Queuebits.SetGameUnlocked(true);
                    return true;
                    break;

                case Game.BlackBox:
                    RecountTotal();
                    if (tracker_Overall.starsWon >= 50) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                        tracker_BuriedTreasure.SetGameUnlocked(true);
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
                    if (tracker_Qupcakery.starsWon >= 27) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                        tracker_TanglesLair.SetGameUnlocked(true);
                        return true;
                    } break;

                case Game.QueueBits:
                    if (tracker_Qupcakery.starsWon >= 10 
                    && tracker_Twintanglement.starsWon >= 10) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.QueueBits);
                        tracker_Queuebits.SetGameUnlocked(true);
                        return true;
                    } break;

                case Game.BlackBox:
                    RecountTotal();
                    if (tracker_Overall.starsWon >= 120) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                        tracker_BuriedTreasure.SetGameUnlocked(true);
                        return true;
                    } break;

                default:
                    return false;
            }
#endif
            return false;
        } */


        // Loads user save to retrieve total stars from each game
        // Each game has slightly different notation for their saves
        /* private void initMinigameStarDisplay(Game game) {
            int tempStars = 0;
            switch(game) {
                case Game.Qupcakes:
                    string data_QC = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Qupcakes);
                    Qupcakery.GameData data2_QC = JsonUtility.FromJson<Qupcakery.GameData>(data_QC);
                    if (data2_QC != null) {
                        tracker_Qupcakery.SetStarDisplay(data2_QC.TotalStars);
                    } return;

                case Game.Labyrinth:
                    string data_TT = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Labyrinth);
                    Labyrinth.TTSaveData data2_TT = JsonUtility.FromJson<Labyrinth.TTSaveData>(data_TT);
                    if (data2_TT != null) {
                        tracker_Twintanglement.SetStarDisplay(data2_TT.TotalStars);
                    } return;
                
                case Game.Circuits:
                    string data_TL = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Circuits);
                    Circuits.Circuits_SaveData data2_TL = JsonUtility.FromJson<Circuits.Circuits_SaveData>(data_TL);
                    if (data2_TL != null) {
                        CheckUnlocked(Game.Circuits);
                        tracker_TanglesLair.SetStarDisplay(data2_TL.totalStars);
                    } return;
                
                case Game.QueueBits:
                    string data_QB = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.QueueBits);
                    QueueBits.QBSaveData data2_QB = JsonUtility.FromJson<QueueBits.QBSaveData>(data_QB);
                    if (data2_QB != null) {
                        CheckUnlocked(Game.QueueBits);
                        tracker_Queuebits.SetStarDisplay(data2_QB.totalStars);
                    } return;

                case Game.BlackBox:
                    string data_BT = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.BlackBox);
                    BlackBox.BBSaveData data2_BT = JsonUtility.FromJson<BlackBox.BBSaveData>(data_BT);
                    if (data2_BT != null) {
                        CheckUnlocked(Game.BlackBox);
                        tracker_BuriedTreasure.SetStarDisplay(data2_BT.totalStars);
                    } return;

                default: return;
            }
        } */


    }    
}