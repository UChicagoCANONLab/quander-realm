using UnityEngine.SceneManagement;

namespace Wrapper
{
    public class BackButton : QButton
    {
        protected override void OnClickedHandler()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
                return;

            SceneManager.LoadScene(0);
            Events.MinigameClosed?.Invoke();
        }
    }
}