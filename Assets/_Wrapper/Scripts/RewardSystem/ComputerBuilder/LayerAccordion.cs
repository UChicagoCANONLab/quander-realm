using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Wrapper
{
    /*
        Attached to each Layer (Layer1-4) in the SuperconductingPanel.
        On click, brings the selected layer to the front and shows its slots.
        All other layers hide their slots.
     */
    public class LayerAccordion : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private LayerAccordion[] allLayers;
        [SerializeField] private GameObject slotsContainer;

        /*
            Called when the player clicks this layer.
            Deselects all layers first, then selects this one.
         */
        public void OnPointerClick(PointerEventData eventData)
        {
            foreach (LayerAccordion layer in allLayers)
                layer.Deselect();
            Select();
        }

        /*
            Brings this layer to the front (renders on top of others)
            and shows its drop slots.
         */
        public void Select()
        {
            transform.SetAsLastSibling();
            if (slotsContainer != null) slotsContainer.SetActive(true);
        }

        /*
            Hides this layer's drop slots.
         */
        public void Deselect()
        {
            if (slotsContainer != null) slotsContainer.SetActive(false);
        }
    }
}