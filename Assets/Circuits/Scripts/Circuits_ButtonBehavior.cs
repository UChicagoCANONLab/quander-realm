using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Circuits 
{
    public class Circuits_ButtonBehavior : MonoBehaviour
    {
        public void toMenu()
        {            
	        var completedLevels = GameData.getCompletedLevels();
            Debug.Log(completedLevels[0]);
            if (!completedLevels[0]) {
                SceneManager.LoadScene("Circuits_Dialogue");
	        }
            else { 
	    
			    SceneManager.LoadScene("Circuits_Menu");
		    }
            //Debug.Log("test");
        }

        public void toTitle()
        {
            SceneManager.LoadScene("Circuits_Title");
        }

        // THIS IS ALSO IN GameBehavior.cs AT LINE 545
    }
}