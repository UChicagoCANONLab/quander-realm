using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Wrapper
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] public GeneralTrackers localTrackerPanel;
        [SerializeField] private Trackers trackers; // on map scene
        [SerializeField] private BonusMenu bonusMenu;

        [Header("Buttons")]
        [SerializeField] private QButton backgroundButton;
        [SerializeField] private BackButton exitButton;
        [SerializeField] private Button bonusButton;

        private bool isOn = false;



        void Awake()
        {
            backgroundButton.onClick.AddListener(() => CloseMenu());
            exitButton.onClick.AddListener(() => CloseMenu());
            bonusButton.onClick.AddListener(() => bonusMenu.ToggleBonusMenu());
        }



        // Navigation functions

        public void ToggleMenu()
        {
            if (SceneManager.GetActiveScene().name == "W_Main")
            {
                trackers.ToggleTrackers(isOn);
            }
            localTrackerPanel.UpdateDisplay();
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
    }
}