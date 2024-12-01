using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;


namespace Qupcakery
{
    public class QCTutorialLevel : MonoBehaviour
    {

        [Header("Sequenced Tutorial Objects")]
        [SerializeField] public GameObject chef;
        [SerializeField] public GameObject textBox;
        [SerializeField] public GameObject textObject;
        [SerializeField] public Text tutorialText;

        private void Start()
        {
            if (GameManagement.Instance.GetCurrentLevelInd() == 1)
            {
                InitiateQCTutorial();
            }
        }

        public void InitiateQCTutorial()
        {
            chef.SetActive(true);
            textBox.SetActive(true);
            textObject.SetActive(true);
            tutorialText.text = "what's going on";
        }
    }
}

