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
        [SerializeField] private GameObject useButton;
        [SerializeField] private GameObject buyButton;
        [SerializeField] private TextMeshProUGUI bonusCost;

        private string[] message = {
            "Would you like to use this bonus?",
            "Purchase and use this bonus?",
            "You cannot use this bonus here. Purchase anyway?"
        };

        public void InitConfirmation(Bonus bonus)
        {
            currBonus = bonus;

        }
    }
}