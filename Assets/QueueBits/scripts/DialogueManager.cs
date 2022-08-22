using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// leveraged from tianle's qupcakery

namespace QueueBits
{
    public class DialogueManager : MonoBehaviour
    {
        public static bool IntroPlayed = false;
        public static bool Level1Played = false;
        public static bool[] tutorialAvailable = new bool[15 + 1];
        static int[] tutorialInd = new int[] { 1, 3, 8, 9, 11, 13, 14};

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