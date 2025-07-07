using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Wrapper 
{
    public class MapManager : MonoBehaviour 
    {
        // BB, CT, LA, QB, QC, Rewards, None, Trivia
        [SerializeField] private MinigameMapIcon[] minigameIcons;
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
            minigameIcons[(int)Game.Circuits].SetInteractable(Events.GetGameUnlocked.Invoke(Game.Circuits));
            minigameIcons[(int)Game.QueueBits].SetInteractable(Events.GetGameUnlocked.Invoke(Game.QueueBits));
            minigameIcons[(int)Game.BlackBox].SetInteractable(Events.GetGameUnlocked.Invoke(Game.BlackBox));
        }

        public void ResetMap()
        {
            minigameIcons[(int)Game.Circuits].SetInteractable(false);
            minigameIcons[(int)Game.QueueBits].SetInteractable(false);
            minigameIcons[(int)Game.BlackBox].SetInteractable(false);
        }

        public void TriggerMapAnimation(bool enable)
        {
            // if (enable) mapAnimator.SetTrigger("Map_Fly_In");
            // else mapAnimator.SetTrigger("Map_Fly_Out");
            // mapAnimator.SetTrigger(enable ? "Map_Fly_Out" : "Map_Fly_In");
        }
        
    }
}