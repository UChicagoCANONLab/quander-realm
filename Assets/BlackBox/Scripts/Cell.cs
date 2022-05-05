using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public abstract class Cell : MonoBehaviour
    {
        public GameObject background;
        public Button button;

        [SerializeField] protected Vector3Int gridPosition = Vector3Int.zero;
        [HideInInspector] public Dir direction;
        [HideInInspector] public CellType cellType;

        protected virtual void Start()
        {
            button.onClick.AddListener(Interact);
        }

        public abstract void Interact();

        public virtual void SetValue(string value) { }

        public abstract bool HasNode();

        public Cell Create(int xPos, int yPos, CellType cellType, Dir direction = Dir.None)
        {
            gridPosition = new Vector3Int(xPos, yPos);
            this.cellType = cellType;
            this.direction = direction;

            return this;
        }
    }
}