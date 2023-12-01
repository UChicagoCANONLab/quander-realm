using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper 
{
    public class MapManager : MonoBehaviour 
    {
        public static MapManager MM;

        public GameObject QC;
        public GameObject TT;
        public GameObject TL;
        public GameObject QB;
        public GameObject BT;

        private string prefix = "MapCanvas 1/MapPanel";

        /* 
        Functions to initialize map and unlock games
        Different for LITE_VERSION and full version
        */
       
#if LITE_VERSION
        public void InitMap() {
            // StarTracker.ST.PrintDict();
            Lock(BT);
            Lock(TL);
            TryUnlockGames();
        }
        public void TryUnlockGames() {
            if (StarTracker.ST.starsPerGame[Game.Qupcakes] >= 12) { 
                Unlock(TL); // Unlock Tangle's Lair (Circuits)
            }
            if (StarTracker.ST.TotalStars >= 50) {
                Unlock(BT); // Unlock Buried Treasure
            }
        }
#else
         public void InitMap() {
            // StarTracker.ST.PrintDict();
            Lock(BT);
            Lock(QB);
            Lock(TL);
            TryUnlockGames();
        }
        public void TryUnlockGames() {
            if (StarTracker.ST.starsPerGame[Game.Qupcakes] >= 27) { 
                Unlock(TL); // Unlock Tangle's Lair (Circuits)
            }
            if (StarTracker.ST.starsPerGame[Game.Qupcakes] >= 10 && StarTracker.ST.starsPerGame[Game.Labyrinth] >= 10) {
                Unlock(QB); // Unlock QueueBits
            }
            if (StarTracker.ST.TotalStars >= 120) {
                Unlock(BT); // Unlock Buried Treasure
            }
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