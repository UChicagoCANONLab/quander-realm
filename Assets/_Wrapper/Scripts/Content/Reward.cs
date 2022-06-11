using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class Reward : MonoBehaviour
    {
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
        private Animator animator;
        private const string stateDisabled = "Disabled";
        private const string stateSelected = "Selected";

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
            //animator.SetBool(stateDisabled, false);
        }

        public void SetContent(RewardAsset rAsset, Color color)
        {
            string cardTypeDisplayName = ObjectNames.NicifyVariableName(rAsset.cardType.ToString());
            string titleDisplayName = ObjectNames.NicifyVariableName(rAsset.title);

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

        public void ToggleSelected(bool isSelected)
        {
            if (animator.GetBool(stateDisabled))
                return;

            animator.SetBool(stateSelected, isSelected);
        }
    }
}
