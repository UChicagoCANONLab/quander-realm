using UnityEngine;
using System;
using System.Collections;

/*
 * Manages game flow for each level 
 * Spawn puzzles
 */
namespace Qupcakery
{
    public class LevelManager : MonoBehaviour
    {
        public GameObject GatePrefab;
        public GameObject TablePrefab;
        public GameObject BeltPrefab;

        public Dispatcher Dispatcher { get; private set; }

        public Level level;
        public int currentBatchNum { get; private set; } = -1;
        public Solution solution { get; private set; } = new Solution();

        private Timer timer;
        private bool levelEnded = false;

        public delegate void LevelEndedHandler();
        public LevelEndedHandler LevelEnded;

        private bool hasTutorial = false;

        private void Start()
        {
            GameManagement gameManager = GameManagement.Instance;
            switch(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
            {
                case "QU_DailyLevel":
                    gameManager.SetGameMode(GameManagement.GameMode.Daily);
                    gameManager.SetCurrentLevel(28);
                    level = gameManager.GetCurrentLevel();
                    break;
                default:
                    gameManager.SetGameMode(GameManagement.GameMode.Regular);
                    level = gameManager.GetCurrentLevel();

                    // If tutorial available, start tutorial sequence
                    int levelInd = level.LevelInd;

                    if (TutorialManager.tutorialAvailable[levelInd])
                    {
                        hasTutorial = true;
                        Wrapper.Events.StartDialogueSequence?.Invoke("QU_Level" + levelInd.ToString());
                        TutorialManager.UpdateAvailability(levelInd);
                        Wrapper.Events.DialogueSequenceEnded += StartLevel;
                    }
                    break;
            }

            /* Create timer */
            timer = new Timer(level.TimeLimit);
            /* Create dispatcher */
            Dispatcher = new Dispatcher(currLevel: level, timer);

            /* Set up level scene */
            SetupUtilities.SetupGameScene(level, GameObjectsManagement.Button,
                TablePrefab, GatePrefab, BeltPrefab);

            PuzzleCorrectionChecker.UpdateOnNewLevel();

            /* Update game state */
            gameManager.game.gameStat.SetLevelJustAttempted(level.LevelInd);

            if (hasTutorial)
                return;

            /* Subscribe to events that trigger level to end */
            timer.TimerEnded += OnLevelEnded;
            Dispatcher.AllBatchDonePublisher += OnLevelEnded;
            Dispatcher.NewBatchDispatchedPublisher += OnNewBatchDispatched;

            /* Set up UI */
            timer.TimerClicked += UIClockController.Instance.OnTimerClicked;
            UIProgressBar.Instance.SetGoal(level.Goal);

            /* Dispatch first batch */
            Dispatcher.DispatchNewBatch();

        }

        /* Only called after the dialogue-based tutorial ends. */
        private void StartLevel()
        {
            /* Subscribe to events that trigger level to end */
            timer.TimerEnded += OnLevelEnded;
            Dispatcher.AllBatchDonePublisher += OnLevelEnded;
            Dispatcher.NewBatchDispatchedPublisher += OnNewBatchDispatched;

            /* Set up UI */
            timer.TimerClicked += UIClockController.Instance.OnTimerClicked;
            UIProgressBar.Instance.SetGoal(level.Goal);

            /* Dispatch first batch */
            Dispatcher.DispatchNewBatch();

            Wrapper.Events.DialogueSequenceEnded -= StartLevel;

            hasTutorial = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (hasTutorial)
                return;

            if (levelEnded)
                return;

            if (GameManagement.Instance.gameMode == GameManagement.GameMode.Regular)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        public void OnLevelEnded()
        {
            levelEnded = true;

            /* Deactivate and reset static game objects */
            GameObjectsManagement.DeactiveAllGameObjects();
            GameObjectsManagement.ResetAllGameObjects();

            int starCnt = GameUtilities.CalculateLevelResult(UIProgressBar.Instance.GetCompletionRate());

            if (starCnt > 0)
            {
                /* If level passed, save progress */
                GameUtilities.SaveLevelResult(level.LevelInd, starCnt);
                GameUtilities.UpdateTotalEarning(UIProgressBar.Instance.GetCurrentEarning());

                GameManagement.Instance.game.gameStat.SetLevelResultAndSave(GameStat.LevelResult.WIN);
            }
            else
            {
                GameManagement.Instance.game.gameStat.SetLevelResultAndSave(GameStat.LevelResult.LOSS);
            }

            if (LevelEnded != null) LevelEnded();

            /* Change scene */
            GameUtilities.LoadMenuBasedOnEndResult(starCnt);
        }

        void OnNewBatchDispatched()
        {
            currentBatchNum += 1;
        }

    }
}

