using UnityEngine;
using UnityEngine.Events;

namespace BlackBox
{
    public static class GameEvents
    {
        public static FireRayEvent FireRay = new FireRayEvent();

        public static MarkerEvent MarkUnit = new MarkerEvent();
    }

    public class FireRayEvent : UnityEvent<Vector3Int, Dir> { }

    public class MarkerEvent : UnityEvent<string, Dir, Vector3Int> { }
}
