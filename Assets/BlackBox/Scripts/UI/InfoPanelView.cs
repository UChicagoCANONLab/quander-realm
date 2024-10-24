using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class InfoPanelView : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI header = null;
        [SerializeField] private TextMeshProUGUI subHeader = null;
        
        [SerializeField] private string[] subHeaderTexts = null;
        /* {    "Click on a path icon to review the meaning",
                "A tombstone is diagonal from the point where Batty turned",
                "There are no tombstones on this path or the one next to it!",
                "Batty bumped into a tombstone! There must be one on this path (not sure where)",
                "If there's two tombstones diagonally, Batty will turn around (not sure where)"
        }; */
        [SerializeField] private string[] subHeaderTexts2 = null;
        /* {    "",
                "Batty can turn multiple times if there's multiple headstones on the path!",
                "",
                "Be aware - Batty might run into multiple headstones on the same path!",
                ""
        }; */
        [SerializeField] private string[] tutorialTexts = null;
        /* {    "Wolfie needs to find the tombstones, and Molly is helping in bat form",
                "Molly can't talk when she's a bat, so she uses these symbols to give us hints",
                "When you think you know where a tombstone is, mark it with a lantern"
                "Find all the tombstones before you run out of energy or stars!"
        }; */

        [Header("Image Sprites")]
        [SerializeField] private Sprite[] iconImages = null;
        [SerializeField] private Sprite[] contextImages = null;
        [SerializeField] private Sprite[] contextImages2 = null;
        [SerializeField] private Sprite[] tutorialImages = null;


        [Header("Container GameObjects")]
        [SerializeField] private GameObject headerIcon = null;
        [SerializeField] private GameObject imageContainer = null;
        [SerializeField] private GameObject tutorialContainer = null;
        [SerializeField] private GameObject buttonContainer = null;
        [SerializeField] private GameObject forwardButtonObj = null;

        [Header("Objects Disabled by tutorialNumber")]
        [SerializeField] private GameObject missButton = null;
        [SerializeField] private GameObject reflectButton = null;
        

        private Animator animator = null;
        private int tutorialNumber = 0;
        private int tutorialSeq = 0;
        private int iconSeq = 0;
        private Marker marker;


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void OnEnable() 
        {
            BBEvents.ShowInfo += InitInfo;
        }
        private void OnDisable() 
        {
            BBEvents.ShowInfo -= InitInfo;
        }


        public void InitInfo() {
            animator.SetBool("On", true);
            tutorialNumber = BBEvents.GetLevel.Invoke().tutorialNumber;

            if (tutorialNumber == 0) {
                missButton.SetActive(false);
                reflectButton.SetActive(false);
            } else if (tutorialNumber == 1) {
                missButton.SetActive(true);
                reflectButton.SetActive(false);
            } else {
                missButton.SetActive(true);
                reflectButton.SetActive(true);
            }
        }


        public void SelectIcon(string markerString) {
            iconSeq = 0;
            marker = (Marker)Enum.Parse(typeof(Marker), markerString);

            buttonContainer.SetActive(false);
            imageContainer.GetComponent<Image>().sprite = contextImages[(int)marker];
            imageContainer.SetActive(true);

            header.text = markerString;
            headerIcon.GetComponent<Image>().sprite = iconImages[(int)marker];
            subHeader.text = subHeaderTexts[(int)marker];

            if (markerString == "Detour" && tutorialNumber == 4) {
                forwardButtonObj.SetActive(true);
            } else if (markerString == "Hit" && tutorialNumber >= 3) {
                forwardButtonObj.SetActive(true);
            } else {
                forwardButtonObj.SetActive(false);
            }
        }

        public void backButton() {
            if (iconSeq == 0) {
                imageContainer.SetActive(false);
                tutorialContainer.SetActive(false);
                buttonContainer.SetActive(true);

                header.text = "Info";
                headerIcon.GetComponent<Image>().sprite = iconImages[0];
                subHeader.text = subHeaderTexts[0];
            } else {
                iconSeq--;
                imageContainer.GetComponent<Image>().sprite = contextImages[(int)marker];
                subHeader.text = subHeaderTexts[(int)marker];
                forwardButtonObj.SetActive(true);
            }
            
        }

        public void forwardButton() {
            iconSeq++;

            imageContainer.GetComponent<Image>().sprite = contextImages2[(int)marker];
            subHeader.text = subHeaderTexts2[(int)marker];
            forwardButtonObj.SetActive(false);
        }


        public void TogglePanel(bool isOn)
        {
            tutorialSeq = 0; iconSeq = 0;
            backButton();
            animator.SetBool("On", isOn);
        }



        public void howToPlay() {
            imageContainer.SetActive(false);
            buttonContainer.SetActive(false);
            tutorialContainer.SetActive(true);

            header.text = "Tutorial";
            headerIcon.GetComponent<Image>().sprite = iconImages[0];

            subHeader.text = tutorialTexts[0];
            tutorialContainer.GetComponent<Image>().sprite = tutorialImages[0];
            tutorialSeq = 0;
        }

        public void tutorialNext() {
            tutorialSeq++;

            if (tutorialSeq > 3) {
                tutorialSeq = 0;
                backButton();
            } else {
                tutorialContainer.GetComponent<Image>().sprite = tutorialImages[tutorialSeq];
                subHeader.text = tutorialTexts[tutorialSeq];
            }
        }
        public void tutorialBack() {
            tutorialSeq--;

            if (tutorialSeq < 0) {
                tutorialSeq = 0;
                backButton();
            } else {
                tutorialContainer.GetComponent<Image>().sprite = tutorialImages[tutorialSeq];
                subHeader.text = tutorialTexts[tutorialSeq];
            }
        }

    }
}
