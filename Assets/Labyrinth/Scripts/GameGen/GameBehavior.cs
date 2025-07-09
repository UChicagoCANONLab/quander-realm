using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Labyrinth 
{ 
    public class GameBehavior : MonoBehaviour
    {
        // private bool winner = false;
        public int hintsUsed = 0;
        public int numStars = 3;

        [Header("Maze Info")]
        public int size;
        public double wallProb = 0.95;
        
        [Header("Path Info")]
        public int steps = 0;
        public int pathLength;

        [Header("Time Info (for research)")]
        // Time in seconds for research data
        private float initTime;
        private float endTime;
        public float timePlayed;

        public static GameBehavior Instance;

        private Dictionary<string, string> rot180 = new Dictionary<string, string>() 
        { {"N", "S"}, {"W", "E"}, {"E", "W"}, {"S", "N"} };

        private Dictionary<string, string> rot90 = new Dictionary<string, string>() 
        { {"N", "W"}, {"W", "S"}, {"E", "N"}, {"S", "E"} };

        private Dictionary<string, string> goalTextNormal = new Dictionary<string, string>() 
        { {"N", "up"}, {"W", "left"}, {"E", "right"}, {"S", "down"} };


    // ~~~~~~~~~~~~~~~ INITIALIZING ~~~~~~~~~~~~~~~


        private void OnEnable() 
        {
            TTEvents.GiveHint += GiveHint;
            TTEvents.RestartLevel += Restart;
            TTEvents.Size += GetSize;
            TTEvents.WallProb += GetWallProb;
            TTEvents.CollectGoal += collectGoal;
            TTEvents.IncrementSteps += incrementSteps;
        }
        private void OnDisable() 
        {
            TTEvents.GiveHint -= GiveHint;
            TTEvents.RestartLevel -= Restart;
            TTEvents.Size -= GetSize;
            TTEvents.WallProb -= GetWallProb;
            TTEvents.CollectGoal -= collectGoal;
            TTEvents.IncrementSteps -= incrementSteps;
        }


        void Start() 
        {
            initTime = Time.time;
            
            TTEvents.StartPlayerMovement?.Invoke();
            TTEvents.StartMaze?.Invoke();
            TTEvents.SetLevelNumber?.Invoke($"{SaveData.Instance.CurrentLevel}");

            if (SaveData.Instance.CurrentLevel == 0) {
                pathLength = 8;
            }
            else {
                pathLength = TTEvents.PathFinder.Invoke(0, size-1, size-1, 0).Length;
            }
            TTEvents.SetProgressBar?.Invoke(pathLength);
        }

        void Update() {
            checkNumStars();

            if (Input.GetKeyDown(KeyCode.Space)) {
                TTEvents.SwitchPlayer?.Invoke();
                TTEvents.SetButtonTrigger("switch", "FromKeyboard");
            }
        }

        public void collectGoal() {
            checkNumStars();

            TTEvents.ClearGoal?.Invoke();
            SaveData.Instance.winner = true;

            endTime = Time.time;
            timePlayed = endTime - initTime;
            SaveData.Instance.updateSave(this);

            TTEvents.LevelComplete?.Invoke(numStars);
            if (SaveData.Instance.CurrentLevel == 15) {
                DialogueAndRewards.Instance.doDialogue(SaveData.Instance.CurrentLevel);
            }
            DialogueAndRewards.Instance.giveReward(SaveData.Instance.CurrentLevel);
            if (SaveData.Instance.CurrentLevel > 0) {
                Wrapper.Events.CollectAndDisplayBadge.Invoke(Wrapper.Game.Labyrinth, SaveData.Instance.CurrentLevel, numStars);
            }

            steps = 0;
        }

        public void checkNumStars() {
            TTEvents.UpdateProgressBar?.Invoke(steps);
            
            if (steps > (int)(pathLength + 2)) { // 1 mistake
                numStars = 2;
            } if (steps > (int)(pathLength + 4)) { // 2 mistakes
                numStars = 1;
            } if (steps > (int)(pathLength + 8)) { // 4 mistakes
                numStars = 0;
            } return;
        }

        

    // ~~~~~~~~~~~~~~~ BUTTONS ~~~~~~~~~~~~~~~

        public void GiveHint() {
            object[] hint = TTEvents.CalculatePathToGoal.Invoke();
            string hintPath = hint[0].ToString();
            int hintDeg = (int)hint[1];

            string hintDir;

            if (hintDeg == 90) {
                hintDir = goalTextNormal[rot90[hintPath[0].ToString()]];
            } else if (hintDeg == 180) {
                hintDir = goalTextNormal[rot180[hintPath[0].ToString()]];
            } else if (hintDeg == 0) { 
                hintDir = goalTextNormal[hintPath[0].ToString()];
            } else { // hintDeg == -1
                hintDir = "switch";
            }
            TTEvents.SetButtonTrigger(hintDir, "Hint");

            hintsUsed++; steps++;
        }


        public void Restart() {
            // Saving restart data
            SaveData.Instance.winner = false;
            endTime = Time.time;
            timePlayed = endTime - initTime;
            SaveData.Instance.updateSave(this);
            if (SaveData.Instance.CurrentLevel > 0) {
                Save.Instance.SaveGame();
            }

            // Resetting level
            initTime = Time.time;

            // if (winner == true) {
            //     winner = false;
                TTEvents.ResetUI?.Invoke();
            // }

            TTEvents.ReturnPlayers?.Invoke();

            TTEvents.GenerateMazes?.Invoke();
            TTEvents.RenderMazes?.Invoke();

            pathLength = TTEvents.PathFinder.Invoke(0, size-1, size-1, 0).Length;
            hintsUsed = 0;
            steps = 0;
            numStars = 3;

            if (!TTEvents.GetPlayer.Invoke(1).current) {
                TTEvents.SwitchPlayer?.Invoke();
            }
            TTEvents.SetProgressBar?.Invoke(pathLength);
        }

        // Get/Set functions
        public int GetSize() {
            return size;
        }
        public double GetWallProb() {
            return wallProb;
        }

        public void incrementSteps() {
            steps++;
        }
    }
}