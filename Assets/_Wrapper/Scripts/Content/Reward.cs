using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BeauRoutine;
using System;

namespace Wrapper
{
    public class Reward : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Button button;

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
        private DisplayType displayType;
        private const string stateDisabled = "Disabled";
        private const string stateSelected = "Selected";
        private const string triggerFlip = "Flip";
        
        public Game game;

        private void OnEnable()
        {
            Events.UnselectAllCards += UnselectSelf;
        }

        private void OnDisable()
        {
            Events.UnselectAllCards -= UnselectSelf;
        }

        public void SetContent(RewardAsset rAsset, Color color, DisplayType displayType)
        {
            string cardTypeDisplayName = GetDisplayName(rAsset.cardType.ToString());

            id = rAsset.rewardID;
            game = rAsset.game;
            this.displayType = displayType;

            cardTypeTextFront.text = cardTypeDisplayName;
            cardTypeColorStrip.color = color;
            titleFront.text = rAsset.title;
            flavorText.text = rAsset.flavorText;
            SetFrontImage(rAsset);

            cardTypeBack.text = cardTypeDisplayName;
            titleBack.text = rAsset.title;
            backText.text = rAsset.backText;

            SetupButton();
        }

        private void SetupButton()
        {
            if (displayType == DisplayType.Featured)
                button.onClick.AddListener(() => animator.SetTrigger(triggerFlip));
            else
                button.onClick.AddListener(() => Routine.Start(SelectCard()));
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

        public IEnumerator UpdateAnimationState()
        {
            if (IsUnlocked())
            {
                while (!(this.gameObject.activeInHierarchy))
                    yield return null;

                animator.SetBool(stateDisabled, false);
            }
        }

        public bool IsUnlocked()
        {
            return Events.IsRewardUnlocked?.Invoke(id) ?? false;
        }

        public IEnumerator SelectCard()
        {
            yield return UpdateAnimationState();
            Events.UnselectAllCards?.Invoke();

            animator.SetBool(stateSelected, true);
            Events.FeatureCard?.Invoke(id.Trim().ToLower());
        }

        private void UnselectSelf()
        {
            animator.SetBool(stateSelected, false);
        }

        private string GetDisplayName(string input)
        {
            return input.Replace("_", " ");
        }
    }
}
