using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wrapper
{
    public class Star : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject fill;

        public void SetStar(bool active, bool filled)
        {
            // background.SetActive(active);
            this.gameObject.SetActive(active);
            fill.SetActive(filled);
        }
    }
}