using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;
using Circuits;
public static class GameData
{


    //public static int CurrLevel { get; set; } = 0;
    private static string datePatt = @"M/d/yyyy hh:mm:ss tt";

    private static bool tutorialShown = false;
    private static List<string> log = new List<string>();
    private static bool dataLoaded = false;
    private static Circuits_SaveData saveData;


    //[DllImport("__Internal")]
    //private static extern void SendData(string callback);

    public static void InitCircuitsSaveData()
    {
        
        if (!dataLoaded)
        {
            Debug.Log("Loading Data!");
            try
            {
                string saveString = Wrapper.Events.GetMinigameSaveData?.Invoke(Wrapper.Game.Circuits);
                Debug.Log(saveString);
                saveData = JsonUtility.FromJson<Circuits_SaveData>(saveString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (saveData == null)
            {
                saveData = new Circuits_SaveData();
            }
            dataLoaded = true;
        }
    }

    public static void hintRequested()
    {
        log.Add(string.Format("{0}-hint-{1}", saveData.currLevel, DateTime.UtcNow.ToString(datePatt)));
    }

    public static void incorrectSub()
    {
        log.Add(string.Format("{0}-incorrect-{1}", saveData.currLevel, DateTime.UtcNow.ToString(datePatt)));
    }

    public static void correctSub()
    {
        log.Add(string.Format("{0}-correct-{1}", saveData.currLevel, DateTime.UtcNow.ToString(datePatt)));
    }

    public static void levelStart()
    {
        log.Add(string.Format("{0}-start-{1}", saveData.currLevel, DateTime.UtcNow.ToString(datePatt)));
    }

    public static void levelRun()
    {
        log.Add(string.Format("{0}-run-{1}", saveData.currLevel, DateTime.UtcNow.ToString(datePatt)));
    }
    public static void checkingSub(string sub)
    {
        log.Add(string.Format("{0}-checkSub-{1}", saveData.currLevel, sub));
    }

    public static int getCurrLevel() {
        return saveData.currLevel;
    }

    public static void setCurrLevel(int l)
    {
        saveData.currLevel = l;
    }

    public static bool[] getCompletedLevels() {
        return saveData.completedLevels;
    }

    public static void levelPassed()
    {
        try
        {
		    saveData.completedLevels[saveData.currLevel] = true;
        }
        catch (Exception ex)
        {

        }
        saveData.currLevel += 1;
        log.Add(string.Format("{0}-passed-{1}", saveData.currLevel, DateTime.UtcNow.ToString(datePatt)));

        Wrapper.Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.Circuits, saveData);
    }



    public static string getNextScene()
    {
        string outString = String.Join(",", saveData.completedLevels.Select(passed => passed ? "1" : "0"));
        outString += "\n";
        outString += String.Join("\n", log);
        Debug.Log("LOG");
        Debug.Log(String.Join("\n", log));
        Debug.Log(outString);

        if (!tutorialShown)
        {
            tutorialShown = true;

        }
        int offset = 0;
        tutorialShown = true;
        switch (saveData.currLevel - offset)
        {
            case 3:
                return "Circuits_Dialogue";
            case 6:
                return "Circuits_Dialogue";
            case 9:
                return "Circuits_Dialogue";
            case 14:
                return "Circuits_Dialogue";
            case 18:
                return "Circuits_Dialogue";
            case 22:
                return "Circuits_Dialogue";
            case 25:
                return "Circuits_Dialogue";
            default:
                tutorialShown = false;
                return "CircuitsLevelScene";
        }
    }


    //public Scen
}
