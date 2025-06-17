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

        private Bonus selectedBonus = null;
        private List<GameObject> usableBonuses = new List<GameObject>();
        private List<GameObject> inactiveBonuses;


        void Awake()
        {
            // backgroundButton.onClick.AddListener(() => CloseBonusMenu());
        }

        void Start()
        {
            InitBonusMenu();
        }

        public void InitBonusMenu()
        {
            foreach (BonusAsset boAsset in GameManager.Instance.bonusAssets)
            {
                GameObject bonusGO = Events.CreateBonus?.Invoke(boAsset, quickSelectHolder);
                usableBonuses.Add(bonusGO);
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
            // selectedBonus.UseBonus(); // Called from BonusConfirmation instead
            /* animator.SetTrigger("BonusUsed");
            CloseBonusMenu();
            parentMenu.CloseMenu(); */

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

            foreach(GameObject bo in usableBonuses)
            {
                bo.GetComponent<Button>().interactable = !confirmationOn;
            }
            if (!confirmationOn) selectedBonus = null;
        }

        public void CloseBonusMenu()
        {
            // if (isOn) parentMenu.backgroundButton.onClick.RemoveListener(CloseBonusMenu);
            // animator.SetBool("IsOn", false);
            // animator.SetBool("ConfirmationOn", false);
            // isOn = false; confirmationOn = false;

            if (isOn) ToggleBonusMenu();
            if (confirmationOn) ToggleConfirmation();
        }
    }
}