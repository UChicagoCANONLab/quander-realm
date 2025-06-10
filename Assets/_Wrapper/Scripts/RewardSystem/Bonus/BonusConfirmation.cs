using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wrapper
{
    public class BonusConfirmation : MonoBehaviour
    {
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
            "Would you like to use this bonus?",
            "Purchase and use this bonus?",
            "You cannot use this bonus here. Purchase for later?"
        };

        /* 
        OPTIONS:
            Use pre-bought bonus
            Purchase without using
            Purchase and use immediately 
        */


        public void InitConfirmation(Bonus bonus)
        {
            foreach (Transform child in bonusHolder.transform)
                Destroy(child.gameObject);

            currBonus = bonus;
            currBonus.DisplayOnly(true);

            Instantiate(bonus.gameObject, bonusHolder.transform);

            bonusName.text = $"{currBonus.title}";
            bonusDescription.text = $"{currBonus.effect}";
            bonusCost.text = $"{currBonus.cost[0]}";

            if (bonus.numberAvailable > 0) useButton.interactable = true;
            if (bonus.currentlyUsable) useButton.interactable = true;
        }

        public void UpdateConfirmation()
        {
            currBonus.UpdateBonus();

            if (currBonus.numberAvailable > 0) useButton.interactable = true;
            if (currBonus.currentlyUsable) useButton.interactable = true;
        }
    }
}