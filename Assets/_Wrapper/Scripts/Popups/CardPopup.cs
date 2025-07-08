using System.Collections;
using UnityEngine;

namespace Wrapper
{
    public class CardPopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject cardContainer;
        // [SerializeField] private QButton okButton;
        [SerializeField] private QButton nextButton;
        [SerializeField] private QButton backButton;
        [SerializeField] private QButton backgroundButton;
        private Reward currCard;

        private void Awake()
        {
            // okButton.onClick.AddListener(() => ToggleDisplay(false));
            backgroundButton.onClick.AddListener(() => ToggleDisplay(false));
            backButton.onClick.AddListener(() => NavigatePage(true));
        }

        public IEnumerator DisplayCard(GameObject rewardGO)
        {
            SetNavigationButtons(true);
            Events.PlaySound?.Invoke("W_Reward");

            rewardGO.transform.SetParent(cardContainer.transform);
            // rewardGO.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);

            currCard = rewardGO.GetComponent<Reward>();
            RewardSaveManager.Instance.UpdateCardData(currCard, "Initial card popup"); // Log when first unlocked in popup

            ToggleDisplay(true);

            while (!(rewardGO.activeInHierarchy))
                yield return null;
            rewardGO.GetComponent<Animator>().SetBool("Disabled", false);
        }

        private void SetNavigationButtons(bool first)
        {
            backButton.interactable = !first;
            nextButton.onClick.RemoveAllListeners();

            if (first) // if on first page
            {
                nextButton.onClick.AddListener(() => NavigatePage(false));
            }
            else // if on second page
            {
                nextButton.onClick.AddListener(() => ToggleDisplay(false));
            }
        }

        private void NavigatePage(bool first)
        {
            currCard.FlipCard();
            SetNavigationButtons(first);
        }

        /// Helper Methods /// 

        public GameObject GetContainerMount()
        {
            return cardContainer;
        }

        private void ToggleDisplay(bool IsOn)
        {
            animator.SetBool("PopupOn", IsOn);
            if (!IsOn) { RewardSaveManager.Instance.UpdateCardData(currCard, "Exiting popup"); }
        }

    }
}
