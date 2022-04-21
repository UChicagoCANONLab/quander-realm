using UnityEngine.Events;

namespace Wrapper
{
    public static class Events
    {
        public static UnityEvent<Minigame> OpenMinigame = new UnityEvent<Minigame>();
        public static UnityEvent<string> PrintDialogue = new UnityEvent<string>();
    }
}
