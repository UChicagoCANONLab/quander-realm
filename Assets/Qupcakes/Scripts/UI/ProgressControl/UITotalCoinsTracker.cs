using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITotalCoinsTracker : MonoBehaviour
{
    public Text text;
    public static UITotalCoinsTracker Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    { 
        Instance = this;      
    }

    private void Start()
    {
        UpdateCoinAmount(PlayerPrefs.GetInt("Earning"));
    }

    public void UpdateCoinAmount(int amount)
    {
        PlayerPrefs.SetInt("Earning", amount);
        text.text = System.Convert.ToString(amount);
    }
}
