using UnityEngine;

namespace BlackBox
{
    public class GridArray : MonoBehaviour
    {
        public float debugLineDuration = 3f;
        public GameObject cellPrefab;

        private int width;
        private int height;
        private Dir direction;
        private Ray ray;
        private Cell[,] gridArray;

        public void Create(int width, int height, Dir direction = Dir.None)
        {
            this.width = width;
            this.height = height;
            this.direction = direction;
            gridArray = new Cell[width, height];

            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    GameObject cellObj = Instantiate(cellPrefab, gameObject.transform);
                    Cell cell = cellObj.GetComponent<Cell>().Create(x, y, direction);
                    gridArray[x, y] = cell;
                }
            }

            SetupListeners();
        }

        private void SetupListeners()
        {
            if (direction == Dir.None)
                GameEvents.FireRay.AddListener((rayOrigin, rayDirection) => FireRay(rayOrigin, rayDirection));
            else
                GameEvents.MarkUnit.AddListener((text, gridDirection, destPosition) => MarkUnits(text, gridDirection, destPosition));
        }

        #region Node and Ray Behaviour

        //    if (cellPosition.x < 0 || cellPosition.x >= width || cellPosition.y < 0 || cellPosition.y >= height)
        //        return;

        //    if (direction == Dir.None) //avoid nodes on edge cells on main grid
        //        if (cellPosition.x == 0 || cellPosition.x == width - 1 || cellPosition.y == 0 || cellPosition.y == height - 1)
        //            return;


        private void FireRay(Vector3Int rayOrigin, Dir rayDirection)
        {
            ray = new Ray(rayOrigin, rayDirection, width, height);

            while (RayInPlay())
                UpdateRayPosition();

            ray.AddMarkers();
        }

        private void UpdateRayPosition()
        {
            Vector3Int oldPosition = ray.position;
            GetFrontAndDiagonalCellPositions(out Vector3Int frontMid, out Vector3Int frontRight, out Vector3Int frontLeft);

            //Check if nodes exist and update position accordingly
            if (HasNode(frontMid)) //Hit
                ray.Kill(width); // since the main board is a square, gridLength = width = height
            else if (HasNode(frontRight) && HasNode(frontLeft)) //Reflect
                ray.Flip(false);
            else if (HasNode(frontRight)) //Detour Left
                ray.TurnLeft();
            else if (HasNode(frontLeft)) //Detour Right
                ray.TurnRight();

            //update position
            ray.Forward();

            //todo: draw debug line

        }

        private void GetFrontAndDiagonalCellPositions(out Vector3Int frontMid, out Vector3Int frontRight, out Vector3Int frontLeft)
        {
            frontMid = ray.position;
            frontRight = ray.position;
            frontLeft = ray.position;

            switch (ray.direction)
            {
                case Dir.Left:
                    frontMid.x++;
                    frontRight.x++; frontRight.y--;
                    frontLeft.x++; frontLeft.y++;
                    break;
                case Dir.Bot:
                    frontMid.y++;
                    frontRight.x++; frontRight.y++;
                    frontLeft.x--; frontLeft.y++;
                    break;
                case Dir.Right:
                    frontMid.x--;
                    frontRight.x--; frontRight.y++;
                    frontLeft.x--; frontLeft.y--;
                    break;
                case Dir.Top:
                    frontMid.y--;
                    frontRight.x--; frontRight.y--;
                    frontLeft.x++; frontLeft.y--;
                    break;
            }
        }

        private bool HasNode(Vector3Int gridPosition)
        {
            if (gridPosition.x < 0 || gridPosition.x >= width || gridPosition.y < 0 || gridPosition.y >= height)
                return false;

            Cell cell = gridArray[gridPosition.x, gridPosition.y];
            if (cell != null)
                return cell.HasNode();

            return false;
        }

        private void MarkUnits(string text, Dir gridDirection, Vector3Int destPosition)
        {
            if (direction != gridDirection)
                return;

            gridArray[destPosition.x, destPosition.y].SetValue(text);
        }

        private bool RayInPlay()
        {
            bool result = true;
            if (ray.position.x < 0 || ray.position.x >= width || ray.position.y < 0 || ray.position.y >= height)
                result = false;

            return result;
        }

        #endregion
    }
}
