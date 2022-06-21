using System;

namespace Wrapper
{
    public static class Events
    {
        /// Game Management ///
        public static Action<Minigame> OpenMinigame;
        public static Action MinigameClosed;
        public static Action ToggleLoadingScreen;

        /// Save System ///
        public static Action<string> SubmitResearchCode;
        public static Action<LoginStatus> UpdateLoginStatus;
        public static Action<string> AddReward;
        public static Action<Game, object> UpdateMinigameSaveData;
        public static Func<Game, string> GetMinigameSaveData;
        public static Func<bool> UpdateRemoteSave;

        /// Dialogue System ///
        public static Action<string> PrintDialogue;
        public static Action SortSequences;
        public static Action<string> StartDialogueSequence;
        public static Action<Dialogue> OpenDialogueView;
        public static Action<Dialogue, int> UpdateDialogueView;
        public static Action CloseDialogueView;
        public static Action<int> ChangeDialogue;
        public static Action<bool> SwitchNextButton;
        public static Action<bool> TogglePreviousButton;

        /// Reward System ///
        public static Func<string, bool> IsRewardUnlocked;
        public static Action<JournalPage> OpenJournalPage;
        public static Action<string> FeatureCard;
        public static Action ResetPageNumbers;

        /// Debug ///
        public static Action<string> BBGotoLevel;
        public static Action BBToggleDebug;
        public static Action BBClearMarkers;
    }
}
