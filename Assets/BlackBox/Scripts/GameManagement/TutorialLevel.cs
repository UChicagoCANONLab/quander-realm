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
        [SerializeField] private GameObject pointerObj;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private TextMeshProUGUI tutorialText;
        private int tutorialSeq = 0;

        private Vector3 goalCoordinate = new Vector3(2, 2, 0);
        private Vector3[] coordinateSeq = new Vector3[] {
            new Vector3(0,4,0),
            new Vector3(0,-1,100),
            new Vector3(-1,2,0),
            new Vector3(-1,2,100),
            new Vector3(1,-1,0),
            new Vector3(4,3,100),
            new Vector3(3,4,0),
            new Vector3(0,0,0), 
            new Vector3(0,0,0) //null, when lantern at 2,2
        };
        private string[] dialogueSeq = new string[] {
            "Let's get started! Click Batty to send her into the graveyard", //(0,4)
            "She passed right through! No treasure on this path or the one next to it", //(MISS)
            "Click here to move Batty, and then click Batty again", //(-1,2)
            "Oh! She bumped into something! There must be treasure in this row", //(HIT)
            "Let's try here. Click the spot and then click Batty", //(1,-1)
            "Hm... Batty turned, there must be a treasure here", //(DETOUR) (2,2)
            "Just to be sure... Let's try here", //(4,3)
            "Great, we found it! Click and drag a lantern to the correct spot", //(2,2)
            "Now let's send Wolfie to see! Congrats, you found the treasure!"
        };
        private GameObject currCell = null;
        private GameObject goalCell = null;



        void OnEnable() 
        {
            BBEvents.InitiateTutorialLevel += InitiateTutorial;
        }
        void OnDisable()
        {
            BBEvents.InitiateTutorialLevel -= InitiateTutorial;
        }


        public void InitiateTutorial() {
            tutorialSeq = 0;
            disableNavCells(leftGridGO);
            disableNavCells(rightGridGO);
            disableNavCells(topGridGO);
            disableNavCells(botGridGO);

            enableNavCell(coordinateSeq[tutorialSeq]);
            tutorialText.text = dialogueSeq[tutorialSeq];
            TutorialAnimator.SetBool("WolfieOn", true);

            goalCell = mainGridGO.transform.GetChild(10).gameObject;
        }

        public void tutorialNext() {
            tutorialSeq++;

            nextButton.SetActive(false);
            TutorialAnimator.SetBool("InfoOn", false);
            TutorialAnimator.SetInteger("TutorialSeq", tutorialSeq);
            
            if (tutorialSeq >= dialogueSeq.Length-1) {
                // TutorialAnimator.SetBool("WolfieOn", false);
                Invoke("endDialogue", 2f);
            }
            enableNavCell(coordinateSeq[tutorialSeq]);
            tutorialText.text = dialogueSeq[tutorialSeq];
        }

        public void navCellNext() {
            if ((!currCell.GetComponent<NavCell>().isMollyAt && currCell.GetComponent<Animator>().GetBool("BatTravelOut"))
            || (tutorialSeq == 0)) {
                Invoke("tutorialNext", 0.5f);
            }
        }


        public void disableNavCells(GameObject parent) {
            for (int i=0; i<4; i++) { //size of tutorial grid, 4x4
                parent.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            }
        }

        public void enableNavCell(Vector3 coor) {
            if (coor.z != 0) { 
                nextButton.SetActive(true);
                TutorialAnimator.SetBool("InfoOn", true);
                TutorialAnimator.SetInteger("TutorialSeq", tutorialSeq);
                // popup with help picture show up
                return; 
            } else if (tutorialSeq == 5 || tutorialSeq == 7) { 
                nextButton.SetActive(true);
                goalCell.GetComponent<Animator>().SetBool("NodeCell/Flagged", true);
                return;
            } else if (tutorialSeq == 8) { return; }

            GameObject parent = null; int i = -1;
            switch(coor.x){
                case -1:    
                    parent = leftGridGO; i = (int)coor.y;
                    break;
                case 4:     
                    parent = rightGridGO; i = (int)coor.y;
                    break;
            } switch (coor.y) {
                case -1:    
                    parent = botGridGO; i = (int)coor.x;
                    break;
                case 4:     
                    parent = topGridGO; i = (int)coor.x;
                    break;
            }
            currCell = parent.transform.GetChild(i).gameObject;
            currCell.GetComponent<Button>().interactable = true;
            currCell.GetComponent<Button>().onClick.AddListener(navCellNext);
            highlightCell(currCell);
            // currCell.GetComponent<Animator>().Play("Highlighted");
            // currCell.GetComponent<Animator>().SetTrigger("Highlighted");
        }

        public void highlightCell(GameObject cell) {
            for (int i=0; i<3; i++) {
                cell.GetComponent<Animator>().SetTrigger("Highlighted");
            } 
        }

        // Helper functions
        public void endDialogue() {
            TutorialAnimator.SetBool("WolfieOn", false);
        }

        
    }
}