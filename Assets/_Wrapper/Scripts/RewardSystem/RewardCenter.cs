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
        [SerializeField] private GameObject ComputerMinigameCanvas; // Left monitor

        [SerializeField] private GeneralTrackers generalTrackers;


        [Header("Animators")]
        [SerializeField] private Animator RewardCenterAnimator;

        [Header("Back Button")]
        [SerializeField] BackButton gameBackButton;



        /* private void OnEnable()
        {
            Events.ReturnToRewardCenter += returnToRewardCenter;
        }
        private void OnDisable()
        {
            Events.ReturnToRewardCenter -= returnToRewardCenter;
        } */

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

        //Called by ComputerMinigame OnClick
        //Fades out the lab and opens the computer minigame landing page
        public void openComputerMinigame() {
            RewardCenterAnimator.SetTrigger("Fader");// fade transition
            RewardCenterAnimator.SetBool("On", false);//hide lab
            // TODO: ComputerMinigameCanvas.GetComponent<Animator>().SetBool("On", true);
            ComputerMinigameCanvas.SetActive(true);
            Events.StartDialogueSequence?.Invoke("CM_Intro");
            Debug.Log("Computer Minigame opened"); // temporary test
        }
        // Called by SuperconductingCard OnClick
        // Opens the Superconducting assembly panel
        public void openSuperconducting() {
            Debug.Log("Superconducting selected");
        }

        // Called by NeutralAtomCard OnClick
        // Opens the Neutral Atom assembly panel
        public void openNeutralAtom() {
            Debug.Log("Neutral Atom selected");
        }

        public void returnToRewardCenter() {
            Events.Delay?.Invoke(0.5f);

            if (RewardCenterAnimator.GetBool("On") == false) 
            {
                RewardCenterAnimator.SetTrigger("Fader");

                RewardJournalCanvas.GetComponent<RewardJournal>().ToggleRewardJournalAnim(false);
                BadgeBulletinCanvas.GetComponent<Animator>().SetBool("On", false);

                RewardCenterAnimator.SetBool("On", true);

                // TODO: close ComputerMinigameCanvas here once canvas exists
            } 
        }

        // original -- used by BackButton.cs
        /* public bool returnToRewardCenter() {
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
        } */

    }
}