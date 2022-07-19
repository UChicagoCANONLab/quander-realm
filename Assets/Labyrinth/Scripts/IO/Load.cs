using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using System.IO;
using System.Runtime.InteropServices;

namespace Labyrinth 
{ 
    public class Load : MonoBehaviour
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        [DllImport("__Internal")]
        private static extern void TwinTanglementLoad(string callback);

        private void Start() {
                TwinTanglementLoad("loadData");
        }
#endif

        public void loadData(string data) {
            Debug.Log("I just got this data:");
            Debug.Log(data);
            //TODO Load Data into memory
        }
    }
}