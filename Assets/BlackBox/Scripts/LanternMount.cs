using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class LanternMount : MonoBehaviour, IDropHandler, IPointerExitHandler
    {
        private Vector3Int gridPosition = Vector3Int.back;

        public void OnDrop(PointerEventData eventData)
        {
            GameObject lanternGO = eventData.pointerDrag;

            lanternGO.transform.SetParent(this.transform);
            lanternGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            if (gridPosition != Vector3Int.back) // using "back" or (0, 0, -1) as default. Z will be 0 if this mount belongs to a cell
                GameEvents.ToggleFlag.Invoke(gridPosition, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag?.GetComponent<Lantern>() != null) // check if lantern is being held by the mouse pointer
                GameEvents.ToggleFlag.Invoke(gridPosition, false);
        }

        public void SetGridPosition(Vector3Int position)
        {
            gridPosition = position;
        }
    }
}
