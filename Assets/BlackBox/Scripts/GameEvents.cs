using UnityEngine;
using UnityEngine.Events;

namespace BlackBox
{
    public static class GameEvents
    {
        public static FireRayEvent FireRay = new FireRayEvent();
        public static MarkerEvent MarkUnit = new MarkerEvent();
        public static FlagEvent ToggleFlag = new FlagEvent();
        public static UnityEvent CheckWinState = new UnityEvent();
        public static LanternEvent ReturnToHome = new LanternEvent();
        public static TextEvent SetEndPanelText = new TextEvent();
    }

    public class FireRayEvent : UnityEvent<Vector3Int, Dir> { }
    public class MarkerEvent : UnityEvent<string, Dir, Vector3Int> { }
    public class FlagEvent : UnityEvent<Vector3Int, bool> { }
    public class LanternEvent : UnityEvent<GameObject> { }
    public class TextEvent : UnityEvent<string> { }

}
