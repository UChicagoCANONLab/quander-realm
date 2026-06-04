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


    
        [SerializeField] private GameObject triviaButton;

        private void OnEnable()
        {
            Events.InitializeMap += InitMap;
            Events.ResetMap += ResetMap;
            Events.ToggleUpgradable += EnableUpgrades;

            mapAnimator.SetTrigger("Map_Fly_In");
        }
        private void OnDisable() 
        {
            Events.InitializeMap -= InitMap;
            Events.ResetMap -= ResetMap;
            Events.ToggleUpgradable -= EnableUpgrades;

            // mapAnimator.SetTrigger("Map_Fly_Out");
        }

        public void InitMap()
        {
            minigameIcons[(int)Game.Circuits].SetInteractable(Events.GetGameUnlocked.Invoke(Game.Circuits));
            minigameIcons[(int)Game.QueueBits].SetInteractable(Events.GetGameUnlocked.Invoke(Game.QueueBits));
            minigameIcons[(int)Game.BlackBox].SetInteractable(Events.GetGameUnlocked.Invoke(Game.BlackBox));


            // Can't be all the minigameIcons because not all of them have icons
            for (int i=0; i<5; i++)
            // foreach(MinigameMapIcon icon in minigameIcons)
            {
                minigameIcons[i].SetMapIcon(Events.GetMinigameMapIcon.Invoke(minigameIcons[i].game));
            }
        }

        public void ResetMap()
        {
            minigameIcons[(int)Game.Circuits].SetInteractable(false);
            minigameIcons[(int)Game.QueueBits].SetInteractable(false);
            minigameIcons[(int)Game.BlackBox].SetInteractable(false);

            foreach(MinigameMapIcon icon in minigameIcons)
            {
                icon.SetMapIcon(0);
            }
        }

        public void TriggerMapAnimation(bool enable)
        {
            // if (enable) mapAnimator.SetTrigger("Map_Fly_In");
            // else mapAnimator.SetTrigger("Map_Fly_Out");
            // mapAnimator.SetTrigger(enable ? "Map_Fly_Out" : "Map_Fly_In");
        }

        public void EnableUpgrades(bool enabled)
        {
            for (int i=0; i<5; i++)
            // foreach(MinigameMapIcon icon in minigameIcons)
            {
                minigameIcons[i].ToggleIconUpgradable(enabled);
            }
            mapAnimator.SetBool("UpgradableOn", enabled);
            // if (!enabled) Events.ToggleTrackers.Invoke(true);
            if (!enabled) {
                GameObject.Find("GameManager/Trackers").GetComponent<Trackers>().ToggleTrackers(true);
            }
            
        }
        
    }
}
