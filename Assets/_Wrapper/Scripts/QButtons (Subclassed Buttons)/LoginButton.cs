using TMPro;
using UnityEngine;

namespace Wrapper
{
    public class LoginButton : QButton
    {
        [SerializeField] private TMP_InputField field;
        [SerializeField] private GameObject loginGO;

        protected override void Start()
        {
            base.Start();

            field = transform.parent.GetComponentInChildren<TMP_InputField>(); // todo: make this more robust
        }

        protected override void OnClickedHandler()
        {
            Debug.Log("Submitted Research Code");
            bool isUserValid = (bool)Events.SubmitResearchCode?.Invoke(field.text);

            if (isUserValid)
                loginGO.SetActive(false);
        }
    }
}
