using System;
using UnityEngine;
using UnityEngine.Events;

namespace BlackBox
{
    public static class BBEvents
    {
        /// Debug
        public static UnityEvent ToggleDebug = new UnityEvent();

        /// Ray and Markers
        public static Action<Vector3Int, Dir> FireRay;
        public static Action<string, Dir, Vector3Int> MarkUnits;
        public static Action<Dir, Vector3Int, Dir, Vector3Int> MarkDetourUnits;
        public static Action<string, Dir, Vector3Int> ToggleLinkedHighlight;

        /// Flags/Lanterns
        public static Action<Vector3Int, bool> ToggleFlag;
        public static Action<GameObject> ReturnLanternHome;
        public static Action<bool> ToggleLanternHeld;
        public static Func<Transform> GetFrontMount;

        /// Energy Bar
        public static Action DecrementEnergy;
        public static Action IndicateEmptyMeter;
        public static Action<int> InitEnergyBar;

        /// Level Submission
        public static Action CheckWinState;
        public static Action<int> UpdateHUDWolfieLives;
        public static Action<WinState> UpdateEndPanel;
        public static Action StartNextLevel;
    }
}
