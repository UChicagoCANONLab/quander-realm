using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

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
                if (SceneManager.GetActiveScene().name == "QU_Level")
                {
                    Time.timeScale = 1;
                }
                Events.ScreenFadeMidAction?.Invoke(() =>
                {
                    SceneManager.LoadScene(0);
                    Events.MinigameClosed?.Invoke();
                }, 0.1F);
            }
        }
    }
}