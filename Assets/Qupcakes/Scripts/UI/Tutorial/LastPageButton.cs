using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Qupcakery
{
    public class LastPageButton : MonoBehaviour
    {
        public GameObject tutorialManager;
        public Image lastButtonImage, nextButtonImage;
        int pageNum;

        public void GoToLastPage()
        {
            pageNum = tutorialManager.GetComponent<TutorialManager>().currentPageNum;

            int totalPageNum = tutorialManager.GetComponent<TutorialManager>().totalPageNum;

            if (totalPageNum != 1)
                nextButtonImage.color = new Color(1f, 1f, 1f, 1f);

            // If current page is the first page
            if (pageNum == 0)
                return;

            // If current page is the second page, visually disable the button
            if (pageNum == 1)
                lastButtonImage.color = new Color(1f, 1f, 1f, 0.5f);
            else
                lastButtonImage.color = new Color(1f, 1f, 1f, 1f);

            // Go to last page
            tutorialManager.GetComponent<TutorialManager>().currentPageNum -= 1;
        }
    }
}
