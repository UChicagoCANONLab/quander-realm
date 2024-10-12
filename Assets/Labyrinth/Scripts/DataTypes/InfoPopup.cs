using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth {
    public class InfoPopup : MonoBehaviour
    {
        // public GameObject[] infoPopups;
        public Animator InfoAnimator;


        private void OnEnable() 
        {
            TTEvents.ShowInfoMessage += showInfoMessage;
        }
        private void OnDisable() 
        {
            TTEvents.ShowInfoMessage -= showInfoMessage;
        }
        

        public void showInfoMessage() {
            InfoAnimator.SetBool("IsOn", true);
            InfoAnimator.SetInteger("Degree", SaveData.Instance.Degree);
            /* int deg = SaveData.Instance.Degree;
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
            } */
        }

        public void closeInfoMessage() {
            InfoAnimator.SetBool("IsOn", false);
            InfoAnimator.SetInteger("Degree", -1);
            // Invoke("helperExit", 2f);
        }

        /* public void helperExit() {
            foreach (GameObject obj in infoPopups) {
                obj.SetActive(false);
            }
        } */
    }
}