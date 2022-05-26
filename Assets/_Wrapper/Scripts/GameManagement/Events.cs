using System;

namespace Wrapper
{
    public static class Events
    {
        public static Action<Minigame> OpenMinigame;
        public static Action MinigameClosed;

        public static Func<string, bool> SubmitResearchCode;

        /// Save System ///
        public static Action<string> AddReward;
        public static Func<bool> UpdateRemoteSave;
        public static Func<Game, object> GetMinigameSaveData;
        public static Action<Game, object> UpdateMinigameSaveData;

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
    }
}
