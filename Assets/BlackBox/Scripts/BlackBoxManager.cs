using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BlackBox
{
    public class BlackBoxManager : MonoBehaviour
    {
        public GameObject gridPrefab;

        [Header("Grid Objects")]
        public GameObject mainGridGO;
        public GameObject leftGridGO;
        public GameObject botGridGO;
        public GameObject rightGridGO;
        public GameObject topGridGO;

        [Header("Grid and Cell Size")]
        [Tooltip("This will determine which values from the below arrays we'll use for: \n\ngrid size (e.g 5x5, 6x6, or 7x7) \ncell size (e.g 200f, 166f, 142f)")]
        public GridSize gridSize = GridSize.Small;

        [Tooltip("Set width/height of the grid that corresponds to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        public int[] gridSizeValues = new int[3] { 5, 6, 7 }; //todo: name these using an editor script to make it easier to understand

        [Tooltip("Set cell size of the main grid that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        public float[] nodeCellSizeValues = new float[3] { 200f, 166.66f, 142.86f };

        [Tooltip("Set cell size of the external grids that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        public float[] navCellSizeValues = new float[3] { 200f, 166.66f, 142.86f };

        void Start()
        {
            CreateAllGrids();
        }

        //todo: Delete Update() later
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha5))
                CreateAllGrids();

            if (Input.GetKeyDown(KeyCode.Alpha6))
                CreateAllGrids(GridSize.Medium);

            if (Input.GetKeyDown(KeyCode.Alpha7))
                CreateAllGrids(GridSize.Large);
        }
#endif

        private void CreateAllGrids(GridSize newGridSize = GridSize.Small)
        {
            gridSize = newGridSize;

            CreateGrid(mainGridGO, Dir.None);
            CreateGrid(leftGridGO, Dir.Left);
            CreateGrid(botGridGO, Dir.Bot);
            CreateGrid(rightGridGO, Dir.Right);
            CreateGrid(topGridGO, Dir.Top);
        }

        private void CreateGrid(GameObject parent, Dir direction = Dir.None)
        {
            int width = 1;
            int height = 1;
            int gridLength = gridSizeValues[(int)gridSize];
            float cellSize = navCellSizeValues[(int)gridSize];

            switch (direction)
            {
                case Dir.None:
                    width = gridLength;
                    height = gridLength;
                    cellSize = nodeCellSizeValues[(int)gridSize];
                    break;
                case Dir.Left:
                case Dir.Right:
                    height = gridLength;
                    break;
                case Dir.Bot:
                case Dir.Top:
                    width = gridLength;
                    break;
            }

            ClearChildren(parent);
            GetGridArray(parent).Create(width, height, direction);
            GetGLG(parent).cellSize = new Vector2(cellSize, cellSize);
        }

        private void ClearChildren(GameObject parent)
        {
            foreach (Transform cell in parent.transform)
                Destroy(cell.gameObject);
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
