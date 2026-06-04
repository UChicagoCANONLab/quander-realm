using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class TriviaResearchData{

        public string Username = Wrapper.Events.GetPlayerResearchCode?.Invoke();
        public string SaveData = string.Empty;
   
}
