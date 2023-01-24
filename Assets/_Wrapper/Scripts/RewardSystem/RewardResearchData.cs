using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper 
{
    public class RewardResearchData : MonoBehaviour
    {
        public string Username = Wrapper.Events.GetPlayerResearchCode?.Invoke();

        public string currentCard;
        public string timeClicked;
        public string displayType;

        public static RewardResearchData Instance;
        public string rewardResearchData = string.Empty;
        
        private void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RewardRDtoString() {
            string jsonData = JsonUtility.ToJson(this);
            rewardResearchData = jsonData;
        }
    }
}