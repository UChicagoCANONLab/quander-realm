using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPageButton : MonoBehaviour
{
    public GameObject tutorialManager;
    public Image lastButtonImage, nextButtonImage;
    int pageNum;

    public void GoToNextPage()
    {
        pageNum = tutorialManager.GetComponent<TutorialManager>().currentPageNum;

        int totalPageNum = tutorialManager.GetComponent<TutorialManager>().totalPageNum;

        if (totalPageNum != 1)
            lastButtonImage.color = new Color(1f, 1f, 1f, 1f);

        // If current page is the last page
        if (pageNum == totalPageNum - 1)
            return;

        // If current page is the second to last page, visually disable the button
        if (pageNum == totalPageNum - 2)
            nextButtonImage.color = new Color(1f, 1f, 1f, 0.5f);
        else
            nextButtonImage.color = new Color(1f, 1f, 1f, 1f);

        // Go to next page
        tutorialManager.GetComponent<TutorialManager>().currentPageNum += 1;

    }
}
