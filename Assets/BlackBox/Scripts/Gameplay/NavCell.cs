using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class NavCell : Cell, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI markerText = null;

        private bool isMarked = false;
        private bool isLinked = false;
        private Dir linkedCellDirection = Dir.None;
        private Vector3Int linkedCellPosition = Vector3Int.back;

        public override void Interact()
        {
            if (isMarked)
                return;

            BBEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public void SetValue(Marker marker)
        {
            animator.SetBool("NavCell/Measurement", true);
            animator.SetInteger("PathType", (int)marker);
            background.gameObject.SetActive(true);
            isMarked = true;
        }

        public void SetValue(Marker marker, int pathNumber, Dir linkedCellDirection, Vector3Int linkedCellPosition)
        {
            animator.SetBool("NavCell/Measurement", true);
            animator.SetInteger("PathType", (int)marker);
            animator.SetInteger("PathNumber", pathNumber);
            background.gameObject.SetActive(true);

            isMarked = true;
            isLinked = true;

            this.linkedCellDirection = linkedCellDirection;
            this.linkedCellPosition = linkedCellPosition;
        }

        // Debug
        private void ResetValue()
        {
            animator.SetBool("NavCell/Measurement", false);
            background.gameObject.SetActive(false);

            markerText.text = string.Empty;

            isMarked = false;
            isLinked = false;

            linkedCellDirection = Dir.None;
            linkedCellPosition = Vector3Int.back;
        }

        public override bool HasNode()
        {
            return false;
        }

        #region Linked Highlight

        protected override void OnEnable()
        {
            base.OnEnable();

            BBEvents.ClearMarkers += ResetValue; // Debug
            BBEvents.ToggleLinkedHighlight += ToggleLinkedHighlight;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            BBEvents.ClearMarkers -= ResetValue; // Debug
            BBEvents.ToggleLinkedHighlight -= ToggleLinkedHighlight;
        }

        private void ToggleLinkedHighlight(string triggerName, Dir linkedCellDirection, Vector3Int linkedCellPosition)
        {
            if (linkedCellDirection == direction && linkedCellPosition == gridPosition)
                animator.SetTrigger(triggerName);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isLinked)
                BBEvents.ToggleLinkedHighlight?.Invoke("Highlighted", linkedCellDirection, linkedCellPosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isLinked)
                BBEvents.ToggleLinkedHighlight?.Invoke("Normal", linkedCellDirection, linkedCellPosition);
        }

        #endregion
    }
}
