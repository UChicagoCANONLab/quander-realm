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
        /* private Vector3[] coordinateSeq = new Vector3[] {
            new Vector3(0,4,0),
            new Vector3(0,-1,100),
            new Vector3(-1,2,0),
            new Vector3(-1,2,100),
            new Vector3(1,-1,0),
            new Vector3(4,3,100),
            new Vector3(3,4,0),
            new Vector3(0,0,0), 
            new Vector3(0,0,0) //null, when lantern at 2,2
        }; */

        private Vector3[] coordinateSeq2 = new Vector3[] {
            new Vector3(0,4,0),
            new Vector3(0,-1, 1),
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

        /* private string[] dialogueSeq = new string[] {
            "Let's get started! Click Batty to send her into the graveyard", //(0,4)
            "She passed right through! No treasure on this path or the one next to it", //(MISS)
            "Click here to move Batty, and then click Batty again", //(-1,2)
            "Oh! She bumped into something! There must be treasure in this row", //(HIT)
            "Let's try here. Click the spot and then click Batty again", //(1,-1)
            "Hm... Batty turned, there must be treasure diagonal from where she turned", //(DETOUR) (2,2)
            "Just to be sure... Let's try here", //(4,3)
            "Great, we found it! Click and drag a lantern to the correct spot", //(2,2)
            "Now let's send Wolfie to check! Congrats, you found the treasure!"
        }; */

        private string[] dialogueSeq2 = new string[] {
            "Let's get started! Click Batty to send her into the graveyard", //(0,4)
            "She passed right through! No treasure on this path or the one next to it", //(MISS) (highlight (0,4),(0,-1))
            "Click here to move Batty", //(-1,2)
            "Click Batty again to send her into the graveyard", //(-1,2)
            "Oh! She bumped into something! There must be treasure in this row", //(HIT) (highlight (-1,2))
            "Let's try here. Click this spot", //(-1,1)
            "Now click Batty again", //(-1,1)
            "Hm... Batty turned, there must be treasure diagonal from where she turned", //(DETOUR) (highlight (-1,1),(1,-1))
            "Just to be sure... Let's try here", //(4,3)
            "Click Batty one more time", //(4,3)
            "Great, we found it! Click and drag a lantern to the correct spot", //(highlight (4,3),(3,4)) (2,2)
            "Now let's send Wolfie to check! Congrats, you found the treasure!"
        };

        private GameObject currCell = null;
        private GameObject goalCell = null;
        private int tutorialSeq = 0;



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

            enableNavCell(coordinateSeq2[tutorialSeq]);
            tutorialText.text = dialogueSeq2[tutorialSeq];
            TutorialAnimator.SetBool("WolfieOn", true);

            goalCell = mainGridGO.transform.GetChild(10).gameObject;
        }

        void FixedUpdate() {
            if ((goalCell != null) && (goalCell.GetComponent<NodeCell>().HasFlag()) && (tutorialSeq < 11)) {
                tutorialSeq = 10;
                tutorialNext();
            }
        }

        public void tutorialNext() {
            tutorialSeq++;

            if (tutorialSeq >= dialogueSeq2.Length-1) {
                // TutorialAnimator.SetBool("WolfieOn", false);
                Invoke("endDialogue", 2f);
            }

            TutorialAnimator.SetInteger("TutorialSeq", tutorialSeq);
            nextButton.SetActive(false);
            TutorialAnimator.SetBool("InfoOn", false);

            enableNavCell(coordinateSeq2[tutorialSeq]);
            tutorialText.text = dialogueSeq2[tutorialSeq];
        }

        public void navCellNext() {
            if ((tutorialSeq == 0)
            || (currCell.GetComponent<NavCell>().isMollyAt && !currCell.GetComponent<Animator>().GetBool("BatTravelOut"))
            || (!currCell.GetComponent<NavCell>().isMollyAt && currCell.GetComponent<Animator>().GetBool("BatTravelOut"))) {
                Invoke("tutorialNext", 0.5f);
            }
        }

        public void disableNavCells(GameObject parent) {
            for (int i=0; i<4; i++) { //size of tutorial grid, 4x4
                parent.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            }
        }

        public void enableNavCell(Vector3 coor) {
            Debug.Log($"Sequence: {tutorialSeq}");

            if (coor.z != 0) {
                if (coor.z == 3) { return; }
                
                BBEvents.ShowHint?.Invoke();
                highlightCurrentCell(); // highlight start cell
                if (coor.z == 1) {
                    nextButton.SetActive(true);
                    TutorialAnimator.SetBool("InfoOn", true);
                } 
                else if (coor.z == 2) {
                    goalCell.GetComponent<Animator>().SetBool("NodeCell/Flagged", true);
                } 
            }

            GameObject parent = null; int i = -1;
            switch(coor.x){
                case -1:    parent = leftGridGO; i = (int)coor.y;   break;
                case 4:     parent = rightGridGO; i = (int)coor.y;  break;
            } 
            switch (coor.y) {
                case -1:    parent = botGridGO; i = (int)coor.x;    break;
                case 4:     parent = topGridGO; i = (int)coor.x;    break;
            }

            if (currCell == parent.transform.GetChild(i).gameObject) { 
                return; 
            } else {
                currCell = parent.transform.GetChild(i).gameObject;
                currCell.GetComponent<Button>().interactable = true;
                currCell.GetComponent<Button>().onClick.AddListener(navCellNext);
            }
            if (coor.z!=0) { Invoke("highlightCurrentCell", 0.25f); } // highlight end cell
        }

        // Helper functions
        public void endDialogue() {
            TutorialAnimator.SetBool("WolfieOn", false);
        }


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