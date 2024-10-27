using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Qupcakery
{
    public class HintButton : MonoBehaviour
    {
        LevelManager manager;
        Level level;
        Solution solution;
        int[] solutionGates = new int[5];

        private void Start()
        {
            manager = FindObjectOfType<LevelManager>();
            level = manager.level;
            solution = manager.solution;
            // Determine gates in correct solution
            foreach(int[] row in solution.Gates)
            {
                foreach(int gate in row)
                {
                    if (gate >= 0)
                    {
                        solutionGates[gate]++;
                    }
                }
            }
        }

        public void GiveHint()
        {
            print(solution);

            // Get current set of gates that are on conveyors
            int[] gates = level.AvailableGates; // Original gates available
            Dictionary<GateType, int> tracker = GateBank.Instance.GateBankTracker; // Unplaced gates


            // GameObject[] gateObjects = GameObject.FindGameObjectsWithTag("Gate");

            // Find first gate that needs to be on conveyor but isn't
            // var available = from i in Enumerable.Range(0, 4) where gates[i] > 0 select i;


            // If it doesn't exist, find first gate that is on conveyor but shouldn't be.

            // Wiggle gate
            // gateObjects[0].GetComponent<Animation>().Play("GateMotion");




            // Find first incorrect conveyor
            // Determine correct solution
            // Wiggle first block in solution
            // print(level.LevelInd);

        }

    }
}

