using UnityEngine;
using System.Collections.Generic;

namespace BlackBox
{
    public class BlackBoxManager : MonoBehaviour
    {
        public int gridSize;
        public float mainCellSize;
        //public float unitCellSize; //todo: allow differently sized unit grids?
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

        private Dictionary<GameObject, BGrid> grids;

        void Start()
        {
            grids = new Dictionary<GameObject, BGrid>();
            grids.Add(mainGridContainer, mainGrid);
            grids.Add(leftGridContainer, leftGrid);
            grids.Add(botGridContainer, botGrid);
            grids.Add(rightGridContainer, rightGrid);
            grids.Add(topGridContainer, topGrid);

            InstantiateGridObjects();
            CreateGrids(gridSize);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mainGrid.Interact(GetMouseWorldPosition());
                leftGrid.Interact(GetMouseWorldPosition());
                botGrid.Interact(GetMouseWorldPosition());
                rightGrid.Interact(GetMouseWorldPosition());
                topGrid.Interact(GetMouseWorldPosition());
            }

            //if (Input.GetKeyDown(KeyCode.Alpha5))
            //    CreateGrids(5);

            //if (Input.GetKeyDown(KeyCode.Alpha6))
            //    CreateGrids(6);

            //if (Input.GetKeyDown(KeyCode.Alpha7))
            //    CreateGrids(7);

            //if (Input.GetKeyDown(KeyCode.Alpha8))
            //    CreateGrids(8);

            //if (Input.GetKeyDown(KeyCode.Alpha9))
            //    CreateGrids(9);
        }

        //private void InitializeGrids()
        //{
        //    foreach(GameObject container in grids.Keys)
        //    {
        //        InstantiateGridGO(container);
        //        CreateGrid(grids[container]);
        //    }
        //}

        //private void CreateGrid(BGrid bGrid)
        //{
            
        //}

        //private void InstantiateGridGO(GameObject container)
        //{
        //    ClearChildren(container);
        //    grids[container] = Instantiate(gridPrefab, container.transform.position, Quaternion.identity, container.transform).GetComponent<BGrid>();
        //}

        private void InstantiateGridObjects()
        {
            mainGrid = Instantiate(gridPrefab, mainGridContainer.transform.position, Quaternion.identity, mainGridContainer.transform).GetComponent<BGrid>();
            leftGrid = Instantiate(gridPrefab, leftGridContainer.transform.position, Quaternion.identity, leftGridContainer.transform).GetComponent<BGrid>();
            botGrid = Instantiate(gridPrefab, botGridContainer.transform.position, Quaternion.identity, botGridContainer.transform).GetComponent<BGrid>();
            rightGrid = Instantiate(gridPrefab, rightGridContainer.transform.position, Quaternion.identity, rightGridContainer.transform).GetComponent<BGrid>();
            topGrid = Instantiate(gridPrefab, topGridContainer.transform.position, Quaternion.identity, topGridContainer.transform).GetComponent<BGrid>();
        }

        private void CreateGrids(int size)
        {
            //ClearChildren();

            mainGrid.CreateGrid(size, size, mainCellSize, mainGridContainer.transform.position, CellType.Node);
            leftGrid.CreateGrid(1, size, mainCellSize, leftGridContainer.transform.position, CellType.Unit, Dir.Left);
            botGrid.CreateGrid(size, 1, mainCellSize, botGridContainer.transform.position, CellType.Unit, Dir.Bot);
            rightGrid.CreateGrid(1, size, mainCellSize, rightGridContainer.transform.position, CellType.Unit, Dir.Right);
            topGrid.CreateGrid(size, 1, mainCellSize, topGridContainer.transform.position, CellType.Unit, Dir.Top);
        }

        private void ClearChildren()
        {
            foreach (GameObject container in grids.Keys)
                foreach (Transform cell in container.transform)
                    Destroy(cell.gameObject);
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
