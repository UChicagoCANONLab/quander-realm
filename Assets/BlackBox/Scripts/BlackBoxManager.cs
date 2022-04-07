using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BlackBox
{
    public class BlackBoxManager : MonoBehaviour
    {
        private float cellSize;

        public GameObject cellPrefab;
        public GameObject gridPrefab;

        [Header("Grid Objects")]
        //public GridArray mainGrid;
        public GameObject mainGridGO;
        //public GridArray leftGrid;
        //public GridArray botGrid;
        //public GridArray rightGrid;
        //public GridArray topGrid;

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

            CreateGrid(mainGridGO, gridSizeValues[(int)gridSize]);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetGridArray(mainGridGO).Interact(GetMouseWorldPosition());
                //leftGrid.Interact(GetMouseWorldPosition());
                //botGrid.Interact(GetMouseWorldPosition());
                //rightGrid.Interact(GetMouseWorldPosition());
                //topGrid.Interact(GetMouseWorldPosition());
            }

            if (Input.GetKeyDown(KeyCode.R))
                CreateGrid(mainGridGO, gridSizeValues[(int)gridSize]);

            //if (Input.GetKeyDown(KeyCode.Alpha6))
            //    CreateGrids(6);

            //if (Input.GetKeyDown(KeyCode.Alpha7))
            //    CreateGrids(7);

            //if (Input.GetKeyDown(KeyCode.Alpha8))
            //    CreateGrids(8);

            //if (Input.GetKeyDown(KeyCode.Alpha9))
            //    CreateGrids(9);
        }

        private void CreateGrid(GameObject parent, int size)
        {
            ClearChildren(parent);

            GetGridArray(parent).Create(size, size, cellSize, parent.transform.position, CellType.Node);
            GetGLG(parent).cellSize = new Vector2(cellSize, cellSize);
            //leftGrid.Create(1, size, cellSize, leftGrid.gameObject.transform.position, cellPrefab, CellType.Unit, Dir.Left);
            //botGrid.Create(size, 1, cellSize, botGrid.gameObject.transform.position, cellPrefab, CellType.Unit, Dir.Bot);
            //rightGrid.Create(1, size, cellSize, rightGrid.gameObject.transform.position, cellPrefab, CellType.Unit, Dir.Right);
            //topGrid.Create(size, 1, cellSize, topGrid.gameObject.transform.position, cellPrefab, CellType.Unit, Dir.Top);
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

        private GridArray GetGridArray(GameObject GO)
        {
            return GO.GetComponent<GridArray>();
        }

        private GridLayoutGroup GetGLG(GameObject GO)
        {
            return GO.GetComponent<GridLayoutGroup>();
        }

    }
}
