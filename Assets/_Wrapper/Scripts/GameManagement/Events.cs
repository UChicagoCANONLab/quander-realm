using System;

namespace Wrapper
{
    public static class Events
    {
        public static Action SortSequences;
        public static Action<Minigame> OpenMinigame;
        public static Action<string> PrintDialogue;
    }
}
