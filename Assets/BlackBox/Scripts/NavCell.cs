using TMPro;
using UnityEngine;

namespace BlackBox
{
    public class NavCell : Cell
    {
        public TextMeshProUGUI markerText;

        public override void Interact()
        {
            GameEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public override void SetValue(string value, Color color)
        {
            markerText.gameObject.SetActive(true);
            markerText.color = color;
            markerText.text = value;
        }

        public override bool HasNode()
        {
            return false;
        }
    }
}
