using UnityEngine;

namespace BlackBox
{
    public class UnitGrid : MonoBehaviour
    {
        [SerializeField] private GameObject navCellPrefab = null;

        private Dir direction = Dir.None;
        private NavCell[,] cellArray = null;

        private static int colorIndex = -1;
        private static readonly Color[] colorArray = new Color[] { 
            Color.red, Color.blue, Color.green, Color.magenta, 
            Color.cyan, Color.black, Color.grey, Color.yellow 
        };

        private void OnDestroy()
        {
            GameEvents.MarkUnits -= MarkUnits;
        }

        public void Create(int width, int height, Dir direction = Dir.None)
        {
            this.direction = direction;
            cellArray = new NavCell[width, height];

            for (int y = 0; y < cellArray.GetLength(1); y++)
            {
                for (int x = 0; x < cellArray.GetLength(0); x++)
                {
                    GameObject navCellObj = Instantiate(navCellPrefab, gameObject.transform);
                    NavCell navCell = navCellObj.GetComponent<NavCell>();

                    navCell.Create(x, y, CellType.Nav, direction);
                    cellArray[x, y] = navCell;
                }
            }

            GameEvents.MarkUnits += MarkUnits;
        }

        private void MarkUnits(string text, Dir gridDirection, Vector3Int destPosition, bool isDetour, bool nextColor)
        {
            if (direction != gridDirection)
                return;

            if (nextColor)
            {
                if (colorIndex == colorArray.Length - 1) //temp: reset if last color
                    colorIndex = -1;

                colorIndex++;
            }

            Color color = isDetour ? colorArray[colorIndex] : Color.white;
            cellArray[destPosition.x, destPosition.y].SetValue(text, color);
        }
    }
}
