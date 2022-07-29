
namespace Wrapper
{
    public class MinigameButton : QButton
    {
        public Minigame minigame;

        protected override void OnClickedHandler()
        {
            base.OnClickedHandler();

            Events.OpenMinigame?.Invoke(minigame);
        }
    }
}
