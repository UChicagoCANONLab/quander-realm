using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    private int cx;
    private int cy;
    private int tx;
    private int ty;

    private int type;
    public bool visited;
    public Dictionary<string, bool> walls;
    public bool goal;
    public int deg;


    private GameBehavior gb = GameObject.Find("GameManagerLocal").GetComponent<GameBehavior>(); 

    public MazeCell(int x, int y)
    {
        cx = x;
        cy = y;
        type = 0;
        visited = false;
        walls = new Dictionary<string, bool>() 
            { {"N", true}, {"S", true}, {"W", true}, {"E", true} };
        goal = false;
        deg = 0;

        // px = (int)(-x + (gb.size - 1)/2); // player coordinates
        // py = (int)(-y + (gb.size - 1)/2);

        tx = (int)(-x + (gb.size-gb.size/4)/2 + (gb.size-1)/2);
        ty = (int)(-y + (gb.size-gb.size/4)/2 + (gb.size-1)/2);

    }

    public int getX {
        get { return cx; }
    }

    public int getY {
        get { return cy; }
    }

    public int getTx {
        get { return tx; }
    }

    public int getTy {
        get { return ty; }
    }

    public Vector3 getTloc {
        get { return ( new Vector3(tx, ty, 0)); }
    }

    public int getType {
        get { return type; }
    }

    public bool getGoal {
        get { return goal; }
    }
    
    public void setDeg(int degree) {
        deg = degree;
        return;
    }

    public string toString() {
        return $"Tile {cx}, {cy}";
    }

    public string getWalls() {
        return $"N: {walls["N"]}, S: {walls["S"]}, E: {walls["E"]}, W: {walls["W"]}";
    }

    public bool isCave() {
        int count = 0;
        foreach (KeyValuePair<string,bool> dir in walls) {
            if (dir.Value == true) {
                count++;
            }
        }
        return (count==3);
    }

    public void toggleGoal(bool val) {
        goal = val;
    }
    
    /* public void setType(int degree) {
        if (this.getWalls() == "N: True, S: True, E: True, W: True") {
            type = 0; }     //NSEW
        else if (this.getWalls() == "N: True, S: True, E: True, W: False") {
            if (degree == 0) { type = 1; }   //NSE
            else if (degree == 90) { type = 4; }
            else if (degree == 180) { type = 2; }
        }
        else if (this.getWalls() == "N: True, S: True, E: False, W: True") {
            if (degree == 0) { type = 2; }     //NSW
            else if (degree == 90) { type = 8; }
            else if (degree == 180) { type = 1; }
        }   
        else if (this.getWalls() == "N: True, S: True, E: False, W: False") {
            if (degree == 0) { type = 3; }     //NS
            else if (degree == 90) { type = 12; }
            else if (degree == 180) { type = 3; }
        }
        else if (this.getWalls() == "N: True, S: False, E: True, W: True") {
            if (degree == 0) { type = 4; }     //NEW
            else if (degree == 90) { type = 2; }
            else if (degree == 180) { type = 8; }
        }
        else if (this.getWalls() == "N: True, S: False, E: True, W: False") {
            if (degree == 0) { type = 5; }     //NE
            else if (degree == 90) { type = 6; }
            else if (degree == 180) { type = 10; }
        }
        else if (this.getWalls() == "N: True, S: False, E: False, W: True") {
            if (degree == 0) { type = 6; }     //NW
            else if (degree == 90) { type = 10; }
            else if (degree == 180) { type = 9; }
        }
        else if (this.getWalls() == "N: True, S: False, E: False, W: False") {
            if (degree == 0) { type = 7; }     //N
            else if (degree == 90) { type = 14; }
            else if (degree == 180) { type = 11; }
        }
        else if (this.getWalls() == "N: False, S: True, E: True, W: True") {
            if (degree == 0) { type = 8; }     //SEW
            else if (degree == 90) { type = 1; }
            else if (degree == 180) { type = 4; }
        }
        else if (this.getWalls() == "N: False, S: True, E: True, W: False") {
            if (degree == 0) { type = 9; }     //SE
            else if (degree == 90) { type = 5; }
            else if (degree == 180) { type = 6; }
        }
        else if (this.getWalls() == "N: False, S: True, E: False, W: True") {
            if (degree == 0) { type = 10; }    //SW
            else if (degree == 90) { type = 9; }
            else if (degree == 180) { type = 5; }
        }
        else if (this.getWalls() == "N: False, S: True, E: False, W: False") {
            if (degree == 0) { type = 11; }    //S
            else if (degree == 90) { type = 13; }
            else if (degree == 180) { type = 7; }
        }
        else if (this.getWalls() == "N: False, S: False, E: True, W: True") {
            if (degree == 0) { type = 12; }    //EW
            else if (degree == 90) { type = 3; }
            else if (degree == 180) { type = 12; }
        }
        else if (this.getWalls() == "N: False, S: False, E: True, W: False") {
            if (degree == 0) { type = 13; }    //E
            else if (degree == 90) { type = 7; }
            else if (degree == 180) { type = 14; }
        }
        else if (this.getWalls() == "N: False, S: False, E: False, W: True") {
            if (degree == 0) { type = 14; }    //W
            else if (degree == 90) { type = 11; }
            else if (degree == 180) { type = 13; }
        }
        else if (this.getWalls() == "N: False, S: False, E: False, W: False") {
            type = 15; }    //none
        else {
            type = 0; }
        return;
    } */

    // now with 4 wall types instead of 16
    public void setType(int degree) {
        if (this.getWalls() == "N: True, S: True, E: True, W: True") {
            type = 0; }     //NSEW
        else if (this.getWalls() == "N: True, S: True, E: True, W: False") {
            if (degree == 0) { type = 1; }   //NSE
            else if (degree == 90) { type = 0; }
            else if (degree == 180) { type = 0; }
        }
        else if (this.getWalls() == "N: True, S: True, E: False, W: True") {
            if (degree == 0) { type = 0; }     //NSW
            else if (degree == 90) { type = 2; }
            else if (degree == 180) { type = 1; }
        }   
        else if (this.getWalls() == "N: True, S: True, E: False, W: False") {
            if (degree == 0) { type = 1; }     //NS
            else if (degree == 90) { type = 2; }
            else if (degree == 180) { type = 1; }
        }
        else if (this.getWalls() == "N: True, S: False, E: True, W: True") {
            if (degree == 0) { type = 0; }     //NEW
            else if (degree == 90) { type = 0; }
            else if (degree == 180) { type = 2; } //DOUBLE CHECK
        }
        else if (this.getWalls() == "N: True, S: False, E: True, W: False") {
            if (degree == 0) { type = 1; }     //NE
            else if (degree == 90) { type = 0; }
            else if (degree == 180) { type = 2; }
        }
        else if (this.getWalls() == "N: True, S: False, E: False, W: True") {
            if (degree == 0) { type = 0; }     //NW
            else if (degree == 90) { type = 2; }
            else if (degree == 180) { type = 3; }
        }
        else if (this.getWalls() == "N: True, S: False, E: False, W: False") {
            if (degree == 0) { type = 1; }     //N
            else if (degree == 90) { type = 2; }
            else if (degree == 180) { type = 3; }
        }
        else if (this.getWalls() == "N: False, S: True, E: True, W: True") {
            if (degree == 0) { type = 2; }     //SEW
            else if (degree == 90) { type = 1; }
            else if (degree == 180) { type = 0; }
        }
        else if (this.getWalls() == "N: False, S: True, E: True, W: False") {
            if (degree == 0) { type = 3; }     //SE
            else if (degree == 90) { type = 1; }
            else if (degree == 180) { type = 0; }
        }
        else if (this.getWalls() == "N: False, S: True, E: False, W: True") {
            if (degree == 0) { type = 2; }    //SW
            else if (degree == 90) { type = 3; }
            else if (degree == 180) { type = 1; }
        }
        else if (this.getWalls() == "N: False, S: True, E: False, W: False") {
            if (degree == 0) { type = 3; }    //S
            else if (degree == 90) { type = 3; }
            else if (degree == 180) { type = 1; }
        }
        else if (this.getWalls() == "N: False, S: False, E: True, W: True") {
            if (degree == 0) { type = 2; }    //EW
            else if (degree == 90) { type = 1; }
            else if (degree == 180) { type = 2; }
        }
        else if (this.getWalls() == "N: False, S: False, E: True, W: False") {
            if (degree == 0) { type = 3; }    //E
            else if (degree == 90) { type = 1; }
            else if (degree == 180) { type = 2; }
        }
        else if (this.getWalls() == "N: False, S: False, E: False, W: True") {
            if (degree == 0) { type = 2; }    //W
            else if (degree == 90) { type = 3; }
            else if (degree == 180) { type = 3; }
        }
        else if (this.getWalls() == "N: False, S: False, E: False, W: False") {
            type = 3; }    //none
        else {
            type = 0; }
        return;
    }


}
