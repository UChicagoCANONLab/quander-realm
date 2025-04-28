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

            CardData = new CardResearchData();
            BadgeData = new BadgeResearchData();
        }

        public void SaveRewardData(object researchData)
        {
            Events.SaveMinigameResearchData?.Invoke(Game.Rewards, researchData);
        }

        public void UpdateCardData(Reward currCard, string desc)
        {
            CardData.currentCard = currCard.titleFront.text;
            CardData.game = currCard.game.ToString();
            CardData.timeClicked = DateTime.Now.ToString("HH:mm:ss tt");
            CardData.displayType = currCard.displayType.ToString();
            CardData.description = desc;

            CardData.cardsPerGame[(int)currCard.game] = Events.HasRewardsFromGame.Invoke(currCard.game);
            CardData.totalCards = Events.HasRewardsFromGame.Invoke(Game.Rewards);

            SaveRewardData(CardData);
        }

        public void UpdateBadgeData()
        {
            // Update as Badge Research data is determined
            SaveRewardData(BadgeData);
        }

    }
}