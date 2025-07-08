using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Labyrinth
{
    public class HintAndInfoReminder : MonoBehaviour
    {
        public GameObject hintButton;
        public GameObject infoButton;

        public string[] reminderMessages = {
            "Remember, if you get stuck, press the Hint button for a hint",
            "If you need a reminder for how to play, press the Info button"
        };

        public TMP_Text message;
        public Animator animator;

        private bool reminderGiven = false;
        private int seq = 0;

        void Start()
        {
            if (SaveData.Instance.CurrentLevel == 1 && !reminderGiven)
            {
                reminderGiven = true; seq = 0;
                ShowReminder();
            }
        }

        public void ShowReminder()
        {
            switch(seq)
            {
                case 0:
                    message.text = reminderMessages[0];
                    animator.SetBool("IsOn", true);
                    hintButton.GetComponent<Animator>().SetTrigger("Hint");
                    break;
                case 1:
                    message.text = reminderMessages[1];
                    infoButton.GetComponent<Animator>().SetTrigger("Hint");
                    break;
                case 2: 
                    animator.SetBool("IsOn", false);
                    break;
            }
        }

        public void NextReminder()
        {
            seq++;
            ShowReminder();
        }
    }
}