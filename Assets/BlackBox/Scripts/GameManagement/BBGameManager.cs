using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Wrapper;
using BeauRoutine;
using UnityEngine.SceneManagement;

namespace BlackBox
{
    public class BBGameManager : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private string firstLevelID = "L01"; // todo: refactor
        [SerializeField] private float rewardPopupDelay = 0.5f;

        [Header("Grid Containers")]
        [SerializeField] private GameObject mainGridGO;
        [SerializeField] private GameObject leftGridGO;
        [SerializeField] private GameObject botGridGO;
        [SerializeField] private GameObject rightGridGO;
        [SerializeField] private GameObject topGridGO;

        [Header("Lantern and Mounts")]
        public Transform lanternFrontMount;

        [SerializeField] private GameObject lanternPrefab;
        [SerializeField] private GameObject[] lanternMounts;

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

        private BBSaveData saveData;
        private const string levelsPath = "BlackBox/Levels";
        private const int totalLives = 3;
        private int livesRemaining;
        private int totalNodes;
        private Level level;
        private bool debug = false;

        #region Unity Functions

        void Start()
        {
            InitSaveData();
            InitLevel();
            StartLevel();
        }

        // Debug
#if UNITY_EDITOR || UNITY_WEBGL
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
                BBEvents.ToggleDebug?.Invoke();
        }
#endif

        private void OnEnable()
        {
            BBEvents.QuitBlackBox += Quit;
            BBEvents.GotoLevel += NextLevel; // Debug
            BBEvents.IsDebug += GetDebugBool; // Debug
            BBEvents.ToggleDebug += ToggleDebug; // Debug
            BBEvents.RestartLevel += StartLevel;
            BBEvents.StartNextLevel += NextLevel;
            BBEvents.CheckWinState += CheckWinState;
            BBEvents.CheckWolfieReady += CheckWolfieReady;
            BBEvents.GetFrontMount += GetLanternFrontMount;
            BBEvents.GetNumEnergyUnits += GetNumEnergyUnits;
            BBEvents.ReturnLanternHome += ReturnLanternHome;
        }

        private void OnDisable()
        {
            BBEvents.QuitBlackBox -= Quit;
            BBEvents.GotoLevel -= NextLevel; // Debug
            BBEvents.IsDebug -= GetDebugBool; // Debug
            BBEvents.ToggleDebug -= ToggleDebug; // Debug
            BBEvents.RestartLevel -= StartLevel;
            BBEvents.StartNextLevel -= NextLevel;
            BBEvents.CheckWinState -= CheckWinState;
            BBEvents.CheckWolfieReady -= CheckWolfieReady;
            BBEvents.GetFrontMount -= GetLanternFrontMount;
            BBEvents.GetNumEnergyUnits -= GetNumEnergyUnits;
            BBEvents.ReturnLanternHome -= ReturnLanternHome;
        }

        #endregion

        #region Level Management

        private void InitSaveData()
        {
            try
            {
                string saveString = Events.GetMinigameSaveData?.Invoke(Game.BlackBox);
                saveData = JsonUtility.FromJson<BBSaveData>(saveString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if (saveData == null)
                saveData = new BBSaveData();
        }

        private void InitLevel()
        {
            string levelID = saveData.currentLevelID.Equals(string.Empty) ? firstLevelID : saveData.currentLevelID;
            level = Resources.Load<Level>(Path.Combine(levelsPath, levelID)); // todo: try catch here?
        }

        private void StartLevel()
        {
            BBEvents.ShowTutorial?.Invoke(saveData, level);
            CreateAllGrids(level.gridSize);
            mainGridGO.GetComponent<MainGrid>().SetNodes(level.nodePositions);

            totalNodes = level.nodePositions.Length;
            livesRemaining = totalLives;

            BBEvents.UpdateHUDWolfieLives?.Invoke(livesRemaining);
            BBEvents.ToggleWolfieButton?.Invoke(false);
            BBEvents.InitEnergyBar?.Invoke();

            InitializeLanterns(level.nodePositions.Length);
        }

        private void NextLevel()
        {
            if (level.nextLevelID == string.Empty)
            {
                Debug.LogFormat("Next level not set for the level: {0}", level.levelID);
                Quit();
                return;
            }   

            // reusing the SetNodes function. Toggles the initially set nodes off
            mainGridGO.GetComponent<MainGrid>().SetNodes(level.nodePositions); // todo: refactor

            level = Resources.Load<Level>(Path.Combine(levelsPath, level.nextLevelID));
            StartLevel();
        }

        // Debug
        private void NextLevel(string levelID)
        {
            Level tempLevel = Resources.Load<Level>(Path.Combine(levelsPath, levelID));
            if (tempLevel == null)
            {
                Debug.LogFormat("Could not find level with ID: {0}", levelID);
                return;
            }

            mainGridGO.GetComponent<MainGrid>().SetNodes(level.nodePositions);

            level = tempLevel;
            StartLevel();
        }

        private void Quit()
        {
            SceneManager.LoadScene(0);
            Events.MinigameClosed?.Invoke();
        }

        private void CheckWinState()
        {
            int numCorrect = mainGridGO.GetComponent<MainGrid>().GetNumCorrect(level.nodePositions);
            bool levelWon = totalNodes == numCorrect;

            if (levelWon)
            {
                Wrapper.Events.PlaySound?.Invoke("BB_WolfieSuccess");
                saveData.currentLevelID = level.nextLevelID;
                Events.UpdateMinigameSaveData?.Invoke(Game.BlackBox, saveData);
            }
            else
            {
                Wrapper.Events.PlaySound?.Invoke("BB_WolfieFail");
                livesRemaining--;
                BBEvents.UpdateHUDWolfieLives?.Invoke(livesRemaining);
            }

            WinState winState = new(totalNodes, numCorrect, levelWon, level.number, livesRemaining);
            Routine.Start(DisplayPlayerFeedBack(winState));
        }

        private IEnumerator DisplayPlayerFeedBack(WinState winState)
        {
            BBEvents.UpdateEndPanel?.Invoke(winState);

            if (winState.levelWon)
            { 
                yield return rewardPopupDelay;
                Events.CollectAndDisplayReward?.Invoke(Game.BlackBox, level.number);
            }
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

        private void ReturnLanternHome(GameObject lanternGO)
        {
            foreach (GameObject mountGO in lanternMounts)
            {
                LanternMount mount = mountGO.GetComponent<LanternMount>();
                if (mount.isEmpty)
                {
                    lanternGO.transform.SetParent(mount.transform);
                    lanternGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    mount.Flag(lanternGO);
                    break;
                }
            } 
        }

        private void CheckWolfieReady()
        {
            bool isWolfieReady = true;

            foreach (GameObject mountGO in lanternMounts)
            {
                LanternMount mount = mountGO.GetComponent<LanternMount>();
                if (mountGO.activeInHierarchy && !(mount.isEmpty))
                {
                    isWolfieReady = false;
                    break;
                }
            }

            BBEvents.ToggleWolfieButton?.Invoke(isWolfieReady);
        }

        private Transform GetLanternFrontMount()
        {
            return lanternFrontMount;
        }

        #endregion

        private int GetNumEnergyUnits()
        {
            return level.numEnergyUnits;
        }

        private bool GetDebugBool()
        {
            return debug;
        }

        private void ToggleDebug()
        {
            debug = !debug;
        }
    }
}
