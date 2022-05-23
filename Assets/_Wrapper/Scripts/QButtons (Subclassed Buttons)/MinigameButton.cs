
namespace Wrapper
{
    public class MinigameButton : QButton
    {
        public Minigame minigame;

        protected override void OnClickedHandler()
        {
            Events.OpenMinigame?.Invoke(minigame);
        }
    }
}
