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

        [Header("Image GameObjects")]
        [SerializeField] private GameObject winImage = null;
        [SerializeField] private GameObject notYetImage = null;
        [SerializeField] private GameObject loseImage = null;
        [SerializeField] private GameObject imageContainer = null;
        [SerializeField] private Sprite[] contextImages = null;

        [Header("Button GameObjects")]
        [SerializeField] private GameObject buttonContainer = null;
        [SerializeField] private GameObject restartLevelGO = null;
        [SerializeField] private GameObject quitGO = null;
        [SerializeField] private GameObject keepPlayingGO = null;
        [SerializeField] private GameObject nextLevelGO = null;

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
