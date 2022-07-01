using UnityEngine;
using System.Collections;

public class ChangeGameModeButton : MonoBehaviour
{
    public void GoToChangeMadeScene()
    {
        SceneManagementUtilities.LoadModeSelectionScene();
    }
}
