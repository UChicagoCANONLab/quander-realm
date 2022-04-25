using UnityEngine;
using Wrapper;

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
        Debug.Log("Dialogue + " + step.ToString());
        Events.ChangeDialogue?.Invoke((int)step);

        //todo: qAnimator.doSomething
    }
}
