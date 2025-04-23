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
        private int counter = 0;
        private Reward currCard;

        private void Awake()
        {
            // okButton.onClick.AddListener(() => ToggleDisplay(false));
            backgroundButton.onClick.AddListener(() => ToggleDisplay(false));
            backButton.interactable = false;
            nextButton.onClick.AddListener(() => FlipCard());
        }

        public IEnumerator DisplayCard(GameObject rewardGO)
        {
            Events.PlaySound?.Invoke("W_Reward");
            rewardGO.transform.SetParent(cardContainer.transform);
            // rewardGO.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
            currCard = rewardGO.GetComponent<Reward>();

            ToggleDisplay(true);

            while (!(rewardGO.activeInHierarchy))
                yield return null;
            rewardGO.GetComponent<Animator>().SetBool("Disabled", false);
        }

        public GameObject GetContainerMount()
        {
            return cardContainer;
        }

        private void ToggleDisplay(bool isOn)
        {
            animator.SetBool("PopupOn", isOn);
        }

        private void FlipCard()
        {
            currCard.animator.SetTrigger("Flip");
            if (counter == 0)
            {
                backButton.onClick.AddListener(() => FlipCard());
                backButton.interactable = true;
                nextButton.onClick.AddListener(() => ToggleDisplay(false));
                counter++;
            } 
            else if (counter == 1)
            {
                backButton.interactable = false;
                nextButton.onClick.AddListener(() => FlipCard());
                nextButton.onClick.RemoveListener(() => ToggleDisplay(false));
                counter--;
            }
        }
    }
}
