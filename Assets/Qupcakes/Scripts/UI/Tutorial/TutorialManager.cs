using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qupcakery
{
    public static class TutorialManager
    {
        public static bool[] tutorialAvailable = new bool[25];
        static int[] tutorialInd = new int[] { 0, 1, 3, 8, 10, 11, 12, 13, 21, 22 };

        public static void UpdateAvailability(int finishedLevel = -1)
        {
            if (finishedLevel == -1)
            {
                foreach (int ind in tutorialInd)
                {
                    tutorialAvailable[ind] = true;
                }
            }
            else
            {             
                tutorialAvailable[finishedLevel] = false;             
            }
        }
    }
}
