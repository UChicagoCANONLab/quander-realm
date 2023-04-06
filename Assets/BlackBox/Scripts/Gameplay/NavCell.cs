using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class NavCell : Cell, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI markerText = null;

        bool isMollyAt = false;
        private bool isMarked = false;
        private bool isLinked = false;
        private Dir linkedCellDirection = Dir.None;
        private Vector3Int linkedCellPosition = Vector3Int.back;

        public override void Interact()
        {
            if (!isMollyAt)
            {
                BBEvents.DisableMolly?.Invoke();
                EnableMolly();
            }
            else if (!isMarked)
                BBEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public void SetValue(Marker marker)
        {
            animator.SetBool("NavCell/Measurement", true);
            animator.SetInteger("PathType", (int)marker);
            animator.SetTrigger("BatTravelIn");
            background.gameObject.SetActive(true);
            isMarked = true;
            isMollyAt = true;
        }

        public void SetValue(Marker marker, int pathNumber, Dir linkedCellDirection, Vector3Int linkedCellPosition, bool isExit)
        {
            animator.SetBool("NavCell/Measurement", true);
            animator.SetInteger("PathType", (int)marker);
            animator.SetInteger("PathNumber", pathNumber);
            if (isExit)
            {
                animator.SetTrigger("BatTravelIn");
                isMollyAt = true;
            }
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

        public void EnableMolly()
        {
            isMollyAt = true;
            animator.SetTrigger("BatPoofIn");
        }

        void DisableMolly()
        {
            if (isMollyAt) animator.SetTrigger("BatPoofOut");
            isMollyAt = false;
        }

        void SendMollyIn()
        {
            if (isMollyAt)
            {
                animator.SetTrigger("BatTravelOut");
                isMollyAt = false;
            }
        }

        #region Linked Highlight

        protected override void OnEnable()
        {
            base.OnEnable();

            BBEvents.ClearMarkers += ResetValue; // Debug
            BBEvents.ToggleLinkedHighlight += ToggleLinkedHighlight;
            BBEvents.DisableMolly += DisableMolly;
            BBEvents.SendMollyIn += SendMollyIn;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            BBEvents.ClearMarkers -= ResetValue; // Debug
            BBEvents.ToggleLinkedHighlight -= ToggleLinkedHighlight;
            BBEvents.DisableMolly -= DisableMolly;
            BBEvents.SendMollyIn -= SendMollyIn;
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
