using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

namespace Labyrinth 
{ 
    public class Save : MonoBehaviour
    {

#if UNITY_WEBGL == true && UNITY_EDITOR == false
        [DllImport("__Internal")]
        private static extern void TwinTanglementSave(string data);
#endif
        public static void SaveGame()
        {
            // Data data = new Data(SaveData.Instance.starsPerLevel, SaveData.Instance.timePerLevel, SaveData.Instance.previousSave);
            Data data = new Data(SaveData.Instance);
            string jsonData = JsonUtility.ToJson(data);
            Debug.Log(jsonData);
            
            // call when want to save data
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                TwinTanglementSave(jsonData);
#endif
            
            return;
        }
        
    }

}