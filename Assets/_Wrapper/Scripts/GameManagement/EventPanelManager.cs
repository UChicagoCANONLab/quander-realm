using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wrapper
{
    public class EventPanelManager : MonoBehaviour
    {
        [Header("--- ROW 1 (Qupcakery & Queuebits) ---")]
        [SerializeField] private MinigameButton qupcakery1;
        [SerializeField] private Slider qupcakeryBar1;
        [SerializeField] private TMP_Text qupcakeryText1;

        [Space(5)]
        [SerializeField] private MinigameButton queuebits1;
        [SerializeField] private Slider queuebitsBar1;
        [SerializeField] private TMP_Text queuebitsText1;
        [SerializeField] private TMP_Text rewardText1;

        [Header("--- ROW 2 (Trivia) ---")]
        [SerializeField] private MinigameButton trivia2;
        [SerializeField] private TMP_Text rewardText2;

        [Header("--- ROW 3 (Qupcakery & Queuebits Return) ---")]
        [SerializeField] private MinigameButton qupcakery3;
        [SerializeField] private Slider qupcakeryBar3;
        [SerializeField] private TMP_Text qupcakeryText3;
        
        [Space(5)]
        [SerializeField] private MinigameButton queuebits3;
        [SerializeField] private Slider queuebitsBar3;
        [SerializeField] private TMP_Text queuebitsText3;
        [SerializeField] private TMP_Text rewardText3;

        [Header("--- ROW 4 (Twintangle, Trivia, Treasure) ---")]
        [SerializeField] private MinigameButton twintangle4;
        [SerializeField] private MinigameButton trivia4;
        [SerializeField] private MinigameButton treasure4;
        [SerializeField] private TMP_Text triviaTimerText;
        [SerializeField] private TMP_Text rewardText4;

        // Constants for Colors
        private readonly Color COLOR_LOCKED = new Color32(130, 130, 130, 255); // #828282
        private readonly Color COLOR_UNLOCKED = Color.white;                   // #FFFFFF
        private readonly Color COLOR_COMPLETED = Color.green;                  // Lime Green

        // State Variables
        private System.DateTime lastTriviaTime;
        private bool isTriviaCooldownActive = false;
        private const double COOLDOWN_MINUTES = 5.0;
        private bool hasPlayedTrivia = false; // Tracks if Row 2 is complete

        public void Start()
        {
            // Load saved trivia state if needed (optional)
            // if (PlayerPrefs.HasKey("HasPlayedTrivia")) hasPlayedTrivia = true;
            
            checkProgress();
        }

        public void TriviaStarted()
        {
            lastTriviaTime = System.DateTime.Now;
            isTriviaCooldownActive = true;
            hasPlayedTrivia = true;

            // Optional: Save this state so they don't lose progress on restart
            // PlayerPrefs.SetInt("HasPlayedTrivia", 1);

            // Disable button immediately
            if (trivia4 != null) trivia4.interactable = false;
            
            // Re-check progress to instantly update UI (turn Row 2 green, unlock Row 3)
            checkProgress();
        }

        public void checkProgress()
        {
            // --- 1. GET DATA ---
            // Get current levels (default to 0 if null/error)
            int currentQupcakery = 0;
            int currentQueuebits = 0;
            int currentCoins = 0;

            if (Events.GetMinigameMaxLevel != null) {
                currentQupcakery = Events.GetMinigameMaxLevel.Invoke(Game.Qupcakes);
                currentQueuebits = Events.GetMinigameMaxLevel.Invoke(Game.QueueBits);
            }
            
            if (Events.GetUserSaveTotalCoins != null) {
                currentCoins = Events.GetUserSaveTotalCoins.Invoke();
            }

            // --- 2. ROW 1 LOGIC ---
            // Goals: Qupcakery Level 5, Queuebits Level 3
            int r1_QupGoal = 5;
            int r1_QueueGoal = 3;

            // Update UI for Row 1
            UpdateMinigameUI(qupcakeryBar1, qupcakeryText1, currentQupcakery, r1_QupGoal);
            UpdateMinigameUI(queuebitsBar1, queuebitsText1, currentQueuebits, r1_QueueGoal);

            bool isRow1Complete = (currentQupcakery >= r1_QupGoal) && (currentQueuebits >= r1_QueueGoal);
            
            // Row 1 is ALWAYS Unlocked
            SetRowState(
                isUnlocked: true, 
                isComplete: isRow1Complete, 
                rewardText: rewardText1, 
                buttons: new MinigameButton[] { qupcakery1, queuebits1 }
            );

            // --- 3. ROW 2 LOGIC ---
            // Goal: Play Trivia once
            // Unlock Condition: Row 1 Complete
            bool isRow2Unlocked = isRow1Complete;
            bool isRow2Complete = hasPlayedTrivia;

            SetRowState(
                isUnlocked: isRow2Unlocked, 
                isComplete: isRow2Complete, 
                rewardText: rewardText2, 
                buttons: new MinigameButton[] { trivia2 }
            );
            if (isRow2Complete){
                trivia2.interactable = false;
            }


            // --- 4. ROW 3 LOGIC ---
            // Goals: Qupcakery Level 11, Queuebits Level 6
            // Unlock Condition: Row 2 Complete
            int r3_QupGoal = 11;
            int r3_QueueGoal = 6;
            
            // Update UI for Row 3
            UpdateMinigameUI(qupcakeryBar3, qupcakeryText3, currentQupcakery, r3_QupGoal);
            UpdateMinigameUI(queuebitsBar3, queuebitsText3, currentQueuebits, r3_QueueGoal);

            bool isRow3Unlocked = isRow2Complete;
            bool isRow3Complete = (currentQupcakery >= r3_QupGoal) && (currentQueuebits >= r3_QueueGoal);

            SetRowState(
                isUnlocked: isRow3Unlocked, 
                isComplete: isRow3Complete, 
                rewardText: rewardText3, 
                buttons: new MinigameButton[] { qupcakery3, queuebits3 }
            );

            // --- 5. ROW 4 LOGIC ---
            // Goal: 400 Coins
            // Unlock Condition: Row 3 Complete
            bool isRow4Unlocked = isRow3Complete;
            bool isRow4Complete = currentCoins >= 400;
            
            SetRowState(
                isUnlocked: isRow4Unlocked, 
                isComplete: isRow4Complete, 
                rewardText: rewardText4, 
                buttons: new MinigameButton[] { twintangle4, trivia4, treasure4 }
            );
        }

        // --- HELPER FUNCTIONS ---

        // Updates a slider and text (e.g., "3/5")
        private void UpdateMinigameUI(Slider bar, TMP_Text text, int current, int target)
        {
            if (bar != null)
            {
                bar.maxValue = target;
                bar.value = current;
            }
            if (text != null)
            {
                // Cap the display at max (so it doesn't show "6/5")
                int displayVal = (current > target) ? target : current;
                text.text = $"{displayVal}/{target}";
            }
        }

        // Handles coloring and interactability
        private void SetRowState(bool isUnlocked, bool isComplete, TMP_Text rewardText, MinigameButton[] buttons)
        {
            // 1. Set Text Color
            if (rewardText != null)
            {
                if (isComplete)
                    rewardText.color = COLOR_COMPLETED;
                else if (isUnlocked)
                    rewardText.color = COLOR_UNLOCKED;
                else
                    rewardText.color = COLOR_LOCKED;
            }

            // 2. Set Button Interactability
            // Buttons are interactable only if the row is unlocked
            foreach (var btn in buttons)
            {
                if (btn != null)
                {
                    // If you have a specific interactable property on MinigameButton, use that.
                    // Assuming it inherits from Button or has an Interactable bool:
                    btn.interactable = isUnlocked; 
                }
            }
        }

        private void Update()
        {
            if (isTriviaCooldownActive)
            {
                System.TimeSpan timeSinceStart = System.DateTime.Now - lastTriviaTime;
                System.TimeSpan cooldownDuration = System.TimeSpan.FromMinutes(COOLDOWN_MINUTES);
                System.TimeSpan timeRemaining = cooldownDuration - timeSinceStart;

                if (timeRemaining.TotalSeconds <= 0)
                {
                    triviaTimerText.text = "Ready!";
                    isTriviaCooldownActive = false;
                    // Only re-enable if the row is actually unlocked!
                    // Calling checkProgress again is safest way to ensure rules are kept
                    checkProgress(); 
                }
                else
                {
                    triviaTimerText.text = $"Ready in {timeRemaining.ToString(@"mm\:ss")}";
                }
            }
        }
    }
}