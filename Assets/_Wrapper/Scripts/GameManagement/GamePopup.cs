using System;
using System.Collections;
using UnityEngine;
using Wrapper;

namespace Wrapper
{
    public class GamePopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject gameContainer;
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
            ToggleGameShown(Game.Circuits, false);
            ToggleGameShown(Game.QueueBits, false);
            ToggleGameShown(Game.BlackBox, false);

            GameObject gameDisplay = ToggleGameShown(game, true);
            ToggleDisplay(true);

            while (!(gameDisplay.activeInHierarchy))
                yield return null;
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

        private GameObject ToggleGameShown(Game game, bool value) {
            string gameName = Enum.GetName(typeof(Game), game);
            string prefix = "GameManager/MinigameUnlockedPopup/Panel/MinigameContainer";
            GameObject gameDisplay = GameObject.Find($"{prefix}/{gameName}");
            gameDisplay.SetActive(value);
            return gameDisplay;
        }
    }
}
