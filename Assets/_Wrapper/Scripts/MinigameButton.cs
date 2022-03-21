using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shared
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
