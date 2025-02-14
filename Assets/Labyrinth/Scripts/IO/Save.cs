using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.InteropServices;

namespace Labyrinth 
{ 
    public class Save : MonoBehaviour
    {

        public static Save Instance;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public int SaveGame() {
            // Save research data
            Data.Instance.researchData.UpdateResearchData(SaveData.Instance);
            Wrapper.Events.SaveMinigameResearchData?.Invoke(Wrapper.Game.Labyrinth, Data.Instance.researchData);

            // Save game data
            Data.Instance.ttSaveData.UpdateTTSaveData(SaveData.Instance);
            Wrapper.Events.UpdateMinigameSaveData?.Invoke(Wrapper.Game.Labyrinth, Data.Instance.ttSaveData);

            return 0;
        }
    }

}