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

            Events.PlaySound?.Invoke(audioName.Equals(string.Empty)? "ButtonClick" : audioName);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            // if clicking reward card, then save card info
            if (eventData.pointerCurrentRaycast.gameObject.name == "Hitbox") {
                Debug.Log(eventData.pointerPress);
                Reward currCard = eventData.pointerPress.GetComponent<Reward>();
                
                Debug.Log(currCard.titleFront.text); //name of card
            }

            base.OnPointerClick(eventData);
        }

        //public override void OnPointerUp(PointerEventData eventData)
        //{
        //    base.OnPointerUp(eventData);
        //    OnUp.Invoke();
        //}
    }
}