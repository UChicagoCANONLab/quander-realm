using System;
using UnityEngine;
using UnityEngine.Events;

namespace BlackBox
{
    public static class BlackBoxEvents
    {
        public static UnityEvent ToggleDebug = new UnityEvent();

        public static Action<Vector3Int, Dir> FireRay;
        public static Action<string, Dir, Vector3Int, bool, bool> MarkUnits;

        public static Action<Vector3Int, bool> ToggleFlag;
        public static Action<GameObject> ReturnLanternHome;

        public static Action DecrementEnergy;
        public static Action IndicateEmptyMeter;
        public static Action<int> InitEnergyBar;

        public static Action CheckWinState;
    }
}
