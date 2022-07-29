using UnityEngine.UI;

namespace Wrapper
{
    public class QButton : Button
    {
        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(OnClickedHandler);
        }

        protected virtual void OnClickedHandler()
        {
            Events.PlaySound("ButtonClick");
        }
    }
}
