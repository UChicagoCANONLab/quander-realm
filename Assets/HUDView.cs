using System.Collections.Generic;
using UnityEngine;

namespace BlackBox
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private GameObject unitContainer = null;
        [SerializeField] private GameObject unitPrefab = null;

        private List<Animator> reversedAnimatorList = null;

        private void OnEnable()
        {
            GameEvents.InitEnergyBar += Init;
            GameEvents.DecrementEnergy += DecrementEnergyBar;
        }

        private void OnDisable()
        {
            GameEvents.InitEnergyBar -= Init;
            GameEvents.DecrementEnergy -= DecrementEnergyBar;
        }

        private void Init(int numUnits)
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

        private void DecrementEnergyBar()
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
