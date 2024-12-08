using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;


namespace Qupcakery
{
    public class QCTutorialLevel : MonoBehaviour
    {
        [Header("Sequenced Tutorial Objects")]
        [SerializeField] public GameObject chef;
        [SerializeField] public GameObject panel;
        [SerializeField] public GameObject textObject;
        [SerializeField] public GameObject pointer;
        [SerializeField] public Text tutorialText;
        [SerializeField] private Animator pointerAnimator;

        private int levelInd;
        private int tutorialSeq = 0;

        // Game/Object controllers
        GameManagement gm;
        ButtonController bc;
        CustomerManager cm;

        // Tutorial Gates
        GameObject tutorialGate1;
        GatePositionController gpc1;
        GameObject tutorialGate2;
        GatePositionController gpc2;
        GameObject tutorialGate3;
        GatePositionController gpc3;


        #region Initiation
        private void Start()
        {
            gm = GameManagement.Instance;
            levelInd = gm.GetCurrentLevelInd();
            

            if (levelInd == 1 ||
                levelInd == 3)
            {
                Invoke("InitiateQCTutorial", 0.2f);
            }
        }

        /* Tells the game that the tutorial is ongoing, and finds the relevant
         * parts of the level: the button, the gate, and the customer controller.
         * Assumes that the tutorial only has one gate. 
         */
        public void InitiateQCTutorial()
        {
            gm.InTutorial = true;

            tutorialSeq = 0;
            pointerAnimator.SetInteger("LevelInd", levelInd);

            bc = GameObject.Find("Button(Clone)").GetComponent<ButtonController>();

            FindCustomer();
            cm.ArrivedAtTable += TutorialNext;
            cm.CakeReceived += HideTutorial;
            
            if (cm.AtTable())
            {
                TutorialNext();
            }
        }

        public void TutorialNext()
        {
            switch (levelInd)
            {
                case 1:
                    Tutorial1Next();
                    break;
                case 3:
                    Tutorial3Next();
                    break;
                default:
                    Debug.Log("Tried to run tutorial on level without one.");
                    break;
            }
        }
        #endregion



        #region Level 1
        private string[] dialogueSeq1 = new string[]
        {
            "The cupcake on the conveyor is what the customer wants, so let's press Play to send it over!",
            "Oops, looks like I made the wrong cupcake! Let's use the Flavor Inverter gate to change it.",
            "That should work! Let's send our cupcake down the conveyor now.",
            "Nice work! I'm going to get back to baking now, but you can handle it from here!"
        };

        public void Tutorial1Next()
        {
            switch (tutorialSeq)
            {
                case 0:
                    // Setup
                    tutorialGate1 = GameObject.FindGameObjectWithTag("Gate");
                    gpc1 = tutorialGate1.GetComponent<GatePositionController>();

                    // Start Tutorial
                    ShowTutorial();
                    FindCustomer();
                    ActivateButton();
                    DeactivateGates();
                    tutorialText.text = dialogueSeq1[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 1:                   
                    ShowTutorial();
                    FindCustomer();
                    ActivateGates();
                    DeactivateButton();
                    tutorialText.text = dialogueSeq1[tutorialSeq];
                    gpc1.GateIsOnBelt += TutorialNext;

                    tutorialSeq++;
                    break;
                case 2:
                    pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
                    ActivateButton();
                    DeactivateGates();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    tutorialText.text = dialogueSeq1[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 3:
                    ShowTutorial();
                    ActivateButton();
                    ActivateGates();
                    gm.InTutorial = false;
                    tutorialText.text = dialogueSeq1[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 4:
                    EndTutorial();
                    break;
                default:
                    break;
            }
            
        }
        #endregion

        #region Level 3
        private string[] dialogueSeq3 = new string[]
        {
            "Here's a new gate! This SWAP gate will switch the positions " +
            "of cupcakes on adjacent belts."
        };

        public void Tutorial3Next()
        {
            switch (tutorialSeq)
            {
                case 0:
                    // Setup
                    foreach (GameObject gate in GameObject.FindGameObjectsWithTag("Gate")) {
                        SpriteResolver resolver = gate.GetComponent<SpriteResolver>();
                        if (resolver.GetLabel() == "NOT")
                        {
                            gpc1 = gate.GetComponent<GatePositionController>();
                        } else if (resolver.GetLabel() == "SWAP")
                        {
                            gpc2 = gate.GetComponent<GatePositionController>();
                        }
                    }
                    TwoGatePosition();

                    // Start Tutorial
                    ShowTutorial();
                    FindCustomer();
                    DeactivateButton();
                    DeactivateGate(gpc1);
                    tutorialText.text = dialogueSeq3[tutorialSeq];
                    gpc2.GateIsOnBelt += TutorialNext;

                    tutorialSeq++;
                    break;
                case 1:
                    ShowTutorial();
                    pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
                    FindCustomer();
                    ActivateGates();
                    DeactivateButton();
                    tutorialText.text = dialogueSeq3[tutorialSeq];
                    gpc1.GateIsOnBelt += TutorialNext;

                    tutorialSeq++;
                    break;
                case 2:
                    pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
                    ActivateButton();
                    DeactivateGates();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    tutorialText.text = dialogueSeq3[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 3:
                    ShowTutorial();
                    pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
                    ActivateButton();
                    ActivateGates();
                    gm.InTutorial = false;
                    tutorialText.text = dialogueSeq3[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 4:
                    EndTutorial();
                    break;
                default:
                    break;
            }

        }
        #endregion



        #region Utilities 
        private void ShowTutorial()
        {
            chef.GetComponent<SpriteRenderer>().enabled = true;
            panel.GetComponent<Image>().enabled = true;
            textObject.GetComponent<Text>().enabled = true;
            pointer.GetComponent<Canvas>().enabled = true;
        }

        private void HideTutorial()
        {
            chef.GetComponent<SpriteRenderer>().enabled = false;
            panel.GetComponent<Image>().enabled = false;
            textObject.GetComponent<Text>().enabled = false;
            pointer.GetComponent<Canvas>().enabled = false;
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
        }

        private void DeactivateGates()
        {
            gm.AllowGateMovement = false;
        }

        private void ActivateGates()
        {
            gm.AllowGateMovement = true;
        }

        private void ActivateGate(GatePositionController gpc)
        {
            gpc.gateActive = true; 
        }

        private void DeactivateGate(GatePositionController gpc)
        {
            gpc.gateActive = false;
        }

        private void DeactivateButton()
        {
            bc.UpdateButtonState(ButtonController.ButtonState.CanNotBePressed);
        }

        private void ActivateButton()
        {
            bc.UpdateButtonState(ButtonController.ButtonState.CanBePressed);
        }

        private void FindCustomer()
        {
            cm = GameObject.Find("Monster(Clone)").GetComponent<CustomerManager>();
        }

        // Move the chef and text to account for two gates.
        private void TwoGatePosition()
        {
            chef.GetComponent<Transform>().position += new Vector3(-30f, 30f, 0);
        }

        public void EndTutorial()
        {
            if (gm.InTutorial)
            {
                gm.InTutorial = false;

                // Clear out any listeners
                cm.ArrivedAtTable -= TutorialNext;
                cm.CakeReceived -= HideTutorial;

                if (levelInd == 1 && tutorialSeq == 2)
                {
                    gpc1.GateIsOnBelt -= TutorialNext;
                }

                HideTutorial();
            }
        }

        public static void ResetTutorial() {
            GameObject.Find("TutorialItems").GetComponent<QCTutorialLevel>().EndTutorial();
        }

        #endregion

    }
}

