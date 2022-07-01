using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenuController : MonoBehaviour
{
    public Image star1, star2, star3;
    public Sprite fullStar;
    
    void Start()
    {
        string level = "Level" + (GameManagement.Instance.GetCurrentLevelInd()).ToString();
        int starCnt = GameUtilities.GetLevelResult(level);

        switch (starCnt)
        {
            case 1:
                star1.sprite = fullStar;
                break;
            case 2:
                star1.sprite = fullStar;
                star2.sprite = fullStar;
                break;
            case 3:
                star1.sprite = fullStar;
                star2.sprite = fullStar;
                star3.sprite = fullStar;
                break;
        }
    }
}
