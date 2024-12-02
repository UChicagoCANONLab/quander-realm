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
        [SerializeField] public GameObject textBox;
        [SerializeField] public GameObject textObject;
        [SerializeField] public GameObject pointer;
        [SerializeField] public Text tutorialText;

        /* Tutorial Events:
         * Puzzle 1:
         *      Activate play, deactivate gate
         *      Dialogue: Looks like the cupcake on the conveyor is the one that the customer wants, so let's press Play to send it over!
         *      Animation: Mouse towards play button, click on it
         *      Transition: click on play button
         *      
         * Puzzle 2:
         *      Activate gate, deactivate play
         *      Dialogue: Oops, looks like I made the wrong cupcake here! Let's use the Flavor Inverter gate to change the cupcake flavor.
         *      Animation: Click on gate, drag to belt
         *      Transition: Gate on carousel
         *      
         *      Activate play, deactivate gate
         *      Dialogue: That should work! Let's send our cupcake down the conveyor now.
         *      Animation: Mouse towards play button, click on it
         *      Transition: Click on play button
         *      
         * Puzzle 3:
         *      Activate everything
         *      Dialogue: Nice work! I'm going to get back to baking now, but you can handle it from here!
         *      Transition: Click on play button
         */

        private string[] dialogueSeq = new string[]
        {
            "It looks like the cupcake on the conveyor is the one that the customer wants, so let's press Play to send it over!",
            "Oops, looks like I made the wrong cupcake here! Let's use the Flavor Inverter gate to change the cupcake flavor.",
            "That should work! Let's send our cupcake down the conveyor now.",
            "Nice work! I'm going to get back to baking now, but you can handle it from here!"
        };

        private int tutorialSeq = 0;
        GameManagement gm;
        ButtonController bc;
        GameObject notGate;
        GatePositionController gpc;
        CustomerManager cm;

        private void Start()
        {
            gm = GameManagement.Instance;
            if (gm.GetCurrentLevelInd() == 1)
            {
                Invoke("InitiateQCTutorial", 0.5f);
            }
        }

        public void InitiateQCTutorial()
        {
            gm.InTutorial = true;
            bc = GameObject.Find("Button(Clone)").GetComponent<ButtonController>();
            FindCustomer();
            notGate = GameObject.FindGameObjectWithTag("Gate");
            gpc = notGate.GetComponent<GatePositionController>();

            cm.ArrivedAtTable += TutorialNext;
            cm.CakeReceived += HideTutorial;
            if (cm.AtTable())
            {
                TutorialNext();
            }
        }

        public void TutorialNext()
        {   
            switch (tutorialSeq)
            {
                case 0:
                    ShowTutorial();
                    FindCustomer();
                    ActivateButton();
                    DeactivateGate();
                    tutorialText.text = dialogueSeq[tutorialSeq];
                    // Animation

                    tutorialSeq++;
                    break;
                case 1:
                    ShowTutorial();
                    FindCustomer();
                    ActivateGate();
                    DeactivateButton();
                    tutorialText.text = dialogueSeq[tutorialSeq];
                    // Animation
                    gpc.GateIsOnBelt += TutorialNext;

                    tutorialSeq++;
                    break;
                case 2:
                    ActivateButton();
                    DeactivateGate();
                    gpc.GateIsOnBelt -= TutorialNext;
                    tutorialText.text = dialogueSeq[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 3:
                    ShowTutorial();
                    ActivateButton();
                    ActivateGate();
                    gm.InTutorial = false;
                    tutorialText.text = dialogueSeq[tutorialSeq];

                    tutorialSeq++;
                    break;
                case 4:
                    EndTutorial();
                    break;
                default:
                    break;
            }
        }

        private void ShowTutorial()
        {
            chef.SetActive(true);
            textBox.SetActive(true);
            textObject.SetActive(true);
            pointer.SetActive(true);
        }

        private void HideTutorial()
        {
            chef.SetActive(false);
            textBox.SetActive(false);
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

        private void EndTutorial()
        {
            cm.ArrivedAtTable -= TutorialNext;
            cm.CakeReceived -= HideTutorial;
            gm.InTutorial = false;
            HideTutorial();
        }

    }
}

