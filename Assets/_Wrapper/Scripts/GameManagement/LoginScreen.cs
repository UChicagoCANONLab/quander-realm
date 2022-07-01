using TMPro;
using UnityEngine;

namespace Wrapper
{
    public class LoginScreen : MonoBehaviour
    {
        [SerializeField] private QButton loginButton;
        [SerializeField] private TMP_InputField field;
        [SerializeField] private TextMeshProUGUI errorText;

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
                case LoginStatus.FormatError:
                    errorText.text = "Login failed: Format Error";
                    break;
                case LoginStatus.DatabaseError:
                    errorText.text = "Login failed: Database Error";
                    break;
                case LoginStatus.RetrievalError:
                    errorText.text = "Login failed: Save file retrieval Error";
                    break;
                case LoginStatus.NonExistentUserError:
                    errorText.text = "Login failed: User does not exist";
                    break;
                default:
                    break;
            }
        }
    }
}
