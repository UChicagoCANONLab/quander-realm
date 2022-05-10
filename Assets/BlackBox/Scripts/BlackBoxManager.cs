using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class BlackBoxManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private EndPanel endPanel = null;
        [SerializeField] private Level level = null;

        [Header("Grid Containers")]
        [SerializeField] private GameObject mainGridGO = null;
        [SerializeField] private GameObject leftGridGO = null;
        [SerializeField] private GameObject botGridGO = null;
        [SerializeField] private GameObject rightGridGO = null;
        [SerializeField] private GameObject topGridGO = null;

        [Header("Lantern Mounts")]
        [SerializeField] private GameObject[] lanternMounts = null;

        [Header("Grid and Cell Size")]
        [Tooltip("This will determine which values from the below arrays we'll use for: \n\ngrid size (e.g 5x5, 6x6, or 7x7) \ncell size (e.g 200f, 166f, 142f)")]
        [SerializeField] 
        private GridSize gridSize = GridSize.Small;

        [Tooltip("Set width/height of the grid that corresponds to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        [SerializeField] 
        private int[] gridSizeValues = new int[3] { 5, 6, 7 }; //todo: name these using an editor script to make it easier to understand

        [Tooltip("Set cell size of the main grid that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        [SerializeField] 
        private float[] nodeCellSizeValues = new float[3] { 200f, 166.66f, 142.86f };

        [Tooltip("Set cell size of the external grids that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        [SerializeField] 
        private float[] navCellSizeValues = new float[3] { 200f, 166.66f, 142.86f };

        #endregion

        void Start()
        {
            if (level == null)
                CreateAllGrids(gridSize);
            else
                CreateAllGrids(level.gridSize);

            mainGridGO.GetComponent<MainGrid>().SetNodes(level.nodePositions);
            InitializeLanterns(level.nodePositions.Length);
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
                BlackBoxEvents.ToggleDebug?.Invoke();
        }

        private void OnEnable()
        {
            BlackBoxEvents.ReturnLanternHome += ReturnLanternHome;
            BlackBoxEvents.CheckWinState += CheckWinState;
        }

        private void OnDisable()
        {
            BlackBoxEvents.ReturnLanternHome -= ReturnLanternHome;
            BlackBoxEvents.CheckWinState -= CheckWinState;
        }

        private void CreateAllGrids(GridSize gSize)
        {
            gridSize = gSize;
            BlackBoxEvents.InitEnergyBar?.Invoke(level.numEnergyUnits); //todo: move this to start when debugging is removed

            CreateMainGrid();

            CreateUnitGrid(leftGridGO, Dir.Left);
            CreateUnitGrid(botGridGO, Dir.Bot);
            CreateUnitGrid(rightGridGO, Dir.Right);
            CreateUnitGrid(topGridGO, Dir.Top);
        }

        private void CreateMainGrid()
        {
            int gridLength = gridSizeValues[(int)gridSize];
            float cellSize = nodeCellSizeValues[(int)gridSize];

            ClearChildren(mainGridGO);
            mainGridGO.GetComponent<MainGrid>().Create(gridLength, gridLength, level.numEnergyUnits);
            mainGridGO.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);
        }

        private void CreateUnitGrid(GameObject parent, Dir direction = Dir.None)
        {
            int width = 1;
            int height = 1;
            int gridLength = gridSizeValues[(int)gridSize];
            float cellSize = navCellSizeValues[(int)gridSize];

            switch (direction)
            {
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
            parent.GetComponent<UnitGrid>().Create(width, height, direction);
            parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);
        }

        private void InitializeLanterns(int length)
        {
            for (int i = 0; i < length; i++)
            {
                lanternMounts[i].SetActive(true);
                lanternMounts[i].GetComponent<LanternMount>().SetColliderActive(gridSize);
            }
        }

        private void ReturnLanternHome(GameObject lantern)
        {
            foreach (GameObject mountGO in lanternMounts)
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

        private void CheckWinState()
        {
            WinState winState = new(level.nodePositions.Length, 
                mainGridGO.GetComponent<MainGrid>().GetNumCorrect(level.nodePositions)); // todo: add level.reward here

            endPanel.gameObject.SetActive(true);
            endPanel.UpdatePanel(winState);
        }

        #region Helpers

        private void ClearChildren(GameObject parent)
        {
            foreach (Transform cell in parent.transform)
                Destroy(cell.gameObject);
        }

        #endregion
    }
}
