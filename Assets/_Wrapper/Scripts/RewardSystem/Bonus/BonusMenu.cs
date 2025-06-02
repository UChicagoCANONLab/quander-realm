using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class BonusMenu : MonoBehaviour
    {
        [SerializeField] private ExitMenu parentMenu;
        [SerializeField] private GameObject quickSelectHolder;
        [SerializeField] private BonusConfirmation confirmationPopup;
        [SerializeField] private QButton backgroundButton;
        [SerializeField] private Animator animator;

        private bool isOn = false;
        private bool confirmationOn = false;

        private Bonus[] usableBonuses;
        private Bonus[] inactiveBonuses;


        void Awake()
        {
            backgroundButton.onClick.AddListener(() => CloseBonusMenu());
        }


        public void MakePurchase(int coins)
        {
            // must be negative to work properly
            Events.UpdateUserSaveTotalCoins.Invoke(coins);
            parentMenu.localTrackerPanel.UpdateDisplay();
            CloseBonusMenu();
        }


        public void ToggleBonusMenu()
        {
            isOn = !isOn;
            animator.SetBool("IsOn", isOn);
        }
        
        public void ToggleConfirmation()
        {
            confirmationOn = !confirmationOn;
            animator.SetBool("ConfirmationOn", confirmationOn);
        }

        public void CloseBonusMenu()
        {
            animator.SetBool("IsOn", false);
            animator.SetBool("ConfirmationOn", false);
            isOn = false;
        }
    }
}