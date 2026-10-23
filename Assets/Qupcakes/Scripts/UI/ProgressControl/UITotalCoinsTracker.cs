using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Qupcakery
{
    public class UITotalCoinsTracker : MonoBehaviour
    {
        // public Text text;
        public static UITotalCoinsTracker Instance { get; private set; }
        public TextMeshProUGUI count;

        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateCoinAmount(GameManagement.Instance.game.gameStat.TotalEarning);
        }

        public void UpdateCoinAmount(int amount)
        {
            // text.text = System.Convert.ToString(amount);
            count.text = $"{amount}";
        }
    }
}
