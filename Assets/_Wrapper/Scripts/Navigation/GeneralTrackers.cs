using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Wrapper
{
    public class GeneralTrackers : MonoBehaviour
    {
        [SerializeField] private TMP_Text totalStarsTMP;
        private int totalStars;
        [SerializeField] private TMP_Text totalCoinsTMP;
        private int totalCoins;
        [SerializeField] private TMP_Text streakLengthTMP;
        private int streakLength;
        [SerializeField] private GameObject fire;
        [SerializeField] private GameObject blueFire;
        // [SerializeField] private Image lanternFront;

        public void UpdateDisplay()
        {
            SetStars(Events.GetOverallTotalStars.Invoke());
            SetCoins(Events.GetUserSaveTotalCoins.Invoke());
            SetStreak(Events.GetStreakLength.Invoke());
        }

        public void ResetDisplay()
        {
            SetStars(0);
            SetCoins(0);
            SetStreak(0);
        }

        private void SetStars(int num)
        {
            totalStars = num;
            totalStarsTMP.text = $"{totalStars}";
        }

        private void SetCoins(int num)
        {
            totalCoins = num;
            totalCoinsTMP.text = $"{totalCoins}";
        }

        private void SetStreak(int num)
        {
            streakLength = num;
            streakLengthTMP.text = $"{streakLength}";

            if (Events.GetStreakFreeze.Invoke() > 0)
            {
                fire.SetActive(false);
                blueFire.SetActive(true);
                blueFire.transform.GetChild(1).GetComponent<TMP_Text>().text = $"+{Events.GetStreakFreeze.Invoke()}";
            }
            else if (streakLength > 0)
            {
                // lanternFront.color = new Color(1f, 1f, 1f);
                fire.SetActive(true);
                blueFire.SetActive(false);
            }
            else 
            {
                // lanternFront.color = new Color(0.3f, 0.3f, 0.3f);
                fire.SetActive(false);
                blueFire.SetActive(false);
            }
        }
    }
}