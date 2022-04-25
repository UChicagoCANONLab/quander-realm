using System;

namespace Wrapper
{
    public static class Events
    {
        public static Action SortSequences;
        public static Action<Minigame> OpenMinigame;

        public static Action<string> PrintDialogue;
        public static Action<string> StartDialogueSequence;
        public static Action<Dialogue> OpenDialogueView;
        public static Action CloseDialogueView;
        public static Action<Dialogue> UpdateDialogueView;
        public static Action<int> ChangeDialogue;
    }
}
