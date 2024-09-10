using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class ButtonBehavior : MonoBehaviour
    {
        // [SerializeField] private GameObject restartLevelGO = null;
        // [SerializeField] private GameObject quitGO = null;
        // [SerializeField] private GameObject keepPlayingGO = null;
        // [SerializeField] private GameObject nextLevelGO = null;
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
            infoPanel.SetActive(true);
        }

    }
}
