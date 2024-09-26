using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Labyrinth
{
    public class NEWTutorial : MonoBehaviour
    {
        private int degree;
        private int seq = 0;
        
        public Animator animator;
        private Player p1;
        // public ButtonBehavior BB;

        [Header("Twin Objects")]
        public TMP_Text twin0Text;
        public TMP_Text twin1Text;

        [Header("Tutorial Dialogue and Images")]
        public GameObject[] tutorialImages;

        public string[] tutorial0 = {
            "Use the keyboard or arrows to move Fran",
            "When you move me,\n Ken moves too! Even through walls!",
            "Press the Switch button to control Fran",
            "Get us to the exit ladder in as few moves as you can"
        };
        public string[] tutorial180 = {
            "See how I’m at the opposite corner as I was last time? Now every move I make...",
            "I do the opposite!",
            "Typical of my annoying sister…",
            ""
        };
        public string[] tutorial90 = {
            "Now my brother goes sideways when I go up and down!",
            "Things are starting to get suuuper funky.",
            "",
            ""
        };


        void Start() {
            degree = SaveData.Instance.Degree;
            p1 = TTEvents.GetPlayer.Invoke(1);

            twinSetup(0);
            twinOn(0, true);
        }


        void Update() {
            // if (p1.getPloc == new Vector3(0,1,0) && seq == 1) {
            if (TTEvents.GetPlayer.Invoke(1).getPloc == new Vector3(0,1,0) && seq == 1) {
                twinOn(1, true);
            }
            // if (p1.getPloc == new Vector3(1,1,0) && degree == 0 && seq == 3){
            if (TTEvents.GetPlayer.Invoke(1).getPloc == new Vector3(1,1,0) && degree == 0 && seq == 3){
                twinOn(1, true);
            }
        }


        // ~~~~~~~~~~~~~~~ Sequenced Dialogue Function ~~~~~~~~~~~~~~~

        public void twinOn(int type, bool isOn) {
            animator.SetBool($"Twin{type}", isOn);
        }

        public void twinSetup(int type) {
            string textTemp;
            Invoke("setImages", 0.5f);

            switch(degree) {
                case 0:
                    textTemp = tutorial0[seq];
                    break;
                case 180:
                    textTemp = tutorial180[seq];
                    break;
                case 90:
                    textTemp = tutorial90[seq];
                    break;
                default:
                    textTemp = "";
                    break;
            }
        
            if (textTemp == "") { return; }
            
            if (type == 0) {
                twin0Text.text = textTemp;
            } else {
                twin1Text.text = textTemp;
            }     
        }

        // ~~~~~~~~~~~~~~~ Button Functions ~~~~~~~~~~~~~~~

        public void nextButtonAGAIN() {
            switch(seq){
                case 0:
                    seq++;
                    twinOn(0, false);
                    twinSetup(1);
                    Invoke("delayPointerUp", 1f);
                    break;
             
                case 1:
                    seq++;
                    twinOn(1, false);
                    twinSetup(0);
        
                    if (degree == 90) { Invoke("delayPointerSwitch", 1f); }
                    else { twinOn(0, true); }
                    break;
                
                case 2:
                    seq++;
                    twinOn(0, false);
                    twinSetup(1);
                    Invoke("delayPointerSwitch", 1f);
                    break;
              
                case 3:
                    seq++;
                    twinOn(1, false);
                    break;
             
                default:
                    break;
            }
            
        }


        public void tutorialNextLevel() {
            if (degree == 0) {
                DialogueAndRewards.Instance.tutorialSeen[0] = true;
                // BB.LevelSelect(1);
                TTEvents.SelectLevel?.Invoke(1);
            } else if (degree == 180) {
                DialogueAndRewards.Instance.tutorialSeen[1] = true;
                // BB.LevelSelect(6);
                TTEvents.SelectLevel?.Invoke(6);
            } else if (degree == 90) {
                DialogueAndRewards.Instance.tutorialSeen[2] = true;
                // BB.LevelSelect(11);
                TTEvents.SelectLevel?.Invoke(11);
            } else { return; }
        }


        // ~~~~~~~~~~~~~~~ Helper Functions ~~~~~~~~~~~~~~~

        public void delayPointerUp() {
            animator.SetTrigger("PointerUp");
        }
        public void delayPointerSwitch() {
            animator.SetTrigger("PointerSwitch");
        }
        
        public void setImages() {
            if (degree == 0) {
                foreach(GameObject i in tutorialImages) {
                    if (i!=null) {  i.SetActive(false); }
                }
                if (seq != 1) {
                    tutorialImages[seq].SetActive(true);
                }
            }
        }

        /* public void imagesOff() {
            if (degree == 0) {
                foreach (GameObject i in tutorialImages) {
                    if (i!=null)    {   i.SetActive(false); }
                }
            }
        }
        public void imagesOn() {
            if (degree == 0 && seq != 1) {
                tutorialImages[seq].SetActive(true);
            }
        } */

    }
}
