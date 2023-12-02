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
            StarTracker.ST.gameUnlocked[Game.QueueBits] = true;

            Lock(BT);
            Lock(TL);
            TryUnlockGames();
        }
        public void TryUnlockGames() {
            if (StarTracker.ST.starsPerGame[Game.Qupcakes] >= 12 
            && StarTracker.ST.gameUnlocked[Game.Circuits] == false) { 
                Unlock(TL); // Unlock Tangle's Lair (Circuits)
                Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.Circuits);
                StarTracker.ST.gameUnlocked[Game.Circuits] = true;
            }
            if (StarTracker.ST.TotalStars >= 50
            && StarTracker.ST.gameUnlocked[Game.QueueBits] == false) {
                Unlock(BT); // Unlock Buried Treasure
                Wrapper.Events.UnlockAndDisplayGame?.Invoke(Game.BlackBox);
                StarTracker.ST.gameUnlocked[Game.BlackBox] = true;
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
            if (StarTracker.ST.CheckUnlocked(Game.Circuits)) {
                Unlock(TL); // Unlock Tangle's Lair (Circuits)
                StarTracker.ST.gameUnlocked[Game.Circuits] = true;
            }
            if (StarTracker.ST.CheckUnlocked(Game.QueueBits)) {
                Unlock(QB); // Unlock QueueBits
                StarTracker.ST.gameUnlocked[Game.QueueBits] = true;
            }
            if (StarTracker.ST.CheckUnlocked(Game.BlackBox)) {
                Unlock(BT); // Unlock Buried Treasure
                StarTracker.ST.gameUnlocked[Game.BlackBox] = true;
            }
            StarTracker.ST.PrintDict();
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