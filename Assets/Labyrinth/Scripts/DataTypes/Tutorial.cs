using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Labyrinth
{
    public class Tutorial : MonoBehaviour
    {
        public int degree; //= 0;
        private int seq = 0;
        
        public Player p1;

        public GameObject upButton;
        public GameObject switchButton;

        [Header("Twin0 Objects")]
        public GameObject twin0Popup;
        public TMP_Text twin0Text;
        public Image twin0Image;
        
        [Header("Twin1 Objects")]
        public GameObject twin1Popup;
        public TMP_Text twin1Text;
        public Image twin1Image;

        [Header("Tutorial 0 Degrees")]
        public string[] tutorial0 = {
            "Use the keyboard or arrows to move Fran",
            "When you move me,\n Ken moves too! Even through walls!",
            "Press the Switch button to control Ken",
            "Get us to the exit ladder in as few moves as you can"
        };
        public Sprite[] images0;
        private bool[] completed0 = {false, false, false, false};

        [Header("Tutorial 180 Degrees")]
        public string[] tutorial180 = {
            "See how I’m at the opposite corner as I was last time? Now every move I make...",
            "I do the opposite!",
            "Typical of my annoying sister…",
            ""
        };
        public Sprite[] images180;
        private bool[] completed180 = {false, false, false, false};

        [Header("Tutorial 90 Degrees")]
        public string[] tutorial90 = {
            "Now my brother goes sideways when I go up and down!",
            "Things are starting to get suuuper funky.",
            "",
            ""
        };
        public Sprite[] images90;
        private bool[] completed90 = {false, false, false, false};

        

        void Start() {
            degree = SaveData.Instance.Degree;
            twinNext(0);
        }


        void Update() {
            if (p1.getPloc == new Vector3(0,1,0) && !completed0[2]) {
                twinNext(1);
            }

            if (p1.getPloc == new Vector3(1,1,0) && !completed0[3]){
                twinNext(0);
            }
        }

        public void twinNext(int type) {
            string textTemp; Sprite imageTemp;

            switch(degree) {
                case 0:
                    textTemp = tutorial0[seq];
                    imageTemp = images0[seq];
                    break;
                case 180:
                    textTemp = tutorial180[seq];
                    imageTemp = images180[seq];
                    break;
                case 90:
                    textTemp = tutorial90[seq];
                    imageTemp = images90[seq];
                    break;
                default:
                    textTemp = ""; imageTemp = null;
                    break;
            }
        
            if (type == 0) {
                twin0Text.text = textTemp;
                if (imageTemp == null) {
                    twin0Image.gameObject.SetActive(false);
                } else {
                    twin0Image.sprite = imageTemp;
                    twin0Image.gameObject.SetActive(true);
                }
                twin0Popup.SetActive(true);
            } 
            else {
                twin1Text.text = textTemp;
                if (imageTemp == null) {
                    twin1Image.gameObject.SetActive(false);
                } else {
                    twin1Image.sprite = imageTemp;
                    twin1Image.gameObject.SetActive(true);
                }
                twin1Popup.SetActive(true);
            }
            
        }


        public void nextButton() {
            switch(seq) {
                case 0:
                    completed0[seq] = true;
                    doExitAnimation(twin0Popup);
                    Invoke("closePopups", 1f);
                    upButton.GetComponent<Animation>().Play();
                    seq++;
                    break;

                case 1:
                    completed0[seq] = true;
                    seq++;
                    twinNext(1);
                    break;

                case 2:
                    completed0[seq] = true;
                    doExitAnimation(twin1Popup);
                    Invoke("closePopups", 1f);
                    switchButton.GetComponent<Animation>().Play();
                    seq++;
                    break;

                case 3:
                    completed0[seq] = true;
                    doExitAnimation(twin0Popup);
                    Invoke("closePopups", 1f);
                    break;

                default:
                    return;
            }
        }

        public void doExitAnimation(GameObject popup) {
            popup.GetComponent<Animation>().Play("Popup-Exit");
        }

        public void closePopups() {
            twin0Popup.SetActive(false);
            twin1Popup.SetActive(false);
        }

    }
}
