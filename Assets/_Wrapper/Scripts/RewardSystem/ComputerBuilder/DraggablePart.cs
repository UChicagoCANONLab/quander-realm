using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Wrapper
{
    /* 
        Attach to a part icon in the inventory to make it draggable. Implements three Unity drag interfaces:
            - IBeginDragHandler  - fires once when the player starts dragging
            - IDragHandler       - fires every frame while the player is holding and moving
            - IEndDragHandler    - fires once when the player releases the mouse/finger
    */
    public class DraggablePart : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private bool isEarned = true;
        [SerializeField] public int targetLayerID; // 1-4, which layer this part belongs to
        [SerializeField] public int targetSlotID;  // 1 or 2, which slot within that layer

        private Canvas rootCanvas;        // the top-level canvas — we reparent here during drag so the part renders on top of everything
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;  // lets us disable raycasts while dragging so the drop slot can detect the drop
        private Transform originalParent; // saved so we can return the part here if dropped in wrong place
        private Vector2 originalPosition; // saved so we can snap back to inventory position if dropped in wrong place

        /*
            Called once by Unity before the first frame.
            Grabs references to components we'll need during dragging.
        */
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
        }
        /* 
            Called once by Unity when the player starts dragging this UI element.
                1. Saves the part's current parent and position so we can return it if needed.
                2. Reparents to the root canvas so the part floats above all other UI.
                3. Disables blocksRaycasts so the drop slot underneath can receive the drop event.
        */
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isEarned)
            {
                Debug.Log($"[DraggablePart] {gameObject.name} is LOCKED — cannot drag");
                eventData.pointerDrag = null; // cancels the drag entirely
                return;
            }

            Debug.Log($"[DraggablePart] Dragging {gameObject.name} — targets Layer {targetLayerID}, Slot {targetSlotID}");
            originalParent = transform.parent;
            originalPosition = rectTransform.anchoredPosition;

            transform.SetParent(rootCanvas.transform); // detach from inventory row, attach to canvas root
            transform.SetAsLastSibling();              // render on top of all other UI
            canvasGroup.blocksRaycasts = false;        // let mouse events pass through to drop slots below
        }

        /*
            Called every frame while the player is dragging.
                1. Moves the part to follow the mouse/finger position.
                2. eventData.delta is how much the pointer moved since last frame.
                3. We divide by scaleFactor to account for canvas scaling.
        */
        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
        }

        /*
            Called once when the player releases the drag.
                - TODO: We can omit this maybe: Re-enables raycasts so this part can be interacted with again.
                - If the part was NOT accepted by a drop slot (still parented to root canvas),
                  it snaps back to its original inventory position.
                - If the part WAS accepted (DropSlot reparented it), tint the inventory
                  slot background purple to show the slot is now empty
         */
        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;

            if (transform.parent == rootCanvas.transform)
            {
                // Still on root canvas = no valid slot accepted it, return to inventory
                Debug.Log($"[DraggablePart] {gameObject.name} returned to inventory - no valid slot");
                transform.SetParent(originalParent);
                rectTransform.anchoredPosition = originalPosition;
            }
            else
            {
                // Part was accepted by a drop slot — tint the inventory slot to show it's empty
                Transform slotBGTransform = originalParent.Find("SlotBackground");
                if (slotBGTransform != null)
                {
                    Image slotBG = slotBGTransform.GetComponent<Image>();
                    if (slotBG != null)
                        slotBG.color = new Color(0.72f, 0.68f, 0.80f, 1f); // light purple = slot empty
                }
                Debug.Log($"[DraggablePart] {gameObject.name} placed — inventory slot tinted purple");
            }
        }

        /*
            Called by ComputerBuilder when the player earns or loses this part.
            If earned = true, the part becomes draggable. If false, it becomes locked and undraggable.
         */
        public void SetEarned(bool earned)
        {
            isEarned = earned;
            Debug.Log($"[DraggablePart] {gameObject.name} earned: {earned}");
        }
    }
}