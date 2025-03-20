using System.Linq;
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
            

            if (TutorialManager.tutorialInd.Contains(levelInd))
            {
                gm.AllowGateMovement = false;
                Invoke("InitiateQCTutorial", 0.5f);
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
                case 2:
                    Tutorial2Next();
                    break;
                case 3:
                    Tutorial3Next();
                    break;
                case 8:
                    Tutorial8Next();
                    break;
                case 9:
                    Tutorial9Next();
                    break;
                case 13:
                    Tutorial13Next();
                    break;
                case 16:
                    Tutorial16Next();
                    break;
                case 23:
                    Tutorial23Next();
                    break;
                case 24:
                    Tutorial24Next();
                    break;
                default:
                    Debug.Log("Tried to run tutorial on level without one: " + levelInd);
                    break;
            }
        }
        #endregion


        #region Level 1 (NOT)
        private string[] dialogueSeq1 = new string[]
        {
            "The cupcake on the conveyor is what the customer wants, so let's press Play to send it over!",
            "Oops, looks like I made the wrong cupcake! Let's use this NOT gate to change it.",
            "That should work! Let's send our cupcake down the conveyor now.",
            "Nice job! I'm going to get back to baking now, but you can handle it from here!"
        };

        public void Tutorial1Next()
        {
            if (tutorialSeq >= dialogueSeq1.Length)
            {
                EndTutorial();
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

                default:
                    break;
            }

            tutorialSeq++;
        }
        #endregion

        #region Level 2 (Time Controls)
        private string[] dialogueSeq2 = new string[]
        {
            "Our customers won't wait forever! The bar under each customer is their " +
            "patience.",
            "The top left bar shows how long my shop will be open - see how many cupcakes " + 
            "you can serve!"
        };

        public void Tutorial2Next()
        {
            if (tutorialSeq >= dialogueSeq2.Length)
            {
                EndTutorial();
                return;
            }

            tutorialText.text = dialogueSeq2[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0:
                    ShowTutorial();
                    gm.AllowGateMovement = true;
                    gm.InTutorial = false;
                    LastPuzzleUtils();
                    break;

                case 1:
                    LastPuzzleUtils();
                    break;

                default:
                    break;
            }

            tutorialSeq++;
        }
        #endregion

        #region Level 3 (SWAP)
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
                EndTutorial();
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
                    AltPosition(2);
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

                default:
                    break;
            }
            tutorialSeq++;

        }
        #endregion

        #region Level 8 (CNOT)
        private string[] dialogueSeq8 = new string[]
        {
            "This is my Controlled NOT (CNOT). It only activates the bottom NOT gate if " +
            "the top cupcake is chocolate!",
            "The top cupcake is chocolate, so the bottom cupcake will be inverted " +
            "into a vanilla cupcake.",
            "If the top cupcake is vanilla, the gate won't do anything. Try using" +
            " it on these cupcakes!",
            "The top cupcake is vanilla, so the bottom cupcake will not change, and" +
            " will remain chocolate.",
            "Looks like we need a flipped version of this flavor inverter! Let's" +
            " start by placing it on the belts.",
            "Now, click on the CNOT gate to flip it!",
            "Great! The NOT symbol is the side where the cupcake can change, and " +
            "the small dot controls it. ",
            "Nice work. Don't forget that the book icon on the right can remind you " +
            "about what any gate does!"
        };

        public void Tutorial8Next()
        {
            if (tutorialSeq >= dialogueSeq8.Length)
            {
                EndTutorial();
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
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 5: // Get player to flip gate
                    DeactivateGate(gpc1);
                    AllowFlip(goc1, true);
                    gpc1.GateIsOnBelt -= TutorialNext;
                    goc1.GateFlipped += TutorialNext;
                    break;

                case 6:
                    ClickPlayUtils();
                    goc1.GateFlipped -= TutorialNext;
                    break;

                case 7:
                    LastPuzzleUtils();
                    break;

                default:
                    break;
            }

            tutorialSeq++;

        }
        #endregion

        #region Level 9 (Flipping Reminder)
        private string[] dialogueSeq9 = new string[]
        {
            "Don't forget that you can click on the CNOT gate to flip it!",
            "Of course, if you need to flip it back you can click on it again."
        };

        public void Tutorial9Next()
        {
            if (tutorialSeq >= dialogueSeq9.Length)
            {
                EndTutorial();
                return;
            }

            tutorialText.text = dialogueSeq9[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0:
                    ShowTutorial();
                    gm.AllowGateMovement = true;
                    gm.InTutorial = false;
                    LastPuzzleUtils();
                    break;

                case 1:
                    LastPuzzleUtils();
                    break;

                default:
                    break;
            }

            tutorialSeq++;
        }
        #endregion

        #region Level 13 (H)
        private string[] dialogueSeq13 = new string[]
        {
            "Sometimes customers want a surprise! This H gate turns " +
            "the cupcake into a mystery.",
            "When this cupcake gets to the customer, it'll randomly choose a " +
            "flavor!",
            "The cupcake will become a mystery no matter what flavor is input into" +
            "the H gate.",
            "Don't forget that the customer wants the mystery, so they'll be happy" +
            " no matter the outcome.",
            "These H gates also turn mystery boxes back into regular cupcakes!",
            "This mystery box has the vanilla coloring, so the H will turn it back" +
            "into vanilla.",
            "Similarly, the H will turn this to chocolate. These boxes may be a mystery," +
            " but H demystifies them."
        };

        public void Tutorial13Next()
        {
            if (tutorialSeq >= dialogueSeq13.Length)
            {
                EndTutorial();
                return;
            }

            tutorialText.text = dialogueSeq13[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0: // Introduce H gate
                    // Setup
                    tutorialGate1 = GameObject.FindGameObjectWithTag("Gate");
                    gpc1 = tutorialGate1.GetComponent<GatePositionController>();
                    goc1 = tutorialGate1.GetComponent<GateOperationController>();
                    gm.AllowGateMovement = true;

                    // Start Tutorial
                    NewPuzzleUtils();
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 1: // Click play once H gate has been placed
                    ClickPlayUtils();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    break;

                case 2: // Show that H makes superposition regardless of input.
                    NewPuzzleUtils();
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 3: // Click play 
                    ClickPlayUtils();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    break;

                case 4: // Show that vanilla superposition + H = vanilla
                    NewPuzzleUtils();
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 5: // Click play
                    ClickPlayUtils();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    break;

                case 6:
                    LastPuzzleUtils();
                    break;

                default:
                    break;
            }

            tutorialSeq++;

        }
        #endregion

        #region Level 16 (Z)
        private string[] dialogueSeq16 = new string[]
        {
            "This Z gate inverts the mystery box, so that it becomes a mystery box " +
            "of the other color!",
            "Now that we've changed the color, if we use the H gate, this box will " +
            "become a vanilla cupcake.",
            "Click play to watch the magic happen.",
            "Something to remember: if the cupcake isn't a mystery box, the Z gate " +
            "won't do anything."
        };

        public void Tutorial16Next()
        {
            if (tutorialSeq >= dialogueSeq16.Length)
            {
                EndTutorial();
                return;
            }

            tutorialText.text = dialogueSeq16[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0: // Introduce Z gate
                    // Setup
                    foreach (GameObject gate in GameObject.FindGameObjectsWithTag("Gate"))
                    {
                        SpriteResolver resolver = gate.GetComponent<SpriteResolver>();
                        if (resolver.GetLabel() == "H")
                        {
                            tutorialGate1 = gate;
                            gpc1 = gate.GetComponent<GatePositionController>();
                        }
                        else if (resolver.GetLabel() == "Z")
                        {
                            tutorialGate2 = gate;
                            gpc2 = gate.GetComponent<GatePositionController>();
                        }
                    }
                    AltPosition(1);
                    
                    gm.AllowGateMovement = true;

                    // Start Tutorial
                    NewPuzzleUtils();
                    DeactivateGate(gpc1);
                    gpc2.GateIsOnBelt += TutorialNext;
                    break;

                case 1: // Force Z to be in slot 1, put H after it.
                    GateSlots.Instance.moveGateToSlot(tutorialGate2, (0, 0));
                    gpc2.GateIsOnBelt -= TutorialNext;
                    DeactivateGate(gpc2);

                    ActivateGate(gpc1);
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 2: // Click play
                    GateSlots.Instance.moveGateToSlot(tutorialGate1, (0, 3));
                    ClickPlayUtils();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    break;

                case 3:
                    LastPuzzleUtils();
                    break;

                default:
                    break;
            }

            tutorialSeq++;

        }
        #endregion

        #region Level 23 (Same Entangle)
        private string[] dialogueSeq23 = new string[]
        {
            "These two want mystery boxes, but they want their cupcakes to be " +
            "the same! Let's start with an H on top.",
            "If we use a CNOT now, the bottom box will only be chocolate if the top " +
            "one becomes chocolate when opened.",
            "That way they'll always have the same flavor cupcake from the mystery " +
            "box! Let's try it now.",
            "Keep in mind that this only works if the bottom flavor is vanilla. Next " +
            "level we'll see what happens if it's not!"
        };

        public void Tutorial23Next()
        {
            if (tutorialSeq >= dialogueSeq23.Length)
            {
                EndTutorial();
                return;
            }

            tutorialText.text = dialogueSeq23[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0: // Start entanglement
                    // Setup
                    foreach (GameObject gate in GameObject.FindGameObjectsWithTag("Gate"))
                    {
                        SpriteResolver resolver = gate.GetComponent<SpriteResolver>();
                        if (resolver.GetLabel() == "CNOT")
                        {
                            tutorialGate1 = gate;
                            gpc1 = gate.GetComponent<GatePositionController>();
                            goc1 = gate.GetComponent<GateOperationController>();
                        }
                        else if (resolver.GetLabel() == "NOT")
                        {
                            tutorialGate2 = gate;
                            gpc2 = gate.GetComponent<GatePositionController>();
                        }
                        else if (resolver.GetLabel() == "H")
                        {
                            tutorialGate3 = gate;
                            gpc3 = gate.GetComponent<GatePositionController>();
                        }
                    }
                    AltPosition(2);

                    gm.AllowGateMovement = true;

                    // Start Tutorial
                    NewPuzzleUtils();
                    DeactivateGate(gpc1);
                    DeactivateGate(gpc2);
                    gpc3.GateIsOnBelt += TutorialNext;
                    break;

                case 1: // Move H to top left, place CNOT.
                    GateSlots.Instance.moveGateToSlot(tutorialGate3, (1, 0));
                    gpc3.GateIsOnBelt -= TutorialNext;
                    DeactivateGate(gpc3);

                    ActivateGate(gpc1);
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 2: // Click play
                    ClickPlayUtils();
                    gpc1.GateIsOnBelt -= TutorialNext;
                    break;

                case 3:
                    LastPuzzleUtils();
                    break;

                default:
                    break;
            }

            tutorialSeq++;

        }
        #endregion

        #region Level 24 (Opposite Entangle)
        private string[] dialogueSeq24 = new string[]
        {
            "These two want opposite mystery boxes. Let's start with the " +
            "same circuit, first with an H at the top left.",
            "Like before, we'll add a CNOT afterwards to make their outcomes " +
            "depend on each other.",
            "Now, let's try switching the bottom cupcake to chocolate. ",
            "This way, if the top mystery cupcake becomes chocolate, then the " +
            "bottom switches to vanilla!",
            "When two mystery boxes depend on each other, we call them entangled. " +
            "Have fun with entanglement!"
        };

        public void Tutorial24Next()
        {
            if (tutorialSeq >= dialogueSeq24.Length)
            {
                EndTutorial();
                return;
            }

            tutorialText.text = dialogueSeq24[tutorialSeq];
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);

            switch (tutorialSeq)
            {
                case 0: // Start entanglement
                    // Setup
                    foreach (GameObject gate in GameObject.FindGameObjectsWithTag("Gate"))
                    {
                        SpriteResolver resolver = gate.GetComponent<SpriteResolver>();
                        if (resolver.GetLabel() == "CNOT")
                        {
                            tutorialGate1 = gate;
                            gpc1 = gate.GetComponent<GatePositionController>();
                            goc1 = gate.GetComponent<GateOperationController>();
                        }
                        else if (resolver.GetLabel() == "NOT")
                        {
                            tutorialGate2 = gate;
                            gpc2 = gate.GetComponent<GatePositionController>();
                        }
                        else if (resolver.GetLabel() == "H")
                        {
                            tutorialGate3 = gate;
                            gpc3 = gate.GetComponent<GatePositionController>();
                        }
                    }
                    AltPosition(2);

                    gm.AllowGateMovement = true;

                    // Start Tutorial
                    NewPuzzleUtils();
                    DeactivateGate(gpc1);
                    DeactivateGate(gpc2);
                    gpc3.GateIsOnBelt += TutorialNext;
                    break;

                case 1: // Move H to top left, place CNOT.
                    GateSlots.Instance.moveGateToSlot(tutorialGate3, (1, 0));
                    gpc3.GateIsOnBelt -= TutorialNext;
                    DeactivateGate(gpc3);

                    ActivateGate(gpc1);
                    gpc1.GateIsOnBelt += TutorialNext;
                    break;

                case 2: // Place NOT in bottom left.
                    gpc1.GateIsOnBelt -= TutorialNext;
                    DeactivateGate(gpc1);

                    ActivateGate(gpc2);
                    gpc2.GateIsOnBelt += TutorialNext;
                    break;

                case 3: // Click play
                    GateSlots.Instance.moveGateToSlot(tutorialGate2, (0, 0));
                    ClickPlayUtils();
                    gpc2.GateIsOnBelt -= TutorialNext;
                    break;

                case 4:
                    LastPuzzleUtils();
                    break;

                default:
                    break;
            }

            tutorialSeq++;

        }
        #endregion



        #region Util: Puzzle Paradigms
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
        #endregion


        #region Util: Functionality
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
                GameObject button = GameObject.Find("Button(Clone)");
                if (button != null)
                {
                    bc = button.GetComponent<ButtonController>();
                }
            }
            if (bc != null)
            {
                bc.UpdateButtonState(ButtonController.ButtonState.CanBePressed);
            }
            
        }
        #endregion


        #region Util: Appearance

        private void ShowTutorial()
        {
            chef.GetComponent<Image>().enabled = true;
            panel.GetComponent<Image>().enabled = true;
            textObject.GetComponent<Text>().enabled = true;
            pointer.GetComponent<Canvas>().enabled = true;
        }

        private void HideTutorial()
        {
            chef.GetComponent<Image>().enabled = false;
            panel.GetComponent<Image>().enabled = false;
            textObject.GetComponent<Text>().enabled = false;
            pointer.GetComponent<Canvas>().enabled = false;
            pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
        }

        // Move the chef and text to account for more gates.
        private void AltPosition(int belts)
        {
            switch (belts)
            {
                case 1:
                    chef.GetComponent<RectTransform>().position += new Vector3(-1.73f, 5.5f, 0);
                    panel.GetComponent<RectTransform>().position += new Vector3(-1.73f, 5.5f, 0);
                    // textObject.GetComponent<RectTransform>().position += new Vector3(-1.73f, 5.5f, 0);
                    break;
                case 2:
                    chef.GetComponent<RectTransform>().position += new Vector3(-1.73f, 6.5f, 0);
                    panel.GetComponent<RectTransform>().position += new Vector3(-1.73f, 6.5f, 0);
                    // textObject.GetComponent<RectTransform>().position += new Vector3(-1.73f, 6.5f, 0);
                    break;
                default:
                    break;
            }
            
        }
        #endregion


        #region Util: Overarching

        private void FindCustomer()
        {
            GameObject monster = GameObject.Find("Monster(Clone)");
            if (monster != null)
            {
                cm = monster.GetComponent<CustomerManager>();
            }
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
            gm.AllowGateMovement = true;
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

