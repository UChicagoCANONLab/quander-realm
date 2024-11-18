using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qupcakery
{
    public class HelpMenuButton : MonoBehaviour
    {
        public GameObject helpPanel;
        public GameObject recipePanel, startPanel;
        public GameObject topBar;

        public void OpenPanel()
        {
            if (GameObject.FindGameObjectsWithTag("InfoPanel").Length > 0)
                return;

            if (helpPanel != null)
            {
                recipePanel.SetActive(false);
                helpPanel.SetActive(true);
                startPanel.SetActive(true);

                int[] level_gates = GameManagement.Instance.GetCurrentLevel().AvailableGates;

                GameObject.Find("NOT").SetActive(level_gates[(int)GateType.NOT] > 0);
                GameObject.Find("CNOT").SetActive(level_gates[(int)GateType.CNOT] > 0);
                GameObject.Find("SWAP").SetActive(level_gates[(int)GateType.SWAP] > 0);
                GameObject.Find("H").SetActive(level_gates[(int)GateType.H] > 0);
                GameObject.Find("Z").SetActive(level_gates[(int)GateType.Z] > 0);

                if (GameManagement.Instance.gameMode == GameManagement.GameMode.Regular)
                {
                    // pause game
                    GameUtilities.PauseGame();
                }

                // Deactivate top bar
                topBar.SetActive(false);
            }
        }
    }
}
