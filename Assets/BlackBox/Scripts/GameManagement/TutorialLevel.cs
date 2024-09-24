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
        [SerializeField] private TextMeshProUGUI tutorialText;
        private int tutorialSeq = 0;

        private Vector3 goalCoordinate = new Vector3(2, 2, 0);
        private Vector3[] coordinateSeq = new Vector3[] {
            new Vector3(0,4,0),
            new Vector3(0,0,5), //null, z!=0
            new Vector3(-1,2,0),
            new Vector3(0,0,5), //null
            new Vector3(1,-1,0),
            new Vector3(2,2,0), //GOAL
            new Vector3(4,1,0),
            new Vector3(2,2,0), //GOAL
            new Vector3(0,0,5) //null
        };
        private string[] dialogueSeq = new string[] {
            "Click Batty to send her into the graveyard", //(0,4)
            "She passed right through! No treasure on this path or the one next to it", //(MISS)
            "Click here to move Batty", //(-1,2)
            "Oh! She bumped into something! There must be treasure in this row", //(HIT)
            "Let's try here", //(1,-1)
            "Hm... Batty turned, there must be a treasure here", //(DETOUR) (2,2)
            "Just to be sure... Let's try here", //(4,1)
            "Great, we found it! Click and drag a lantern to the correct spot", //(2,2)
            "Now let's send Wolfie to see! Congrats, you found the treasure!"
        };

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
        }

        public void tutorialNext() {
            tutorialSeq++;
            if (tutorialSeq >= dialogueSeq.Length) {
                TutorialAnimator.SetBool("WolfieOn", false);
            }

            enableNavCell(coordinateSeq[tutorialSeq]);
            tutorialText.text = dialogueSeq[tutorialSeq];
        }


        public void disableNavCells(GameObject parent) {
            for (int i=0; i<4; i++) { //size of tutorial grid, 4x4
                parent.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            }
        }

        public void enableNavCell(Vector3 coor) {
            if (coor.z != 0) { return; }
            else if (coor == goalCoordinate) { return; }

            GameObject parent = null; int i = -1;
            switch(coor.x ){
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
            GameObject cell = parent.transform.GetChild(i).gameObject;
            cell.GetComponent<Button>().interactable = true;
            // cell.GetComponent<Animator>().Play("")
        }

        
    }
}