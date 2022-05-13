using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class BBGameManager : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private string firstLevelID = "L01"; // todo: refactor

        [Header("Grid Containers")]
        [SerializeField] private GameObject mainGridGO = null;
        [SerializeField] private GameObject leftGridGO = null;
        [SerializeField] private GameObject botGridGO = null;
        [SerializeField] private GameObject rightGridGO = null;
        [SerializeField] private GameObject topGridGO = null;

        [Header("Lantern and Mounts")]
        [SerializeField] private GameObject lanternPrefab = null;
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

        private const string levelsPath = "BlackBox/Levels";
        private const int totalLives = 3;
        private int livesRemaining;
        private int totalNodes;
        private Level level = null;

        #region Unity Functions

        void Start()
        {
            level = Resources.Load<Level>(Path.Combine(levelsPath, firstLevelID));
            StartLevel();
        }

        //todo: Delete Update() later
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha5))
            //    CreateAllGrids(GridSize.Small);

            //if (Input.GetKeyDown(KeyCode.Alpha6))
            //    CreateAllGrids(GridSize.Medium);

            //if (Input.GetKeyDown(KeyCode.Alpha7))
            //    CreateAllGrids(GridSize.Large);

            if (Input.GetKeyDown(KeyCode.D))
                BBEvents.ToggleDebug?.Invoke();
        }

        private void OnEnable()
        {
            BBEvents.ReturnLanternHome += ReturnLanternHome;
            BBEvents.CheckWinState += CheckWinState;
            BBEvents.StartNextLevel += NextLevel;
        }

        private void OnDisable()
        {
            BBEvents.ReturnLanternHome -= ReturnLanternHome;
            BBEvents.CheckWinState -= CheckWinState;
            BBEvents.StartNextLevel -= NextLevel;
        }

        #endregion

        #region Level Management

        private void StartLevel()
        {
            CreateAllGrids(level.gridSize);
            mainGridGO.GetComponent<MainGrid>().SetNodes(level.nodePositions);

            totalNodes = level.nodePositions.Length;
            livesRemaining = totalLives;

            BBEvents.UpdateHUDWolfieLives?.Invoke(livesRemaining);
            BBEvents.InitEnergyBar?.Invoke(level.numEnergyUnits);

            InitializeLanterns(level.nodePositions.Length);
        }

        private void NextLevel()
        {
            if (level.nextLevelID == string.Empty)
            {
                Debug.LogFormat("Next level not set for the level: {0}", level.levelID);
                return;
            }   

            // reusing the SetNodes function. Toggles the initially set nodes off
            mainGridGO.GetComponent<MainGrid>().SetNodes(level.nodePositions); // todo: refactor

            level = Resources.Load<Level>(Path.Combine(levelsPath, level.nextLevelID));
            StartLevel();
        }

        private void CheckWinState()
        {
            int numCorrect = mainGridGO.GetComponent<MainGrid>().GetNumCorrect(level.nodePositions);
            bool levelWon = totalNodes == numCorrect;

            if (!levelWon)
            {
                livesRemaining--;
                BBEvents.UpdateHUDWolfieLives?.Invoke(livesRemaining);
            }

            WinState winState = new(totalNodes, numCorrect, levelWon, livesRemaining); // todo: add level.reward here
            BBEvents.UpdateEndPanel?.Invoke(winState);
        }

        #endregion

        #region Grids

        private void CreateAllGrids(GridSize gSize)
        {
            gridSize = gSize;

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

        private void ClearChildren(GameObject parent)
        {
            foreach (Transform cell in parent.transform)
                Destroy(cell.gameObject);
        }

        #endregion

        #region Lanterns

        private void InitializeLanterns(int length)
        {
            ResetLanternsAndMounts();

            for (int i = 0; i < length; i++)
            {
                lanternMounts[i].SetActive(true);
                lanternMounts[i].GetComponent<LanternMount>().SetColliderActive(gridSize);
            }
        }

        private void ResetLanternsAndMounts()
        {
            foreach (GameObject mountObject in lanternMounts)
            {
                mountObject.SetActive(false);

                LanternMount mount = mountObject.GetComponent<LanternMount>();
                mount.EvaluateEmpty();
                if (mount.isEmpty)
                    mount.SetMountedLantern(Instantiate(lanternPrefab, mountObject.transform));
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

        #endregion
    }
}