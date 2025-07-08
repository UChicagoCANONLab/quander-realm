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

            if (coins < 0) {
                coinNum.text = $"{coins}";
            } 
            else {
                coinNum.text = $"+{coins}";
            }
            

            ToggleDisplay(true);
            yield return 2f;
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
