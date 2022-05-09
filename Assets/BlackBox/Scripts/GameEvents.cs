using System;
using UnityEngine;
using UnityEngine.Events;

namespace BlackBox
{
    public static class GameEvents
    {
        //todo: switch all events to Actions
        public static FireRayEvent FireRay = new FireRayEvent();
        public static FlagEvent ToggleFlag = new FlagEvent();
        public static LanternEvent ReturnToHome = new LanternEvent();
        public static TextEvent SetEndPanelText = new TextEvent();
        public static UnityEvent CheckWinState = new UnityEvent();
        public static UnityEvent ToggleDebug = new UnityEvent();

        public static Action<string, Dir, Vector3Int, bool, bool> MarkUnits;
        public static Action DecrementEnergy;
        public static Action<int> InitEnergyBar;
    }

    public class FireRayEvent : UnityEvent<Vector3Int, Dir> { }
    public class FlagEvent : UnityEvent<Vector3Int, bool> { }
    public class LanternEvent : UnityEvent<GameObject> { }
    public class TextEvent : UnityEvent<string> { }

}
