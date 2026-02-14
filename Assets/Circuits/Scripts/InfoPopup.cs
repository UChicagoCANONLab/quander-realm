using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Circuits
{
    public class InfoPopup : MonoBehaviour
    {
        public GameObject[] gatePictures;
        private Vector2[] imageSize = { 
            new Vector2(975,750),
            new Vector2(975,750), // one image
            new Vector2(780,600),
            new Vector2(525,405),
            new Vector2(600,375),
            new Vector2(495,375),
            new Vector2(495,375),
            new Vector2(390,300) // all 7 images
        };
        public GridLayoutGroup gl;

        public void SetInfo(int level) 
        {
            ResetInfo();
            int i = 0;

            if (level < 3) {
                i = 1;
            } else if (level < 5) {
                i = 2;
            } else if (level < 7) {
                i = 3;
            } else if (level < 14) {
                i = 4;
            } else if (level < 15) {
                i = 5;
            } else if (level < 22) {
                i = 6;
            } else {
                i = 7;
            }
            for (int j=i; j<gatePictures.Length; j++) {
                gatePictures[j].SetActive(false);
            }

            gl.cellSize = imageSize[i];
        }

        public void ResetInfo() 
        {
            foreach(GameObject GO in gatePictures) {
                GO.SetActive(true);
            }
        }
    }
}