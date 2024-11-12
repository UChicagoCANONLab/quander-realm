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

        private string prefix = "MapCanvas/MapPanel";

        /* 
        Functions to initialize map and unlock games

        */
        public void InitMap() {
            // StarTracker.ST.PrintDict();
            Lock(BT);
            Lock(QB);
            Lock(TL);
            TryUnlockGames();
        }
        public void TryUnlockGames() {
            if (Events.GetGameUnlocked.Invoke(Game.Circuits)) {
                Unlock(TL); // Unlock Tangle's Lair (Circuits)
            }
            if (Events.GetGameUnlocked.Invoke(Game.QueueBits)) {
                Unlock(QB); // Unlock QueueBits
            }
            if (Events.GetGameUnlocked.Invoke(Game.BlackBox)) {
                Unlock(BT); // Unlock Buried Treasure
            }
        }

        /* 
        Functions to lock and unlock games 
        */

        public void Unlock(GameObject game) {
            game.GetComponent<MinigameButton>().enabled = true;
            GameObject.Find($"{prefix}/{game.name}/Locked").SetActive(false);
        }

        public void Lock(GameObject game) {
            game.GetComponent<MinigameButton>().enabled = false;
            GameObject.Find($"{prefix}/{game.name}/Locked").SetActive(true);
        }
    }
}