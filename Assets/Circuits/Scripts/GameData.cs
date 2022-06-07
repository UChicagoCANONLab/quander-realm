using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public static class GameData
{


    public static int CurrLevel { get; set; } = 0;
    public static int max_level = 20;
    public static bool[] completedLevels = new bool[Constants.N_LEVELS];

    private static bool tutorialShown = false;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
    [DllImport("__Internal")]
    private static extern void SendData(string callback);
#endif

    public static string getNextScene() {
        string outString = "";
        foreach (var score in completedLevels)
        {
            outString += score ? "1" : "0";
        }
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    SendData (outString);
#endif
        if (tutorialShown) {
            tutorialShown = false;
            return "CircuitsLevelScene";
	    }
        int offset = 0;
        tutorialShown = true;
        switch (CurrLevel - offset) {
            case 3:
                return "DialogC-01";
            case 6:
                return "DialogC-02";
            case 9:
                return "DialogC-02A";
            case 14:
                return "DialogC-03B";
            case 18:
                return "DialogC-03A";
            case 22:
                return "DialogC-04";
            default:
                tutorialShown = false;
                return "CircuitsLevelScene";
        }
    }


    //public Scen
}