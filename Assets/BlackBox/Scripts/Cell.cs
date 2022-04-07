using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public abstract class Cell : MonoBehaviour
    {
        public GameObject background;
        public Button button;

        [HideInInspector] public Vector3Int gridPosition;
        [HideInInspector] public Dir direction;
        [HideInInspector] public CellType cellType;

        protected void Start()
        {
            button.onClick.AddListener(Interact);
        }

        public abstract void Interact();

        public abstract void SetValue(string value);

        public abstract bool HasNode();

        public Cell Create(int xPos, int yPos, Dir direction = Dir.None)
        {
            gridPosition = new Vector3Int(xPos, yPos);
            this.direction = direction;

            return this;
        }
    }
}