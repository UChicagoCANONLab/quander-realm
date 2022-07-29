using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Wrapper
{
    public class QButton : Button
    {
        [SerializeField] protected string audioName = string.Empty;
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            Events.PlaySound(audioName.Equals(string.Empty)? "ButtonClick" : audioName);
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