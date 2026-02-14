using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qupcakery
{
    public static class TutorialManager
    {
        public static bool IntroPlayed = false; 
        public static bool OutroPlayed = false;
        public static bool[] tutorialAvailable = new bool[Constants.MaxLevelCnt+1];

#if LITE_VERSION
    public static int[] tutorialInd = new int[] { 1, 3, 8, 13, 16 };
#else
    public static int[] tutorialInd = new int[] { 1, 2, 3, 6, 9, 13, 23, 24 };
#endif

        public static void UpdateAvailability(int finishedLevel = -1)
        {
            if (finishedLevel == -1)
            {
                // Disabled dialogue if not intro/outro now that tutorials are implemented
                /* foreach (int ind in tutorialInd)
                {
                    if (ind > GameManagement.Instance.game.gameStat.MaxLevelCompleted)
                        tutorialAvailable[ind] = true;
                    else
                        tutorialAvailable[ind] = false;
                } */
                if (GameManagement.Instance.game.gameStat.MaxLevelCompleted >= 1) {
                    IntroPlayed = true;
                }
            }
            else if (GameManagement.Instance.game.gameStat.MaxLevelCompleted
            == GameManagement.Instance.GetTotalLevelCnt() - 1) 
            {
                    OutroPlayed = true;
            }
            else
            {             
                tutorialAvailable[finishedLevel] = false;             
            }
        }
    }
}
