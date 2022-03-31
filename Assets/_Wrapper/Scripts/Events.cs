using UnityEngine.Events;

namespace Wrapper
{
    public static class Events
    {
        public static UnityEvent<Minigame> OpenMinigame = new UnityEvent<Minigame>();
    }
}
