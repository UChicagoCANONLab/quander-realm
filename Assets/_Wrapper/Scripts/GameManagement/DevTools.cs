using Filament.Debugging;
using UnityEngine;

namespace Wrapper
{
    public class DevTools : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        void Start()
        {
            Debug.Log("Enabled Dev Tools");
            DebugConsole.RegisterModule(new SessionDebugModule());
        }
#endif
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public class SessionDebugModule : IConsoleModule
    {
        public bool Paused { get; set; }

        public void OnTimeScaleChanged(float inNewTimeScale) { }

        public void ReportStats(ConsoleStats ioStats) { }
    }

    public static class GlobalCommands
    {
        [Command("dia", "Open the dialogue UI and start the given sequence"), UnityEngine.Scripting.Preserve]
        static private void OpenDialogueSequence(string sequenceID)
        {
            Events.StartDialogueSequence?.Invoke(sequenceID);
        }

        [Command("close dia", "Close dialogue UI"), UnityEngine.Scripting.Preserve]
        static private void CloseDialogueUI()
        {
            Events.CloseDialogueView?.Invoke();
        }

        [Command("lvl", "Go to a specific level"), UnityEngine.Scripting.Preserve]
        static private void GotoLevel(string levelID)
        {
            try
            {
                BlackBox.BBEvents.GotoLevel?.Invoke(levelID);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        [Command("clm", "Clear Markers"), UnityEngine.Scripting.Preserve]
        static private void ClearMarkers()
        {
            try
            {
                BlackBox.BBEvents.ClearMarkers?.Invoke();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        [Command("d", "Toggle BlackBox debug mode"), UnityEngine.Scripting.Preserve]
        static private void ToggleDebug()
        {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
            try
            {
                BlackBox.BBEvents.ToggleDebug?.Invoke();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
#else
            Debug.LogWarning("Command only works for ios and android");
#endif
        }
    }
#endif
}