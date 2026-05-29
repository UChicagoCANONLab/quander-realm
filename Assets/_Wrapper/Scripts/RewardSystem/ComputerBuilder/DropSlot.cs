using UnityEngine;
using UnityEngine.EventSystems;

namespace Wrapper
{
    /*
        Attach this to each slot (Slot1, Slot2) inside every layer.
        Listens for a DraggablePart being dropped onto it via IDropHandler.
        
        Each slot has a layerID (1-4) and slotID (1 or 2) that must match the dropped part's targetLayerID and targetSlotID, otherwise
        the drop is rejected and the part returns to the inventory.
     */
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private ComputerLayer layer; // reference to the layer this slot belongs to
        [SerializeField] public int layerID;          // 1-4, must match DraggablePart.targetLayerID
        [SerializeField] public int slotID;           // 1 or 2, must match DraggablePart.targetSlotID

        private DraggablePart placedPart;

        public bool IsEmpty => placedPart == null;

        /*
            Called  when a dragged part is released over this slot.
            Checks three conditions before accepting the drop:
                1. The dropped object has a DraggablePart component
                2. The part's targetLayerID and targetSlotID match this slot's IDs
                3. This slot is currently empty
            If all pass, calls PlacePart(). Otherwise logs the rejection reason.
         */
        public void OnDrop(PointerEventData eventData)
        {
            DraggablePart part = eventData.pointerDrag?.GetComponent<DraggablePart>();
            if (part == null) return;

            if (part.targetLayerID != layerID || part.targetSlotID != slotID)
            {
                Debug.Log($"[DropSlot] WRONG SLOT — {part.gameObject.name} needs Layer {part.targetLayerID} Slot {part.targetSlotID}, got Layer {layerID} Slot {slotID}");
                return;
            }

            if (!IsEmpty)
            {
                Debug.Log($"[DropSlot] Layer {layerID} Slot {slotID} already occupied");
                return;
            }

            PlacePart(part);
        }

        /*
            Accepts the part and locks it into this slot.

            Steps:
                1. Store a reference to the placed part
                2. Reparent the part to this slot so it moves with it
                3. Stretch the part's RectTransform to fill the slot with 10px padding
                4. Disable the DraggablePart component so it can't be dragged again
                5. Notify the parent ComputerLayer that a part has been placed
            so it can check if the layer is now complete
         */
        private void PlacePart(DraggablePart part)
        {
            placedPart = part;
            part.transform.SetParent(transform);

            RectTransform rt = part.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(10, 10);
            rt.offsetMax = new Vector2(-10, -10);

            part.enabled = false;

            Debug.Log($"[DropSlot] CORRECT — {part.gameObject.name} placed in Layer {layerID} Slot {slotID}");

            if (layer != null)
                layer.OnPartPlaced();
        }
    }
}