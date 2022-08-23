using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qupcakery
{
    public static class TutorialManager
    {
        public static bool IntroPlayed = false; 
        public static bool[] tutorialAvailable = new bool[Constants.MaxLevelCnt+1];
        static int[] tutorialInd = new int[] { 1, 3, 8, 9, 13, 14, 16, 23, 24 };

        public static void UpdateAvailability(int finishedLevel = -1)
        {
            if (finishedLevel == -1)
            {
                foreach (int ind in tutorialInd)
                {
                    if (ind > GameManagement.Instance.game.gameStat.MaxLevelCompleted)
                        tutorialAvailable[ind] = true;
                    else
                        tutorialAvailable[ind] = false;
                }
                if (GameManagement.Instance.game.gameStat.MaxLevelCompleted >= 1)
                    IntroPlayed = true;
            }
            else
            {             
                tutorialAvailable[finishedLevel] = false;             
            }
        }
    }
}
