using System;
using System.Collections;
using UnityEngine;
using Wrapper;

namespace Wrapper
{
    public class GamePopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private QButton okButton;
        [SerializeField] private QButton backgroundButton;

        private void Awake()
        {
            okButton.onClick.AddListener(() => ToggleDisplay(false));
            backgroundButton.onClick.AddListener(() => ToggleDisplay(false));
        }

        public IEnumerator DisplayGame(Game game)
        {
            Events.PlaySound?.Invoke("W_Reward");

            // Reset Game display
            animator.SetInteger("Game", -1);
            animator.SetInteger("Game", (int)game);
            
            ToggleDisplay(true);

            while (animator.GetInteger("Game") == -1)
                yield return null;
        }

        private void ToggleDisplay(bool isOn)
        {
            animator.SetBool("PopupOn", isOn);
        }

    }
}
