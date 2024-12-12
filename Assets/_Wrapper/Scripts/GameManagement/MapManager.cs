using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Wrapper 
{
    public class MapManager : MonoBehaviour 
    {
        public GameObject QC;
        public GameObject TT;
        public GameObject TL;
        public GameObject QB;
        public GameObject BT;


        private void OnEnable()
        {
            Events.InitializeMap += InitMap;
        }
        private void OnDisable() 
        {
            Events.InitializeMap -= InitMap;
        }

        public void InitMap() {            
            TL.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.Circuits);
            QB.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.QueueBits);
            BT.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.BlackBox);
        }

        public void OpenTanglesTrivia() {
            // SceneManager.LoadScene("TanglesTrivia");
            Events.OpenMinigame.Invoke(Minigame.Trivia);
        }
        
    }
}