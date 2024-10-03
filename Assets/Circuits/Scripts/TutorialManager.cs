using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Circuits{ 
public class TutorialManager : MonoBehaviour
{
    public Text textbox;
    public Animation animation;
    public Image image;
    public GameObject hand;
    public Sprite[] subImages;
    public void gatesSelected(HashSet<BaseGateBehavior> gates){

            switch (GameData.getCurrLevel())
            {
                case 0:
                    if(gates.Count == 2) {
					    textbox.text = "Great job!\nWe can now click the Substitute button!";
                        animation.Play("TutorialSub");
                    }
                    break;
                default:
                    break;
            }
    }
    public void load(int level){
        Debug.Log("Loaded level");
		    hand.active = false;
            switch (level)
            {
                case 0:
				    textbox.text = "Welcome to my lair!\nCan you find this substituion?";
				    animation.Play("TutorialHH");
                    image.sprite = subImages[0];
                    hand.active = true;
                    break;
                case 3:
                    textbox.text = "We’re adding new substitutions now! The box with an X on it is called a Not-Gate.";
                    image.sprite = subImages[1];
                    break;
                case 5:
                    textbox.text = "Use the three gates to make one Not-Gate.";
                    image.sprite = subImages[2];
                    break;
                case 7:
                    textbox.text = "Two Not-Gates cancel out, so you don’t have to worry about them as much anymore!";
                    image.sprite = subImages[3];
                    break;
                case 14:
                    textbox.text = "The large gate is called a control-NOT or CNOT!";
                    image.sprite = subImages[4];
                    break;
                case 15:
                    textbox.text = "You can also turn a control-Z gate into a CNOT!";
                    image.sprite = subImages[5];
                    break;
                case 22:
                    textbox.text = "FIVE gates into one?!\nThis is getting out of hand!";
                    image.sprite = subImages[6];
                    break;
                default:
                    break;
            }
            if(level == 0){
        }
    }
    public void substitution(bool simplified){
        if(simplified){
                switch (GameData.getCurrLevel())
                {
                    case 0:
						textbox.text = "Incredible!\nNow all you need to do is hit the run button!";
                        break;
                    case 3:
                        textbox.text = "THREE gates into just one Z-Gate!?\n\nNOT bad!";
                        break;
                    case 5:
                        textbox.text = "Got it? Good! Keep going!";
                        break;
                    case 7:
                        textbox.text = "Hope that helps you out with these harder levels! You’re doing a great job!";
                        break;
                    case 14:
                        textbox.text = "Incredible! I can't CONTROL my excitement!";
                        break;
                    case 15:
                        textbox.text = "Quantastic!!";
                        break;
                    case 22:
                        textbox.text = "Almost done! Just a few more levels!";
                        break;
                    default:
                        break;
                }

        }
		hand.active = true;
	    animation.Play("TutorialRun");

    }
}
}