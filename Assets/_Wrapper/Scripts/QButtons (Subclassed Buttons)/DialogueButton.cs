using System;
using UnityEngine;
using UnityEngine.EventSystems;

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            Events.ChangeDialogue?.Invoke((int)step);
        }
    }
}