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

        private string prefix = "MapCanvas 1/MapPanel";

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
            if (StarTracker.ST.CheckUnlocked(Game.Circuits)) {
                Unlock(TL); // Unlock Tangle's Lair (Circuits)
            }
            if (StarTracker.ST.CheckUnlocked(Game.QueueBits)) {
                Unlock(QB); // Unlock QueueBits
            }
            if (StarTracker.ST.CheckUnlocked(Game.BlackBox)) {
                Unlock(BT); // Unlock Buried Treasure
            }
            // StarTracker.ST.PrintDict();
        }
#endif

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