using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class ButtonBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject infoPanel = null;

        private Animator animator = null;
        private int level;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }


        public void RestartLevel() {
            BBEvents.RestartLevel?.Invoke();
        }

        public void ToLevelSelect() {
            BBEvents.CloseLevel?.Invoke();
        }

        public void QuitBlackBox() {
            BBEvents.QuitBlackBox?.Invoke();
        }

        public void StartNextLevel() {
            BBEvents.StartNextLevel?.Invoke();
        }

        public void ShowInfoPanel() {
            infoPanel.GetComponent<Animator>().SetBool("On", true);
        }

        public void AskForHint() {
            BBEvents.ShowHint?.Invoke();
        }

    }
}
