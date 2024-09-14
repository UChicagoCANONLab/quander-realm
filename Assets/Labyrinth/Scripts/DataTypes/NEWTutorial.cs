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
        
        public Player p1;
        public ButtonBehavior BB;

        [Header("Twin Objects")]
        public TMP_Text twin0Text;
        public TMP_Text twin1Text;

        [Header("Tutorial Dialogue and Images")]
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

        private Animator animator = null;


        private void Awake() {
            animator = GetComponent<Animator>();
        }


        void Start() {
            animator.Play("NEWTutorial_Twin0_toOn");
            degree = SaveData.Instance.Degree;
            twinNext(0);
            animator.SetTrigger($"Deg{degree}Seq0");
        }


        void Update() {
            if (p1.getPloc == new Vector3(0,1,0)) {
                animator.SetTrigger($"Deg{degree}Seq1");
            }
            if (p1.getPloc == new Vector3(1,1,0) && degree == 0){
                animator.SetTrigger("Deg0Seq3");
            }
        }


        // ~~~~~~~~~~~~~~~ Sequenced Dialogue Function ~~~~~~~~~~~~~~~

        public void twinNext(int type) {
            string textTemp;

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
            seq++;
            animator.SetInteger("DegSeqClose", seq);

            switch(seq){
                case 1:
                    Invoke("delayPointerUp", 1f);
                    twinNext(1);
                    break;
                case 2:
                    twinNext(0);
                    if (degree != 90) {
                        animator.SetTrigger($"Deg{degree}Seq2");
                        seq++;
                        break;
                    } Invoke("delayPointerSwitch", 1f);
                    break;
                case 3:
                    twinNext(1);
                    Invoke("delayPointerSwitch", 1f);
                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }


        public void tutorialNextLevel() {
            if (degree == 0) {
                DialogueAndRewards.Instance.tutorialSeen[0] = true;
                BB.LevelSelect(1);
            } else if (degree == 180) {
                DialogueAndRewards.Instance.tutorialSeen[1] = true;
                BB.LevelSelect(6);
            } else if (degree == 90) {
                DialogueAndRewards.Instance.tutorialSeen[2] = true;
                BB.LevelSelect(11);
            } else { return; }
        }


        // ~~~~~~~~~~~~~~~ Helper Functions ~~~~~~~~~~~~~~~

        public void delayPointerUp() {
            animator.SetTrigger("PointerUp");
        }
        public void delayPointerSwitch() {
            animator.SetTrigger("PointerUp");
        }

    }
}
