using System;
using System.Linq;
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

        [SerializeField] private string firstLevelID = "4.1"; // todo: refactor
        [SerializeField] private float rewardPopupDelay = 0.5f;

        [Header("Level Select")]
        [SerializeField] GameObject gameBoard;
        [SerializeField] GameObject gameUI;
        [SerializeField] GameObject levelSelect;
        [SerializeField] LevelButton[] levelButtons;
        [SerializeField] QButton gameBackButton;
        [SerializeField] Animator backgroundAnimator;

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
        // private int[] gridSizeValues = new int[3] { 5, 6, 7 }; //todo: name these using an editor script to make it easier to understand
        private int[] gridSizeValues = new int[4] { 5, 6, 7, 4 };

        [Tooltip("Set cell size of the main grid that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        [SerializeField] 
        // private float[] nodeCellSizeValues = new float[3] { 200f, 166.66f, 142.86f };
        private float[] nodeCellSizeValues = new float[4] { 200f, 166.66f, 142.86f, 250f };

        [Tooltip("Set cell size of the external grids that correspond to the above \"Grid Size\".\n\n0 = Small, \n1 = Medium. \n2 = Large")]
        [SerializeField] 
        // private float[] navCellSizeValues = new float[3] { 200f, 166.66f, 142.86f };
        private float[] navCellSizeValues = new float[4] { 200f, 166.66f, 142.86f, 250f };

        [SerializeField] private BBSaveManager SM;

        #endregion

        private const string levelsPath = "BlackBox/Levels";
        private const int totalLives = 3;
        private int livesRemaining;
        private int totalNodes;
        private Level level;
        private bool debug = false;
        private float[] playTimes = {0f,0f};


        #region Unity Functions

        void Start()
        {
            Events.PlayMusic?.Invoke("BB_Music");

            InitSaveData();
            InitLevel(); 
            //StartLevel();     moved into InitLevel with level select 
        }

        // Debug
#if UNITY_EDITOR || UNITY_WEBGL
        private void Update()
        {
            if (Events.IsDebugEnabled.Invoke() && Input.GetKeyDown(KeyCode.D))
                BBEvents.ToggleDebug?.Invoke();
        }
#endif

        private void OnEnable()
        {
            BBEvents.QuitBlackBox += Quit;
            if (Events.IsDebugEnabled.Invoke())
            {
                BBEvents.GotoLevel += NextLevel; // Debug
                BBEvents.IsDebug += GetDebugBool; // Debug
                BBEvents.ToggleDebug += ToggleDebug; // Debug
            }
            BBEvents.RestartLevel += RestartLevel;
            BBEvents.StartNextLevel += NextLevel;
            BBEvents.CheckWinState += CheckWinState;
            BBEvents.CheckWolfieReady += CheckWolfieReady;
            BBEvents.GetFrontMount += GetLanternFrontMount;
            BBEvents.GetNumEnergyUnits += GetNumEnergyUnits;
            BBEvents.LoseLife += LoseLife;
            BBEvents.GetLivesRemaining += GetLivesRemaining;
            BBEvents.ReturnLanternHome += ReturnLanternHome;
            BBEvents.CompleteBlackBox += PlayEndDialog;
            BBEvents.PlayLevel += SetAndPlayLevel;
            gameBackButton.onClick.AddListener(() => BBEvents.CloseLevel?.Invoke());
            BBEvents.OpenLevelSelect += ShowLevelSelect;
            BBEvents.GetLevel += GetLevelObject;
        }

        private void OnDisable()
        {
            BBEvents.QuitBlackBox -= Quit;
            if (Wrapper.Events.IsDebugEnabled.Invoke())
            {
                BBEvents.GotoLevel -= NextLevel; // Debug
                BBEvents.IsDebug -= GetDebugBool; // Debug
                BBEvents.ToggleDebug -= ToggleDebug; // Debug
            }
            BBEvents.RestartLevel -= RestartLevel;
            BBEvents.StartNextLevel -= NextLevel;
            BBEvents.CheckWinState -= CheckWinState;
            BBEvents.CheckWolfieReady -= CheckWolfieReady;
            BBEvents.GetFrontMount -= GetLanternFrontMount;
            BBEvents.GetNumEnergyUnits -= GetNumEnergyUnits;
            BBEvents.LoseLife -= LoseLife;
            BBEvents.GetLivesRemaining -= GetLivesRemaining;
            BBEvents.ReturnLanternHome -= ReturnLanternHome;
            BBEvents.CompleteBlackBox -= PlayEndDialog;
            BBEvents.PlayLevel -= SetAndPlayLevel;
            gameBackButton.onClick.RemoveListener(() => BBEvents.CloseLevel?.Invoke());
            BBEvents.OpenLevelSelect -= ShowLevelSelect;
            BBEvents.GetLevel -= GetLevelObject;
        }

        #endregion

        #region Level Management

        private void InitSaveData()
        {
            SM.LoadGame();

            /* try
            {
                string saveString = Events.GetMinigameSaveData?.Invoke(Wrapper.Game.BlackBox);
                saveData = JsonUtility.FromJson<BBSaveData>(saveString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            if ((saveData == null) || saveData.starsPerLevel.Length < 24) {
                saveData = new BBSaveData();
            }
            int temp = 0;
            foreach (int i in saveData.starsPerLevel) { temp += i; }
            saveData.totalStars = temp; */
        }

        private void InitLevel()
        {
            string levelID = SM.saveData.currentLevelID.Equals(string.Empty) ? firstLevelID : SM.saveData.currentLevelID;
            level = Resources.Load<Level>(Path.Combine(levelsPath, levelID)); // todo: try catch here?

            if (levelID == firstLevelID) {
                Events.StartDialogueSequence?.Invoke("BB_Intro_0");
            }

            ShowLevelSelect(true);
        }

        private void StartLevel()
        {
            ShowLevelSelect(false);
            Events.ToggleBackButton?.Invoke(false);

            BBEvents.UpdateHUDLevelNumber?.Invoke(level.number);
            BBEvents.ShowTutorial?.Invoke(SM.saveData, level);
            BBEvents.ClearHints?.Invoke();
            CreateAllGrids(level.gridSize);
            mainGridGO.GetComponent<MainGrid>().SetNodes(level.nodePositions);

            if (level.levelID == firstLevelID) {
                BBEvents.InitiateTutorialLevel?.Invoke(); 
            }
            if (level.number <= 6) {
                backgroundAnimator.SetBool("FogActive", false);
            } else { backgroundAnimator.SetBool("FogActive", true); }

            totalNodes = level.nodePositions.Length;
            livesRemaining = totalLives;

            BBEvents.UpdateHUDWolfieLives?.Invoke(livesRemaining);
            BBEvents.ToggleWolfieButton?.Invoke(false);
            BBEvents.InitEnergyBar?.Invoke();

            InitializeLanterns(level.nodePositions.Length);

            playTimes[0] = Time.time;
        }

        private void RestartLevel() 
        {
            // Now collecting data when the player restarts, but not when already saved WinState
            if (livesRemaining > 0)
            {
                playTimes[1] = Time.time;
                SM.researchData.hintsUsed = BBEvents.GetHintsUsed.Invoke();
                SM.researchData.timePlayed = playTimes[1] - playTimes[0];

                SM.researchData.winStateString = $"\"RESTART\", \"livesRemaining\": {livesRemaining}, \"wonLevel\": false";
                SM.SaveGame();
            }
            StartLevel();
        }

        private void NextLevel()
        {
            if (level.nextLevelID == string.Empty)
            {
                // Debug.LogFormat("Next level not set for the level: {0}", level.levelID);
                // Quit();
                ShowLevelSelect(true);
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
            Events.ToggleBackButton?.Invoke(true);
            Events.MinigameClosed?.Invoke();
        }

        private void CheckWinState()
        {
            int numCorrect = mainGridGO.GetComponent<MainGrid>().GetNumCorrect(level.nodePositions);
            bool levelWon = totalNodes == numCorrect;

            if (levelWon)
            {
                Wrapper.Events.PlaySound?.Invoke("BB_WolfieSuccess");

                int origStars = SM.saveData.starsPerLevel[level.number - 1];

                if (origStars < livesRemaining) {
                    SM.saveData.totalStars += (livesRemaining - origStars);
                    SM.saveData.starsPerLevel[level.number - 1] = livesRemaining;
                }

                // Earn coins for level completion
                int coins = Math.Max(livesRemaining - origStars, 0) * 10 + Math.Min(livesRemaining, origStars);


                // PLAY END OF LEVEL DIALOGUE IF APPLICABLE
                TrySetNewLevelSave();

                if (level.levelID == firstLevelID) {
                    BBEvents.EndTutorialLevel?.Invoke();
                }
            }
            else
            {
                Wrapper.Events.PlaySound?.Invoke("BB_WolfieFail");
                LoseLife();
            }

            WinState winState = new(totalNodes, numCorrect, levelWon, level.number, livesRemaining, ParseLevelID(level.nextLevelID) == -1);
            Routine.Start(DisplayPlayerFeedBack(winState));
        }

        void TrySetNewLevelSave()
        {
            if (SM.saveData.currentLevelID == string.Empty || ParseLevelID(SM.saveData.currentLevelID) < ParseLevelID(level.nextLevelID))
            {
                SM.saveData.currentLevelID = level.nextLevelID;
                // Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.BlackBox, saveData);
            }
            // else Debug.Log("Player has completed a higher level; save data not updated.");
            
            // Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.BlackBox, saveData);
            // SM.SaveGame();
        }

        private IEnumerator DisplayPlayerFeedBack(WinState winState)
        {
            BBEvents.UpdateEndPanel?.Invoke(winState);
            
            if (winState.levelWon || winState.livesRemaining == 0) 
            {    
                playTimes[1] = Time.time;
                SM.researchData.timePlayed = playTimes[1] - playTimes[0];
                SM.researchData.hintsUsed = BBEvents.GetHintsUsed.Invoke();

                SM.researchData.winStateString = JsonUtility.ToJson(winState);
                SM.SaveGame();
            }

            if (winState.levelWon)
            { 
                yield return rewardPopupDelay;
                Events.CollectAndDisplayReward?.Invoke(Game.BlackBox, level.number);
            }
        }

        void PlayEndDialog()
        {
            try
            {
                bool complete = SM.saveData.completed;
            }
            catch (Exception)
            {
                BBSaveData data = new BBSaveData
                {
                    gameID = SM.saveData.gameID,
                    currentLevelID = SM.saveData.currentLevelID,
                    tutorialsSeen = SM.saveData.tutorialsSeen,
                    completed = false
                };

                SM.saveData = data;
                // Events.UpdateMinigameSaveData?.Invoke(Game.BlackBox, saveData);
                SM.SaveGame();
            }
            finally
            {
                bool complete = SM.saveData.completed;
                if (!complete)
                {
                    Events.StartDialogueSequence?.Invoke("BB_End");
                    SM.saveData.completed = true;
                    // Events.UpdateMinigameSaveData?.Invoke(Game.BlackBox, saveData);
                    SM.SaveGame();
                }
            }
        }

        public Level GetLevelObject() {
            return level;
        }

        #endregion

        #region Level Select

        void ShowLevelSelect(bool show)
        {
            levelSelect.SetActive(show);
            gameBoard.SetActive(!show);
            gameUI.SetActive(!show);

            if (show) InitLevelSelect();
        }

        void InitLevelSelect()
        {
            Events.ToggleBackButton(true);

            if (level.levelID != SM.saveData.currentLevelID)
            {
                // if (saveData.currentLevelID[0] == 'L') {
                //     level = Resources.Load<Level>(Path.Combine(levelsPath, firstLevelID));
                // } else {
                    string levelID = SM.saveData.currentLevelID.Equals(string.Empty) ? firstLevelID : SM.saveData.currentLevelID;
                    level = Resources.Load<Level>(Path.Combine(levelsPath, levelID));
                // }
            }

            try
            {
                bool complete = SM.saveData.completed;
            }
            catch (Exception)
            {
                BBSaveData data = new BBSaveData
                {
                    gameID = SM.saveData.gameID,
                    currentLevelID = SM.saveData.currentLevelID,
                    tutorialsSeen = SM.saveData.tutorialsSeen,
                    completed = false
                };

                SM.saveData = data;
                // Events.UpdateMinigameSaveData?.Invoke(Game.BlackBox, saveData);
                SM.SaveGame();
            }
            finally
            {
                int levelNum = ParseLevelID(level.levelID) + (SM.saveData.completed ? 1 : 0);
                if (levelNum > 0)
                {
                    for (int i = 0; i < levelButtons.Length; i++) {
                        levelButtons[i].SetButtonState(levelNum, GetLevelStars(i));
                    }
                }
            }
        }

        void SetAndPlayLevel(string levelID)
        {
            level = Resources.Load<Level>(Path.Combine(levelsPath, levelID));
            StartLevel();
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
            BBEvents.ToggleWolfieButton?.Invoke(level.nodePositions.Length == BBEvents.LanternPlacedCount.Invoke());
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

        private void LoseLife() 
        {
            livesRemaining--;
            BBEvents.UpdateHUDWolfieLives?.Invoke(livesRemaining);

            if (livesRemaining == 0)
            {
                int numCorrect = mainGridGO.GetComponent<MainGrid>().GetNumCorrect(level.nodePositions);
                bool levelWon = totalNodes == numCorrect;

                WinState winState = new(totalNodes, numCorrect, levelWon, level.number, livesRemaining, ParseLevelID(level.nextLevelID) == -1);
                Routine.Start(DisplayPlayerFeedBack(winState));
            }
        }

        private int GetLivesRemaining()
        {
            return livesRemaining;
        }

        private bool GetDebugBool()
        {
            return debug;
        }

        private void ToggleDebug()
        {
            debug = !debug;
        }

        public static int ParseLevelID(string levelID)
        {
            if (levelID == "") return -1;

            int[] temp = levelID.Split(".").Select(int.Parse).ToArray();
            int levelNum = ((temp[0]-4) * 6) + temp[1];
            
            return levelNum;
        }

        public static string ParseLevelID(int level)
        {
            int prefix = (level/6) + 4; 
            int num = level % 6;

            string levelText = $"{prefix}.{num}";
            if (num==0) { levelText = $"{prefix-1}.6"; }

            return levelText;
        }

        public int GetLevelStars(int level) {
            if (level >= SM.saveData.starsPerLevel.Length) return 0;
            return SM.saveData.starsPerLevel[level];
        }
    }
}
