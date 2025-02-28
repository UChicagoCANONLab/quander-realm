using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class RewardCenter : MonoBehaviour
    {
        [Header("Navigation Buttons")]
        [SerializeField] private GameObject DailyQuests;
        [SerializeField] private GameObject PuzzlesOfTheDay;
        [SerializeField] private GameObject BadgeBulletinCanvas;
        [SerializeField] private GameObject RewardJournalCanvas;

        [Header("Numerical Counters")]
        [SerializeField] private GameObject StarTracker;
        [SerializeField] private GameObject CoinTracker;


        [Header("Animators")]
        [SerializeField] private Animator RewardCenterAnimator;

        [Header("Back Button")]
        [SerializeField] BackButton gameBackButton;



        private void OnEnable()
        {
            Events.ReturnToRewardCenter += returnToRewardCenter;
            // gameBackButton = GameObject.Find("GameManager/BackButton").GetComponent<BackButton>();
            // gameBackButton.onClick.AddListener(returnToRewardCenter);
        }

        private void OnDisable()
        {
            Events.ReturnToRewardCenter -= returnToRewardCenter;
            // gameBackButton.onClick.RemoveListener(returnToRewardCenter);
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
            if (RewardCenterAnimator.GetBool("On") == false) 
            {
                RewardJournalCanvas.GetComponent<Animator>().SetBool("On", false);
                BadgeBulletinCanvas.GetComponent<Animator>().SetBool("On", false);

                RewardCenterAnimator.SetBool("On", true);
                return true;
            } 
            return false;
        }

    }
}