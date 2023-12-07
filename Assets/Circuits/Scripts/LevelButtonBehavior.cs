using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Circuits 
{
    public class LevelButtonBehavior : MonoBehaviour
    {
        public Text buttonText;
        //public Image star;

        // public Sprite starSprite;
        public GameObject[] starSprites;
        
        public GameObject panel;
        public GameObject locked;
        //public Text scoreText;
        private LevelSelectorBehavior owner;
        private int level;

        public Sprite[] numberSprites;


        public void onClick()
        {
            owner.onLevelSelection(level);
        }

        public void init(int l, bool completed, int stars, LevelSelectorBehavior o)
        {
            owner = o;
            level = l;
            buttonText.text = $"{level}";

            Image numberObject = panel.transform.Find("LevelNumber").GetComponent<Image>();
            numberObject.sprite = numberSprites[level];
            
            GetComponent<Button>().interactable = true;

            //scoreText.text = "";
            /* if(completed)
            {
                Image starObject = panel.transform.Find($"Star").GetComponent<Image>();
                // starObject.sprite = starSprite;
                //scoreText.text += "*";
            } */

            if (level > GameData.getMaxLevelUnlocked()) {
                locked.SetActive(true);
                GetComponent<Button>().enabled = false;
            }

            for (int i=0; i< stars; i++) {
                starSprites[i].SetActive(true);
            }
        }
    }
}