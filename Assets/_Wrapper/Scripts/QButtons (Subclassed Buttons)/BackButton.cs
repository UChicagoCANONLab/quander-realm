using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Wrapper
{
    public class BackButton : QButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Events.ToggleTitleScreen?.Invoke(true);
            }
            else
            {
                SceneManager.LoadScene(0);
                Events.MinigameClosed?.Invoke();
            }
        }
    }
}