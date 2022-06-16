using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Wrapper
{
    public class Reward : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [Header("Front")]
        [SerializeField] private TextMeshProUGUI cardTypeTextFront;
        [SerializeField] private Image cardTypeColorStrip;
        [SerializeField] private TextMeshProUGUI titleFront;
        [SerializeField] private TextMeshProUGUI flavorText;
        [SerializeField] private Image image;

        [Header("Back")]
        [SerializeField] private TextMeshProUGUI cardTypeBack;
        [SerializeField] private TextMeshProUGUI titleBack;
        [SerializeField] private TextMeshProUGUI backText;

        private string id;
        private const string stateDisabled = "Disabled";
        private const string stateSelected = "Selected";

        public void SetContent(RewardAsset rAsset, Color color)
        {
            string cardTypeDisplayName = GetDisplayName(rAsset.cardType.ToString());
            string titleDisplayName = GetDisplayName(rAsset.title);

            id = rAsset.rewardID;

            cardTypeTextFront.text = cardTypeDisplayName;
            cardTypeColorStrip.color = color;
            titleFront.text = titleDisplayName;
            flavorText.text = rAsset.flavorText;
            SetFrontImage(rAsset);

            cardTypeBack.text = cardTypeDisplayName;
            titleBack.text = titleDisplayName;
            backText.text = rAsset.backText;
        }

        private void SetFrontImage(RewardAsset rAsset)
        {
            Sprite cardSprite = Resources.Load<Sprite>(rAsset.imagePath);
            if (cardSprite == null)
            {
                Debug.LogFormat("Cannot find image at path {0}", rAsset.imagePath);
                return;
            }

            image.sprite = cardSprite;
        }

        public IEnumerator InitAnimationState()
        {
            if (IsUnlocked())
            {
                yield return null;
                animator.SetBool(stateDisabled, false);
            }
        }

        public bool IsUnlocked()
        {
            return Events.IsRewardUnlocked?.Invoke(id) ?? false;
        }

        public void ToggleSelected(bool isSelected)
        {
            if (animator.GetBool(stateDisabled))
            {
                Debug.LogFormat("Cannot 'Select' disabled card: {0}", titleFront.text);
                return;
            }

            animator.SetBool(stateSelected, isSelected);
        }

        private string GetDisplayName(string input)
        {
            return input;
        }
    }
}
