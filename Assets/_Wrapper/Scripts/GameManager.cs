using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Events.OpenMinigame.AddListener(OpenMinigame);
            Events.BackToMain.AddListener(() => SceneManager.LoadScene(0));
        }

        private void OpenMinigame(Minigame minigame)
        {
            Debug.Log("Opening " + minigame.name);
            SceneManager.LoadScene(minigame.StartScene);
        }
    }
}
