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
        public double wallProb = 0.5;
        
        [Header("Path Info")]
        public int steps = 0;
        public int pathLength;

        // [Header("Feedback Objects")]
        // public Star[] stars;
        // public GameObject levelNumber;

        [Header("Time Info (for research)")]
        // Time in seconds for research data
        private float initTime;
        private float endTime;
        public float timePlayed;

        [Header("GameObjects")]
        public Maze MAZE;
        public PlayerMovement PM;
        // public ButtonBehavior BTN;
        // public ProgressBar PB;
        public UIManager UI;

        public static GameBehavior Instance;

        private Dictionary<string, string> rot180 = new Dictionary<string, string>() 
        { {"N", "S"}, {"W", "E"}, {"E", "W"}, {"S", "N"} };

        private Dictionary<string, string> rot90 = new Dictionary<string, string>() 
        { {"N", "W"}, {"W", "S"}, {"E", "N"}, {"S", "E"} };

        private Dictionary<string, string> goalTextNormal = new Dictionary<string, string>() 
        { {"N", "Up"}, {"W", "Left"}, {"E", "Right"}, {"S", "Down"} };


    // ~~~~~~~~~~~~~~~ INITIALIZING ~~~~~~~~~~~~~~~

        void Start() 
        {
            initTime = Time.time;
            
            PM.StartPM();
            MAZE.StartMaze();

            UI.SetLevelNumber($"{SaveData.Instance.CurrentLevel}");

            if (SaveData.Instance.CurrentLevel == 0) {
                pathLength = 8;
            }
            else {
                // levelNumber.GetComponent<TextMeshProUGUI>().text = $"{SaveData.Instance.CurrentLevel}";
                pathLength = MAZE.pathfinder(0, size-1, size-1, 0).Length;
            }
            // PB.resetBar();
            // PB.initializeBar(pathLength);
            UI.SetProgressBar(pathLength);
        }

        void Update() {
            checkNumStars();

            if (Input.GetKeyDown(KeyCode.Space)) {
                PM.SwitchPlayer();
            }
        }

        public void collectGoal() {
            checkNumStars();

            MAZE.clearGoal();
            winner = true;

            endTime = Time.time;
            timePlayed = endTime - initTime;
            SaveData.Instance.updateSave(this);

            UI.LevelComplete(numStars);
            if (SaveData.Instance.CurrentLevel == 15) {
                DialogueAndRewards.Instance.doDialogue(SaveData.Instance.CurrentLevel);
            }
            DialogueAndRewards.Instance.giveReward(SaveData.Instance.CurrentLevel);

            steps = 0;
        }

        public void checkNumStars() {
            // PB.detractBar(steps);
            UI.UpdateProgressBar(steps);
            if (steps > (int)(pathLength + 2)) { // 1 mistake
                numStars = 2;
            }
            if (steps > (int)(pathLength + 4)) { // 2 mistakes
                numStars = 1;
            }
            if (steps > (int)(pathLength + 8)) { // 4 mistakes
                numStars = 0;
            }
            return;
        }

        

    // ~~~~~~~~~~~~~~~ BUTTONS ~~~~~~~~~~~~~~~

        public void GiveHint() {
            string hintDir;

            object[] hint = MAZE.calcPathToGoal();
            string hintPath = hint[0].ToString();
            int hintDeg = (int)hint[1];


            if (hintDeg == 90) {
                hintDir = goalTextNormal[rot90[hintPath[0].ToString()]];
                // hintText = $"Hint: Try going {goalTextNormal[rot90[hintPath[0].ToString()]]}!";
            }
            else if (hintDeg == 180) {
                hintDir = goalTextNormal[rot180[hintPath[0].ToString()]];
                // hintText = $"Hint: Try going {goalTextNormal[rot180[hintPath[0].ToString()]]}!";
            }
            else { //if (hintDeg == 0) {
                hintDir = goalTextNormal[hintPath[0].ToString()];
                // hintText = $"Hint: Try going {goalTextNormal[hintPath[0].ToString()]}!";
            }
            
            string buttonPath = $"Canvases/CanvasOver/GameplayButtons/MovementButtons/{hintDir}";
            // GameObject.Find(buttonPath).GetComponent<ParticleSystem>().Play();
            GameObject.Find(buttonPath).GetComponent<Animation>().Play("ButtonHighlight");
            // Button button = GameObject.Find(hintDir); //.GetComponent<SpriteRenderer>();
            // button.IsHighlighted = true;

            hintsUsed++; steps++;
        }


        public void Restart() {
            // Saving restart data
            SaveData.Instance.winner = false;
            endTime = Time.time;
            timePlayed = endTime - initTime;
            SaveData.Instance.updateSave(this);
            if (SaveData.Instance.CurrentLevel > 0) {
                // Save.SaveTTSaveData();
                Save.Instance.SaveGame();
            }

            // Resetting level
            initTime = Time.time;

            PM.player1.returnPlayer();
            PM.player2.returnPlayer();

            MAZE.generateMazes();
            MAZE.renderMazes();

            if (winner == true) {
                winner = false;
                UI.Reset();
            }

            pathLength = MAZE.pathfinder(0, size-1, size-1, 0).Length;
            hintsUsed = 0;
            steps = 0;
            numStars = 3;

            /* for (int i=0; i<3; i++) {
                stars[i].resetStar();
            } */

            if (PM.player1.current == false) {
                PM.SwitchPlayer();
            }

            // PB.resetBar();
            // PB.initializeBar(pathLength);
            UI.SetProgressBar(pathLength);
        }

    }
}