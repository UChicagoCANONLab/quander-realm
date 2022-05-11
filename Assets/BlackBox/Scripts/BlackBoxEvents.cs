using System;
using UnityEngine;
using UnityEngine.Events;

namespace BlackBox
{
    public static class BlackBoxEvents
    {
        public static UnityEvent ToggleDebug = new UnityEvent();

        public static Action<Vector3Int, Dir> FireRay;
        public static Action<string, Dir, Vector3Int> MarkUnits;
        public static Action<Dir, Vector3Int, Dir, Vector3Int> MarkDetourUnits;
        public static Action<string, Dir, Vector3Int> ToggleLinkedHighlight;

        public static Action<Vector3Int, bool> ToggleFlag;
        public static Action<GameObject> ReturnLanternHome;
        public static Action<bool> ToggleLanternHeld;

        public static Action DecrementEnergy;
        public static Action IndicateEmptyMeter;
        public static Action<int> InitEnergyBar;

        public static Action CheckWinState;
    }
}
