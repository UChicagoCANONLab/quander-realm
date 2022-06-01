using TMPro;
using UnityEngine;

namespace Wrapper
{
    public class LoginScreen : MonoBehaviour
    {
        [SerializeField] private QButton loginButton;
        [SerializeField] private TMP_InputField field;

        private void Awake()
        {
            loginButton.onClick.AddListener(SubmitCode);    
        }

        private void OnEnable()
        {
            Events.UpdateLoginStatus += ToggleLoginScreen;
        }

        private void OnDisable()
        {
            Events.UpdateLoginStatus -= ToggleLoginScreen;
        }

        private void SubmitCode()
        {
            Events.SubmitResearchCode?.Invoke(field.text);
        }

        private void ToggleLoginScreen(LoginStatus status)
        {
            // todo: create error popup, update error text
            switch (status)
            {
                case LoginStatus.Success:
                    gameObject.SetActive(false);
                    break;
                case LoginStatus.Failure:
                case LoginStatus.FormatError:
                case LoginStatus.DatabaseError:
                case LoginStatus.RetrievalError:
                case LoginStatus.NonExistentUserError:
                default:
                    break;
            }
        }
    }
}
