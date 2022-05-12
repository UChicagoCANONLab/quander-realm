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
            BBEvents.MarkUnits -= MarkUnit;
            BBEvents.MarkDetourUnits -= MarkLinkedUnits;
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

            BBEvents.MarkUnits += MarkUnit; //todo: creating new grids re-registers
            BBEvents.MarkDetourUnits += MarkLinkedUnits; //todo: creating new grids re-registers
        }

        private void MarkUnit(string text, Dir gridDirection, Vector3Int destPosition)
        {
            if (direction != gridDirection)
                return;

            cellArray[destPosition.x, destPosition.y].SetValue(text);
        }

        //todo: clean up?
        private void MarkLinkedUnits(Dir entryDirection, Vector3Int entryPosition, Dir exitDirection, Vector3Int exitPosition)
        {
            //Entry Cell
            if (entryDirection == direction)
            {
                //if (colorIndex == colorArray.Length - 1) //temp: reset if last color
                //    colorIndex = -1;

                //colorIndex++; // todo: this isn't guaranteed to happen before the exit part of the function

                cellArray[entryPosition.x, entryPosition.y].SetValue("D", Color.white, exitDirection, exitPosition);
            }

            //Exit Cell
            if (exitDirection == direction)
                cellArray[exitPosition.x, exitPosition.y].SetValue("D", Color.white, entryDirection, entryPosition);
        }
    }
}
