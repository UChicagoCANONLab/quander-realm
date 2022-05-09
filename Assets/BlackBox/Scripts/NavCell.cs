using TMPro;
using UnityEngine;

namespace BlackBox
{
    public class NavCell : Cell
    {
        [SerializeField] private TextMeshProUGUI markerText;

        private bool isMarked = false;

        public override void Interact()
        {
            if (isMarked)
                return;

            BlackBoxEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public override void SetValue(string value, Color color)
        {
            markerText.gameObject.SetActive(true);
            markerText.color = color;
            markerText.text = value;
            isMarked = true;
        }

        public override bool HasNode()
        {
            return false;
        }
    }
}
