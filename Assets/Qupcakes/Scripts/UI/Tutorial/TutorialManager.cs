using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject closeButton;
    public Text text;
    public Image tutorialImage;
    public int currentPageNum = 0;
    public int totalPageNum;

    public Image nextPageButton, lastPageButton;

    int level = 0;
    bool tutorialAvailable = false;
    bool tutorialCompleted = false;

    private void Awake()
    {
        level = GameManagement.
            Instance.GetCurrentLevel().LevelInd;
}

    private void Start()
    {
        // Check if there is a tutorial available for this level
        if (Utilities.tutorialSprites.ContainsKey(level)
            && !Utilities.completedTutorials.Contains(level))
        {
            // Pause current game
            GameUtilities.PauseGame();
            tutorialAvailable = true;
            totalPageNum = Utilities.tutorialSprites[level].Count;           

            lastPageButton.color = new Color(1f, 1f, 1f, 0.5f);

            if (totalPageNum == 1)
                nextPageButton.color = new Color(1f, 1f, 1f, 0.5f);

            tutorialPanel.SetActive(true);
        }
        else
            tutorialPanel.SetActive(false);
    }

    private void Update()
    {
        // If current level doesn't have tutorial
        if (!tutorialAvailable)
            return;

        // If tutorial has been completed 
        if (tutorialCompleted)
        {
            closeButton.SetActive(true);
            Utilities.completedTutorials.Add(level);
        }

        for (int pageIndex = 0; pageIndex < totalPageNum; pageIndex++)
        {
            // #TODO: Generalize
            if (level == 1)
            {
                switch (currentPageNum)
                {
                    case 0:
                        string line1 = "Welcome to our conveyor-belt Quantum Qupcakeria!";
                        string line2 = "We offer the best chocolate and vanilla qupcakes.";
                        string line3 = "Help our customers get their favorite qupcakes!";
                        text.text = line1 + "\n" + line2 + "\n" + line3; 
                        break;
                    case 1:
                        text.text = "Press the play button in the lower right corner to deliver pre-baked cakes to customers.";
                        break;
                    case 2:              
                        text.text = "Oops! What if our qupcake doesn’t match the customer order?";
                        break;
                    case 3:
                        text.text = "It’s time to use our quantum baking gadgets!";
                        break;
                    case 4:
                        string line15 = "You have unlocked your first gadget - the flavor-inverter!";
                        string line25 = "Check out the quantum recipe below:";
                        text.text = line15 + "\n" + line25;
                        break;
                    case 5:
                        string line16 = "Drag the gadget to the belt and it will operate following our quantum baking recipe.";
                        string line26 = "Now try these out with your first customers!";
                        text.text = line16 + "\n" + line26;
                        break;
                    default:                      
                        break;
                }
            }

            if (currentPageNum == 0 && level == 3)
                text.text = "Congrats! You unlocked the flavor-swapper!";


            if (level == 8)
            {
                switch (currentPageNum)
                {
                    case 0:
                        text.text = "Congrats! You unlocked the chocolate-powered flavor-inverter! " +
                            "This gadget works on two belts at the same time. " +
                            "Drag it in between two belts to use it. " +
                            "If you send a chocolate qupcake through the control (dot channel)," +
                            " the flavor of the target qupcake (cross channel) will be flipped. ";
                        // #TODO: make it more clear that the control doesn't change 
                        break;
                    case 1:
                        text.text = "Note that if you use a vanilla qupcake as the control (dot channel)" +
                            ", nothing will happen!";
                        break;
                }
            }

            if (currentPageNum == 0 && level == 10)
                text.text = "Note that you can click on the gadget to swap the positions " +
                    "of the two channels when the gadget is placed on the belts. ";

            if (currentPageNum == 0 && level == 11)
                text.text = "Congrats! You've unlocked the mystery wrapper - H gate! " +
                    "This quantum gadget creates mystery boxes based on " +
                    "the input qupcake. You won't be able to tell what's inside " +
                    "until you open the box -- half of the times you get vanilla, " +
                    "half of the times you get chocolate. " +
                    "Who doesn't like some tasty surprises?";

            if (level == 12)
            {
                switch (currentPageNum)
                {
                    case 0:
                        text.text = "The H gate can also unwrap mystery boxes! " +
                   "Note that chocolate and vanilla qupcakes follow different rules. " +
                   "Be careful!";
                        break;
                    case 1:
                        text.text = "Mystery gadget recap: \n  (Note that you can always " +
                   "check the recipe by clicking on the help ? button)";
                        break;
                }
            }

            if (level == 13)
            {
                switch (currentPageNum)
                {
                    case 0:
                        text.text = "Congrats! You've unlocked the mystery-inverter - Z gate!";
                        break;
                    case 1:
                        text.text = "Notice! The mystery-inverter cannot act on non-mystery boxes";
                        break;
                }
            }

            if (pageIndex == currentPageNum)
            {
                tutorialImage.sprite = Utilities.tutorialSprites[level][currentPageNum];
            }

            if (currentPageNum == totalPageNum - 1)
                tutorialCompleted = true;
        }
    }

}
