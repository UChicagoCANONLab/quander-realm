using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Wrapper
{
    public class RewardSaveManager : MonoBehaviour
    {
        public CardResearchData CardData;
        public BadgeResearchData BadgeData;

        public RewardData RewardData;

        public static RewardSaveManager Instance;

        // This data structure is established so that no loadable SaveData is used.
        // Multiple types of research data are usable from this save manager.
        // CardData is fully implemented; BadgeData is set up to be determined later.
        // Data for the coin usage can also be logged through this bucket.

        private void Awake() 
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // RewardData = new RewardData();
        }

        public void SaveRewardData()
        {
            Events.SaveMinigameResearchData?.Invoke(Game.Rewards, RewardData);
        }

        public void UpdateCardData(Reward currCard, string desc)
        {
            CardData = new CardResearchData();

            CardData.currentCard = currCard.titleFront.text;
            CardData.game = currCard.game.ToString();
            CardData.timeClicked = DateTime.Now.ToString("HH:mm:ss tt");
            CardData.displayType = currCard.displayType.ToString();
            CardData.description = desc;

            // CardData.cardsPerGame[(int)currCard.game] = Events.HasRewardsFromGame.Invoke(currCard.game);
            CardData.totalCards = Events.HasRewardsFromGame.Invoke(Game.Rewards);

            RewardData.UpdateRewardData(CardData);
            SaveRewardData();
        }

        public void UpdateBadgeData()
        {
            BadgeData = new BadgeResearchData();

            // Update as Badge Research data is determined
            RewardData.UpdateRewardData(BadgeData);
            SaveRewardData();
        }

    }
}