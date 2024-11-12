using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Wrapper 
{
    public class StarTracker : MonoBehaviour 
    {

        [SerializeField] private StarTrackerObj starTotal;
        [SerializeField] private StarTrackerObj starTotalMain;
        
        [SerializeField] private StarTrackerObj starQupcakery;
        [SerializeField] private StarTrackerObj starTwinTanglement;
        [SerializeField] private StarTrackerObj starTanglesLair;
        [SerializeField] private StarTrackerObj starQueueBits;
        [SerializeField] private StarTrackerObj starBuriedTreasure;
        
        [SerializeField] private StarTrackerObj starChallenge;

        [SerializeField] private Animator starTrackerPanelAnim;
        private bool panelVisible = false;



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
            starTrackerPanelAnim.SetBool("ShowPanel", panelVisible);
        }






        public int TotalStars; // total possible is 291

        // Set up Singleton for StarTracker
        /* public static StarTracker ST;
        private void Awake() {
            if (ST != null) {
                Destroy(gameObject);
                return;
            }
            ST = this;
            DontDestroyOnLoad(gameObject);
        } */

        /* 
        Dictionary and helper methods 
        */

        // Total stars per each game
        /* public Dictionary<Game, int> starsPerGame = new Dictionary<Game, int>() {
            {Game.BlackBox, 0},     // max 45
            {Game.Circuits, 0},     // max 75
            {Game.Labyrinth, 0},    // max 45
            {Game.QueueBits, 0},    // max 45
            {Game.Qupcakes, 0}      // max 81
        }; */
        private Game[] games = new Game[] {
            Game.BlackBox, Game.Circuits, Game.Labyrinth, Game.QueueBits, Game.Qupcakes
        };
        /* public void PrintDict() {
            foreach (Game game in games) {
                Debug.Log($"{game}: {starsPerGame[game]}; Unlocked: {gameUnlocked[game]}");
            }
            Debug.Log("-----");
        } */

        /* 
        Game unlock status 
        */
        public Dictionary<Game, bool> gameUnlocked = new Dictionary<Game, bool>() {
            {Game.BlackBox, false},
            {Game.Circuits, false},
            {Game.Labyrinth, true},
            {Game.QueueBits, false},
            {Game.Qupcakes, true}
        };

        /* 
        Function to check if game meets unlocked status
        Different for LITE_VERSION and full version
        */

        public bool CheckUnlocked(Game game) {
            if (gameUnlocked[game]) return true;
#if LITE_VERSION
            switch(game) {
                case Game.Circuits:
                    if (starQupcakery.starsWon >= 12) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                        gameUnlocked[Game.Circuits] = true;
                        return true;
                    } break;
                case Game.QueueBits:
                    // No pop-up
                    gameUnlocked[Game.QueueBits] = true;
                    return true;
                    break;
                case Game.BlackBox:
                    if (starTotal.starsWon >= 50) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                        gameUnlocked[Game.BlackBox] = true;
                        return true;
                    } break;
                default:
                    return false;
            }
#else
            switch(game) {
                case Game.Circuits:
                    if (starQupcakery.starsWon >= 27) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                        gameUnlocked[Game.Circuits] = true;
                        return true;
                    } break;
                case Game.QueueBits:
                    if (starQupcakery.starsWon >= 10 
                    && starTwinTanglement.starsWon >= 10) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.QueueBits);
                        gameUnlocked[Game.QueueBits] = true;
                        return true;
                    } break;
                case Game.BlackBox:
                    if (starTotal.starsWon >= 120) {
                        Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                        gameUnlocked[Game.BlackBox] = true;
                        return true;
                    } break;
                default:
                    return false;
            }
#endif
            return false;
        }


        // GameObject that displays the star count
        public TMP_Text scoreNumber; 

        // Function to Initialize the StarTracker, called during LoginRoutine in SaveManager

       /*  public void InitStarTracker_Lite() {
            ResetStarCounts();
            GameObject.Find("MapCanvas/MapPanel").GetComponent<MapManager>().InitMap();
        } */

        public void DelayInitTrackers() {
            Invoke("InitStarTracker", 0.5f);
        }


        public void InitStarTracker() {
            // DelayInitialization();
#if LITE_VERSION
            ResetStarCounts();
            // GameObject.Find("MapCanvas/MapPanel").GetComponent<MapManager>().InitMap();
#else
            // InitTTStars();
            // InitQCStars();
            // InitTLStars();
            // InitQBStars();
            // InitBTStars();

            foreach (Game minigame in games) {
                initMinigameStars(minigame);
            }
            ResetTotal();

            // ResetStarDisplay();
            // GameObject.Find("MapCanvas/MapPanel").GetComponent<MapManager>().InitMap();
            // PrintDict();
#endif
            Events.InitializeMap?.Invoke();
        }


        /* 
        Updating star display and dictionary 
        */

        public void ResetTotal() {
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

        // Reset the TMPro Asset that displays the count
        /* public void ResetStarDisplay() {
            int i = 0;
            foreach (Game g in starsPerGame.Keys) {
                i += starsPerGame[g];
            }
            TotalStars = i;
            scoreNumber.text = $"{i}";
        } */

        // Resets all the counts to be zero for when NewGame button pressed
        public void ResetStarCounts() {
            /* foreach (Game g in games) {
                starsPerGame[g] = 0;
            } */

            starQupcakery.SetStarDisplay(0);
            starTwinTanglement.SetStarDisplay(0);
            starTanglesLair.SetStarDisplay(0);
            starQueueBits.SetStarDisplay(0);
            starBuriedTreasure.SetStarDisplay(0);
            
            starTotal.SetStarDisplay(0);
            TotalStars = 0;
            scoreNumber.text = "0";

            gameUnlocked[Game.BlackBox] = false;
            gameUnlocked[Game.Circuits] = false;
            gameUnlocked[Game.QueueBits] = false;
        }

        // Sets private Dictionary values and updates display
        /* public void UpdateStarTracker(Game game, int i) {
            starsPerGame[game] = i;
            ResetStarDisplay();
        } */

        /* 
        Loading TotalStars from each game 
        */

        private IEnumerator DelayInitialization() {
            Debug.Log("DELAY");
            yield return new WaitForSeconds(0.1f);
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
                        starTanglesLair.SetStarDisplay(data2_TL.totalStars);
                        gameUnlocked[Game.Circuits] = CheckUnlocked(Game.Circuits);
                    } return;
                
                case Game.QueueBits:
                    string data_QB = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.QueueBits);
                    QueueBits.QBSaveData data2_QB = JsonUtility.FromJson<QueueBits.QBSaveData>(data_QB);
                    if (data2_QB != null) {
                        starQueueBits.SetStarDisplay(data2_QB.totalStars);
                        gameUnlocked[Game.QueueBits] = CheckUnlocked(Game.QueueBits);
                    } return;

                case Game.BlackBox:
                    string data_BT = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.BlackBox);
                    BlackBox.BBSaveData data2_BT = JsonUtility.FromJson<BlackBox.BBSaveData>(data_BT);
                    if (data2_BT != null) {
                        starBuriedTreasure.SetStarDisplay(data2_BT.totalStars);
                        gameUnlocked[Game.BlackBox] = CheckUnlocked(Game.BlackBox);
                    } return;

                default: return;
                }
            }
        }


/* 
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
        } */
        
}