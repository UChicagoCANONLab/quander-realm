using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Wrapper 
{
    public class Trackers : MonoBehaviour 
    {

        [SerializeField] private StarTracker starTotal;
        [SerializeField] private StarTracker starTotalMain;
        
        [SerializeField] private StarTracker starQupcakery;
        [SerializeField] private StarTracker starTwinTanglement;
        [SerializeField] private StarTracker starTanglesLair;
        [SerializeField] private StarTracker starQueueBits;
        [SerializeField] private StarTracker starBuriedTreasure;

        // [SerializeField] private StarTracker[] starMinigames;
        // ORDER: 0-Qupcakery, 1-TwinTanglement, 2-Tangle's Lair, 3-QueueBits, 4-Buried Treasure
        
        [SerializeField] private StarTracker starChallenge;

        [SerializeField] private Animator trackerAnimator;
        private bool panelVisible = false;


        public int TotalStars; 
        public TMP_Text scoreNumber; 

        private Game[] games = new Game[] {
            Game.Qupcakes, Game.Labyrinth, Game.Circuits, Game.QueueBits, Game.BlackBox
        };
        /* public Dictionary<Game, bool> gameUnlocked = new Dictionary<Game, bool>() {
            {Game.BlackBox, false},
            {Game.Circuits, false},
            {Game.Labyrinth, true},
            {Game.QueueBits, false},
            {Game.Qupcakes, true}
        }; */



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



        public void ToggleStarPanel() {
            panelVisible = !panelVisible;
            trackerAnimator.SetBool("ShowPanel", panelVisible);
        }






        public bool CheckUnlocked(Game game) {
            // if (gameUnlocked[game]) return true;
#if LITE_VERSION
            switch(game) {
                case Game.Qupcakes: return true;
                case Game.Labyrinth: return true;
                case Game.Circuits:
                    if (starQupcakery.starsWon >= 12) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                        // gameUnlocked[Game.Circuits] = true;
                        // starTanglesLair.gameUnlocked = true;
                        starTanglesLair.SetGameUnlocked(true);
                        return true;
                    } break;
                case Game.QueueBits:
                    // No pop-up
                    // gameUnlocked[Game.QueueBits] = true;
                    // starQueueBits.gameUnlocked = true;
                    starQueueBits.SetGameUnlocked(true);
                    return true;
                    break;
                case Game.BlackBox:
                    RecountTotal();
                    if (starTotal.starsWon >= 50) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                        // gameUnlocked[Game.BlackBox] = true;
                        // starBuriedTreasure.gameUnlocked = true;
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
                        // gameUnlocked[Game.Circuits] = true;
                        // starTanglesLair.gameUnlocked = true;
                        starTanglesLair.SetGameUnlocked(true);
                        return true;
                    } break;
                case Game.QueueBits:
                    if (starQupcakery.starsWon >= 10 
                    && starTwinTanglement.starsWon >= 10) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.QueueBits);
                        // gameUnlocked[Game.QueueBits] = true;
                        // starQueueBits.gameUnlocked = true;
                        starQueueBits.SetGameUnlocked(true);
                        return true;
                    } break;
                case Game.BlackBox:
                    RecountTotal();
                    if (starTotal.starsWon >= 120) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                        // gameUnlocked[Game.BlackBox] = true;
                        // starBuriedTreasure.gameUnlocked = true;
                        starBuriedTreasure.SetGameUnlocked(true);
                        return true;
                    } break;
                default:
                    return false;
            }
#endif
            return false;
        }


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


        public void RecountTotal() {
            int i = 0;
            i += starQupcakery.starsWon;
            i += starTwinTanglement.starsWon;
            i += starTanglesLair.starsWon;
            i += starQueueBits.starsWon;
            i += starBuriedTreasure.starsWon;
            
            starTotal.SetStarDisplay(i);
            TotalStars = i;
            scoreNumber.text = $"{i}";
        }


        // Resets all the counts to be zero for when NewGame button pressed
        public void ResetStarCounts() {
            starQupcakery.ResetStarDisplay();
            starTwinTanglement.ResetStarDisplay();
            starTanglesLair.ResetStarDisplay();
            starQueueBits.ResetStarDisplay();
            starBuriedTreasure.ResetStarDisplay();
            
            starTotal.ResetStarDisplay();
            TotalStars = 0;
            scoreNumber.text = "0";

            // gameUnlocked[Game.BlackBox] = false;
            // gameUnlocked[Game.Circuits] = false;
            // gameUnlocked[Game.QueueBits] = false;
        }


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
                        // starTanglesLair.gameUnlocked = CheckUnlocked(Game.Circuits);
                        CheckUnlocked(Game.Circuits);
                        starTanglesLair.SetStarDisplay(data2_TL.totalStars);
                        // gameUnlocked[Game.Circuits] = CheckUnlocked(Game.Circuits);
                    } return;
                
                case Game.QueueBits:
                    string data_QB = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.QueueBits);
                    QueueBits.QBSaveData data2_QB = JsonUtility.FromJson<QueueBits.QBSaveData>(data_QB);
                    if (data2_QB != null) {
                        // starQueueBits.gameUnlocked = CheckUnlocked(Game.QueueBits);
                        CheckUnlocked(Game.QueueBits);
                        starQueueBits.SetStarDisplay(data2_QB.totalStars);
                        // gameUnlocked[Game.QueueBits] = CheckUnlocked(Game.QueueBits);
                    } return;

                case Game.BlackBox:
                    string data_BT = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.BlackBox);
                    BlackBox.BBSaveData data2_BT = JsonUtility.FromJson<BlackBox.BBSaveData>(data_BT);
                    if (data2_BT != null) {
                        // starBuriedTreasure.gameUnlocked = CheckUnlocked(Game.BlackBox);
                        CheckUnlocked(Game.BlackBox);
                        starBuriedTreasure.SetStarDisplay(data2_BT.totalStars);
                        // gameUnlocked[Game.BlackBox] = CheckUnlocked(Game.BlackBox);
                    } return;

                default: return;
                }
            }
        }
        
}