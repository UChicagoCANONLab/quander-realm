using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class LanternMount : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            GameObject lanternGO = eventData.pointerDrag;

            lanternGO.transform.SetParent(this.transform);
            lanternGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
