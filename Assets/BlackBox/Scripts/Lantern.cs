using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class Lantern : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        [SerializeField] private Canvas canvas;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform frontMountTransform = null;
        //[SerializeField] private GameObject handle;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        #region Interface Functions

        public void OnPointerDown(PointerEventData eventData)
        {
            animator.SetBool("Hold", true);
            BlackBoxEvents.ToggleLanternHeld?.Invoke(true);
        }

        //todo: tween anchored position to handle's position
        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;

            if (frontMountTransform != null)
            {
                transform.SetParent(frontMountTransform);
                transform.SetAsLastSibling();
            }

            UpdateAnimator(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateAnimator(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            animator.SetInteger("DragDirection", 0);
            animator.SetFloat("Velocity", 0f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            animator.SetBool("Hold", false);
            BlackBoxEvents.ToggleLanternHeld?.Invoke(false);
            //drop?
        }

        #endregion

        private void UpdateAnimator(PointerEventData eventData)
        {
            Vector2 oldPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            Vector2 newPosition = rectTransform.anchoredPosition;

            animator.SetInteger("DragDirection", GetDirection(oldPosition, newPosition));
            animator.SetFloat("Velocity", GetVelocity(oldPosition, newPosition));
        }

        //todo: fix this
        private float GetVelocity(Vector2 oldPosition, Vector2 newPosition)
        {
            return 1;

            //if (oldPosition == newPosition)
            //    return 0;

            //Debug.Log(Vector2.Distance(newPosition, oldPosition));
            //return Mathf.Lerp(1f, 0f, 1 / Vector2.Distance(newPosition, oldPosition));
        }

        private int GetDirection(Vector2 oldPosition, Vector2 newPosition)
        {
            if (oldPosition == newPosition)
                return 0;

            return (int)Mathf.Sign(newPosition.x - oldPosition.x);
        }
    }
}
