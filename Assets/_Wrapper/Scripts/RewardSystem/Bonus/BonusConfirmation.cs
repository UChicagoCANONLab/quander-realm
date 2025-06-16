using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wrapper
{
    public class BonusConfirmation : MonoBehaviour
    {
        [SerializeField] private BonusMenu parentMenu;

        [Header("Bonus Information")]
        [SerializeField] private Bonus currBonus;
        [SerializeField] private GameObject bonusHolder;
        [SerializeField] private TextMeshProUGUI bonusName;
        [SerializeField] private TextMeshProUGUI bonusDescription;

        [Header("Use/Buy Information")]
        [SerializeField] private TextMeshProUGUI confirmationMessage;
        [SerializeField] private Button useButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI bonusCost;

        private string[] message = {
            "Purchase or use this bonus",
            "You cannot use this bonus here",
            "Buy a bonus to use it"
        };

        /* 
        OPTIONS:
            Use pre-bought bonus
            Purchase without using
            Purchase and use immediately 
        */


        public void InitConfirmation(Bonus bonus)
        {
            currBonus = Instantiate(bonus.gameObject, bonusHolder.transform).GetComponent<Bonus>();
            currBonus.DisplayOnly(true);

            bonusName.text = $"{currBonus.title}";
            bonusDescription.text = $"{currBonus.effect}";
            bonusCost.text = $"{currBonus.cost[0]}";

            if ((bonus.numberAvailable > 0) && bonus.currentlyUsable) useButton.interactable = true;
            // else if (bonus.currentlyUsable) useButton.interactable = true;
        }

        public void UpdateConfirmation()
        {
            currBonus.UpdateBonus();

            if ((currBonus.numberAvailable > 0) && currBonus.currentlyUsable) useButton.interactable = true;
            // else if (currBonus.currentlyUsable) useButton.interactable = true;
        }


        public void CloseConfirmationButton()
        {
            parentMenu.ToggleConfirmation();
            currBonus = null;
            foreach (Transform child in bonusHolder.transform)
                Destroy(child.gameObject);
        }

        public void BuyBonusButton()
        {
            currBonus.UpdateBonus();
            parentMenu.BuyBonusUpdate();
            UpdateConfirmation();
        }

        public void UseBonusButton()
        {
            bool used = currBonus.UseBonus();
            if (used) parentMenu.UseBonusUpdate();
        }
    }
}