using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;


namespace Qupcakery
{
    public class HintButton : MonoBehaviour
    {

        [Header("Sequenced Tutorial Objects")]
        [SerializeField] public GameObject chef;
        [SerializeField] public GameObject panel;
        [SerializeField] public GameObject textObject;
        [SerializeField] public Text tutorialText;

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

            if (GameManagement.Instance.InTutorial)
            {
                return; // Don't give a hint if you're still in tutorial!
            }

            if (level == null)
            {
                level = manager.level;
                solution = manager.solution;
            }

            int[] gatesInUse = new int[5];
            
            GateType hintGateType = GateType.None;
            bool tooManyGates = false;

            int[] gates = level.AvailableGates; // Original gates available
            Dictionary<GateType, int> tracker = GateBank.Instance.GateBankTracker; // Unplaced gates

            // Get current set of gates that are on conveyors
            for (int i = 0; i < 5; i++)
            {
                if (gates[i] > 0)
                {
                    gatesInUse[i] = gates[i] - tracker[(GateType)i];
                }
            }

            // Find first gate where solution and current are different
            for (int i = 0; i < gates.Length; i++)
            {
                // Prioritize gates that aren't on conveyor but should be
                if (gatesInUse[i] < solution.GateCount[i])
                {
                    hintGateType = (GateType)i;
                    tooManyGates = false;
                    break;
                }

                // Give a hint for a gate that is on the conveyor ONLY if too
                // many gates
                if (gatesInUse[i] > solution.GateCount[i])
                {
                    hintGateType = (GateType)i;
                    tooManyGates = true;
                }
            }

            // Find a gate to hint
            GameObject[] gateObjects = GameObject.FindGameObjectsWithTag("Gate");
            bool gaveHint = false;
            foreach(GameObject gate in gateObjects)
            {
                SpriteResolver resolver = gate.GetComponent<SpriteResolver>();
                
                if (resolver.GetLabel() == hintGateType.ToString())
                {
                    // If the gate is used too often, find a gate on the conveyor
                    if (tooManyGates)
                    {
                        if (resolver.GetCategory() == "OnBelt")
                        {
                            gate.GetComponent<Animation>().Play("GateMotion");
                            gaveHint = true;
                            break;
                        }
                    } else { // Otherwise, find a gate on the bench
                        if (resolver.GetCategory() == "Basic")
                        {
                            gate.GetComponent<Animation>().Play("GateMotion");
                            gaveHint = true;
                            break;
                        }
                    }
                    
                }
            }

            if (!gaveHint)
            {
                tutorialText.text = "You're using the right gates! That's all" +
                    " the help I can give you right now - I have to get back to " +
                    "baking!";
                chef.GetComponent<SpriteRenderer>().enabled = true;
                panel.GetComponent<Image>().enabled = true;
                textObject.GetComponent<Text>().enabled = true;
                Invoke("EndHintMessage", 5);
            }

        }

        private void EndHintMessage()
        {
            chef.GetComponent<SpriteRenderer>().enabled = false;
            panel.GetComponent<Image>().enabled = false;
            textObject.GetComponent<Text>().enabled = false;
        }

    }
}

