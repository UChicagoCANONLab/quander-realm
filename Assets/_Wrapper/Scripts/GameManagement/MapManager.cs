using System.Collections;
using System.Collections.Generic;
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

        /* 
        Functions to initialize map and unlock games
        */
        
        public void InitMap() {
            // Lock(BT);
            // Lock(QB);
            // Lock(TL);
            // TryUnlockGames();

            TL.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.Circuits);
            QB.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.QueueBits);
            BT.GetComponent<MinigameButton>().interactable = Events.GetGameUnlocked.Invoke(Game.BlackBox);
        }


        /* public void TryUnlockGames() {
            if (Events.GetGameUnlocked.Invoke(Game.Circuits)) {
                TL.GetComponent<MinigameButton>().interactable = true;
            }
            if (Events.GetGameUnlocked.Invoke(Game.QueueBits)) {
                QB.GetComponent<MinigameButton>().interactable = true;
            }
            if (Events.GetGameUnlocked.Invoke(Game.BlackBox)) {
                BT.GetComponent<MinigameButton>().interactable = true;
            }
        } */

    }
}