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

        [SerializeField] private GameObject SuperconductingPanel;

        [SerializeField] private GameObject ComputerCardsPanel;
        [SerializeField] private GameObject TitlePanel;


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

        // Track if dialogue has been shown already
        private bool cmIntroSeen = false;

        //Called by ComputerMinigame OnClick
        //Fades out the lab and opens the computer minigame landing page
        public void openComputerMinigame() {
            RewardCenterAnimator.SetTrigger("Fader");// fade transition
            RewardCenterAnimator.SetBool("On", false);//hide lab
            ComputerMinigameCanvas.SetActive(true);// show computer minigame canvas
            SuperconductingPanel.SetActive(false);
            ComputerCardsPanel.SetActive(true); 
            TitlePanel.SetActive(true);

            // Only play intro dialogue on first visit
            if (!cmIntroSeen)
            {
                Events.StartDialogueSequence?.Invoke("CM_Intro");
                cmIntroSeen = true;
            }
            Debug.Log("Computer Minigame opened"); 
        }

        // Called by BackButton inside SuperconductingPanel
        // Returns to the computer landing page without replaying dialogue
        public void showComputerLanding() {
            SuperconductingPanel.SetActive(false);
            ComputerCardsPanel.SetActive(true);
            TitlePanel.SetActive(true);
            // landing page elements are already visible inside ComputerMinigame_View
            Debug.Log("Returned to computer landing");
        }

        // Called by SuperconductingCard OnClick
        // Opens the Superconducting assembly panel
        public void openSuperconducting() {
            
            ComputerCardsPanel.SetActive(false);
            TitlePanel.SetActive(false);

            // Show assembly panel
            SuperconductingPanel.SetActive(true);
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
                ComputerMinigameCanvas.SetActive(false);

                RewardCenterAnimator.SetBool("On", true);
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