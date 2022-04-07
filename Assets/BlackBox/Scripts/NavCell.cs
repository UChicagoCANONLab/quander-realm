using TMPro;

namespace BlackBox
{
    public class NavCell : Cell
    {
        public TextMeshProUGUI markerText;

        public override void Interact()
        {
            GameEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public override void SetValue(string value)
        {
            markerText.gameObject.SetActive(true);
            markerText.text = value;
        }

        public override bool HasNode()
        {
            return false;
        }
    }
}
