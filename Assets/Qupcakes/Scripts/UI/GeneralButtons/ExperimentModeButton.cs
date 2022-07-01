using UnityEngine;
using System.Collections;

public class ExperimentModeButton : MonoBehaviour
{
    public void EnterExperimentMode()
    {
        GameManagement.Instance.SetGameMode(GameManagement.GameMode.Experiment);
        SceneManagementUtilities.LoadLevelSelectionMenu();
    }
}
