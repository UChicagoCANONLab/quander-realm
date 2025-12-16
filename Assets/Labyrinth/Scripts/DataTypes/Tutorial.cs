using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Labyrinth
{
    public class Tutorial : MonoBehaviour
    {
        private int degree;
        private int seq = 0;
        private int pointerSeq = 0;
        private Player p1;

        public Animator twinAnimator;
        public Animator pointerAnimator;
        
        public TMP_Text textObj;

        private string[] tutorial0 = {
            "Use the keyboard or arrows to move me around the maze.",
            "When you move Ken, I move too! Even through walls!",
            "Press the Switch button to control Fran if I run into a wall.",
            "Get us to the exit ladder in as few moves as you can. Let's go!"
        };
        private string[] tutorial180 = {
            "See how Fran is at the opposite corner as last time? Now every move I make...",
            "I do the opposite!",
            "Typical of my annoying sister..."
        };
        private string[] tutorial90 = {
            "Now Fran goes sideways when I go up and down!",
            "Things are starting to get suuuper funky."
        };


        void Start() {
            degree = SaveData.Instance.Degree;
            p1 = TTEvents.GetPlayer.Invoke(1);
            
            twinAnimator.SetInteger("Degree", degree);
            dialogueSetup();
            twinAnimator.SetBool("IsOn", true);
        }


        void Update() {
            // Debug.Log(p1.getPloc);

            if (pointerSeq == 0 && p1.getPloc == new Vector3(0,1,0)) {
                pointerAnimator.SetBool("UpOn", false);
                pointerAnimator.SetBool("SwitchOn", true);
                pointerSeq=1;
            }
            if (pointerSeq == 1 && p1.getPloc == new Vector3(0,0,0)) {
                pointerAnimator.SetBool("SwitchOn", false);
                pointerSeq=2;
            }
            if (pointerSeq == 2 && p1.getPloc == new Vector3(2,2,0)) {
                pointerAnimator.SetBool("SwitchOn", true);
                pointerSeq=3;
            }
            if (pointerSeq == 3 && p1.getPloc == new Vector3(2,1,0)) {
                pointerAnimator.SetBool("SwitchOn", false);
                pointerSeq=4;
            }
        }


        // ~~~~~~~~~~~~~~~ Sequenced Dialogue Function ~~~~~~~~~~~~~~~

        public void nextDialogue() {
            seq++; 
            twinAnimator.SetInteger("Seq", seq);
            twinAnimator.SetInteger("TwinActive", seq%2);
            bool end = false;

            if      (degree==0   &&  seq==tutorial0.Length)     { end = true; }
            else if (degree==180 &&  seq==tutorial180.Length)   { end = true; }
            else if (degree==90  &&  seq==tutorial90.Length)    { end = true; }
            else {
                dialogueSetup();
            }

            if (end) {
                twinAnimator.SetBool("IsOn", false);
                pointerAnimator.SetBool("UpOn", true);
                return;
            }
        }


        public void dialogueSetup() {
            string textTemp;

            switch(degree) {
                case 0:
                    textTemp = tutorial0[seq]; break;
                case 180:
                    textTemp = tutorial180[seq]; break;
                case 90:
                    textTemp = tutorial90[seq]; break;
                default:
                    textTemp = ""; break;
            }            
            textObj.text = textTemp;
        }

        // ~~~~~~~~~~~~~~~ Button Functions ~~~~~~~~~~~~~~~


        public void tutorialNextLevel() {
            if (degree == 0) {
                DialogueAndRewards.Instance.tutorialSeen[0] = true;
                TTEvents.SelectLevel?.Invoke(1);
            } 
            else if (degree == 180) {
                DialogueAndRewards.Instance.tutorialSeen[1] = true;
                TTEvents.SelectLevel?.Invoke(6);
            } 
            else if (degree == 90) {
                DialogueAndRewards.Instance.tutorialSeen[2] = true;
                TTEvents.SelectLevel?.Invoke(11);
            } 
            else { return; }
        }

    }
}
