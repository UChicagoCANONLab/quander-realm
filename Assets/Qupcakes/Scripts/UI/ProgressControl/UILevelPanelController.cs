using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qupcakery
{
    public class UILevelPanelController : MonoBehaviour
    {
        public Text t;

        private void Start()
        {
            t.text = "LEVEL " + GameManagement.
                Instance.GetCurrentLevel().LevelInd;
        }
    }
}
