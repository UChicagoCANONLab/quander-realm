using UnityEngine.UI;

namespace Wrapper
{
    public class AddSpaceButton : QButton
    {
        private InputField inputField;

        protected override void Start()
        {
            inputField = transform.parent.GetComponentInChildren<InputField>();    
        }

        protected override void OnClickedHandler()
        {
            inputField.text += " ";
        }
    }
}