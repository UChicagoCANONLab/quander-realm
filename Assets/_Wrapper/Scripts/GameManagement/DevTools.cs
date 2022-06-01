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

        [Command("r", "add Reward to savefile"), UnityEngine.Scripting.Preserve]
        static private void AddReward(string rewardID)
        {
            Events.AddReward?.Invoke(rewardID);
        }
    }
#endif
}