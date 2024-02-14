using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Qupcakery
{
    public class HelpHButton : MonoBehaviour
    {
        public GameObject recipePanel;
        public GameObject startPanel;
        public Image recipeImage;

        public void ShowHRecipe()
        {
            startPanel.SetActive(false);
            recipePanel.SetActive(true);
            recipePanel.GetComponentInChildren<Text>().text = "Houdini Device (H Gate)";

            recipeImage.sprite = Utilities.helpMenuSprites.Where(obj => obj.name == "H-new").SingleOrDefault();
        }
    }
}