using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qupcakery
{
    public class NewGameButton : MonoBehaviour
    {
        public void NewGame()
        {
            GameUtilities.CreateNewGame();
            SceneManagementUtilities.LoadModeSelectionScene();
        }
    }
}
