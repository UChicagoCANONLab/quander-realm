using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject gameplayButtons;
    public GameObject gameplayObjects;
    public GameObject[] starsWon;

    /* private Maze maze;
    private PlayerMovement pm;
    private GameBehavior gb;

    public void Start() {
        maze = GameObject.Find("MazeGen").GetComponent<Maze>();
        pm = GameObject.Find("Players").GetComponent<PlayerMovement>();
        gb = GameObject.Find("GameManagerLocal").GetComponent<GameBehavior>();
    } 
 */

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            LoadMainMenu();
        }
    }

    public void LoadLevelSelectMenu() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit() {
        Application.Quit();
    }

    public void Win(int goalsCollected) {
        winScreen.SetActive(true);
        gameplayButtons.SetActive(false);
        gameplayObjects.SetActive(false);
        starsWon[goalsCollected].SetActive(true);
        Time.timeScale = 0f;
    }

    public void UndoWin(int goalsCollected) {
        winScreen.SetActive(false);
        gameplayButtons.SetActive(true);
        gameplayObjects.SetActive(true);
        starsWon[goalsCollected].SetActive(false);
        Time.timeScale = 1f;
    }

    public void LevelSelect(int sel) {
        string currScene;

        switch(sel) {
            case 1:
                currScene = "4x4.0.1";
                break;
            case 2:
                currScene = "4x4.0.2";
                break;
            case 3:
                currScene = "5x5.0.1";
                break;
            case 4:
                currScene = "5x5.0.2";
                break;
            case 5:
                currScene = "6x6.0";
                break;

            case 6:
                currScene = "4x4.180";
                break;
            case 7:
                currScene = "5x5.180.1";
                break;
            case 8:
                currScene = "5x5.180.2";
                break;
            case 9:
                currScene = "6x6.180.1";
                break;
            case 10:
                currScene = "6x6.180.2";
                break;

            case 11:
                currScene = "4x4.90.1";
                break;
            case 12:
                currScene = "4x4.90.2";
                break;
            case 13:
                currScene = "5x5.90.1";
                break;
            case 14:
                currScene = "5x5.90.2";
                break;
            case 15:
                currScene = "6x6.90";
                break;
            
            case 16:
                currScene = "MainMenu";
                break;
            default:
                currScene = "LevelSelect";
                break;
        }
        SceneManager.LoadScene(currScene);
    }

}
