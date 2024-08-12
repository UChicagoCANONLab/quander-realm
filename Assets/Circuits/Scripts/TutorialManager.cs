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
    public void gatesSelected(HashSet<BaseGateBehavior> gates){
        if(GameData.getCurrLevel() == 0 && gates.Count == 2){
            textbox.text = "Great job!\nWe can now click the Substitute button!";
            animation.Play("TutorialSub");
        }
        // Debug.Log("yay!");
    }
    public void load(int level){
        Debug.Log("Loaded level");
        if(level == 0){
Debug.Log(level);
            textbox.text = "Welcome to my lair!\nCan you find this substituion?";
            animation.Play("TutorialHH");
        }
    }
    public void substitution(bool simplified){
        if(simplified){
            textbox.text = "Incredible!\nNow all you need to do is hit the run button!";
            animation.Play("TutorialRun");
        }

    }
}
}