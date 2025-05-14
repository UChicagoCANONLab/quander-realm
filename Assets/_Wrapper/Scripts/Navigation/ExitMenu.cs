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
        [SerializeField] private GeneralTrackers localTrackerPanel;
        [SerializeField] private Trackers trackers;

        [Header("Buttons")]
        [SerializeField] private QButton backgroundButton;
        [SerializeField] private BackButton exitButton;

        private bool isOn = false;



        void Awake()
        {
            backgroundButton.onClick.AddListener(() => CloseMenu());
            exitButton.onClick.AddListener(() => CloseMenu());
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