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

        private bool buyable = true;
        private bool available = true;
        private bool usable = true;



        public void InitConfirmation(Bonus bonus)
        {
            if (currBonus != null)
            {
                CloseConfirmationButton();
            }
            currBonus = Instantiate(bonus.gameObject, bonusHolder.transform).GetComponent<Bonus>();
            currBonus.DisplayOnly(true);

            bonusName.text = $"{currBonus.title}";
            bonusDescription.text = $"{currBonus.effect}";
            bonusCost.text = $"{currBonus.cost[0]}";

            // if ((bonus.numberAvailable > 0) && bonus.currentlyUsable) useButton.interactable = true;
            currBonus.UpdateBonus();
            SetConfirmationType();
        }

        public void SetConfirmationType()
        {
            available = true; usable = true; buyable = true;

            // Check if bonus available
            if (currBonus.numberAvailable == 0) available = false;
            // Check if able to use bonus
            if (!currBonus.currentlyUsable) usable = false;
            // Check if able to buy bonus
            if (Events.GetUserSaveTotalCoins.Invoke() < currBonus.cost[0]) buyable = false;

            // Set confirmation message
            if (usable == true) {
                if (buyable == true) {
                    if (available == true) {
                        confirmationMessage.text = "Buy or use a bonus!";
                    }
                    else confirmationMessage.text = "Buy this bonus to use it";
                }
                else confirmationMessage.text = "Get more coins to buy this bonus";
            }
            else confirmationMessage.text = "You cannot use this bonus here";

            // Set button interactability
            useButton.interactable = (available && usable);
            buyButton.interactable = buyable;
        }

        public void UpdateConfirmation()
        {
            currBonus.UpdateBonus();

            // if ((currBonus.numberAvailable > 0) && currBonus.currentlyUsable) useButton.interactable = true;
            SetConfirmationType();
        }

        public void ResetCurrentBonus()
        {
            currBonus = null;
            foreach (Transform child in bonusHolder.transform)
                Destroy(child.gameObject);
        }


        public void CloseConfirmationButton()
        {
            parentMenu.ToggleConfirmation();
            ResetCurrentBonus();
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