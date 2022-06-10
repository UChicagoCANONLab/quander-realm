using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    private int goalsCollected = 0;
    private int numGoals = 1;
    public bool winner = false;
    private int hintsUsed = 0;
    private string hintText = "Press Hint if you get stuck!";
    private int numStars = 3;

    public int degree;
    public int size;
    public double wallProb = 0.5;
    
    public int steps = 0;
    public int pathLength;

    public Star[] stars; 

    public int currentLevel;

    public Maze maze;
    public PlayerMovement pm;
    public Camera cam;
    public ButtonBehavior btn;

    private Dictionary<string, string> rot180 = new Dictionary<string, string>() 
    { {"N", "S"}, {"W", "E"}, {"E", "W"}, {"S", "N"} };

    private Dictionary<string, string> rot90 = new Dictionary<string, string>() 
    { {"N", "W"}, {"W", "S"}, {"E", "N"}, {"S", "E"} };

    private Dictionary<string, string> rotNEG90 = new Dictionary<string, string>() 
    { {"N", "E"}, {"W", "N"}, {"E", "S"}, {"S", "W"} };

    private Dictionary<string, string> goalTextNormal = new Dictionary<string, string>() 
    { {"N", "up"}, {"W", "left"}, {"E", "right"}, {"S", "down"} };


// ~~~~~~~~~~~~~~~ INITIALIZING ~~~~~~~~~~~~~~~

    void Start() 
    {
        Time.timeScale = 1f;
        maze = GameObject.Find("MazeGen").GetComponent<Maze>();
        pm = GameObject.Find("Players").GetComponent<PlayerMovement>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        btn = GameObject.Find("GameManagerLocal").GetComponent<ButtonBehavior>();

        if (size == 4) {
            cam.orthographicSize = 4;    
        }
        else {
            cam.orthographicSize = size - size/4;
        }
        if (size%2 == 0) {
            cam.transform.position += new Vector3(0.5f,0.5f,0);
        }
        
        pm.StartPM();
        maze.StartMaze();
        
        pathLength = maze.pathfinder(0, size-1, size-1, 0).Length;
    }

    void Update() {
        checkNumStars();
        if (winner == true) {
            btn.Win(numStars);
        }
        /* if (Input.GetKeyDown(KeyCode.Escape)) {
            btn.LoadMainMenu();
        } */
        if (Input.GetKeyDown(KeyCode.Space)) {
            pm.SwitchPlayer();
        }
    }

    public void collectGoal() {
        goalsCollected++;

        if (goalsCollected == numGoals) {
            maze.clearGoal();
            winner = true;
            btn.Win(numStars);
        }
        steps = 0;
    }

    public void checkNumStars() {
        if (steps > (int)(1.25 * pathLength)) {
            stars[2].visibilityOff();
            numStars = 2;
        }
        if (steps > (int)(1.5 * pathLength)) {
            stars[1].visibilityOff();
            numStars = 1;
        }
        if (steps > (int)(1.75 * pathLength)) {
            stars[0].visibilityOff();
            numStars = 0;
        }
        return;
    }

   /*  void OnGUI() {
        if (winner != true) {
            GUI.Box(new Rect(10, 10, 150, 25), "Goals Collected: " + goalsCollected);
            GUI.Box(new Rect(10, 350, 200, 25), hintText);
        }
    } */

    

// ~~~~~~~~~~~~~~~ BUTTONS ~~~~~~~~~~~~~~~

    public void GiveHint() {
        string hintDir;
        if (hintsUsed == 3) {
            hintText = "You used all your hints!";
            return;
        }
        object[] hint = maze.calcPathToGoal();
        string hintPath = hint[0].ToString();
        int hintDeg = (int)hint[1];

        if (hintDeg == 0) {
            hintDir = goalTextNormal[hintPath[0].ToString()];
            // hintText = $"Hint: Try going {goalTextNormal[hintPath[0].ToString()]}!";
        }
        else if (hintDeg == 180) {
            hintDir = goalTextNormal[rot180[hintPath[0].ToString()]];
            // hintText = $"Hint: Try going {goalTextNormal[rot180[hintPath[0].ToString()]]}!";
        }
        else if (hintDeg == 90) {
            hintDir = goalTextNormal[rot90[hintPath[0].ToString()]];
            // hintText = $"Hint: Try going {goalTextNormal[rot90[hintPath[0].ToString()]]}!";
        }

        hintsUsed++;
    }

    public void Restart() {
        pm.player1.returnPlayer();
        pm.player2.returnPlayer();

        maze.generateMazes();
        maze.renderMazes();

        if (winner == true) {
            winner = false;
            btn.UndoWin(numStars);
        }

        pathLength = maze.pathfinder(0, size-1, size-1, 0).Length;
        goalsCollected = 0;
        numGoals = 1;
        hintsUsed = 0;
        steps = 0;
        numStars = 3;

        for (int i=0; i<3; i++) {
            stars[i].resetStar();
        }

        if (pm.player1.current == false) {
            pm.SwitchPlayer();
        }
    }

    public void MoveButton(string mov) {
        Vector3 press = pm.getButtonPress(mov);
    }

    public void NextLevel() {
        int nextLev = currentLevel + 1;
        btn.LevelSelect(nextLev);
    }
}
