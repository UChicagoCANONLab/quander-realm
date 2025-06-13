using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Wrapper
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] public GeneralTrackers localTrackerPanel;
        [SerializeField] private TMP_Text researchCodeText;

        [Header("Display Objects")]
        [SerializeField] private Trackers trackers; // on map scene
        [SerializeField] private QButton backgroundButton;
        [SerializeField] private BackButton exitButton;
        [SerializeField] private LoadingPopup loadingPanel;

        [Header("Bonuses")]
        [SerializeField] private BonusMenu bonusMenu;
        [SerializeField] private Button bonusButton;

        [Header("Credits and More Info")]
        [SerializeField] private ConfirmationPopup learnMoreConfirm;
        [SerializeField] private QButton creditsButton;
        [SerializeField] private QButton moreInfoButton;
        [SerializeField] private string moreInfoURL = "https://www.epiqc.cs.uchicago.edu/zines";
        [SerializeField] private CreditsFiller creditsPanel;
        [SerializeField] private Credits creditsData;

        [Header("Logout and New Game")]
        [SerializeField] private ConfirmationPopup newGameConfirm;
        [SerializeField] private QButton newGameButton;
        [SerializeField] private QButton logoutButton;

        private bool isOn = false;



        private void Awake()
        {
            backgroundButton.onClick.AddListener(CloseMenu);
            exitButton.onClick.AddListener(CloseMenu);
            bonusButton.onClick.AddListener(() => bonusMenu.ToggleBonusMenu());

            newGameButton.onClick.AddListener(OpenNewConfirm);
            logoutButton.onClick.AddListener(Logout);
            creditsButton.onClick.AddListener(OpenCredits);
            moreInfoButton.onClick.AddListener(OpenLearnConfirm);
        }

        private void OnDestroy()
        {
            backgroundButton.onClick.RemoveListener(CloseMenu);
            exitButton.onClick.RemoveListener(CloseMenu);
            bonusButton.onClick.RemoveListener(() => bonusMenu.ToggleBonusMenu());

            newGameButton.onClick.RemoveListener(OpenNewConfirm);
            logoutButton.onClick.RemoveListener(Logout);
            creditsButton.onClick.RemoveListener(OpenCredits);
            moreInfoButton.onClick.RemoveListener(OpenLearnConfirm);
        }

        public void UpdateMenu()
        {
            researchCodeText.text = $"ID: {Events.GetPlayerResearchCode.Invoke()}";
            localTrackerPanel.UpdateDisplay();
            
            bool main = false;
            if (SceneManager.GetActiveScene().name == "W_Main")
            {
                main = true;
                trackers.ToggleTrackers(isOn);
            }             
            exitButton.interactable = !main;
            // creditsButton.interactable = main;
            newGameButton.interactable = main;
            logoutButton.interactable = main;
            
        }


        // Navigation functions

        public void ToggleMenu()
        {
            /* if (SceneManager.GetActiveScene().name == "W_Main")
            {
                trackers.ToggleTrackers(isOn);
            }
            localTrackerPanel.UpdateDisplay(); */
            UpdateMenu();
            animator.SetBool("IsOn", !isOn);
            isOn = !isOn;
        }

        public void CloseMenu()
        {
            if (SceneManager.GetActiveScene().name == "W_Main")
            {
                trackers.ToggleTrackers(true);
            }
            animator.SetBool("IsOn", false);
            isOn = false;
        }

        public void DEBUG()
        {
            Debug.Log("CLICKED BUTTON");
        }

        // Button Functions

        public void OpenNewConfirm()
        {
            newGameConfirm.SetConfirmationData(NewGame, null);
            newGameConfirm.OpenConfirmation();
        }

        public void NewGame()
        {
            // clear data and show loading screen for a bit, then refresh title screen
            loadingPanel.OpenPopup();
            Events.ClearSaveFile?.Invoke();

            Events.ResetStarCounts?.Invoke();
            Events.ResetMap?.Invoke();
            //Events.SetNewPlayerStatus?.Invoke(true);
            
            CloseMenu();
            // Events.Delay.Invoke(2f);
            // Events.ToggleTitleScreen.Invoke(true);
        }

        public void Logout()
        {
            Events.Logout?.Invoke();
            CloseMenu();
        }

        public void OpenCredits()
        {
            Events.ScreenFadeMidAction?.Invoke(() =>
            {
                creditsPanel.gameObject.SetActive(true);
                creditsPanel.LoadCredits(creditsData);
            }, 0.1F);
        }

        public void OpenLearnConfirm()
        {
            learnMoreConfirm.SetConfirmationData(OpenLearnMore, null);
            learnMoreConfirm.OpenConfirmation();
        }

        public void OpenLearnMore()
        {
            Debug.Log("OPEN URL: " + moreInfoURL);
            Application.OpenURL(moreInfoURL);
        }

    }
}