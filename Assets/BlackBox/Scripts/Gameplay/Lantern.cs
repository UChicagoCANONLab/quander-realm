using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class Lantern : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler
    {
        private CanvasGroup canvasGroup = null;
        private LanternMount parentMount = null;
        private RectTransform rectTransform = null;

        [SerializeField] private Canvas canvas = null;
        [SerializeField] private Animator animator = null;

        private void Awake()
        {
            parentMount = GetParentMount();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        #region Interface Functions

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
            animator.SetBool("Hold", true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            canvasGroup.blocksRaycasts = false;
            BBEvents.ToggleLanternHeld?.Invoke(true);

            if (parentMount != null)
                parentMount.UnFlag();

            transform.SetParent(BBEvents.GetFrontMount?.Invoke());
            transform.SetAsLastSibling();
            parentMount = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateAnimator(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");
            animator.SetBool("Hold", false);
            animator.SetFloat("Velocity", 0f);
            animator.SetInteger("DragDirection", 0);

            canvasGroup.blocksRaycasts = true;
            BBEvents.ToggleLanternHeld?.Invoke(false);
            StartCoroutine("ReturnHomeIfUnmounted");
        }

        #endregion

        #region Helpers

        private IEnumerator ReturnHomeIfUnmounted()
        {
            yield return null;

            if (GetParentMount() == null)
            {
                Debug.Log("Return called");
                BBEvents.ReturnLanternHome?.Invoke(this.gameObject);
            }
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SetParentMount(LanternMount parent)
        {
            parentMount = parent;
        }

        private LanternMount GetParentMount()
        {
            return transform.parent.GetComponent<LanternMount>();
        }

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

        #endregion
    }
}
