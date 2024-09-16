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
                "There is not a tombstone on this path (or the path next to it!)",
                "Batty bumped into a tombstone! There must be one on this path",
                "If there's two tombstones diagonally, Batty will turn around"
        }; */

        [Header("Image GameObjects")]
        [SerializeField] private GameObject imageContainer = null;
        [SerializeField] private Sprite[] contextImages = null;

        [Header("Button GameObjects")]
        [SerializeField] private GameObject buttonContainer = null;

        private Animator animator = null;
        private int level;

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
            buttonContainer.SetActive(true);

            header.text = "Info";
            headerIcon.GetComponent<Image>().sprite = iconImages[0];
            subHeader.text = subHeaderTexts[0];
        }



        public void TogglePanel(bool isOn)
        {
            animator.SetBool("On", isOn);
        }

    }
}
