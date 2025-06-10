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
        [SerializeField] private Image lanternFront;

        public void UpdateDisplay()
        {
            SetStars(Events.GetOverallTotalStars.Invoke());
            SetCoins(Events.GetUserSaveTotalCoins.Invoke());
            SetStreak(Events.GetStreakLength.Invoke());
        }

        public void SetStars(int num)
        {
            totalStars = num;
            totalStarsTMP.text = $"{totalStars}";
        }

        public void SetCoins(int num)
        {
            totalCoins = num;
            totalCoinsTMP.text = $"{totalCoins}";
        }

        public void SetStreak(int num)
        {
            streakLength = num;
            streakLengthTMP.text = $"{streakLength}";

            if (streakLength > 0)
            {
                lanternFront.color = new Color(1f, 1f, 1f);
                fire.SetActive(true);
            }
            else 
            {
                lanternFront.color = new Color(0.3f, 0.3f, 0.3f);
                fire.SetActive(false);
            }
        }
    }
}