using UnityEngine;
using System.Collections.Generic;

namespace BlackBox
{
    public class BlackBoxManager : MonoBehaviour
    {
        private float cellSize;

        public GameObject cellPrefab;
        public GameObject gridPrefab;

        [Header("Grid Objects")]
        public BGrid mainGrid;
        public BGrid leftGrid;
        public BGrid botGrid;
        public BGrid rightGrid;
        public BGrid topGrid;

        [Header("Grid and Cell Size")]
        [Tooltip("This will determine which values from the below arrays we'll use for: \n\ngrid size (e.g 5x5, 6x6, or 7x7) \ncell size (e.g 200f, 166f, 142f)")]
        public GridSize gridSize = GridSize.Small;

        [Tooltip("Set width/height of the grid that corresponds to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        public int[] gridSizeValues = new int[3] { 5, 6, 7 }; //todo: name these using an editor script to make it easier to understand

        [Tooltip("Set cell sizes that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        public float[] cellSizeValues = new float[3] { 200f, 166.66f, 142.86f } ;

        //public float unitCellSize; //todo: allow differently sized unit grids?

        void Start()
        {
            cellSize = cellSizeValues[((int)gridSize)];
            CreateGrids(gridSizeValues[(int)gridSize]);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mainGrid.Interact(GetMouseWorldPosition());
                //leftGrid.Interact(GetMouseWorldPosition());
                //botGrid.Interact(GetMouseWorldPosition());
                //rightGrid.Interact(GetMouseWorldPosition());
                //topGrid.Interact(GetMouseWorldPosition());
            }

            if (Input.GetKeyDown(KeyCode.R))
                CreateGrids(gridSizeValues[(int)gridSize]);

            //if (Input.GetKeyDown(KeyCode.Alpha6))
            //    CreateGrids(6);

            //if (Input.GetKeyDown(KeyCode.Alpha7))
            //    CreateGrids(7);

            //if (Input.GetKeyDown(KeyCode.Alpha8))
            //    CreateGrids(8);

            //if (Input.GetKeyDown(KeyCode.Alpha9))
            //    CreateGrids(9);
        }

        private void CreateGrids(int size)
        {
            ClearChildren(mainGrid.gameObject);

            mainGrid.Create(size, size, cellSize, mainGrid.gameObject.transform.position, cellPrefab, CellType.Node);
            //leftGrid.CreateGrid(1, size, cellSize, leftGridContainer.transform.position, CellType.Unit, Dir.Left);
            //botGrid.CreateGrid(size, 1, cellSize, botGridContainer.transform.position, CellType.Unit, Dir.Bot);
            //rightGrid.CreateGrid(1, size, cellSize, rightGridContainer.transform.position, CellType.Unit, Dir.Right);
            //topGrid.CreateGrid(size, 1, cellSize, topGridContainer.transform.position, CellType.Unit, Dir.Top);
        }

        private void ClearChildren(GameObject parent)
        {
            foreach (Transform cell in parent.transform)
                Destroy(cell.gameObject);
        }

        public Vector3 GetMouseWorldPosition()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f;
            return worldPosition;
        }
    }
}
