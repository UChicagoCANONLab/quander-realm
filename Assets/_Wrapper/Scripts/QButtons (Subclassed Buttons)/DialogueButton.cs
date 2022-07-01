using System;
using UnityEngine;

namespace Wrapper
{
    public class DialogueButton : QButton
    {
        [SerializeField] public Step step = Step.Forward;

        protected override void OnEnable()
        {
            if (this.step == Step.Backward)
                Events.TogglePreviousButton += ToggleInteractable;
        }

        protected override void OnDisable()
        {
            if (this.step == Step.Backward)
                Events.TogglePreviousButton -= ToggleInteractable;
        }

        private void ToggleInteractable(bool isOn)
        {
            interactable = isOn;
        }

        protected override void OnClickedHandler()
        {
            Events.ChangeDialogue?.Invoke((int)step);
        }
    }
}