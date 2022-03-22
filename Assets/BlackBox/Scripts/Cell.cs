using UnityEngine;

namespace BlackBox
{
    public class Cell : MonoBehaviour
    {
        public bool hasNode = false;
        public GameObject spriteBG;
        public GameObject spriteNode;
        public TextMesh text;
        public Vector3Int gridPosition;
        [HideInInspector] public Dir direction;
        [HideInInspector] public CellType cellType;

        private float cellSize;
        private Vector3 origin;

        public Cell CreateCell(int xPos, int yPos, float cellSize, Vector3 origin, CellType cellType = CellType.Node, Dir direction = Dir.None)
        {
            this.cellSize = cellSize;
            this.origin = origin;
            this.cellType = cellType;
            this.direction = direction;
            gridPosition = new Vector3Int(xPos, yPos);

            return this;
        }

        public void Interact()
        {
            if (cellType == CellType.Node)
                ToggleNode();
            else
                GameEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public void SetText(string markerText)
        {
            text.gameObject.SetActive(true);
            text.text = markerText;
        }

        private Vector3 GetWorldPosition(Vector3 position)
        {
            return position * cellSize + origin;
        }

        private void ToggleNode()
        {
            hasNode = !(hasNode);
            spriteNode.SetActive(hasNode);
        }
    }
}
