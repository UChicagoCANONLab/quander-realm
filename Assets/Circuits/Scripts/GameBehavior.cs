using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Circuits 
{
    public class GameBehavior : MonoBehaviour
    {
        public Text textbox;

        //private LevelGenerator levelGen;

        public Color selected;

        public float timeForHint = 5.0f;

        public GameObject gatesObject;
        public GameObject canvasObject;

        //public GameObject[] stars;

        public Camera camera;

        public GameObject sparkPrefab;

        public GameObject linePrefab;

        public Transform linesTransform;

        public Animator earthquakeAnimator;

        public Animator flamesAnimator;

        public ReminderPopup reminder;

        public Animator flareAnimator;
        //private HashSet<LinkedListNode<GateData>> selection = new HashSet<LinkedListNode<GateData>>();

        private float timeToNextLevel = float.MaxValue;
        private float hintTimer = 0;

        private const float TIME_BUFFER = 4f;

        private bool hintProvided = false;

        private int[] levelScores = new int[CTConstants.N_LEVELS];

        private float sceneScale = 1f;

        //public GameObject mainGUI;

        //private bool started = false;

        private bool hintUsed = false;

        private int sortingIndexOffset = 0;
        private List<List<String>> circuit;
        protected HashSet<BaseGateBehavior> selection;

        private BaseGateBehavior[,] gateObjects;

        bool simplified = false;

        // public bool titleScene = false;
        public Sprite[] numberSprites;

        // public GameObject book;
        public InfoPopup info;
        public GameObject gates;
        public GameObject grid;

        private int completedSubstitutions = 0;
        private bool fullyOptimized = false;
        private int expectedSubstitutions = 0;

        [Range(0f, 1f)] public float sparkDensity = 0.14f;
        public float sparkTickInterval = 0.08f;
        [Range(0f, 0.5f)] public float sparkTravelVariation = 0.25f;
        [Range(0f, 1f)] public float sparkEarlyFizzleChance = 0.12f;
        [Range(0f, 0.2f)] public float sparkMinTravelRatio = 0.02f;
        public float sparkSpawnJitter = 0.15f;

        private float sparkTickTimer = 0f;
        private System.Random sparkRng = new System.Random();

        private const float NEXT_LEVEL_DELAY = 5f;
        private const float DISASTER_MIN_DELAY = 10f;
        private const float DISASTER_MAX_DELAY = 30f;
        private const float DISASTER_WARNING_LEAD = 5f;

        private const string EARTHQUAKE_TRIGGER = "Earthquake";

        private const string FLAMES_TRIGGER = "Flames";

        private const string FLARE_TRIGGER = "Flare";

        private float secondsUntilDisaster = float.MaxValue;
        private bool disasterWarningIssued = false;

        private bool reminderIsListeningToSub = false;

        public HashSet<int> tutorialLevels;

        private bool HasRemainingReductions()
        {
            if (circuit == null)
            {
                return false;
            }

            for (int y = 0; y < circuit.Count; y++)
            {
                for (int x = 0; x < circuit[y].Count; x++)
                {
                    string currGate = circuit[y][x];
                    if (currGate != null && currGate[currGate.Length - 1] == '0')
                    {
                        var reductions = LevelGenerator.checkGateReduction(x, y, circuit);
                        if (reductions.Count > 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool EvaluateOptimizationState()
        {
            fullyOptimized = !HasRemainingReductions();
            simplified = fullyOptimized;
            return fullyOptimized;
        }

        private float GetEstimatedOptimizationRatio()
        {
            if (expectedSubstitutions <= 0)
            {
                return fullyOptimized ? 1f : 0f;
            }

            return Mathf.Clamp01((float)completedSubstitutions / expectedSubstitutions);
        }

        private float GetAmbientSparkDistance(float circuitLen)
        {
            if (fullyOptimized)
            {
                return circuitLen * 1.5f;
            }

            float visualRatio = Mathf.Clamp(GetEstimatedOptimizationRatio(), 0f, 0.8f);

            float variation = ((float)sparkRng.NextDouble() * 2f - 1f) * sparkTravelVariation;
            float sampledRatio = Mathf.Clamp01(visualRatio + variation);

            float normalizedVisualRatio = visualRatio / 0.8f;
            float currentEarlyFizzleChance = sparkEarlyFizzleChance * (1f - normalizedVisualRatio);

            if (sparkRng.NextDouble() < currentEarlyFizzleChance)
            {
                sampledRatio *= (float)sparkRng.NextDouble() * 0.35f;
            }

            sampledRatio = Mathf.Max(sparkMinTravelRatio, sampledRatio) * .7f;
            return sampledRatio * circuitLen;
        }

        private void SpawnAmbientSparkOnWire(int wireIndex)
        {
            if (circuit == null || sparkPrefab == null || camera == null)
            {
                return;
            }

            int nLines = circuit.Count;
            float yCord = (nLines - wireIndex) * CTConstants.gridResolution_h * sceneScale;
            Vector3 offset = new Vector3(0f, ((-nLines / 2) - .5f) * CTConstants.gridResolution_h) * sceneScale;

            float xJitter = (((float)sparkRng.NextDouble() * 2f) - 1f) * sparkSpawnJitter * sceneScale;

            GameObject spark = Instantiate(sparkPrefab);
            spark.transform.localScale = Vector3.one * sceneScale;

            float circuitLen = 4f * camera.orthographicSize;
            spark.transform.position = new Vector3(-20f + xJitter, yCord) + offset;

            float distance = GetAmbientSparkDistance(circuitLen);
            spark.GetComponent<SparkBehavior>().runSpark(circuitLen / 5f, distance, GetComponent<TimerManager>());
        }

        private void ProcessAmbientSparkTick()
        {
            if (circuit == null)
            {
                return;
            }

            for (int wireIndex = 0; wireIndex < circuit.Count; wireIndex++)
            {
                if (sparkRng.NextDouble() < sparkDensity)
                {
                    SpawnAmbientSparkOnWire(wireIndex);
                }
            }
        }

        private void ScheduleNextDisaster()
        {
            secondsUntilDisaster = Mathf.Lerp(
                DISASTER_MIN_DELAY,
                DISASTER_MAX_DELAY,
                (float)sparkRng.NextDouble()
            );
            disasterWarningIssued = false;
        }

        private void warnUser()
        {
            Debug.Log("Warning!");
        }

        private void triggerDisaster(string mode)
        {
            switch (mode)
            {
                case "HEAT":
                    flamesAnimator.SetTrigger(FLAMES_TRIGGER);
                    break;
                case "EARTHQUAKE":
                    earthquakeAnimator.SetTrigger(EARTHQUAKE_TRIGGER);
                    break;

                case "FLARE":
                    flareAnimator.SetTrigger(FLARE_TRIGGER);
                    break;
                
                default:
                    break;
            }


            SparkBehavior.RequestKillAllSparks();
            ScheduleNextDisaster();


        }

        private void triggerDisaster()
        {
            if(tutorialLevels.Contains(GameData.getCurrLevel()))
            {
                return;
            }

            Debug.Log("Disaster!");

            if (earthquakeAnimator != null)
            {
                earthquakeAnimator.SetTrigger(EARTHQUAKE_TRIGGER);
            }

            SparkBehavior.RequestKillAllSparks();
            ScheduleNextDisaster();
        }

        protected void renderCircuit(List<List<String>> newCircuit)
        {
            foreach (Transform child in gatesObject.transform)
            {
                Destroy(child.gameObject);
            }

            gatesObject.transform.localScale = Vector3.one;
            selection = new HashSet<BaseGateBehavior>();
            circuit = newCircuit;

            int circuitSize = circuit[0].Count;
            int nLines = circuit.Count;
            int nCols = circuit[0].Count;

            string prefix = "Circuits/Prefabs/";
            var loadedGate = Resources.Load($"{prefix}H_Gate");
            gateObjects = new BaseGateBehavior[circuit.Count, circuit[0].Count];
            Vector3 offset = new Vector3((-nCols / 2) * CTConstants.gridResolution_w, ((-nLines / 2) - .5f) * CTConstants.gridResolution_h);

            for (int i = 0; i < circuit.Count; i++)
            {
                string row = "";
                for (int j = 0; j < circuit[i].Count; j++)
                {
                    row += circuit[i][j];
                    String gateSelected = circuit[i][j];
                    if (circuit[i][j] == null) { gateSelected = ""; }
                    gateSelected = gateSelected.ToUpper();

                    switch (gateSelected)
                    {
                        case "H-0":
                            instantiateGate($"{prefix}H_Gate", j, i, nLines, circuit[i].Count);
                            break;
                        case "Z-0":
                            instantiateGate($"{prefix}Z_Gate", j, i, nLines, circuit[i].Count);
                            break;
                        case "X-0":
                            instantiateGate($"{prefix}NOT_Gate", j, i, nLines, circuit[i].Count);
                            break;
                        case "CX-0":
                            instantiateGate($"{prefix}CNOT_Gate", j, i, nLines, circuit[i].Count);
                            break;
                        case "CZ-0":
                            instantiateGate($"{prefix}CZ_Gate", j, i, nLines, circuit[i].Count);
                            break;
                        case "SWAP-0":
                            instantiateGate($"{prefix}SWAP_Gate", j, i, nLines, circuit[i].Count);
                            break;
                        default:
                            break;
                    }
                }
            }

            gatesObject.transform.localScale = Vector3.one * sceneScale;
        }

        protected void Start()
        {
            GameData.levelStart();
            completedSubstitutions = 0;
            expectedSubstitutions = 0;
            reminderIsListeningToSub = false;

            timeToNextLevel = float.MaxValue;
            fullyOptimized = false;
            simplified = false;
            selection = new HashSet<BaseGateBehavior>();

            String[] gatesToSample;
            String[] allowedSubstitutions;
            int nLines = 1;
            int nGates;
            int nExpansions;

            tutorialLevels = new HashSet<int>();
            tutorialLevels.Add(0);
            tutorialLevels.Add(1);
            tutorialLevels.Add(2);
            tutorialLevels.Add(3);
            tutorialLevels.Add(5);
            tutorialLevels.Add(7);
            tutorialLevels.Add(14);
            tutorialLevels.Add(15);
            tutorialLevels.Add(22);

            List<List<String>> tempCircuit = null;
            LevelGenerator.LevelBuildResult buildResult = null;

            try
            {
                Wrapper.Events.CollectAndDisplayReward?.Invoke(Wrapper.Game.Circuits, GameData.getCurrLevel());
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

            TextMeshProUGUI levelNumber = GameObject.Find("Canvas/LevelNumber/Text").GetComponent<TextMeshProUGUI>();
            if (GameData.getCurrLevel() < CTConstants.N_LEVELS)
            {
                levelNumber.text = $"{GameData.getCurrLevel()}";
            }

            if (GameData.getCurrLevel() >= CTConstants.N_LEVELS)
            {
                SceneManager.LoadScene("Circuits_Menu");
                return;
            }
            else if (GameData.getCurrLevel() <= 9 || tutorialLevels.Contains(GameData.getCurrLevel()))
            {
                String[] empty = new String[0];
                int startingSize = 9;
                tempCircuit = new List<List<string>>(3);
                tempCircuit.Add(new List<String>(startingSize));
                List<String> row = tempCircuit[0];

                const string H = "H-0";
                const string X = "X-0";
                const string Z = "Z-0";
                const string CX1 = "CX-0";
                const string CX2 = "CX-1";
                const string CZ1 = "CZ-0";
                const string CZ2 = "CZ-1";

                switch (GameData.getCurrLevel())
                {
                    case 0:
                        row.Add(H);
                        row.Add(H);
                        break;
                    case 1:
                        row.Add(H);
                        row.Add(H);
                        row.Add(X);
                        break;
                    case 2:
                        row.Add(Z);
                        row.Add(X);
                        row.Add(H);
                        row.Add(H);
                        break;
                    case 3:
                        row.Add(H);
                        row.Add(X);
                        row.Add(H);
                        break;
                    case 4:
                        row.Add(Z);
                        row.Add(H);
                        row.Add(H);
                        row.Add(X);
                        row = new List<string>();
                        tempCircuit.Add(row);
                        row.Add(H);
                        row.Add(X);
                        row.Add(H);
                        break;
                    case 5:
                        row.Add(H);
                        row.Add(Z);
                        row.Add(H);
                        break;
                    case 6:
                        row.Add(H);
                        row.Add(X);
                        row.Add(Z);
                        row.Add(H);
                        row.Add(X);
                        row.Add(H);
                        row.Add(Z);
                        row.Add(H);
                        row.Add(Z);
                        row.Add(H);
                        row.Add(Z);
                        row.Add(H);
                        break;
                    case 7:
                        row.Add(X);
                        row.Add(X);
                        break;
                    case 8:
                        row.Add(H);
                        row.Add(H);
                        row.Add(X);
                        row.Add(Z);
                        row = new List<string>();
                        tempCircuit.Add(row);
                        row.Add(H);
                        row.Add(Z);
                        row.Add(H);
                        row = new List<string>();
                        tempCircuit.Add(row);
                        row.Add(X);
                        row.Add(Z);
                        row.Add(H);
                        row.Add(X);
                        row.Add(H);
                        row.Add(Z);
                        row.Add(X);
                        break;
                    case 9:
                        row.Add(H);
                        row.Add(X);
                        row.Add(X);
                        row.Add(H);
                        row.Add(X);
                        row.Add(H);
                        row.Add(Z);
                        row.Add(H);
                        row.Add(X);
                        row.Add(H);
                        row.Add(Z);
                        row.Add(H);
                        row.Add(X);
                        break;
                    case 14:
                        row.Add(null);
                        row.Add(CX1);
                        row = new List<string>();
                        row.Add(H);
                        row.Add(CX2);
                        row.Add(H);
                        tempCircuit.Add(row);
                        row = new List<string>();
                        tempCircuit.Add(row);
                        break;
                    case 15:
                        row.Add(null);
                        row.Add(CZ1);
                        row = new List<string>();
                        row.Add(H);
                        row.Add(CZ2);
                        row.Add(H);
                        tempCircuit.Add(row);
                        row = new List<string>();
                        tempCircuit.Add(row);
                        break;
                    case 22:
                        row.Add(H);
                        row.Add(CX1);
                        row.Add(H);
                        row = new List<string>();
                        row.Add(H);
                        row.Add(CX2);
                        row.Add(H);
                        tempCircuit.Add(row);
                        row = new List<string>();
                        tempCircuit.Add(row);
                        break;
                    default:
                        break;
                }

                int circuitLen = 0;
                foreach (var currRow in tempCircuit)
                {
                    circuitLen = Math.Max(circuitLen, currRow.Count);
                }

                foreach (var currRow in tempCircuit)
                {
                    for (int i = currRow.Count; i < circuitLen; i++)
                    {
                        currRow.Add(null);
                    }
                }
            }
            else if (GameData.getCurrLevel() < 14)
            {
                System.Random rng = new System.Random();
                nLines = rng.Next(2, 5);
                nGates = GameData.getCurrLevel() + rng.Next(4) + (int)(nLines / 2);
                gatesToSample = new string[] { "X", "Z" };
                allowedSubstitutions = new string[] { "X", "Z" };
                nExpansions = 3 + rng.Next(4);
                buildResult = LevelGenerator.GenerateLevel(nLines, nGates, gatesToSample, nExpansions, allowedSubstitutions);
                tempCircuit = buildResult.circuit;
                expectedSubstitutions = buildResult.expectedSubstitutions;
            }
            else if (GameData.getCurrLevel() < 19)
            {
                System.Random rng = new System.Random();
                nLines = rng.Next(4, 5);
                nGates = GameData.getCurrLevel() + rng.Next(4) + (int)(nLines / 2) - 5;
                gatesToSample = new string[] { "X", "Z", "CZ" };
                allowedSubstitutions = new string[] { "X", "Z", "CZ" };
                nExpansions = 4 + rng.Next(4);
                buildResult = LevelGenerator.GenerateLevel(nLines, nGates, gatesToSample, nExpansions, allowedSubstitutions);
                tempCircuit = buildResult.circuit;
                expectedSubstitutions = buildResult.expectedSubstitutions;
            }
            else if (GameData.getCurrLevel() < 23)
            {
                System.Random rng = new System.Random();
                nLines = rng.Next(5, 7);
                nGates = GameData.getCurrLevel() + rng.Next(4) + (int)(nLines / 2) - 8;
                gatesToSample = new string[] { "X", "Z", "CZ", "CX" };
                allowedSubstitutions = new string[] { "X", "Z", "CZ", "CX" };
                nExpansions = 5 + rng.Next(4);
                buildResult = LevelGenerator.GenerateLevel(nLines, nGates, gatesToSample, nExpansions, allowedSubstitutions);
                tempCircuit = buildResult.circuit;
                expectedSubstitutions = buildResult.expectedSubstitutions;
            }
            else
            {
                System.Random rng = new System.Random();
                nLines = rng.Next(6, 7);
                nGates = GameData.getCurrLevel() + rng.Next(4) + (int)(nLines / 2) - 10;
                gatesToSample = new string[] { "X", "Z", "CZ", "CX" };
                allowedSubstitutions = new string[] { "X", "Z", "CZ", "CX", "CX2" };
                nExpansions = 5 + rng.Next(4);
                buildResult = LevelGenerator.GenerateLevel(nLines, nGates, gatesToSample, nExpansions, allowedSubstitutions);
                tempCircuit = buildResult.circuit;
                expectedSubstitutions = buildResult.expectedSubstitutions;
            }

            sceneScale = Math.Min(Math.Min(1f, 6.5f / tempCircuit[0].Count), 3.5f / nLines);
            renderCircuit(tempCircuit);
            sparkTickTimer = 0f;
            EvaluateOptimizationState();
            ScheduleNextDisaster();

            int nCols = circuit[0].Count;
            nLines = circuit.Count;
            Vector3 offset = new Vector3((-nCols / 2) * CTConstants.gridResolution_w, ((-nLines / 2) - .5f) * CTConstants.gridResolution_h * sceneScale);

            for (int i = 0; i < circuit.Count; i++)
            {
                var currLine = Instantiate(linePrefab);
                LineRenderer lr = currLine.GetComponent<LineRenderer>();
                lr.SetWidth(sceneScale, sceneScale);
                float yCord = (nLines - i) * CTConstants.gridResolution_h * sceneScale;

                Vector3[] positions = { new Vector3(-200, yCord) + offset, new Vector3(200, yCord) + offset };
                lr.SetPositions(positions);
                currLine.transform.parent = linesTransform;
            }
        }

        private void instantiateGate(String resPath, int x, int y, int nLines, int nCols)
        {
            var loadedGate = Resources.Load(resPath);
            GameObject currGate = (GameObject)Instantiate(loadedGate);
            BaseGateBehavior gb = currGate.GetComponent<BaseGateBehavior>();
            gb.x = x;
            gb.y = y;
            gb.gameBehavior = this;
            currGate.transform.parent = gatesObject.transform;
            Vector3 offset = new Vector3((-nCols / 2) * CTConstants.gridResolution_w, (-nLines / 2) * CTConstants.gridResolution_h);
            currGate.transform.position = new Vector3(CTConstants.gridResolution_w * x, CTConstants.gridResolution_h * (nLines - y - 1)) + offset;
            gateObjects[y, x] = gb;
        }

        public void goToLevel(int l)
        {
        }

        public void getMessageFromReminder(string m)
        {
            switch (m)
            {
                case "SUBSTITUTE_MODE":
                    reminderIsListeningToSub = true;
                    break;
                
                case "HEAT":
                    triggerDisaster("HEAT");
                    break;
                
                case "EARTHQUAKE":
                    triggerDisaster("EARTHQUAKE");
                    break;
                
                case "FLARE":
                    triggerDisaster("FLARE");
                    break;
                
                default:
                    return;
            }
            
        }

        public int getScore(int i)
        {
            return levelScores[i];
        }

        public void onMenuClicked()
        {
        }

        public void toggleGate(BaseGateBehavior gate)
        {
            if (gate.selected)
            {
                selection.Add(gate);
            }
            else
            {
                selection.Remove(gate);
            }
        }

        public void checkSubstitution()
        {
            HashSet<Tuple<int, int>> selectedCords = new HashSet<Tuple<int, int>>();
            if (selection.Count == 0)
            {
                return;
            }

            List<string> subString = new List<string>();
            foreach (var gate in selection)
            {
                Tuple<int, int> cords = new Tuple<int, int>(gate.x, gate.y);
                selectedCords.Add(cords);
                subString.Add(string.Format("({0}:{1},{2})", circuit[gate.y][gate.x], gate.x, gate.y));
            }

            GameData.checkingSub(String.Join("_", subString));
            var simplifiedCircuit = LevelGenerator.checkSubstitution(selectedCords, circuit);

            if (simplifiedCircuit != null)
            {
                if (reminderIsListeningToSub)
                {
                    reminder.SubstituteNext();
                    reminderIsListeningToSub = false;
                }
                GameData.correctSub();
                completedSubstitutions++;
                renderCircuit(simplifiedCircuit);
                EvaluateOptimizationState();

                if (fullyOptimized && timeToNextLevel == float.MaxValue)
                {
                    timeToNextLevel = NEXT_LEVEL_DELAY;
                }
            }
            else
            {
                StarDisplay.SD.AddPenalty();
                GameData.incorrectSub();
                EvaluateOptimizationState();
            }
        }

        private bool RefreshSimplifiedState()
        {
            simplified = true;

            for (int y = 0; y < circuit.Count; y++)
            {
                for (int x = 0; x < circuit[y].Count; x++)
                {
                    string currGate = circuit[y][x];
                    if (currGate != null && currGate[currGate.Length - 1] == '0')
                    {
                        var reductions = LevelGenerator.checkGateReduction(x, y, circuit);
                        if (reductions.Count > 0)
                        {
                            simplified = false;
                            return simplified;
                        }
                    }
                }
            }

            return simplified;
        }

        private float GetCurrentRunSparkDistance(float circuitLen, System.Random rng)
        {
            return simplified
                ? circuitLen * 1.5f
                : ((float)rng.NextDouble() * .5f + .15f) * (circuitLen / 2);
        }

        private void EmitRunSparks()
        {
            System.Random rng = new System.Random();

            int nLines = circuit.Count;
            int sparksToSend = 4;

            for (int nSpark = 0; nSpark < sparksToSend; nSpark++)
            {
                Vector3 offset = new Vector3(
                    -nSpark * CTConstants.gridResolution_w,
                    ((-nLines / 2) - .5f) * CTConstants.gridResolution_h
                ) * sceneScale;

                for (int i = 0; i < circuit.Count; i++)
                {
                    float sparkOffset = 0;
                    float yCord = (nLines - i) * CTConstants.gridResolution_h * sceneScale;
                    GameObject spark = Instantiate(sparkPrefab);
                    spark.transform.localScale = Vector3.one * sceneScale;

                    float circuitLen = 4f * camera.orthographicSize;
                    spark.transform.position = new Vector3(-20f, yCord) + offset + new Vector3(sparkOffset, 0);

                    float distance = GetCurrentRunSparkDistance(circuitLen, rng);
                    spark.GetComponent<SparkBehavior>().runSpark(circuitLen / 3f, distance, GetComponent<TimerManager>());
                }
            }
        }

        private void HandleSolvedRun()
        {
            if (simplified)
            {
                GTimer nextLevel = GetComponent<GTimer>();
                nextLevel.startTimer();
            }
        }

        public void tryRun()
        {
            // SparkBehavior.RequestKillAllSparks();
            // flareAnimator.SetTrigger(FLARE_TRIGGER);
        }

        private void updateSelection(BaseGateBehavior gate)
        {
            gate.toggle();
        }

        public void flashHint()
        {
            StarDisplay.SD.AddPenalty();

            for (int y = 0; y < circuit.Count; y++)
            {
                for (int x = 0; x < circuit[y].Count; x++)
                {
                    string currGate = circuit[y][x];
                    if (currGate != null && currGate[currGate.Length - 1] == '0')
                    {
                        var reductions = LevelGenerator.checkGateReduction(x, y, circuit);
                        if (reductions.Count > 0)
                        {
                            GameData.hintRequested();
                            var reduction = reductions[0];
                            foreach (var keyvalue in reduction)
                            {
                                Tuple<int, int> cords = keyvalue.Key;
                                gateObjects[cords.Item2, cords.Item1].highlight();
                            }
                            return;
                        }
                    }
                }
            }
        }

        public void toMenu()
        {
            SceneManager.LoadScene("Circuits_Menu");
        }

        public void showInfo()
        {
            info.SetInfo(GameData.getCurrLevel());
            info.gameObject.SetActive(true);
            gates.SetActive(false);
            grid.SetActive(false);
        }

        public void hideInfo()
        {
            Debug.Log("Hide");
            info.gameObject.SetActive(false);
            gates.SetActive(true);
            grid.SetActive(true);
        }

        public void toTitle()
        {
            SceneManager.LoadScene("Circuits_Title");
        }

        public void loadNextLevel()
        {
            GameData.levelPassed();
            SceneManager.LoadScene(GameData.getNextScene());

            StarDisplay.SD.ResetStars();
        }

        public void restartLevel()
        {
            SceneManager.LoadScene(GameData.getNextScene());

            StarDisplay.SD.ResetStars();
        }

        private void Update()
        {
            if (timeToNextLevel != float.MaxValue)
            {
                timeToNextLevel -= Time.deltaTime;
                if (timeToNextLevel <= 0f)
                {
                    timeToNextLevel = float.MaxValue;
                    loadNextLevel();
                    return;
                }
            }

            if (secondsUntilDisaster != float.MaxValue)
            {
                secondsUntilDisaster -= Time.deltaTime;

                if (!disasterWarningIssued && secondsUntilDisaster <= DISASTER_WARNING_LEAD)
                {
                    disasterWarningIssued = true;
                    warnUser();
                }

                if (secondsUntilDisaster <= 0f)
                {
                    triggerDisaster();
                }
            }

            if (sparkTickInterval > 0f)
            {
                sparkTickTimer += Time.deltaTime;

                while (sparkTickTimer >= sparkTickInterval)
                {
                    sparkTickTimer -= sparkTickInterval;
                    ProcessAmbientSparkTick();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
                if (hit.collider)
                {
                    updateSelection(hit.collider.transform.parent.parent.GetComponent<BaseGateBehavior>());
                }
            }
        }
    }
}