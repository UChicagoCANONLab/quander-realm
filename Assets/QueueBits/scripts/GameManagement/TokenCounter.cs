using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using QueueBits;

namespace QueueBits {
    public class TokenCounter : MonoBehaviour
    {
        public TMP_Text counter100;
        public TMP_Text counter75;
        public TMP_Text counter50;

        public bool isPlayer;

        public GameObject[] counterObjects50;

        public void setCounter(int prob, int value) {
            if (prob == 100) {counter100.text = value.ToString();}
            else if (prob == 75) {counter75.text = value.ToString();}
            else {counter50.text = value.ToString();}
        }

        public void disable50() {
            foreach (GameObject i in counterObjects50) {
                i.SetActive(false);
            }
        }
        
    }
}