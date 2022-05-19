using Filament.Debugging;
using UnityEngine;

namespace BlackBox
{
    public class BBDevTools : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public static class BBCommands
        {
            [Command("lvl", "Go to a specific level"), UnityEngine.Scripting.Preserve]
            static private void GotoLevel(string levelID)
            {
                BBEvents.GotoLevel?.Invoke(levelID);
            }

            [Command("clm", "Clear Markers"), UnityEngine.Scripting.Preserve]
            static private void ClearMarkers()
            {
                BBEvents.ClearMarkers?.Invoke();
            }

            [Command("d", "Toggle BlackBox debug mode"), UnityEngine.Scripting.Preserve]
            static private void ToggleDebug()
            {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
                BBEvents.ToggleDebug?.Invoke();
#else
                Debug.LogWarning("Command only works for ios and android");
#endif
            }
        }
#endif
    }
}