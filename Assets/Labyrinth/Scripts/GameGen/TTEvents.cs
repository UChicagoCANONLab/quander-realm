using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Labyrinth
{
    public static class TTEvents
    {
        // PlayerMovement
        public static Action StartPlayerMovement;
        public static Action SwitchPlayer;
        public static Func<string,Vector3> GetButtonPress;
        public static Func<int, Player> GetPlayer;
        public static Action ReturnPlayers;

        // Maze
        public static Action StartMaze;
        public static Action GenerateMazes;
        public static Action RenderMazes;
        public static Action ClearGoal;
        public static Func<int,int,int,int,string> PathFinder;
        public static Func<object[]> CalculatePathToGoal;
        public static Func<int,TM> GetMap;
        
        // UIManager
        public static Action<string> SetLevelNumber;
        public static Action<int> SetProgressBar;
        public static Action<int> UpdateProgressBar;
        public static Action ResetUI;
        public static Action<int> LevelComplete;
        // InfoPopup
        public static Action ShowInfoMessage;
        // ButtonBehavior
        public static Action<int> SelectLevel;

        // GameBehavior
        public static Action GiveHint;
        public static Action RestartLevel;
        public static Func<int> Size;
        public static Func<double> WallProb;
        public static Action CollectGoal;
        public static Action IncrementSteps;

        



    }
}