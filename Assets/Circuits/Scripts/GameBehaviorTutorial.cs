using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Circuits 
{
    public class GameBehaviorTutorial : GameBehavior
    {
        
        //private HashSet<LinkedListNode<GateData>> selection = new HashSet<LinkedListNode<GateData>>();

        private float timeToNextLevel = float.MaxValue;
        private float hintTimer = 0;

        private const float TIME_BUFFER = 4f;

        private bool hintProvided = false;

        private int[] levelScores = new int[CTConstants.N_LEVELS];

        private float sceneScale = 1f;

        public Animation hand;

        //public GameObject mainGUI;

        //private bool started = false;

        private bool hintUsed = false;

        private int sortingIndexOffset = 0;
        private List<List<String>> circuit;
        // private HashSet<BaseGateBehavior> selection;

        private BaseGateBehavior[,] gateObjects;

        bool simplified = false;

        // public bool titleScene = false;


        // private void renderCircuit(List<List<String>> newCircuit)
        // {
        //     foreach (Transform child in gatesObject.transform)
        //     {
        //         Destroy(child.gameObject);
        //     }
        //     gatesObject.transform.localScale = Vector3.one;
        //     selection = new HashSet<BaseGateBehavior>();
        //     circuit = newCircuit;
        //     int circuitSize = circuit[0].Count;
        //     int nLines = circuit.Count;
        //     int nCols = circuit[0].Count;
        //     //camera.orthographicSize = Math.Max(circuitSize * 2.5f, 25);


        //     string prefix = "Circuits/Prefabs/";
        //     var loadedGate = Resources.Load($"{prefix}H_Gate");
        //     gateObjects = new BaseGateBehavior[circuit.Count, circuit[0].Count];
        //     Vector3 offset = new Vector3((-nCols / 2) * CTConstants.gridResolution_w, ((-nLines / 2) - .5f) * CTConstants.gridResolution_h);
        //     for (int i = 0; i < circuit.Count; i++)
        //     {
        //         string row = "";
        //         for (int j = 0; j < circuit[i].Count; j++)
        //         {
        //             row += circuit[i][j];
        //             String gateSelected = circuit[i][j];
        //             if (circuit[i][j] == null) { gateSelected = ""; }
        //             gateSelected = gateSelected.ToUpper();
        //             switch (gateSelected)
        //             {
        //                 case "H-0":
        //                     instantiateGate($"{prefix}H_Gate", j, i, nLines, circuit[i].Count);
        //                     break;
        //                 case "Z-0":
        //                     instantiateGate($"{prefix}Z_Gate", j, i, nLines, circuit[i].Count);
        //                     break;
        //                 case "X-0":
        //                     instantiateGate($"{prefix}NOT_Gate", j, i, nLines, circuit[i].Count);
        //                     break;
        //                 case "CX-0":
        //                     instantiateGate($"{prefix}CNOT_Gate", j, i, nLines, circuit[i].Count);
        //                     break;
        //                 case "CZ-0":
        //                     instantiateGate($"{prefix}CZ_Gate", j, i, nLines, circuit[i].Count);
        //                     break;
        //                 case "SWAP-0":
        //                     instantiateGate($"{prefix}SWAP_Gate", j, i, nLines, circuit[i].Count);
        //                     break;
        //                 default:
        //                     //Debug.Log("No Match for: " + gateSelected);
        //                     break;
        //             }
        //         }
        //     }
        //     gatesObject.transform.localScale = Vector3.one * sceneScale;
        // }

    

        public void onMenuClicked()
        {
            //selection = new HashSet<LinkedListNode<GateData>>();
            //mainGUI.SetActive(true);
        }

        public void toggleGate(BaseGateBehavior gate)
        {
            base.toggleGate(gate);
            Debug.Log("Toggle");
            if(GameData.getCurrLevel() == 0){
                Debug.Log("level 0");
                if(selection.Count == 2){
                    Debug.Log("yay");
                }
            }
            // if (gate.selected)
            // {
            //     selection.Add(gate);
            // }
            // else
            // {
            //     selection.Remove(gate);
            // }
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

                GameData.correctSub();
                renderCircuit(simplifiedCircuit);
                // Debug.Log("!Valid Substiution!");
            }
            else
            {
                StarDisplay.SD.AddPenalty();
                GameData.incorrectSub();
            }
        }


        public void tryRun()
        {
            GameData.levelRun();
            System.Random rng = new System.Random();
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
                            break;
                        }
                    }
                }
                if (!simplified)
                {
                    break;
                }
            }

            float sparkSeparation = CTConstants.gridResolution_w;

            int nLines = circuit.Count;
            int nCols = circuit[0].Count;
            int sparksToSend = 4;

            for (int nSpark = 0; nSpark < sparksToSend; nSpark++)
            {


                Vector3 offset = new Vector3(-nSpark * CTConstants.gridResolution_w, ((-nLines / 2) - .5f) * CTConstants.gridResolution_h) * sceneScale;
                for (int i = 0; i < circuit.Count; i++)
                {
                    float sparkOffset = 0;
                    float yCord = (nLines - i) * CTConstants.gridResolution_h * sceneScale;
                    GameObject spark = Instantiate(sparkPrefab);
                    spark.transform.localScale = Vector3.one * sceneScale;
                    float circuitLen = 4f * camera.orthographicSize;
                    spark.transform.position = new Vector3(-20f, yCord) + offset + new Vector3(sparkOffset, 0);

                    float speed = circuitLen / 4f;
                    float distance = simplified ? circuitLen * 1.5f : ((float)rng.NextDouble() * .5f + .15f) * (circuitLen / 2);
                    spark.GetComponent<SparkBehavior>().runSpark(circuitLen / 3f, distance, GetComponent<TimerManager>());


                }

            }
            if (simplified)
            {
                GTimer nextLevel = GetComponent<GTimer>();
                nextLevel.startTimer();
            }
        }




        private void updateSelection(BaseGateBehavior gate)
        {
            gate.toggle();
        }

        public void flashHint()
        {
            // StarDisplay.SD.LoseStar();
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
                            //if (stars[0].active)
                            //{
                            //    stars[0].active = false;
                            //}
                            //else
                            //{
                            //    stars[1].active = false;
                            //}/star


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
        private void Update()
        {
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