using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Wrapper
{
    public static class TriviaCooldownHelper
    {
        private const string TIMER_KEY = "TriviaLastPlayedTime";
        private const string PLAYED_KEY = "HasPlayedTriviaRow2"; // New Key
        private const double COOLDOWN_MINUTES = 5.0;

        // --- EXISTING TIMER LOGIC ---
        public static void StartCooldown()
        {
            long binaryTime = DateTime.Now.ToBinary();
            PlayerPrefs.SetString(TIMER_KEY, binaryTime.ToString());

            // NEW: Also mark that we have played at least once
            PlayerPrefs.SetInt(PLAYED_KEY, 1);

            PlayerPrefs.Save();
        }

        public static TimeSpan GetRemainingTime()
        {
            if (!PlayerPrefs.HasKey(TIMER_KEY)) return TimeSpan.Zero;

            string temp = PlayerPrefs.GetString(TIMER_KEY);
            long binaryTime = Convert.ToInt64(temp);
            DateTime lastTime = DateTime.FromBinary(binaryTime);

            TimeSpan remaining = TimeSpan.FromMinutes(COOLDOWN_MINUTES) - (DateTime.Now - lastTime);

            if (remaining.TotalSeconds <= 0) return TimeSpan.Zero;
            return remaining;
        }

        public static bool IsReady()
        {
            return GetRemainingTime() == TimeSpan.Zero;
        }

        // --- NEW HELPER FOR ROW 2 COMPLETION ---
        public static bool HasPlayedTrivia()
        {
            // Returns true if the key exists and is set to 1
            return PlayerPrefs.GetInt(PLAYED_KEY, 0) == 1;
        }
        // Inside TriviaCooldownHelper class...

        /// <summary>
        /// Wipes the timer and the "played" status for a fresh start.
        /// </summary>
        public static void ClearData()
        {
            PlayerPrefs.DeleteKey(TIMER_KEY);
            PlayerPrefs.DeleteKey(PLAYED_KEY);
            PlayerPrefs.Save();
            Debug.Log("Trivia Data Cleared!");
        }
    }

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

        public void Start()
        {
            checkProgress();
        }

        public void TriviaStarted()
        {
            // 1. Tell the static helper to save the time AND the "Played" status
            TriviaCooldownHelper.StartCooldown();

            // 2. Refresh UI immediately
            checkProgress();
        }


        public void checkProgress()
        {
            // --- 1. GET DATA ---
            int currentQupcakery = 0;
            int currentQueuebits = 0;
            int currentCoins = 0;

            if (Events.GetMinigameMaxLevel != null)
            {
                currentQupcakery = Events.GetMinigameMaxLevel.Invoke(Game.Qupcakes);
                currentQueuebits = Events.GetMinigameMaxLevel.Invoke(Game.QueueBits);
            }

            if (Events.GetUserSaveTotalCoins != null)
            {
                currentCoins = Events.GetUserSaveTotalCoins.Invoke();
            }

            // --- 2. ROW 1 LOGIC ---
            int r1_QupGoal = 5;
            int r1_QueueGoal = 3;

            UpdateMinigameUI(qupcakeryBar1, qupcakeryText1, currentQupcakery, r1_QupGoal);
            UpdateMinigameUI(queuebitsBar1, queuebitsText1, currentQueuebits, r1_QueueGoal);

            bool isRow1Complete = (currentQupcakery >= r1_QupGoal) && (currentQueuebits >= r1_QueueGoal);

            SetRowState(
                isUnlocked: true, // Always unlocked
                isComplete: isRow1Complete,
                rewardText: rewardText1,
                buttons: new MinigameButton[] { qupcakery1, queuebits1 }
            );

            // --- 3. ROW 2 LOGIC (Updated) ---
            bool isRow2Unlocked = isRow1Complete;

            // FIX: Check the persistent Helper, not a local variable
            bool isRow2Complete = TriviaCooldownHelper.HasPlayedTrivia();

            SetRowState(
                isUnlocked: isRow2Unlocked,
                isComplete: isRow2Complete,
                rewardText: rewardText2,
                buttons: new MinigameButton[] { trivia2 }
            );

            // If completed, disable the button (assuming you only play Row 2 once?)
            if (isRow2Complete && trivia2 != null)
            {
                trivia2.interactable = false;
            }


            // --- 4. ROW 3 LOGIC ---
            int r3_QupGoal = 11;
            int r3_QueueGoal = 6;

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
            bool isRow4Unlocked = isRow3Complete;
            bool isRow4Complete = currentCoins >= 700;

            SetRowState(
                isUnlocked: isRow4Unlocked,
                isComplete: isRow4Complete,
                rewardText: rewardText4,
                buttons: new MinigameButton[] { twintangle4, trivia4, treasure4 }
            );

            // CRITICAL OVERRIDE: 
            if (!TriviaCooldownHelper.IsReady())
            {
                if (trivia4 != null) trivia4.interactable = false;
            }
        }

        // --- HELPER FUNCTIONS ---
        private void UpdateMinigameUI(Slider bar, TMP_Text text, int current, int target)
        {
            if (bar != null)
            {
                bar.maxValue = target;
                bar.value = current;
            }
            if (text != null)
            {
                int displayVal = (current > target) ? target : current;
                text.text = $"{displayVal}/{target}";
            }
        }

        private void SetRowState(bool isUnlocked, bool isComplete, TMP_Text rewardText, MinigameButton[] buttons)
        {
            if (rewardText != null)
            {
                if (isComplete) rewardText.color = COLOR_COMPLETED;
                else if (isUnlocked) rewardText.color = COLOR_UNLOCKED;
                else rewardText.color = COLOR_LOCKED;
            }

            foreach (var btn in buttons)
            {
                if (btn != null) btn.interactable = isUnlocked;
            }
        }

        // Inside EventPanelManager class...

        public void ResetEventProgress()
        {
            // 1. Clear the specific trivia data
            Debug.Log("CLEAR!");
            TriviaCooldownHelper.ClearData();

            // 2. Reset the text manually (since the update loop might not catch it instantly)
            if (triviaTimerText != null)
                triviaTimerText.text = "Ready!";

            // 3. Re-run the logic to lock/unlock rows based on the "fresh" state


            // --- 1. GET DATA ---
            int currentQupcakery = 0;
            int currentQueuebits = 0;
            int currentCoins = 0;

            // --- 2. ROW 1 LOGIC ---
            int r1_QupGoal = 5;
            int r1_QueueGoal = 3;

            UpdateMinigameUI(qupcakeryBar1, qupcakeryText1, currentQupcakery, r1_QupGoal);
            UpdateMinigameUI(queuebitsBar1, queuebitsText1, currentQueuebits, r1_QueueGoal);

            bool isRow1Complete = false;

            SetRowState(
                isUnlocked: true, // Always unlocked
                isComplete: isRow1Complete,
                rewardText: rewardText1,
                buttons: new MinigameButton[] { qupcakery1, queuebits1 }
            );

            // --- 3. ROW 2 LOGIC (Updated) ---
            bool isRow2Unlocked = false;

            // FIX: Check the persistent Helper, not a local variable
            bool isRow2Complete = false;

            SetRowState(
                isUnlocked: isRow2Unlocked,
                isComplete: isRow2Complete,
                rewardText: rewardText2,
                buttons: new MinigameButton[] { trivia2 }
            );

            // If completed, disable the button (assuming you only play Row 2 once?)
            // if (isRow2Complete && trivia2 != null)
            // {
                trivia2.interactable = false;
            // }


            // --- 4. ROW 3 LOGIC ---
            int r3_QupGoal = 11;
            int r3_QueueGoal = 6;

            UpdateMinigameUI(qupcakeryBar3, qupcakeryText3, currentQupcakery, r3_QupGoal);
            UpdateMinigameUI(queuebitsBar3, queuebitsText3, currentQueuebits, r3_QueueGoal);

            bool isRow3Unlocked = false;;
            bool isRow3Complete = false;

            SetRowState(
                isUnlocked: isRow3Unlocked,
                isComplete: isRow3Complete,
                rewardText: rewardText3,
                buttons: new MinigameButton[] { qupcakery3, queuebits3 }
            );

            // --- 5. ROW 4 LOGIC ---
            bool isRow4Unlocked = false;
            bool isRow4Complete = false;

            SetRowState(
                isUnlocked: isRow4Unlocked,
                isComplete: isRow4Complete,
                rewardText: rewardText4,
                buttons: new MinigameButton[] { twintangle4, trivia4, treasure4 }
            );

            // CRITICAL OVERRIDE: 
            if (!TriviaCooldownHelper.IsReady())
            {
                if (trivia4 != null) trivia4.interactable = false;
            }

            // checkProgress();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            checkProgress();
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!TriviaCooldownHelper.IsReady())
            {
                System.TimeSpan remaining = TriviaCooldownHelper.GetRemainingTime();
                if (triviaTimerText != null)
                    triviaTimerText.text = $"Ready in {remaining.ToString(@"mm\:ss")}";

                if (trivia4 != null) trivia4.interactable = false;
            }
            else
            {
                if (triviaTimerText != null && triviaTimerText.text != "Ready!")
                {
                    triviaTimerText.text = "Ready!";
                    checkProgress();
                }
            }
        }
    }
}