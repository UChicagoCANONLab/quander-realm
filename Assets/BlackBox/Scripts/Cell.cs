using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public abstract class Cell : MonoBehaviour
    {
        [SerializeField] protected Vector3Int gridPosition = Vector3Int.zero;
        [SerializeField] protected GameObject background = null;
        [SerializeField] protected Animator animator = null;
        [SerializeField] private Button button = null;

        [HideInInspector] public Dir direction = Dir.None;
        [HideInInspector] public CellType cellType = CellType.EdgeNode;

        protected virtual void Start()
        {
            button.onClick.AddListener(Interact);
        }

        public abstract void Interact();

        public virtual void SetValue(string value, Color color) { }

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