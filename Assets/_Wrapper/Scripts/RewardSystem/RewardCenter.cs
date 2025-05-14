using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeauRoutine;
using TMPro;

namespace Wrapper
{
    public class RewardCenter : MonoBehaviour
    {
        [Header("Navigation Buttons")]
        [SerializeField] private GameObject DailyQuests;
        [SerializeField] private GameObject PuzzlesOfTheDay;
        [SerializeField] private GameObject BadgeBulletinCanvas;
        [SerializeField] private GameObject RewardJournalCanvas;

        // [Header("Numerical Counters")]
        // [SerializeField] private TextMeshProUGUI StarTracker;
        // [SerializeField] private TextMeshProUGUI CoinTracker;
        // [SerializeField] private TextMeshProUGUI StreakTracker;
        [SerializeField] private GeneralTrackers generalTrackers;


        [Header("Animators")]
        [SerializeField] private Animator RewardCenterAnimator;

        [Header("Back Button")]
        [SerializeField] BackButton gameBackButton;



        private void OnEnable()
        {
            Events.ReturnToRewardCenter += returnToRewardCenter;
        }
        private void OnDisable()
        {
            Events.ReturnToRewardCenter -= returnToRewardCenter;
        }

        private void Awake()
        {
            generalTrackers.UpdateDisplay();
        }

        private void Start()
        {
            Events.Delay?.Invoke(0.5f);
            // DelayStart();
            RewardJournalCanvas.GetComponent<Animator>().SetBool("On", false);
            BadgeBulletinCanvas.GetComponent<Animator>().SetBool("On", false);

            // SetRewardCenterTrackers();
            RewardCenterAnimator.SetBool("On", true);
        }


        public void openRewardJournal() {
            RewardCenterAnimator.SetBool("On", false);
            RewardJournalCanvas.GetComponent<Animator>().SetBool("On", true);
            RewardJournalCanvas.GetComponent<RewardJournal>().InitFirstPage();
        }

        public void openBadgeBulletin() {
            RewardCenterAnimator.SetBool("On", false);
            BadgeBulletinCanvas.GetComponent<Animator>().SetBool("On", true);
            BadgeBulletinCanvas.GetComponent<BadgeManager>().DelayGetStars();
        }


        public bool returnToRewardCenter() {
            Events.Delay?.Invoke(0.5f);

            if (RewardCenterAnimator.GetBool("On") == false) 
            {
                RewardJournalCanvas.GetComponent<Animator>().SetBool("On", false);
                BadgeBulletinCanvas.GetComponent<Animator>().SetBool("On", false);

                RewardCenterAnimator.SetBool("On", true);
                return true;
            } 
            return false;
        }

        // public void SetRewardCenterTrackers() {
        //     StarTracker.text = $"{Events.GetOverallTotalStars.Invoke()}";
        //     CoinTracker.text = $"{Events.GetUserSaveTotalCoins.Invoke()}";
        //     StreakTracker.text = $"{Events.GetStreakLength.Invoke()}";
        // }

    }
}