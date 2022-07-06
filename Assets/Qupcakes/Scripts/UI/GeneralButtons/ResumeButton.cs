using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Qupcakery
{
    public class ResumeButton : MonoBehaviour
    {
        public Image buttonImage;

        private void Awake()
        {
            buttonImage.color = new Color(1f, 1f, 1f, 1f);
            if (!GameManagement.Instance.GameInProgress())
                DeactiveResumeButton();
        }

        public void DeactiveResumeButton()
        {
            buttonImage.color = new Color(1f, 1f, 1f, 0.5f);
        }

        public void ResumeGame()
        {
            if (GameManagement.Instance.GameInProgress())
            {
                SceneManagementUtilities.LoadModeSelectionScene();
            }
        }
    }
}
