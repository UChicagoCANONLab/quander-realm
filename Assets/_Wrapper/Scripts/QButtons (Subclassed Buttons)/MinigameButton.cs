using UnityEngine.EventSystems;

namespace Wrapper
{
    public class MinigameButton : QButton
    {
        public Minigame minigame;
        public bool upgradeMode = false;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (interactable && !upgradeMode) 
            {
                base.OnPointerClick(eventData);

                Events.ScreenFadeMidAction?.Invoke(() => Events.OpenMinigame?.Invoke(minigame), 0.2F);
            }
            else if (interactable && upgradeMode)
            {
                base.OnPointerClick(eventData);
            }
        }
    }
}
