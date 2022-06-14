using System;
using UnityEngine;

public class GameModeButton : MonoBehaviour
{
    public void EnterGameMode()
    {
        GameManagement.Instance.SetGameMode(GameManagement.GameMode.Regular);
        SceneManagementUtilities.LoadLevelSelectionMenu();
    }
}
