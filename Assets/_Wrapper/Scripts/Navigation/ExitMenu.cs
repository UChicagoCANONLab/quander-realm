using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class ExitMenu : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GeneralTrackers trackerPanel;

        private bool isOn = false;



        public void ToggleMenu()
        {
            trackerPanel.UpdateDisplay();
            animator.SetBool("IsOn", !isOn);
            isOn = !isOn;
        }
    }
}