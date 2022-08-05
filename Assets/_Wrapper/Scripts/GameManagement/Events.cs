using System;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public static class Events
    {
        /// Game Management ///
        public static Action<Minigame> OpenMinigame;
        public static Action MinigameClosed;
        public static Action ToggleLoadingScreen;
        public static Action OpenLoginScreen;

        /// Save System ///
        public static Action<string> SubmitResearchCode;
        public static Action<LoginStatus> UpdateLoginStatus;
        public static Action<Game, object> UpdateMinigameSaveData;
        public static Func<Game, string> GetMinigameSaveData;
        public static Func<bool> UpdateRemoteSave;
        public static Action ClearSaveFile;

        /// Dialogue System ///
        public static Action<string> PrintDialogue;
        public static Action SortSequences;
        public static Action<string> StartDialogueSequence;
        public static Action<Dialogue> OpenDialogueView;
        public static Action<Dialogue, int> UpdateDialogueView;
        public static Action CloseDialogueView;
        public static Action DialogueSequenceEnded;
        public static Action<int> ChangeDialogue;
        public static Action<bool> SwitchNextButton;
        public static Action<bool> TogglePreviousButton;

        /// Reward System ///
        public static Func<string, bool> AddReward;
        public static Func<string, bool> IsRewardUnlocked;
        public static Action<JournalPage> OpenJournalPage;
        public static Action<string> FeatureCard;
        public static Action ResetPageNumbers;
        public static Func<int, Toggle> GetNavDot;
        public static Action<JournalPage> SwitchPage;
        public static Action UnselectAllCards;
        public static Action<JournalPage> UpdateTab;
        public static Action<Game, int> CollectAndDisplayReward;
        public static Func<RewardAsset, GameObject, DisplayType, GameObject> CreatRewardCard;

        /// Audio System ///
        public static Action<string> PlayMusic;
        public static Action StopMusic;
        public static Action<string> PlaySound;

        /// Debug ///
        public static Action<string> BBGotoLevel;
        public static Action BBToggleDebug;
        public static Action BBClearMarkers;
        public static Action<string> ShowCardPopup;
    }
}
