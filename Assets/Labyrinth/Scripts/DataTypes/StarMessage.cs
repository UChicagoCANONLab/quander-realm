using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth 
{ 
    public class StarMessage : MonoBehaviour
    {
        public Animator StarAnimator;

        public void showStars(int numStars) {
            StarAnimator.SetInteger("NumStars", numStars);
        }

        public void resetStars() {
            StarAnimator.SetInteger("NumStars", 0);
        }
    }
}