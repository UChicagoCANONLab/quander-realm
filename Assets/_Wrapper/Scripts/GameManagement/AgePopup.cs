using System;
using System.Collections;
using UnityEngine;
using Wrapper;
using UnityEngine.UI;
using TMPro;

namespace Wrapper
{
    public class AgePopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject gameContainer;
        [SerializeField] private QButton okButton;
        [SerializeField] private QButton backgroundButton;
        public TMP_Dropdown ageSelector;
        public int selection;

        private void Awake()
        {
            okButton.onClick.AddListener(() => ToggleDisplay(false));
            backgroundButton.onClick.AddListener(() => ToggleDisplay(false));
        }

        public IEnumerator DisplayAgePopup()
        {
            Events.PlaySound?.Invoke("W_Reward");

            ageSelector.onValueChanged.AddListener(delegate { dropdownValueChanged(ageSelector); });
            // Reset Game display
            // ToggleGameShown(Game.Circuits, false);
            // ToggleGameShown(Game.QueueBits, false);
            // ToggleGameShown(Game.BlackBox, false);
            // GameObject gameDisplay = ToggleGameShown(game, true);
            
            ToggleDisplay(true);

            yield return null;
            // while (!(gameDisplay.activeInHierarchy))
            //     yield return null;
            // gameDisplay.GetComponent<Animator>().SetBool("Disabled", false);
        }

        public GameObject GetContainerMount()
        {
            return gameContainer;
        }

        private void ToggleDisplay(bool isOn)
        {
            animator.SetBool("PopupOn", isOn);
        }

        public void dropdownValueChanged(TMP_Dropdown selector) {
            selection = selector.value;
            Debug.Log(selector.value);
        }


    }
}
