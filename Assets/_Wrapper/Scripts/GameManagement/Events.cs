using System;

namespace Wrapper
{
    public static class Events
    {
        public static Action<Minigame> OpenMinigame;

        public static Func<string, bool> SubmitResearchCode;

        /// Dialogue System ///
        public static Action<string> PrintDialogue;
        public static Action SortSequences;
        public static Action<string> StartDialogueSequence;
        public static Action<Dialogue> OpenDialogueView;
        public static Action CloseDialogueView;
        public static Action<Dialogue, int> UpdateDialogueView;
        public static Action<int> ChangeDialogue;
        public static Action<bool> TogglePreviousButton;
        public static Action<bool> SwitchNextButton;
    }
}
