using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Circuits
{
    public class ReminderPopup : MonoBehaviour
    {
        public enum HighlightTarget
        {
            None,
            HintButton,
            InfoButton,
            StarButton
        }

        [Serializable]
        public class ReminderStep
        {
            [TextArea(2, 5)]
            public string text;
            public HighlightTarget highlight = HighlightTarget.None;
            public bool showNextButton = true;
            public string message = "";
        }

        [Serializable]
        public class LevelReminderSequence
        {
            public int level;
            public ReminderStep[] steps;
        }

        public GameObject hintButton;
        public GameObject infoButton;
        public GameObject starButton;

        public GameBehavior gameBehavior;

        public GameObject nextButton;

        public Animator animator;
        public TextMeshProUGUI description;

        [Header("Per-level reminder sequences")]
        public LevelReminderSequence[] levelSequences;

        private Animator hintButtonAnimator;
        private Animator infoButtonAnimator;
        private Animator starButtonAnimator;

        private LevelReminderSequence activeSequence;
        private bool reminderGiven = false;
        private int seq = 0;

        private const string POPUP_BOOL = "IsOn";
        private const string HINT_TRIGGER = "Hint";
        private const string NORMAL_TRIGGER = "Normal";

        private void Awake()
        {
            if (hintButton != null)
            {
                hintButtonAnimator = hintButton.GetComponent<Animator>();
            }

            if (infoButton != null)
            {
                infoButtonAnimator = infoButton.GetComponent<Animator>();
            }

            if (starButton != null)
            {
                starButtonAnimator = starButton.GetComponent<Animator>();
            }
        }

        private void Start()
        {
            int currLevel = GameData.getCurrLevel();
            activeSequence = GetSequenceForLevel(currLevel);

            if (activeSequence != null && activeSequence.steps != null && activeSequence.steps.Length > 0)
            {
                ShowReminder();
            }
        }

        private LevelReminderSequence GetSequenceForLevel(int level)
        {
            if (levelSequences == null)
            {
                return null;
            }

            foreach (var sequence in levelSequences)
            {
                if (sequence != null && sequence.level == level)
                {
                    return sequence;
                }
            }

            return null;
        }

        public void ShowReminder()
        {
            if (reminderGiven || activeSequence == null || activeSequence.steps == null)
            {
                return;
            }

            if (seq >= activeSequence.steps.Length)
            {
                EndReminder();
                return;
            }

            ReminderStep step = activeSequence.steps[seq];

            if (description != null)
            {
                description.text = step.text;
            }

            if (animator != null)
            {
                animator.SetBool(POPUP_BOOL, true);
            }

            if (nextButton != null)
            {
                nextButton.SetActive(step.showNextButton);
            }

            SetButtonHighlights(step.highlight);
        }

        public void Next()
        {
            if (reminderGiven)
            {
                return;
            }

            if (activeSequence != null && activeSequence.steps != null && seq < activeSequence.steps.Length)
            {
                ReminderStep currentStep = activeSequence.steps[seq];

                if (!string.IsNullOrEmpty(currentStep.message) && gameBehavior != null)
                {
                    gameBehavior.getMessageFromReminder(currentStep.message);
                }
            }

            seq++;
            ShowReminder();
        }

        public void SubstituteNext()
        {
            if (reminderGiven)
            {
                return;
            }

            seq++;
            ShowReminder();
        }

        public void EndReminder()
        {
            reminderGiven = true;
            ClearHighlights();

            if (nextButton != null)
            {
                nextButton.SetActive(true);
            }

            if (animator != null)
            {
                animator.SetBool(POPUP_BOOL, false);
            }
        }

        private void SetButtonHighlights(HighlightTarget highlight)
        {
            switch (highlight)
            {
                case HighlightTarget.HintButton:
                    SetHintHighlighted(true);
                    SetInfoHighlighted(false);
                    SetStarHighlighted(false);
                    break;

                case HighlightTarget.InfoButton:
                    SetHintHighlighted(false);
                    SetInfoHighlighted(true);
                    SetStarHighlighted(false);
                    break;

                case HighlightTarget.StarButton:
                    SetHintHighlighted(false);
                    SetInfoHighlighted(false);
                    SetStarHighlighted(true);
                    break;

                default:
                    SetHintHighlighted(false);
                    SetInfoHighlighted(false);
                    SetStarHighlighted(false);
                    break;
            }
        }

        private void ClearHighlights()
        {
            SetHintHighlighted(false);
            SetInfoHighlighted(false);
            SetStarHighlighted(false);
        }

        private void SetHintHighlighted(bool highlighted)
        {
            if (hintButtonAnimator == null)
            {
                return;
            }

            hintButtonAnimator.SetTrigger(highlighted ? HINT_TRIGGER : NORMAL_TRIGGER);
        }

        private void SetInfoHighlighted(bool highlighted)
        {
            if (infoButtonAnimator == null)
            {
                return;
            }

            infoButtonAnimator.SetTrigger(highlighted ? HINT_TRIGGER : NORMAL_TRIGGER);
        }

        private void SetStarHighlighted(bool highlighted)
        {
            if (starButtonAnimator == null)
            {
                return;
            }

            starButtonAnimator.SetTrigger(highlighted ? HINT_TRIGGER : NORMAL_TRIGGER);
        }
    }
}