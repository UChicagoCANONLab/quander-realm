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
        [SerializeField] private GameObject headerIcon = null;
        [SerializeField] private TextMeshProUGUI subHeader = null;
        
        [SerializeField] private Sprite[] iconImages = null;
        [SerializeField] private string[] subHeaderTexts = null;
        /* {    "Click on a path icon to review the meaning",
                "A tombstone is diagonal from the point where Batty turned",
                "There are no tombstones on this path or the one next to it!",
                "Batty bumped into a tombstone! There must be one on this path (not sure where)",
                "If there's two tombstones diagonally, Batty will turn around (not sure where)"
        }; */
        [SerializeField] private string[] tutorialTexts = null;
        /* {    "Wolfie needs to find the tombstones, and Molly is helping in bat form",
                "Molly can't talk when she's a bat, so she uses these symbols to give us hints",
                "When you think you know where a tombstone is, mark it with a lantern"
                "Find all the tombstones before you run out of energy or stars!"
        }; */

        [Header("Image GameObjects")]
        [SerializeField] private GameObject imageContainer = null;
        [SerializeField] private GameObject tutorialContainer = null;
        [SerializeField] private Sprite[] contextImages = null;
        [SerializeField] private Sprite[] tutorialImages = null;

        [Header("Button GameObjects")]
        [SerializeField] private GameObject buttonContainer = null;
        

        private Animator animator = null;
        private int level;
        private int tutorialSeq = 0;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }


        public void SelectIcon(string markerString) {
            // Marker markerEnum = (Marker)marker;
            Marker marker = (Marker)Enum.Parse(typeof(Marker), markerString);

            buttonContainer.SetActive(false);
            imageContainer.GetComponent<Image>().sprite = contextImages[(int)marker];
            imageContainer.SetActive(true);

            header.text = markerString;
            headerIcon.GetComponent<Image>().sprite = iconImages[(int)marker];
            subHeader.text = subHeaderTexts[(int)marker];

        }

        public void backButton() {
            imageContainer.SetActive(false);
            tutorialContainer.SetActive(false);
            buttonContainer.SetActive(true);

            header.text = "Info";
            headerIcon.GetComponent<Image>().sprite = iconImages[0];
            subHeader.text = subHeaderTexts[0];
        }


        public void TogglePanel(bool isOn)
        {
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
