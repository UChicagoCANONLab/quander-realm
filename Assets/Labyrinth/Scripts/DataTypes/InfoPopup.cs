using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Labyrinth {
    public class InfoPopup : MonoBehaviour
    {
        // public GameObject[] infoPopups;
        public Animator InfoAnimator;
        public TMP_Text title;
        public TMP_Text description;

        private Dictionary<int,string> infoText = new Dictionary<int,string> {
            {0,     "Fran and Ken move the same way"},
            {180,    "Fran and Ken move in opposite directions"},
            {90,   "Fran and Ken move one rotation apart (e.g. Fran goes up, Ken goes left)"}
        };


        private void OnEnable() 
        {
            TTEvents.ShowInfoMessage += showInfoMessage;
        }
        private void OnDisable() 
        {
            TTEvents.ShowInfoMessage -= showInfoMessage;
        }
        

        void Start() {
            int degree = SaveData.Instance.Degree;
            title.text = $"{degree}° Rotation:";
            description.text = infoText[degree];
            InfoAnimator.SetInteger("Degree", degree);
        }

        public void showInfoMessage() {
            InfoAnimator.SetBool("IsOn", true);
        }

        public void closeInfoMessage() {
            InfoAnimator.SetBool("IsOn", false);
        }

    }
}