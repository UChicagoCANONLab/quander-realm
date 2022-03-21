using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shared
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            GameEvents.testEvent.AddListener(OpenMinigame);    
        }

        private void OpenMinigame(Minigame minigame)
        {
            Debug.Log("Opening " + minigame.name);
            SceneManager.LoadScene(minigame.StartScene);
        }
    }
}
