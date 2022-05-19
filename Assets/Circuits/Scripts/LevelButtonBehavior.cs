using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public Text buttonText;
    //public Image star;
    public Sprite starSprite;
    public GameObject panel;
    //public Text scoreText;
    private LevelSelectorBehavior owner;
    private int level;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        owner.onLevelSelection(level);
    }

    public void init(int l, bool completed, LevelSelectorBehavior o)
    {
        owner = o;
        level = l;
        buttonText.text = $"{level}";
            GetComponent<Button>().interactable = true;
            //scoreText.text = "";
            if(completed)
            {
                Image starObject = panel.transform.Find($"Star").GetComponent<Image>();
                starObject.sprite = starSprite;
                //scoreText.text += "*";
	        }
    }
}
