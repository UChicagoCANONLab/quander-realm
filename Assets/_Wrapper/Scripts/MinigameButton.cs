using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class MinigameButton : MonoBehaviour
    {
        public Minigame minigame;

        private void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => GameEvents.testEvent.Invoke(minigame));
        }
    }
}
