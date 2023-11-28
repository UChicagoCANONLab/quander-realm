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

        public void InitMap() {
            StarTracker.ST.PrintDict();

            Lock(Game.BlackBox);
            Lock(Game.QueueBits);
            Lock(Game.Circuits);
            Lock(Game.Qupcakes);
            Lock(Game.Labyrinth);

        }

        public void Unlock(Game game) {
            switch (game) {
                case Game.BlackBox:
                    BT.GetComponent<MinigameButton>().enabled = true;
                    GameObject.Find($"{prefix}/{BT.name}/Locked").SetActive(false);
                    break;

                case Game.Circuits:
                    TL.GetComponent<MinigameButton>().enabled = true;
                    GameObject.Find($"{prefix}/{TL.name}/Locked").SetActive(false);
                    break;

                case Game.QueueBits:
                    QB.GetComponent<MinigameButton>().enabled = true;
                    GameObject.Find($"{prefix}/{QB.name}/Locked").SetActive(false);
                    break;

                default: // For TT and QB
                    break;
            }
        }

        public void Lock(Game game) {
            switch (game) {
                case Game.BlackBox:
                    BT.GetComponent<MinigameButton>().enabled = false;
                    GameObject.Find($"{prefix}/{BT.name}/Locked").SetActive(true);
                    break;

                case Game.Circuits:
                    TL.GetComponent<MinigameButton>().enabled = false;
                    GameObject.Find($"{prefix}/{TL.name}/Locked").SetActive(true);
                    break;

                case Game.QueueBits:
                    QB.GetComponent<MinigameButton>().enabled = false;
                    GameObject.Find($"{prefix}/{QB.name}/Locked").SetActive(true);
                    break;

                default: // For TT and QB
                    break;
            }
        }





    }
}