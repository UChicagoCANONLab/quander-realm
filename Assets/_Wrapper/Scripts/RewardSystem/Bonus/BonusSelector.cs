using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wrapper
{
    public class BonusSelector : MonoBehaviour
    {
        [SerializeField] public Bonus bonus;
        [SerializeField] private int numberBuyable;

        private bool inSelector = false;
        private int numberUsed = 0;
        private int numberUnlocked = 0;

        // [Header("Display GameObjects: General")]
        // [SerializeField] private Image gemType;
        // [SerializeField] private TextMeshProUGUI titleText;

        [SerializeField] private GameObject bonusContainer;
        [SerializeField] private GameObject bonusPrefab;

        [Header("Display GameObjects: In Shop")]
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI numAvailableText;

        // [Header("Display GameObjects: Usable")]
        // [SerializeField] private TextMeshProUGUI numUsableText;
        // [SerializeField] private GameObject counterObject;


        public void InitSelector(Bonus bonus)
        {
            this.bonus = bonus;

            descriptionText.text = bonus.unlockCriteria;
            costText.text = $"{bonus.cost}";

            numberBuyable = bonus.CalculateNumberBuyable();
            numAvailableText.text = $"{numberBuyable}";

            bonus.counterObject.SetActive(false);

        }

    }
}