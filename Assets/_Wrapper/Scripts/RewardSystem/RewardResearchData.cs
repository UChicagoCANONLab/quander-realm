using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper 
{
    public class RewardResearchData : MonoBehaviour
    {
        public string currentCard;
        public string timeClicked;
        public string displayType;

        public static RewardResearchData Instance;
        
        private void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Events.PlayMusic?.Invoke("W_RewardMusic");
        }

        public string RewardRDtoString() {
            string jsonData = JsonUtility.ToJson(this);
            return jsonData;
        }

        // DESTROYING INSTANCES WHEN NOT IN USE
        private void OnEnable() {
            Wrapper.Events.MinigameClosed += DestroyDataObject;
        }
        private void OnDisable(){
            Wrapper.Events.MinigameClosed -= DestroyDataObject;
        }
        private void DestroyDataObject() {
            Time.timeScale = 1f;
            Destroy(GameObject.Find("RewardResearchData"));
        }
    }
}