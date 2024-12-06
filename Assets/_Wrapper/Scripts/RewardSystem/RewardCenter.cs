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


        [Header("Animators")]
        [SerializeField] private Animator RewardCenterAnimator;
        [SerializeField] private Animator RewardJournalAnimator;
        [SerializeField] private Animator BadgeBulletinAnimator;


        public void openRewardJournal() {
            RewardCenterAnimator.SetBool("On", false);
            RewardJournalAnimator.SetBool("On", true);
        }

        public void openBadgeBulletin() {
            RewardCenterAnimator.SetBool("On", false);
            BadgeBulletinAnimator.SetBool("On", true);
        }
    }
}