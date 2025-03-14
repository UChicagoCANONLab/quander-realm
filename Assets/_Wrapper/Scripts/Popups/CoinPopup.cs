using System;
using System.Collections;
using UnityEngine;
using Wrapper;
using TMPro;

namespace Wrapper
{
    public class CoinPopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        // [SerializeField] private QButton okButton;
        [SerializeField] private QButton backgroundButton;
        [SerializeField] private TMP_Text coinNum;

        private void Awake()
        {
            // okButton.onClick.AddListener(() => ToggleDisplay(false));
            backgroundButton.onClick.AddListener(() => ToggleDisplay(false));
        }

        public IEnumerator DisplayCoins(int coins)
        {
            Events.PlaySound?.Invoke("W_Reward");

            coinNum.text = $"+{coins}";

            ToggleDisplay(true);
            Events.Delay(2f);
            ToggleDisplay(false);

            while(coinNum.text == "") 
                yield return null;
        }

        private void ToggleDisplay(bool isOn)
        {
            animator.SetBool("PopupOn", isOn);
        }

    }
}
