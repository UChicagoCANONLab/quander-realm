using UnityEngine.EventSystems;

namespace Wrapper
{
    public class MinigameButton : QButton
    {
        public Minigame minigame;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            Events.OpenMinigame?.Invoke(minigame);
        }
    }
}
