using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Qupcakery
{
    public class HintButton : MonoBehaviour
    {
        Level level;

        private void Start()
        {
            level = FindObjectOfType<LevelManager>().level;
        }

        public void GiveHint()
        {
            // Get gates
            int[] gates = level.AvailableGates;
            var available = from i in Enumerable.Range(0, 4) where gates[i] > 0 select i;

            GameObject[] gateObjects = GameObject.FindGameObjectsWithTag("Gate");
            gateObjects[0].GetComponent<Animation>().Play("GateMotion");

            // Find first incorrect conveyor
            // Determine correct solution
            // Wiggle first block in solution
            // print(level.LevelInd);

        }

    }
}

