using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlackBox
{
    public class TutorialLevel : MonoBehaviour
    {
        [Header("Grid Containers")]
        [SerializeField] private GameObject mainGridGO;
        [SerializeField] private GameObject leftGridGO;
        [SerializeField] private GameObject botGridGO;
        [SerializeField] private GameObject rightGridGO;
        [SerializeField] private GameObject topGridGO;

        [Header("Sequenced Tutorial Objects")]
        [SerializeField] private Animator TutorialAnimator;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private TextMeshProUGUI tutorialText;

        private Vector3 goalCoordinate = new Vector3(2, 2, 0);

        private Vector3[] coordinateSeq = new Vector3[] {
            new Vector3(0,4,0),
            new Vector3(0,-1, 1),
            new Vector3(0,0, 4),
            new Vector3(-1,2,0),
            new Vector3(-1,2,0),
            new Vector3(-1,2, 1),
            new Vector3(-1,1,0),
            new Vector3(-1,1,0),
            new Vector3(1,-1, 1),
            new Vector3(3,4,0),
            new Vector3(3,4,0),
            new Vector3(4,3, 2), //highlight goal (2,2,0)
            new Vector3(0,0, 3)
        };

        private string[] dialogueSeq = new string[] {
            "Let's get started! Click Batty to send her into the graveyard.", //(0,4)
            "She passed right through! No treasure on this path or the one next to it.", //(MISS) (highlight (0,4),(0,-1))
            "Notice that when you send Batty, your energy meter at the top goes down...",
            "So be careful! Click here to move Batty.", //(-1,2)
            "Click Batty again to send her into the graveyard.", //(-1,2)
            "Oh! She bumped into something! There must be treasure in this row.", //(HIT) (highlight (-1,2))
            "Let's try here. Click this spot.", //(-1,1)
            "Now click Batty again.", //(-1,1)
            "Hm... Batty turned, there must be treasure diagonal from where she turned.", //(DETOUR) (highlight (-1,1),(1,-1))
            "Just to be sure... Let's try here.", //(4,3)
            "Click Batty one more time.", //(4,3)
            "Great, we found it! Click and drag a lantern to the correct spot.", //(highlight (4,3),(3,4)) (2,2)
            "Now let's send Wolfie to check! Congrats, you found the treasure!"
        };

        private GameObject currCell = null;
        private GameObject goalCell = null;
        private int tutorialSeq = 0;


        void OnEnable() 
        {
            BBEvents.InitiateTutorialLevel += InitiateTutorial;
            BBEvents.EndTutorialLevel += EndTutorial;
        }
        void OnDisable()
        {
            BBEvents.InitiateTutorialLevel -= InitiateTutorial;
            BBEvents.EndTutorialLevel -= EndTutorial;
        }

        /* Called from BBEvents */
        public void InitiateTutorial() {
            TutorialAnimator.SetBool("TutorialActive", true);

            tutorialSeq = 0;
            disableNavCells(leftGridGO);
            disableNavCells(rightGridGO);
            disableNavCells(topGridGO);
            disableNavCells(botGridGO);

            enableNavCell(coordinateSeq[tutorialSeq]);
            tutorialText.text = dialogueSeq[tutorialSeq];
            TutorialAnimator.SetBool("WolfieOn", true);
            TutorialAnimator.SetInteger("TutorialSeq", tutorialSeq);

            goalCell = mainGridGO.transform.GetChild(10).gameObject;
            // turn off HUD stuff
        }

        /* Called from BBEvents */
        public void EndTutorial() {
            TutorialAnimator.SetBool("TutorialActive", false);
        }

        
        void FixedUpdate() {
            // If starting tutorial didn't work, try again
            if ((rightGridGO.transform.GetChild(0).gameObject.GetComponent<Button>().interactable) 
            && TutorialAnimator.GetBool("TutorialActive") == true) {
                EndTutorial();
                Invoke("InitiateTutorial", 0.1f);
            }
            // If lantern placed correctly, end tutorial 
            if ((goalCell != null) && (goalCell.GetComponent<NodeCell>().HasFlag()) && (tutorialSeq < 12)) {
                tutorialSeq = 11;
                tutorialNext();
            }
        }

        /* "Next" called from clicking button on Wolfie popup */
        public void tutorialNext() {
            tutorialSeq++;

            if (tutorialSeq >= dialogueSeq.Length-1) {
                // TutorialAnimator.SetBool("WolfieOn", false);
                Invoke("endDialogue", 2f);
                return;
            }

            TutorialAnimator.SetInteger("TutorialSeq", tutorialSeq);
            nextButton.SetActive(false);
            TutorialAnimator.SetBool("InfoOn", false);

            enableNavCell(coordinateSeq[tutorialSeq]);
            tutorialText.text = dialogueSeq[tutorialSeq];
        }

        /* Helper function */
        public void endDialogue() {
            TutorialAnimator.SetBool("WolfieOn", false);
        }

        /* "Next" called from clicking cell */
        public void navCellNext() {
            if ((tutorialSeq == 0)
            || (currCell.GetComponent<NavCell>().isMollyAt && !currCell.GetComponent<Animator>().GetBool("BatTravelOut"))
            || (!currCell.GetComponent<NavCell>().isMollyAt && currCell.GetComponent<Animator>().GetBool("BatTravelOut"))) {
                Invoke("tutorialNext", 0.5f);
            }
        }

        /* Disables all cells to init grid for sequenced clicking */
        public void disableNavCells(GameObject parent) {
            for (int i=0; i<4; i++) { //size of tutorial grid, 4x4
                parent.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            }
        }

        /* Enables cell in sequence, plus does nother actions depending on z-value */
        public void enableNavCell(Vector3 coor) {
            if (coor.z != 0) {
                if (coor.z == 3) { return; } // end
                if (coor.z == 4) { // indicate energy meter
                    nextButton.SetActive(true);
                    BBEvents.IndicateEmptyMeter?.Invoke();
                    return;
                }
                BBEvents.ShowHint?.Invoke(); // show path line
                highlightCurrentCell(); // highlight start cell of path
                
                if (coor.z == 1) { // show helper images
                    nextButton.SetActive(true);
                    TutorialAnimator.SetBool("InfoOn", true);
                } 
                else if (coor.z == 2) { // highlight goal cell
                    goalCell.GetComponent<Animator>().SetBool("NodeCell/Flagged", true);
                } 
            }

            // finding current cell game object
            GameObject parent = null; int i = -1;
            switch(coor.x){
                case -1:    parent = leftGridGO; i = (int)coor.y;   break;
                case 4:     parent = rightGridGO; i = (int)coor.y;  break;
            } 
            switch (coor.y) {
                case -1:    parent = botGridGO; i = (int)coor.x;    break;
                case 4:     parent = topGridGO; i = (int)coor.x;    break;
            }

            // If already the current cell, do nothing
            if (currCell == parent.transform.GetChild(i).gameObject) { 
                return; 
            } else {
                // Disable original currCell if not null
                if (currCell != null) { 
                    currCell.GetComponent<Button>().interactable = false;
                } 
                // Set new currCell
                currCell = parent.transform.GetChild(i).gameObject;
                currCell.GetComponent<Button>().interactable = true;
                currCell.GetComponent<Button>().onClick.AddListener(navCellNext);
            }
            if (coor.z!=0) { Invoke("highlightCurrentCell", 0.25f); } // highlight end cell of path
        }

        /* Still working on this one */
        public void highlightCurrentCell() {
            if (currCell == null) { return; }

            NavCell currNavCell = currCell.GetComponent<NavCell>();

            /* if (isOn && currNavCell.isMollyAt) {
            // if (isOn && currCell.GetComponent<Animator>().GetBool("BatTravelIn")) {
                currCell.GetComponent<Animator>().SetTrigger("BatPoofOut");
            } 
            else if (!isOn && currNavCell.isMollyAt) {
                currCell.GetComponent<Animator>().SetTrigger("BatPoofIn");
            } */
            
            currCell.GetComponent<Animator>().SetTrigger("Tutorial");
            // BBEvents.ToggleLinkedHighlight?.Invoke("Tutorial", currNavCell.linkedCellDirection, currNavCell.linkedCellPosition);
        }


    }
}