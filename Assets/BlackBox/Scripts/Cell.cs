using TMPro;
using UnityEngine;

namespace BlackBox
{
    public class Cell : MonoBehaviour
    {
        public bool hasNode = false;
        public GameObject container;
        public GameObject spriteBG;
        public GameObject spriteNode;
        public TextMeshProUGUI markerText;
        public Vector3Int gridPosition;

        [HideInInspector] public Dir direction;
        [HideInInspector] public CellType cellType;

        private float cellSize;
        private Vector3 origin;
        //private static bool flip = true;

        public Cell CreateCell(int xPos, int yPos, float cellSize, Vector3 origin, CellType cellType = CellType.Node, Dir direction = Dir.None)
        {
            this.cellSize = cellSize;
            this.origin = origin;
            this.cellType = cellType;
            this.direction = direction;
            gridPosition = new Vector3Int(xPos, yPos);

            container.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);

            //if (cellType == CellType.Node) // todo: fix for even sized grids(e.g. 6x6)
            //{
            //    spriteBG.SetActive(flip);
            //    flip = !flip;
            //}

            return this;
        }

        public void Interact()
        {
            if (cellType == CellType.Node)
                ToggleNode();
            else
                GameEvents.FireRay?.Invoke(gridPosition, direction);
        }

        public void SetText(string value)
        {
            markerText.gameObject.SetActive(true);
            markerText.text = value;
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
