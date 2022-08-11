using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using BeauRoutine;

namespace BlackBox
{
    public class Lantern : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler
    {
        private CanvasGroup canvasGroup;
        private LanternMount parentMount;
        private RectTransform rectTransform;
        private Vector2 currentMousePos;

        [SerializeField] private Canvas canvas;
        [SerializeField] private Animator animator;

        private void Awake()
        {
            parentMount = GetParentMount();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        //private void Update()
        //{
        //    if (animator.GetBool("Hold"))
        //        UpdateAnimator();
        //}

        #region Interface Functions

        public void OnPointerDown(PointerEventData eventData)
        {
            animator.SetBool("Hold", true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
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
            animator.SetBool("Hold", false);
            animator.SetFloat("Velocity", 0f);
            animator.SetInteger("DragDirection", 0);

            canvasGroup.blocksRaycasts = true;
            BBEvents.ToggleLanternHeld?.Invoke(false);
            Routine.Start(ReturnHomeIfUnmounted());
        }

        #endregion

        #region Helpers

        private void UpdateAnimator(PointerEventData eventData)
        {
            float currentVelocity = animator.GetFloat("Velocity");

            Vector2 oldPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            Vector2 newPosition = rectTransform.anchoredPosition;

            animator.SetInteger("DragDirection", GetDirection(oldPosition, newPosition));
            animator.SetFloat("Velocity", GetVelocity(oldPosition, newPosition, currentVelocity));
        }

        //private void UpdateAnimator()
        //{
        //    float currentVelocity = animator.GetFloat("Velocity");
        //    Vector2 oldPosition = rectTransform.anchoredPosition;
        //    Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //    //rectTransform.anchoredPosition += delta / canvas.scaleFactor;
        //    rectTransform.anchoredPosition = delta;
        //    Vector2 newPosition = rectTransform.anchoredPosition;

        //    animator.SetInteger("DragDirection", GetDirection(oldPosition, newPosition));
        //    animator.SetFloat("Velocity", GetVelocity(oldPosition, newPosition, currentVelocity));
        //}

        private float GetVelocity(Vector2 oldPosition, Vector2 newPosition, float currentVelocity)
        {
            float newVelocity = 1 - (1 / (Vector2.Distance(newPosition, oldPosition) / 2));
            float delta = 1 - (1 / (Vector2.Distance(newPosition, oldPosition) * 4));


            Debug.LogFormat("vel: {0}, del: {1}", newVelocity, delta);
            if (newVelocity <= 0.01)
                return 0;

            return Mathf.Lerp(currentVelocity, newVelocity, 1 - delta);
        }

        private int GetDirection(Vector2 oldPosition, Vector2 newPosition)
        {
            float newVelocity = 1 - (1 / (Vector2.Distance(newPosition, oldPosition) / 2));


            //Debug.LogFormat("dirVel {0}", newVelocity);

            //if (newVelocity <= 0.45)
            //    return 0;

            return (int)Mathf.Sign(newPosition.x - oldPosition.x);
        }

        private IEnumerator ReturnHomeIfUnmounted()
        {
            yield return null;

            if (GetParentMount() == null)
            {
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


        #endregion
    }
}
