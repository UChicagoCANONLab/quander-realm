using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Wrapper
{
    public class RewardData : MonoBehaviour
    {
        public RewardSaveObject RewardResearchData;
        // Doesn't need SaveData because nothing to load -- not a minigame

        public static RewardData Instance;

        private void Awake() 
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            RewardResearchData = new RewardSaveObject();
        }

        public void SaveRewardData()
        {
            Debug.Log(JsonUtility.ToJson(RewardResearchData));
            Events.SaveMinigameResearchData?.Invoke(Game.Rewards, RewardResearchData);
            // Does not have SaveData to update -- not a minigame
        }

        public void UpdateRewardResearchData(Reward currCard, string desc)
        {
            RewardResearchData.currentCard = currCard.titleFront.text;
            RewardResearchData.game = currCard.game.ToString();
            RewardResearchData.timeClicked = DateTime.Now.ToString("HH:mm:ss tt");
            RewardResearchData.displayType = currCard.displayType.ToString();
            RewardResearchData.description = desc;

            RewardResearchData.cardsPerGame[(int)currCard.game] = Events.HasRewardsFromGame.Invoke(currCard.game);
            RewardResearchData.totalCards = Events.HasRewardsFromGame.Invoke(Game.Rewards);

            SaveRewardData();
        }

    }
}