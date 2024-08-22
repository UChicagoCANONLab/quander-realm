using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth
{
    public class Tutorial : MonoBehaviour
    {
        public Player p1;

        public GameObject upButton;
        public GameObject switchButton;

        public bool[] completed = {false, false, false, false};

        [Header("Sequenced GameObjects")]
        // LEFT
        public GameObject instruction1;
        // public string tutorial1 = "Use the keyboard or arrows to move Fran"; // at (0,0)/start
            // when disable --> highlight arrow button (with "hint" infrastructure)
        
        // RIGHT
        public GameObject instruction2;
        public GameObject text1;
        public GameObject text2;
        // public string tutorial2 = "When you move me, Ken moves too! Even through walls!"; // at (0,1)
        // public string tutorial3 = "Press the Switch button to control Ken";
            // when disable --> highlight switch button

        // LEFT
        public GameObject instruction3;
        // public string tutorial4 = "Get us to the exit ladder in as few moves as you can"; // at (1,1)
            // when disable --> 
        



        void Update() {
            if (p1.getPloc == new Vector3(0,1,0) && !completed[2]) {
                instruction2.SetActive(true);
            }
            if (p1.getPloc == new Vector3(1,1,0) && !completed[3]){
                instruction3.SetActive(true);
            }
        }

        public void nextButton(int type) {
            switch(type){
                case 0:
                    completed[type] = true;
                    doExitAnimation(instruction1);
                    Invoke("closePopups", 1f);
                    upButton.GetComponent<Animation>().Play();
                    break;

                case 1:
                    completed[type] = true;
                    text1.SetActive(false);
                    text2.SetActive(true);
                    break;

                case 2:
                    completed[type] = true;
                    doExitAnimation(instruction2);
                    Invoke("closePopups", 1f);
                    switchButton.GetComponent<Animation>().Play();
                    break;

                case 3:
                    completed[type] = true;
                    doExitAnimation(instruction3);
                    Invoke("closePopups", 1f);
                    break;

                default:
                    break;
            }

        }

        public void doExitAnimation(GameObject popup) {
            popup.GetComponent<Animation>().Play("Popup-Exit");
        }

        public void closePopups() {
            instruction1.SetActive(false);
            instruction2.SetActive(false);
            instruction3.SetActive(false);
        }

    }
}
