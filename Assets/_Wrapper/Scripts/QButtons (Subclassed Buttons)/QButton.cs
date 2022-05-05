using UnityEngine.UI;

namespace Wrapper
{
    public abstract class QButton : Button
    {
        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(OnClickedHandler);
        }

        protected abstract void OnClickedHandler();
    }
}
