using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            backgroundButton.onClick.AddListener(() => CloseBonusMenu());
        }

        void Start()
        {
            InitBonusMenu();
        }

        public void SelectBonus(Bonus b)
        {
            selectedBonus = b;
            confirmationPopup.InitConfirmation(b);
            ToggleConfirmation();
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


        public void MakePurchase()
        {
            if (!Events.AddBonus(selectedBonus.bonusID)) return;
            // must be negative to work properly
            Events.UpdateUserSaveTotalCoins.Invoke(-1*selectedBonus.cost[0]);
            
            parentMenu.localTrackerPanel.UpdateDisplay();
            confirmationPopup.UpdateConfirmation();
            selectedBonus.UpdateBonus();
        }

        public void UseBonus()
        {
            selectedBonus.UseBonus();
            animator.SetTrigger("UseBonus");
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

            foreach(GameObject bo in usableBonuses)
            {
                bo.GetComponent<Button>().interactable = !confirmationOn;
            }
        }

        public void CloseBonusMenu()
        {
            animator.SetBool("IsOn", false);
            animator.SetBool("ConfirmationOn", false);
            isOn = false; confirmationOn = false;
        }
    }
}