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

        protected override void OnEnable()
        {
            Events.LoginEvent += ToggleLoginScreen;
        }

        protected override void OnDisable()
        {
            Events.LoginEvent -= ToggleLoginScreen;
        }

        private void ToggleLoginScreen(bool isLoggedIn)
        {
            loginGO.SetActive(!(isLoggedIn));
        }

        protected override void OnClickedHandler()
        {
            Events.SubmitResearchCode?.Invoke(field.text);
        }
    }
}
