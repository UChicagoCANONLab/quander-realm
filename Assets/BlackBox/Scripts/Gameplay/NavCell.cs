using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class NavCell : Cell, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI markerText = null;

        bool isMollyAt = false;
        private bool isMarked = false;
        private bool isLinked = false;
        private Dir linkedCellDirection = Dir.None;
        private Vector3Int linkedCellPosition = Vector3Int.back;

        const float highlightDelayTime = 0.2F;
        BeauRoutine.Routine highlightDelay;

        public override void Interact()
        {
            if (BBEvents.IsInteractionDelayed.Invoke()) return;
            BBEvents.DelayInteraction?.Invoke(false);

            if (!isMollyAt)
            {
                BBEvents.DisableMolly?.Invoke();
                EnableMolly();
            }
            else if (!isMarked)
                BBEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public void SetDelayedValue(Marker marker)
        {
            BBEvents.DelayReaction?.Invoke(() =>
            {
                animator.SetBool("NavCell/Measurement", true);
                animator.SetInteger("PathType", (int)marker);

                if (marker == Marker.Miss) SetFlyOppositeAnim(direction);
                else SetFlyDirectionAnim(direction);

                if (marker == Marker.Hit) animator.SetTrigger("BatDazed");
                animator.SetTrigger("ChangeOrientation");
                animator.SetTrigger("BatTravelIn");

                background.gameObject.SetActive(true);
                isMarked = true;
                isMollyAt = true;
            });
        }

        public void SetValue(Marker marker) // miss, hit, reflection
        {
            animator.SetBool("NavCell/Measurement", true);
            animator.SetInteger("PathType", (int)marker);

            background.gameObject.SetActive(true);
            isMarked = true;
        }

        public void SetDelayedValue(Marker marker, int pathNumber, Dir linkedCellDirection, Vector3Int linkedCellPosition)
        {
            BBEvents.DelayReaction?.Invoke(() =>
            {
                animator.SetBool("NavCell/Measurement", true);
                animator.SetInteger("PathType", (int)marker);
                animator.SetInteger("PathNumber", pathNumber);

                SetFlyDirectionAnim(linkedCellDirection);
                animator.SetTrigger("ChangeOrientation");
                animator.SetTrigger("BatTravelIn");
                isMollyAt = true;

                background.gameObject.SetActive(true);

                isMarked = true;
                isLinked = true;

                this.linkedCellDirection = linkedCellDirection;
                this.linkedCellPosition = linkedCellPosition;
            });
        }

        public void SetValue(Marker marker, int pathNumber, Dir linkedCellDirection, Vector3Int linkedCellPosition)    // detour only
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

        public void EnableMolly(bool playSound = true)
        {
            isMollyAt = true;
            animator.SetTrigger("BatPoofIn");
            if (playSound) Wrapper.Events.PlaySound?.Invoke("BB_MollySonar");
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
                SetFlyDirectionAnim(direction);
                animator.SetTrigger("ChangeOrientation");
                animator.SetTrigger("BatTravelOut");
                Wrapper.Events.PlaySound?.Invoke("BB_MollyEnter");
                isMollyAt = false;
            }
        }

        void SetFlyDirectionAnim(Dir direction)
        {
            switch (direction)
            {
                case Dir.Top:
                    animator.SetInteger("FlyOrientation", 1);
                    break;
                case Dir.Right:
                    animator.SetInteger("FlyOrientation", 4);
                    break;
                case Dir.Bot:
                    animator.SetInteger("FlyOrientation", 3);
                    break;
                case Dir.Left:
                    animator.SetInteger("FlyOrientation", 2);
                    break;

                default: break;
            }
        }

        void SetFlyOppositeAnim(Dir direction)
        {
            switch (direction)
            {
                case Dir.Top:
                    animator.SetInteger("FlyOrientation", 3);
                    break;
                case Dir.Right:
                    animator.SetInteger("FlyOrientation", 2);
                    break;
                case Dir.Bot:
                    animator.SetInteger("FlyOrientation", 1);
                    break;
                case Dir.Left:
                    animator.SetInteger("FlyOrientation", 4);
                    break;

                default: break;
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
            {
                Debug.Log($"Setting {triggerName} state to: {linkedCellDirection} - {linkedCellPosition}");
                animator.SetTrigger(triggerName);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
#if UNITY_EDITOR || UNITY_WEBGL
            if (isLinked)
                BBEvents.ToggleLinkedHighlight?.Invoke("Highlighted", linkedCellDirection, linkedCellPosition);
#endif
        }

        public void OnPointerExit(PointerEventData eventData)
        {
#if UNITY_EDITOR || UNITY_WEBGL
            if (isLinked)
                BBEvents.ToggleLinkedHighlight?.Invoke("Normal", linkedCellDirection, linkedCellPosition);
#endif
        }

        public void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_IOS || UNITY_ANDROID
            if (isLinked)
            {
                BBEvents.ToggleLinkedHighlight?.Invoke("Highlighted", linkedCellDirection, linkedCellPosition);
                if (highlightDelay.Exists()) highlightDelay.Stop();
            }
#endif
        }

        public void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_IOS || UNITY_ANDROID
            if (isLinked)
            {
                highlightDelay.Replace(DelayHighlight());
            }
#endif
        }

        System.Collections.IEnumerator DelayHighlight()
        {
            yield return highlightDelayTime;
            if (isLinked)
                BBEvents.ToggleLinkedHighlight?.Invoke("Normal", linkedCellDirection, linkedCellPosition);
        }

#endregion
    }
}
