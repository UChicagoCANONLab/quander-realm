using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Wrapper 
{
    public class MapManager : MonoBehaviour 
    {
        // public GameObject QC;
        // public GameObject TT;
        // public GameObject TL;
        // public GameObject QB;
        // public GameObject BT;

        // BB, CT, LA, QB, QC, Rewards, None, Trivia
        [SerializeField] private MinigameButton[] minigameButtons;
        [SerializeField] private Animator mapAnimator;


        private void OnEnable()
        {
            Events.InitializeMap += InitMap;
            Events.ResetMap += ResetMap;

            mapAnimator.SetTrigger("Map_Fly_In");
        }
        private void OnDisable() 
        {
            Events.InitializeMap -= InitMap;
            Events.ResetMap -= ResetMap;

            // mapAnimator.SetTrigger("Map_Fly_Out");
        }

        public void InitMap() 
        {
            minigameButtons[(int)Game.Circuits].interactable = Events.GetGameUnlocked.Invoke(Game.Circuits);
            minigameButtons[(int)Game.QueueBits].interactable = Events.GetGameUnlocked.Invoke(Game.QueueBits);
            minigameButtons[(int)Game.BlackBox].interactable = Events.GetGameUnlocked.Invoke(Game.BlackBox);

            // TL.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.Circuits);
            // QB.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.QueueBits);
            // BT.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.BlackBox);
        }

        public void ResetMap()
        {
            // TL.GetComponent<MinigameButton>().interactable = false;
            // QB.GetComponent<MinigameButton>().interactable = false;
            // BT.GetComponent<MinigameButton>().interactable = false;
            minigameButtons[(int)Game.Circuits].interactable = false;
            minigameButtons[(int)Game.QueueBits].interactable = false;
            minigameButtons[(int)Game.BlackBox].interactable = false;
        }

        public void TriggerMapAnimation(bool enable)
        {
            // if (enable) mapAnimator.SetTrigger("Map_Fly_In");
            // else mapAnimator.SetTrigger("Map_Fly_Out");
            // mapAnimator.SetTrigger(enable ? "Map_Fly_Out" : "Map_Fly_In");
        }
        
    }
}