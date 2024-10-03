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
        private bool winner = false;
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

        [Header("GameObjects")]
        // public Maze MAZE;
        // public PlayerMovement PM;
        // public UIManager UI;

        public static GameBehavior Instance;

        private Dictionary<string, string> rot180 = new Dictionary<string, string>() 
        { {"N", "S"}, {"W", "E"}, {"E", "W"}, {"S", "N"} };

        private Dictionary<string, string> rot90 = new Dictionary<string, string>() 
        { {"N", "W"}, {"W", "S"}, {"E", "N"}, {"S", "E"} };

        private Dictionary<string, string> goalTextNormal = new Dictionary<string, string>() 
        { {"N", "Up"}, {"W", "Left"}, {"E", "Right"}, {"S", "Down"} };


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
            // PM.StartPM();
            // MAZE.StartMaze();
            // UI.SetLevelNumber($"{SaveData.Instance.CurrentLevel}");

            if (SaveData.Instance.CurrentLevel == 0) {
                pathLength = 8;
            }
            else {
                // pathLength = MAZE.pathfinder(0, size-1, size-1, 0).Length;
                pathLength = TTEvents.PathFinder.Invoke(0, size-1, size-1, 0).Length;
            }
            TTEvents.SetProgressBar?.Invoke(pathLength);
            // UI.SetProgressBar(pathLength);
        }

        void Update() {
            checkNumStars();

            if (Input.GetKeyDown(KeyCode.Space)) {
                // PM.SwitchPlayer();
                TTEvents.SwitchPlayer?.Invoke();
            }
        }

        public void collectGoal() {
            checkNumStars();

            // MAZE.clearGoal();
            TTEvents.ClearGoal?.Invoke();
            winner = true;

            endTime = Time.time;
            timePlayed = endTime - initTime;
            SaveData.Instance.updateSave(this);

            TTEvents.LevelComplete?.Invoke(numStars);
            // UI.LevelComplete(numStars);
            if (SaveData.Instance.CurrentLevel == 15) {
                DialogueAndRewards.Instance.doDialogue(SaveData.Instance.CurrentLevel);
            }
            DialogueAndRewards.Instance.giveReward(SaveData.Instance.CurrentLevel);

            steps = 0;
        }

        public void checkNumStars() {
            TTEvents.UpdateProgressBar?.Invoke(steps);
            // UI.UpdateProgressBar(steps);
            
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
            string hintDir;

            // object[] hint = MAZE.calcPathToGoal();
            object[] hint = TTEvents.CalculatePathToGoal.Invoke();
            string hintPath = hint[0].ToString();
            int hintDeg = (int)hint[1];


            if (hintDeg == 90) {
                hintDir = goalTextNormal[rot90[hintPath[0].ToString()]];
            } else if (hintDeg == 180) {
                hintDir = goalTextNormal[rot180[hintPath[0].ToString()]];
            } else { 
                hintDir = goalTextNormal[hintPath[0].ToString()];
            }
            
            string buttonPath = $"Canvases/CanvasOver/GameplayButtons/MovementButtons/{hintDir}";
            // GameObject.Find(buttonPath).GetComponent<ParticleSystem>().Play();
            GameObject.Find(buttonPath).GetComponent<Animation>().Play("ButtonHighlight");

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

            if (winner == true) {
                winner = false;
                // UI.Reset();
                TTEvents.ResetUI?.Invoke();
            }

            // PM.player1.returnPlayer();
            // PM.player2.returnPlayer();
            TTEvents.ReturnPlayers?.Invoke();

            // MAZE.generateMazes();
            // MAZE.renderMazes();
            TTEvents.GenerateMazes?.Invoke();
            TTEvents.RenderMazes?.Invoke();

            // pathLength = MAZE.pathfinder(0, size-1, size-1, 0).Length;
            pathLength = TTEvents.PathFinder.Invoke(0, size-1, size-1, 0).Length;
            hintsUsed = 0;
            steps = 0;
            numStars = 3;

            // if (PM.player1.current == false) {
            if (!TTEvents.GetPlayer.Invoke(1).current) {
                // PM.SwitchPlayer();
                TTEvents.SwitchPlayer?.Invoke();
            }
            // UI.SetProgressBar(pathLength);
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