using System;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class BlackBoxManager : MonoBehaviour
    {
        #region Variables

        public Button wolfieButton; //todo: use an event instead of this reference
        public GameObject EndPanel; //todo: use an event instead of this reference
        public GameObject gridPrefab = null;
        public Level level = null;

        [Header("Grid Containers")]
        public GameObject mainGridGO = null;
        public GameObject leftGridGO = null;
        public GameObject botGridGO = null;
        public GameObject rightGridGO = null;
        public GameObject topGridGO = null;

        [Header("Lantern Mounts")]
        public GameObject[] lanternMounts;

        [Header("Grid and Cell Size")]
        [Tooltip("This will determine which values from the below arrays we'll use for: \n\ngrid size (e.g 5x5, 6x6, or 7x7) \ncell size (e.g 200f, 166f, 142f)")]
        public GridSize gridSize = GridSize.Small;

        [Tooltip("Set width/height of the grid that corresponds to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        public int[] gridSizeValues = new int[3] { 5, 6, 7 }; //todo: name these using an editor script to make it easier to understand

        [Tooltip("Set cell size of the main grid that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        public float[] nodeCellSizeValues = new float[3] { 200f, 166.66f, 142.86f };

        [Tooltip("Set cell size of the external grids that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        public float[] navCellSizeValues = new float[3] { 200f, 166.66f, 142.86f };

        #endregion

        void Start()
        {
            wolfieButton.onClick.AddListener(CheckWinState);
            GameEvents.ReturnToHome.AddListener((lantern) => ReturnLanternHome(lantern));
            //GameEvents.CheckWinState.AddListener(CheckWinState);

            if (level == null)
                CreateAllGrids(gridSize);
            else
                CreateAllGrids(level.gridSize);

            GetGridArray(mainGridGO).SetNodes(level.nodePositions);
            InitializeLanterns(level.nodePositions.Length);
        }

        private void ReturnLanternHome(GameObject lantern)
        {
            foreach(GameObject mountGO in lanternMounts)
            {
                LanternMount mount = mountGO.GetComponent<LanternMount>();

                if (mount.isEmpty)
                {
                    lantern.transform.SetParent(mount.transform);
                    lantern.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    mount.EvaluateEmpty();
                    break;
                }
            }
        }

        //todo: Delete Update() later
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha5))
                CreateAllGrids(GridSize.Small);

            if (Input.GetKeyDown(KeyCode.Alpha6))
                CreateAllGrids(GridSize.Medium);

            if (Input.GetKeyDown(KeyCode.Alpha7))
                CreateAllGrids(GridSize.Large);

            if (Input.GetKeyDown(KeyCode.D))
                GameEvents.ToggleDebug?.Invoke();
        }

        private void CheckWinState()
        {
            int numCorrect = GetGridArray(mainGridGO).GetNumCorrect(level.nodePositions);
            int numNodes = level.nodePositions.Length;

            EndPanel.SetActive(true);

            if (numCorrect == numNodes)
            {
                GameEvents.SetEndPanelText?.Invoke("You Won!");
                Debug.Log("Win");
            }
            else
            {
                GameEvents.SetEndPanelText?.Invoke("You found " + numCorrect + " out of " + numNodes + " nodes.");
                Debug.LogFormat("Lose: {0}/{1}", numCorrect, numNodes);
            }
        }

        private void CreateAllGrids(GridSize gSize)
        {
            gridSize = gSize;

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

        private void InitializeLanterns(int length)
        {
            for (int i = 0; i < length; i++)
            {
                lanternMounts[i].SetActive(true);
                lanternMounts[i].GetComponent<LanternMount>().SetColliderActive(gridSize);
            }
        }

        #region Helpers

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

        #endregion
    }
}
