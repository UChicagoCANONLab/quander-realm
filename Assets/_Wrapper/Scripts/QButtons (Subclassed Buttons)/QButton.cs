using UnityEngine.UI;
 using UnityEngine.EventSystems;

namespace Wrapper
{
    public class QButton : Button
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            Events.PlaySound("ButtonClick");
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        //public override void OnPointerUp(PointerEventData eventData)
        //{
        //    base.OnPointerUp(eventData);
        //    OnUp.Invoke();
        //}
    }
}