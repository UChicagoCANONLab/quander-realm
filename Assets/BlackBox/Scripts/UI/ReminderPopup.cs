using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlackBox
{
    public class ReminderPopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private string[] reminderText = {
            "Remember, if you get stuck, you can press the Hint button for a hint",
            "If you want a reminder for how to play, press the Info button"
        };

        [SerializeField] private GameObject hintButton;
        [SerializeField] private GameObject infoButton;
        private bool reminderGiven = false;
        private int seq = 0;
        

        private void OnEnable()
        {
            BBEvents.ShowReminder += ShowReminder;
        }
        private void OnDisable()
        {
            BBEvents.ShowReminder -= ShowReminder;
        }


        public void ShowReminder()
        {
            if (reminderGiven) return;

            switch(seq)
            {
                case 0:
                    description.text = reminderText[0];
                    animator.SetBool("IsOn", true);
                    hintButton.GetComponent<Animator>().SetTrigger("Hint");
                    break;
                case 1:
                    description.text = reminderText[1];
                    hintButton.GetComponent<Animator>().SetTrigger("Normal");
                    infoButton.GetComponent<Animator>().SetTrigger("Hint");
                    break;
                case 2:
                    reminderGiven = true;
                    infoButton.GetComponent<Animator>().SetTrigger("Normal");
                    animator.SetBool("IsOn", false);
                    break;
            }
            return;
        }

        public void Next()
        {
            seq++;
            ShowReminder();
        }
    }
}