using UnityEngine;

namespace Wrapper
{
    public class DialogueButton : QButton
    {
        public enum Step
        {
            Forward = 1,
            Backward = -1
        };

        [SerializeField] public Step step = Step.Forward;
        [SerializeField] private Animator qAnimator = null;

        protected override void OnClickedHandler()
        {
            Events.ChangeDialogue?.Invoke((int)step);

            //todo: qAnimator.doSomething
        }
    }
}