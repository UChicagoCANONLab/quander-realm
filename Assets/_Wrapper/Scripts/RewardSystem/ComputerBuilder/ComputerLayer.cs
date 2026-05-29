using UnityEngine;

namespace Wrapper
{
    public class ComputerLayer : MonoBehaviour
    {
        [SerializeField] private string layerName;
        [SerializeField] private int totalSlots = 2;

        private int filledSlots = 0;

        public void OnPartPlaced()
        {
            filledSlots++;
            Debug.Log($"[ComputerLayer] {layerName}: {filledSlots}/{totalSlots} parts placed");

            if (filledSlots >= totalSlots)
            {
                Debug.Log($"[ComputerLayer] {layerName} COMPLETE!");
                OnLayerComplete();
            }
        }

        private void OnLayerComplete()
        {
            // TODO: trigger layer reveal animation
            Debug.Log($"[ComputerLayer] Triggering completion event for {layerName}");
        }
    }
}