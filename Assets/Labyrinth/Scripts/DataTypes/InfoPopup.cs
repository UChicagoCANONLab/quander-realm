using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth {
    public class InfoPopup : MonoBehaviour
    {
        public GameObject[] infoPopups;


        private void OnEnable() 
        {
            TTEvents.ShowInfoMessage += showInfoMessage;
        }
        private void OnDisable() 
        {
            TTEvents.ShowInfoMessage -= showInfoMessage;
        }
        

        public void showInfoMessage() {
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
            this.gameObject.SetActive(true);
            }
        }

        public void closeInfoMessage() {
            this.gameObject.GetComponent<Animation>().Play("Popup-Exit");
            Invoke("helperExit", 2f);
        }

        public void helperExit() {
            this.gameObject.SetActive(false);
            foreach (GameObject obj in infoPopups) {
                obj.SetActive(false);
            }
        }
    }
}