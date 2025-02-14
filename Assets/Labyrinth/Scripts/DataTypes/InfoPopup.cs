using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth {
    public class InfoPopup : MonoBehaviour
    {
        public GameObject[] infoPopups;
        public Animator animator;


        private void OnEnable() 
        {
            TTEvents.ShowInfoMessage += showInfoMessage;
        }
        private void OnDisable() 
        {
            TTEvents.ShowInfoMessage -= showInfoMessage;
        }
        

        public void showInfoMessage() {
            animator.SetBool("IsOn", true);
            int deg = SaveData.Instance.Degree;
            switch(deg) {
                case 0:
                    infoPopups[0].SetActive(true);
                    break;
                case 180:
                    infoPopups[1].SetActive(true);
                    break;
                case 90:
                    infoPopups[2].SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void closeInfoMessage() {
            animator.SetBool("IsOn", false);
            Invoke("helperExit", 2f);
        }

        public void helperExit() {
            foreach (GameObject obj in infoPopups) {
                obj.SetActive(false);
            }
        }
    }
}