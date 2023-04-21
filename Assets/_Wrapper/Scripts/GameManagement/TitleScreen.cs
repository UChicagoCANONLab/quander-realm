using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class TitleScreen : MonoBehaviour
    {
        [Header("Main Components"), SerializeField]
        GameObject mainContainer;
        [SerializeField]
        Button playButton;
        [SerializeField]
        Button continueButton;
        [SerializeField]
        Button newButton;
        [SerializeField]
        Button logoutButton;
        [SerializeField]
        ConfirmationPopup newGameConfirm;
        [SerializeField]
        ConfirmationPopup loadingPanel; // using code just for open/close consistency

        [Space, SerializeField]
        Button creditsButton;
        [SerializeField]
        GameObject creditsPanel;

        [Space, SerializeField]
        Button learnMoreButton;
        [SerializeField]
        ConfirmationPopup learnMoreConfirm;
        [SerializeField, Header("More Info URL")]
        string moreInfoURL = "https://www.epiqc.cs.uchicago.edu/zines";

        bool newPlayer = true;

        private void OnEnable()
        {
            // events
            Events.SetNewPlayerStatus += SetNewPlayer;
            Events.ToggleTitleScreen += ToggleTitleScreen;

            // buttons
            playButton.onClick.AddListener(PlayGame);
            continueButton.onClick.AddListener(ContinueGame);
            newButton.onClick.AddListener(OpenNewConfirm);
            logoutButton.onClick.AddListener(LogOut);
            creditsButton.onClick.AddListener(OpenCredits);
            learnMoreButton.onClick.AddListener(OpenLearnConfirm);
        }

        private void OnDisable()
        {
            // events
            Events.SetNewPlayerStatus -= SetNewPlayer;
            Events.ToggleTitleScreen -= ToggleTitleScreen;

            // buttons
            playButton.onClick.RemoveListener(PlayGame);
            continueButton.onClick.RemoveListener(ContinueGame);
            newButton.onClick.RemoveListener(OpenNewConfirm);
            logoutButton.onClick.RemoveListener(LogOut);
            creditsButton.onClick.RemoveListener(OpenCredits);
            learnMoreButton.onClick.RemoveListener(OpenLearnConfirm);
        }

        void ToggleTitleScreen(bool enable)
        {
            mainContainer.SetActive(enable);
            //creditsPanel.SetActive(false);
            newGameConfirm.ForceCloseConfirmation();
            learnMoreConfirm.ForceCloseConfirmation();
            loadingPanel.ForceCloseConfirmation();

        }

        void SetNewPlayer(bool hasData)
        {
            newPlayer = !hasData;

            if (newPlayer)
            {
                playButton.gameObject.SetActive(true);
                continueButton.gameObject.SetActive(false);
                newButton.gameObject.SetActive(false);
            }
            else
            {
                playButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(true);
                newButton.gameObject.SetActive(true);
            }
            // logout button is always active
        }

        void ContinueGame()
        {
            Events.ToggleTitleScreen?.Invoke(false);
        }

        void OpenNewConfirm()
        {
            newGameConfirm.SetConfirmationData(NewGame, null);
            newGameConfirm.OpenConfirmation();
        }

        void NewGame()
        {
            // clear data and show loading screen for a bit, then refresh title screen
            loadingPanel.OpenConfirmation();
            Events.ClearSaveFile?.Invoke();

            //Events.ToggleTitleScreen?.Invoke(false);
        }

        void PlayGame()
        {
            // same as continue with new save data
            Events.ToggleTitleScreen?.Invoke(false);
        }

        void LogOut()
        {
            Events.Logout?.Invoke();
        }

        void OpenCredits()
        {
            // TODO: implement when ready
        }

        void OpenLearnConfirm()
        {
            learnMoreConfirm.SetConfirmationData(OpenLearnMore, null);
            learnMoreConfirm.OpenConfirmation();
        }

        void OpenLearnMore()
        {
            Debug.Log("OPEN URL: " + moreInfoURL);
            Application.OpenURL(moreInfoURL);
        }
    }
}