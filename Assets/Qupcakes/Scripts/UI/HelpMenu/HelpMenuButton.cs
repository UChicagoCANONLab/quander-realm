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
        public GameObject notGadget;
        public GameObject cnotGadget;
        public GameObject swapGadget;
        public GameObject hGadget;
        public GameObject zGadget;

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

                notGadget.SetActive(level_gates[(int)GateType.NOT] > 0);
                cnotGadget.SetActive(level_gates[(int)GateType.CNOT] > 0);
                swapGadget.SetActive(level_gates[(int)GateType.SWAP] > 0);
                hGadget.SetActive(level_gates[(int)GateType.H] > 0);
                zGadget.SetActive(level_gates[(int)GateType.Z] > 0);

                GameUtilities.PauseGame();

                // Deactivate top bar
                topBar.SetActive(false);
            }
        }
    }
}
