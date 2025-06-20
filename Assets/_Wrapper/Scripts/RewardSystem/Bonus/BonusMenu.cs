using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeauRoutine;

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

        [SerializeField] private Bonus selectedBonus = null;
        private List<GameObject> bonusHolder = new List<GameObject>();


        void Start()
        {
            InitBonusMenu();
        }

        public void InitBonusMenu()
        {
            foreach (BonusAsset boAsset in GameManager.Instance.bonusAssets)
            {
                GameObject bonusGO = Events.CreateBonus?.Invoke(boAsset, quickSelectHolder);
                bonusHolder.Add(bonusGO);
                bonusGO.GetComponent<Button>().onClick.AddListener(() => SelectBonus(bonusGO.GetComponent<Bonus>()));
            }
        }

        public void SelectBonus(Bonus b)
        {
            selectedBonus = b;
            confirmationPopup.InitConfirmation(b);
            ToggleConfirmation();
        }

        // Called from BuyBonus in BonusConfirmation
        public void BuyBonusUpdate()
        {
            if (!Events.AddBonus(selectedBonus.bonusID)) return;
            Events.UpdateUserSaveTotalCoins.Invoke(-1*selectedBonus.cost[0]); // must be negative
            
            parentMenu.localTrackerPanel.UpdateDisplay();
            selectedBonus.UpdateBonus();
        }

        // Called from UseBonus in BonusConfirmation
        public void UseBonusUpdate()
        {
            Routine.Start(UseBonusRoutine());
        }
        public IEnumerator UseBonusRoutine()
        {
            animator.SetTrigger("BonusUsed");
            yield return 3f;
            CloseBonusMenu();
            parentMenu.CloseMenu();
        }




        public void ToggleBonusMenu()
        {
            isOn = !isOn;
            animator.SetBool("IsOn", isOn);
            // if (isOn) parentMenu.backgroundButton.onClick.AddListener(CloseBonusMenu);
            // else parentMenu.backgroundButton.onClick.RemoveListener(CloseBonusMenu);
        }
        
        public void ToggleConfirmation()
        {
            confirmationOn = !confirmationOn;
            animator.SetBool("ConfirmationOn", confirmationOn);

            /* foreach(GameObject bo in bonusHolder)
            {
                bo.GetComponent<Button>().interactable = !confirmationOn;
            } */
            if (!confirmationOn) 
            {
                // selectedBonus = null;
                confirmationPopup.ResetCurrentBonus();
            }
        }

        public void CloseBonusMenu()
        {
            if (isOn) ToggleBonusMenu();
            if (confirmationOn) ToggleConfirmation();
            selectedBonus = null;
        }
    }
}