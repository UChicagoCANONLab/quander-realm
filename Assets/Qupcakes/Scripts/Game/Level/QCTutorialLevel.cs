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
        GateOperationController goc1;
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
                levelInd == 3 ||
                levelInd == 8)
            {
                gm.AllowGateMovement = false;
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
            DeactivateGates();

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
                case 8:
                    Tutorial8Next();
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
            if (tutorialSeq >= dialogueSeq1.Length)
            {
                return;
            }

            tutorialText.text = dialogueSeq1[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0:
                    // Setup
                    tutorialGate1 = GameObject.FindGameObjectWithTag("Gate");
                    gpc1 = tutorialGate1.GetComponent<GatePositionController>();
                    gm.AllowGateMovement = true;

                    // Start Tutorial
                    ShowTutorial();
                    ClickPlayUtils();
                    break;

                case 1:
                    NewPuzzleUtils();
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 2:
                    ClickPlayUtils();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    break;

                case 3:
                    LastPuzzleUtils();
                    break;

                case 4:
                    EndTutorial();
                    break;

                default:
                    break;
            }

            tutorialSeq++;
        }
        #endregion

        #region Level 3
        private string[] dialogueSeq3 = new string[]
        {
            "Here's a new gate! This SWAP gate will switch the positions " +
            "of cupcakes on adjacent belts. Try it here!",
            "When you click play, you'll see the gate swap the positions of" +
            " these two cupcakes.",
            "This gate is only swapping their positions and not their flavors." +
            " Try using it on these cupcakes! ",
            "You'll see that this swap does nothing, because the cupcakes are the " +
            "same flavor!",
            "Have fun with this gate! I'm going back to baking."
        };

        public void Tutorial3Next()
        {
            if (tutorialSeq >= dialogueSeq3.Length)
            {
                return;
            }

            tutorialText.text = dialogueSeq3[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0: // Tell student to use SWAP Gate
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
                    gm.AllowGateMovement = true;

                    // Start Tutorial
                    NewPuzzleUtils();
                    DeactivateGate(gpc1);
                    gpc2.GateIsOnBelt += TutorialNext;
                    break;

                case 1: // Click play once swap gate has been placed
                    ClickPlayUtils();           
                    gpc2.GateIsOnBelt -= TutorialNext;
                    break;

                case 2: // Tell student to use SWAP on identical cupcakes
                    NewPuzzleUtils();
                    DeactivateGate(gpc1);
                    gpc2.GateIsOnBelt += TutorialNext;
                    break;

                case 3: // Send SWAP. 
                    ClickPlayUtils();
                    gpc2.GateIsOnBelt -= TutorialNext;
                    break;

                case 4:
                    LastPuzzleUtils();
                    break;

                case 5:
                    EndTutorial();
                    break;

                default:
                    break;
            }
            tutorialSeq++;

        }
        #endregion


        #region Level 8
        private string[] dialogueSeq8 = new string[]
        {
            "This is my Chocolate-Controlled Flavor Inverter. It changes " +
            "the bottom cupcake if the top one is chocolate!",
            "The top cupcake is chocolate, so the bottom cupcake will change.",
            "If the top cupcake is vanilla, the gate won't do anything. Try using" +
            " it on these cupcakes!",
            "The top cupcake is vanilla, so the bottom cupcake will not change!",
            "..."
        };

        public void Tutorial8Next()
        {
            if (tutorialSeq >= dialogueSeq8.Length)
            {
                return;
            }

            tutorialText.text = dialogueSeq8[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0: // Introduce CNOT gate
                    // Setup
                    tutorialGate1 = GameObject.FindGameObjectWithTag("Gate");
                    gpc1 = tutorialGate1.GetComponent<GatePositionController>();
                    goc1 = tutorialGate1.GetComponent<GateOperationController>();
                    gm.AllowGateMovement = true;

                    // Start Tutorial
                    NewPuzzleUtils();
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 1: // Click play once CNOT gate has been placed
                    ClickPlayUtils();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    break;

                case 2: // Show CNOT on 0 in control
                    NewPuzzleUtils();
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 3: // Click play 
                    ClickPlayUtils();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    break;

                case 4: // Show flipping
                    NewPuzzleUtils();

                    break;

                case 5:
                    EndTutorial();
                    break;
                default:
                    break;
            }

            tutorialSeq++;

        }
        #endregion


        #region Utilities
        private void NewPuzzleUtils()
        {
            ShowTutorial();
            FindCustomer();
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
            DeactivateButton();
            ActivateGates();
        }

        private void LastPuzzleUtils()
        {
            ShowTutorial();
            ActivateButton();
            ActivateGates();
        }

        private void ClickPlayUtils()
        {
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
            ActivateButton();
            DeactivateGates();
        }


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
            DeactivateGate(gpc1);
            DeactivateGate(gpc2);
            DeactivateGate(gpc3);
        }

        private void ActivateGates()
        {
            ActivateGate(gpc1);
            ActivateGate(gpc2);
            ActivateGate(gpc3);
        }

        private void ActivateGate(GatePositionController gpc)
        {
            if (gpc != null)
            {
                gpc.gateActive = true;
            }
            AllowFlip(goc1, true);
        }

        private void DeactivateGate(GatePositionController gpc)
        {
            if (gpc != null)
            {
                gpc.gateActive = false;
            }
            AllowFlip(goc1, false);
        }

        private void AllowFlip(GateOperationController goc, bool setting)
        {
            if (goc != null)
            {
                goc.canFlip = setting;
            }
        }

        private void DeactivateButton()
        {
            bc.UpdateButtonState(ButtonController.ButtonState.CanNotBePressed);
        }

        private void ActivateButton()
        {
            if (bc == null)
            {
                bc = GameObject.Find("Button(Clone)").GetComponent<ButtonController>();
            }
            bc.UpdateButtonState(ButtonController.ButtonState.CanBePressed);
        }

        private void FindCustomer()
        {
            GameObject monster = GameObject.Find("Monster(Clone)");
            if (monster != null)
            {
                cm = monster.GetComponent<CustomerManager>();
            }
        }

        // Move the chef and text to account for two gates.
        private void TwoGatePosition()
        {
            chef.GetComponent<Transform>().position += new Vector3(-1.7f, 6.5f, 0);
            panel.GetComponent<RectTransform>().position += new Vector3(-1.7f, 6.5f, 0);
            textObject.GetComponent<RectTransform>().position += new Vector3(-1.7f, 6.5f, 0);

        }

        public void EndTutorial()
        {
            gm.InTutorial = false;

            // Clear out any listeners
            FindCustomer();
            if (cm != null)
            {
                cm.ArrivedAtTable -= TutorialNext;
                cm.CakeReceived -= HideTutorial;
            }

            if (gpc1 != null)
            {
                gpc1.GateIsOnBelt -= TutorialNext;
                
            }
            if (gpc2 != null)
            {
                gpc2.GateIsOnBelt -= TutorialNext;
            }
            if (gpc3 != null)
            {
                gpc3.GateIsOnBelt -= TutorialNext;
            }

            // Make sure everything is active
            ActivateGates();
            ActivateButton();
            AllowFlip(goc1, true);

            HideTutorial();
        }

        public static void ResetTutorial() {
            GameObject tutorial = GameObject.Find("TutorialItems");
            if (tutorial != null)
            {
                tutorial.GetComponent<QCTutorialLevel>().EndTutorial();
            }
        }

        #endregion

    }
}

