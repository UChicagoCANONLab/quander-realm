using System;
using UnityEngine;

namespace BlackBox
{
    public class GameManager : MonoBehaviour
    {
        public int gridSize;
        public float cellSize;
        public Vector2 origin;

        public BGrid mainGrid;
        public BGrid leftGrid;
        public BGrid botGrid;
        public BGrid rightGrid;
        public BGrid topGrid;

        void Start()
        {
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

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

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

        private void CreateGrids(int size)
        {
            ClearGrids();

            mainGrid.CreateGrid(size, size, cellSize, origin, CellType.Node);

            leftGrid.CreateGrid(1, size, cellSize, origin + new Vector2(-cellSize * 1.5f, 0), CellType.Unit, Dir.Left);
            botGrid.CreateGrid(size, 1, cellSize, origin + new Vector2(0, -cellSize * 1.5f), CellType.Unit, Dir.Bot);
            rightGrid.CreateGrid(1, size, cellSize, origin + new Vector2(size * cellSize + cellSize / 2, 0), CellType.Unit, Dir.Right);
            topGrid.CreateGrid(size, 1, cellSize, origin + new Vector2(0, size * cellSize + cellSize / 2), CellType.Unit, Dir.Top);
        }

        private void ClearGrids()
        {
            foreach(Transform grid in transform)
                foreach (Transform cell in grid.transform)
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
