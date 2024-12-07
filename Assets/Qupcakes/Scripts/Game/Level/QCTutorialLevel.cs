using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



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
        GameManagement gm;
        ButtonController bc;
        GameObject tutorialGate;
        GatePositionController gpc;
        CustomerManager cm;

        #region Initiation
        private void Start()
        {
            gm = GameManagement.Instance;
            levelInd = gm.GetCurrentLevelInd();

            if (levelInd == 1)
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
            bc = GameObject.Find("Button(Clone)").GetComponent<ButtonController>();

            FindCustomer();
            cm.ArrivedAtTable += TutorialNext;
            cm.CakeReceived += HideTutorial;

            tutorialGate = GameObject.FindGameObjectWithTag("Gate");
            gpc = tutorialGate.GetComponent<GatePositionController>();
            
            if (cm.AtTable())
            {
                TutorialNext();
            }
        }
        #endregion

        public void TutorialNext()
        {
            switch(levelInd)
            {
                case 1:
                    Tutorial1Next();
                    break;
                default:
                    Debug.Log("Tried to run tutorial on level without one.");
                    break;
            }
        }

        #region Level 1
        private string[] dialogueSeq1 = new string[]
        {
            "It looks like the cupcake on the conveyor is the one that the customer wants, so let's press Play to send it over!",
            "Oops, looks like I made the wrong cupcake here! Let's use the Flavor Inverter gate to change the cupcake flavor.",
            "That should work! Let's send our cupcake down the conveyor now.",
            "Nice work! I'm going to get back to baking now, but you can handle it from here!"
        };

        public void Tutorial1Next()
        {
            switch (tutorialSeq)
            {
                case 0:
                    ShowTutorial();
                    FindCustomer();
                    ActivateButton();
                    DeactivateGate();
                    tutorialText.text = dialogueSeq1[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 1:
                    ShowTutorial();
                    pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
                    FindCustomer();
                    ActivateGate();
                    DeactivateButton();
                    tutorialText.text = dialogueSeq1[tutorialSeq];
                    gpc.GateIsOnBelt += TutorialNext;

                    tutorialSeq++;
                    break;
                case 2:
                    pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
                    ActivateButton();
                    DeactivateGate();
                    gpc.GateIsOnBelt -= TutorialNext;
                    tutorialText.text = dialogueSeq1[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 3:
                    ShowTutorial();
                    pointerAnimator.SetInteger("TutorialSeq", tutorialSeq);
                    ActivateButton();
                    ActivateGate();
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



        #region Utilities 
        private void ShowTutorial()
        {
            chef.SetActive(true);
            panel.SetActive(true);
            textObject.SetActive(true);
            pointer.SetActive(true);
        }

        private void HideTutorial()
        {
            chef.SetActive(false);
            panel.SetActive(false);
            textObject.SetActive(false);
            pointer.SetActive(false);
        }

        private void DeactivateGate()
        {
            gm.AllowGateMovement = false;
        }

        private void ActivateGate()
        {
            gm.AllowGateMovement = true;
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
                    gpc.GateIsOnBelt -= TutorialNext;
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

