using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private GameObject unitContainer = null;
        [SerializeField] private GameObject unitPrefab = null;
        [SerializeField] private Button wolfieButton = null;

        private Animator HUDAnimator = null;
        private List<Animator> reversedAnimatorList = null;

        private void Awake()
        {
            wolfieButton.onClick.AddListener(() => BlackBoxEvents.CheckWinState?.Invoke());
            HUDAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            BlackBoxEvents.InitEnergyBar += InitEnergyBar;
            BlackBoxEvents.IndicateEmptyMeter += IndicateEmpty;
            BlackBoxEvents.DecrementEnergy += DecrementEnergy;
        }

        private void OnDisable()
        {
            BlackBoxEvents.InitEnergyBar -= InitEnergyBar;
            BlackBoxEvents.IndicateEmptyMeter -= IndicateEmpty;
            BlackBoxEvents.DecrementEnergy -= DecrementEnergy;
        }

        private void InitEnergyBar(int numUnits)
        {
            foreach (Transform child in unitContainer.transform)
                Destroy(child.gameObject);

            reversedAnimatorList = new List<Animator>();
            for(int i = 0; i < numUnits; i++)
            {
                GameObject unit = Instantiate(unitPrefab, unitContainer.transform);
                reversedAnimatorList.Insert(0, unit.GetComponent<Animator>());
            }
        }

        private void IndicateEmpty()
        {
            HUDAnimator.SetTrigger("EmptyMeter");
        }

        private void DecrementEnergy()
        {
            foreach(Animator animator in reversedAnimatorList)
            {
                if (animator.GetBool("On"))
                {
                    animator.SetBool("On", false);
                    break;
                }
            }
        }
    }
}
