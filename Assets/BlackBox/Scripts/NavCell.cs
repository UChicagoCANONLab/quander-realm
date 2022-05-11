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

            BlackBoxEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public void SetValue(string value)
        {
            animator.SetBool("NavCell/Measurement", true);
            background.gameObject.SetActive(true);
            markerText.text = value;
            isMarked = true;
        }

        public void SetValue(string value, Color color, Dir linkedCellDirection, Vector3Int linkedCellPosition)
        {
            animator.SetBool("NavCell/Measurement", true);
            background.gameObject.SetActive(true);
            markerText.color = color;
            markerText.text = value;
            isMarked = true;
            isLinked = true;

            this.linkedCellDirection = linkedCellDirection;
            this.linkedCellPosition = linkedCellPosition;
        }

        public override bool HasNode()
        {
            return false;
        }

        #region Linked Highlight

        private void OnEnable()
        {
            BlackBoxEvents.ToggleLinkedHighlight += ToggleLinkedHighlight;
        }

        private void OnDisable()
        {
            BlackBoxEvents.ToggleLinkedHighlight -= ToggleLinkedHighlight;
        }

        private void ToggleLinkedHighlight(string triggerName, Dir linkedCellDirection, Vector3Int linkedCellPosition)
        {
            if (linkedCellDirection == direction && linkedCellPosition == gridPosition)
                animator.SetTrigger(triggerName);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isLinked)
                BlackBoxEvents.ToggleLinkedHighlight?.Invoke("Highlighted", linkedCellDirection, linkedCellPosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isLinked)
                BlackBoxEvents.ToggleLinkedHighlight?.Invoke("Normal", linkedCellDirection, linkedCellPosition);
        }

        #endregion
    }
}
