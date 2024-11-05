using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class RewardCenter : MonoBehaviour
    {
        [Header("Navigation Buttons")]
        [SerializeField] private GameObject DailyQuests;
        [SerializeField] private GameObject PuzzlesOfTheDay;
        [SerializeField] private GameObject BadgeBulletin;
        [SerializeField] private GameObject RewardJournal;

        [Header("Numerical Counters")]
        [SerializeField] private GameObject StarTracker;
        [SerializeField] private GameObject CoinTracker;
    }
}