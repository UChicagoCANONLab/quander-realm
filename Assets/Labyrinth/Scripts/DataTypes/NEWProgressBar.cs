using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Labyrinth {
    public class NEWProgressBar : MonoBehaviour
    {
        private int pathLength;
        private int buffer = 9;

        public GameObject parent;
        public GameObject stepPrefab;


        public void initializeBar(int length) {
            parent.GetComponent<HorizontalLayoutGroup>().enabled = true;

            pathLength = (int)length;

            for (int i=0; i<(pathLength+buffer); i++) {
                GameObject currStep = Instantiate(stepPrefab, parent.transform);

                if (i==0 || i==4 || i==6) { 
                    currStep.GetComponent<Animator>().SetBool("Star", true);
                }

            }

            /* for (int i=0; i<pathLength; i++) {
                GameObject.Find($"{path}/Steps/Step{i}").SetActive(true);
            } */
            
            Invoke("horizOff", 1);
        }

        private void horizOff() {
            parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }

        public void resetBar() {
            /* for (int i=0; i<moveTiles.Length; i++) {
                GameObject.Find($"{path}/Steps/{moveTiles[i]}").SetActive(true);
            }
            for (int j=0; j<20; j++) {
                GameObject.Find($"{path}/Steps/Step{j}").SetActive(false);
            } */

            foreach(Transform step in parent.transform) {
                Destroy(step.gameObject);
            }
        }


        public void detractBar(int currStep) {   
            if (currStep == 0) { return; }
            
            int i = (pathLength+buffer) - currStep;
            if (i < 0) { 
                return; 
            } else {
                parent.transform.GetChild(i).gameObject.GetComponent<Animator>().SetTrigger("Disable");
            }

            /* if (currStep <= pathLength) {
                GameObject.Find($"{path}/Steps/Step{pathLength-currStep}").SetActive(false);
            }
            else {
                if ((currStep - pathLength) > 9) {
                    return;
                }
                GameObject.Find($"{path}/Steps/{moveTiles[currStep-pathLength-1]}").SetActive(false);
            } */
        }
    
    }
}