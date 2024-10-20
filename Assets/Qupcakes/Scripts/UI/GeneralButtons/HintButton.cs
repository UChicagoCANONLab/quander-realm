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

        private void Start()
        {
            manager = FindObjectOfType<LevelManager>();
            level = manager.level;
            solution = manager.solution;
        }

        public void GiveHint()
        {
            // Get gates
            // int[] gates = level.AvailableGates;

            print(solution);

            // var available = from i in Enumerable.Range(0, 4) where gates[i] > 0 select i;

            // GameObject[] gateObjects = GameObject.FindGameObjectsWithTag("Gate");
            // gateObjects[0].GetComponent<Animation>().Play("GateMotion");

            // Find first incorrect conveyor
            // Determine correct solution
            // Wiggle first block in solution
            // print(level.LevelInd);

        }

    }
}

