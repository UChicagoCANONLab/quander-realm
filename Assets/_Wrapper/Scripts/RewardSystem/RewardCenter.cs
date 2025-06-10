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
        // [SerializeField] private GameObject PuzzlesOfTheDay; // left blue screen
        [SerializeField] private GameObject BadgeBulletinCanvas; // right blue screen
        [SerializeField] private GameObject RewardJournalCanvas; // journal

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
            // RewardJournalCanvas.GetComponent<Animator>().SetBool("On", false);
            // BadgeBulletinCanvas.GetComponent<Animator>().SetBool("On", false);

            RewardCenterAnimator.SetBool("On", true);
        }


        public void openRewardJournal() {
            RewardCenterAnimator.SetTrigger("Fader");
            RewardCenterAnimator.SetBool("On", false);
            RewardJournalCanvas.GetComponent<RewardJournal>().ToggleRewardJournalAnim(true);
            RewardJournalCanvas.GetComponent<RewardJournal>().InitFirstPage();
        }

        public void openBadgeBulletin() {
            RewardCenterAnimator.SetTrigger("Fader");
            RewardCenterAnimator.SetBool("On", false);
            BadgeBulletinCanvas.GetComponent<Animator>().SetBool("On", true);
            BadgeBulletinCanvas.GetComponent<BadgeManager>().DelayGetStars();
        }


        public bool returnToRewardCenter() {
            Events.Delay?.Invoke(0.5f);

            if (RewardCenterAnimator.GetBool("On") == false) 
            {
                RewardCenterAnimator.SetTrigger("Fader");

                RewardJournalCanvas.GetComponent<RewardJournal>().ToggleRewardJournalAnim(false);
                BadgeBulletinCanvas.GetComponent<Animator>().SetBool("On", false);

                RewardCenterAnimator.SetBool("On", true);
                return true;
            } 
            return false;
        }

    }
}