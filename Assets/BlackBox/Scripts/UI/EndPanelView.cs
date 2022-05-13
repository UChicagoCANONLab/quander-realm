using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class EndPanelView : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI header = null;
        [SerializeField] private TextMeshProUGUI subHeader = null;

        [Header("Image GameObjects")]
        [SerializeField] private GameObject winImage = null;
        [SerializeField] private GameObject notYetImage = null;
        [SerializeField] private GameObject loseImage = null;

        [Header("Button GameObjects")]
        [SerializeField] private GameObject restartLevelGO = null;
        [SerializeField] private GameObject quitGO = null;
        [SerializeField] private GameObject keepPlayingGO = null;
        [SerializeField] private GameObject nextLevelGO = null;

        private Animator animator = null;

        private void Awake()
        {
            SetupButtonFunctionality();
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            BBEvents.UpdateEndPanel += UpdatePanel;
        }

        private void OnDisable()
        {
            BBEvents.UpdateEndPanel -= UpdatePanel;
        }

        #region Buttons

        private void SetupButtonFunctionality()
        {
            restartLevelGO.GetComponent<Button>().onClick.AddListener(() => animator.SetBool("On", false));
            quitGO.GetComponent<Button>().onClick.AddListener(() => animator.SetBool("On", false));
            keepPlayingGO.GetComponent<Button>().onClick.AddListener(() => animator.SetBool("On", false));
            nextLevelGO.GetComponent<Button>().onClick.AddListener(StartNextLevel);
        }

        private void StartNextLevel()
        {
            animator.SetBool("On", false);
            BBEvents.StartNextLevel?.Invoke();
        }

        #endregion

        #region Update Panel

        private void UpdatePanel(WinState winState)
        {
            DisableAllElements();
            animator.SetBool("On", true);

            if (winState.levelWon)
                SetInfo(winImage, headerText: "We did it!", subHeaderText: "We found all of the items!", nextLevelGO);

            else
            {
                animator.SetInteger("Lives", winState.livesRemaining);

                if (winState.livesRemaining > 0)
                {
                    SetInfo(notYetImage, headerText: "Not Quite…",
                        subHeaderText: "We found " + winState.numCorrect + " out of " + winState.numNodes + " items",
                        keepPlayingGO);
                }
                else
                    SetInfo(loseImage, headerText: "Game Over", subHeaderText: "You ran out of lives"); 
            }
        }

        private void DisableAllElements()
        {
            restartLevelGO.SetActive(false);
            quitGO.SetActive(false);
            keepPlayingGO.SetActive(false);
            nextLevelGO.SetActive(false);

            winImage.SetActive(false);
            notYetImage.SetActive(false);
            loseImage.SetActive(false);
        }

        private void SetInfo(GameObject imageGO, string headerText, string subHeaderText, GameObject buttonGO = null)
        {
            imageGO.SetActive(true);
            header.text = headerText;
            subHeader.text = subHeaderText;

            if (buttonGO != null)
                buttonGO.SetActive(true);
        }

        #endregion
    }
}
