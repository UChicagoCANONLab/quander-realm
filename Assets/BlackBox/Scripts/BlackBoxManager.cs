using UnityEngine;

namespace BlackBox
{
    public class BlackBoxManager : MonoBehaviour
    {

        public int gridSize;
        public float mainCellSize;
        public float unitCellSize;
        public GameObject gridPrefab;

        [Header("Grid Parents")]
        public GameObject mainGridContainer;
        public GameObject leftGridContainer;
        public GameObject botGridContainer;
        public GameObject rightGridContainer;
        public GameObject topGridContainer;

        private BGrid mainGrid;
        private BGrid leftGrid;
        private BGrid botGrid;
        private BGrid rightGrid;
        private BGrid topGrid;

        void Start()
        {
            ClearChildren(mainGridContainer);
            InstantiateGridObjects();
            CreateGrids(gridSize);
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

            if (Input.GetKeyDown(KeyCode.Alpha5))
                CreateGrids(5);

            if (Input.GetKeyDown(KeyCode.Alpha6))
                CreateGrids(6);

            if (Input.GetKeyDown(KeyCode.Alpha7))
                CreateGrids(7);

            if (Input.GetKeyDown(KeyCode.Alpha8))
                CreateGrids(8);

            if (Input.GetKeyDown(KeyCode.Alpha9))
                CreateGrids(9);
        }

        private void InstantiateGridObjects()
        {
            mainGrid = Instantiate(gridPrefab, mainGridContainer.transform.position, Quaternion.identity, mainGridContainer.transform).GetComponent<BGrid>();
            //leftGrid = Instantiate(gridPrefab, leftGridOrigin.position, Quaternion.identity, transform).GetComponent<BGrid>();
            //botGrid = Instantiate(gridPrefab, botGridOrigin.position, Quaternion.identity, transform).GetComponent<BGrid>();
            //rightGrid = Instantiate(gridPrefab, rightGridOrigin.position, Quaternion.identity, transform).GetComponent<BGrid>();
            //topGrid = Instantiate(gridPrefab, topGridOrigin.position, Quaternion.identity, transform).GetComponent<BGrid>();
        }

        private void CreateGrids(int size)
        {
            ClearChildren(mainGrid.gameObject);

            mainGrid.CreateGrid(size, size, mainCellSize, mainGridContainer.transform.position, CellType.Node);

            //leftGrid.CreateGrid(1, size, cellSize, origin + new Vector2(-cellSize * 1.5f, 0), CellType.Unit, Dir.Left);
            //botGrid.CreateGrid(size, 1, cellSize, origin + new Vector2(0, -cellSize * 1.5f), CellType.Unit, Dir.Bot);
            //rightGrid.CreateGrid(1, size, cellSize, origin + new Vector2(size * cellSize + cellSize / 2, 0), CellType.Unit, Dir.Right);
            //topGrid.CreateGrid(size, 1, cellSize, origin + new Vector2(0, size * cellSize + cellSize / 2), CellType.Unit, Dir.Top);
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
