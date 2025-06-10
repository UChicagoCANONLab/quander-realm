using Filament.Content;
using UnityEngine;

namespace Wrapper
{
    public class BonusAsset : ContentAsset
    {
        [SerializeField, ContentValue("Title")]
        public string title;

        [SerializeField, ContentValue("Game")]
        public Game game;

        [SerializeField, ContentValue("Cost")]
        public int cost;

        [SerializeField, ContentValue("Number")]
        public int number;

        [SerializeField, ContentValue("Effect")]
        public string effect;

        [SerializeField, ContentValue("Unlock Criteria")]
        public string unlockCriteria;

        [SerializeField, ContentValue("Criteria Type")]
        public CriteriaType criteriaType;
        
        [SerializeField, ContentValue("Dependency")]
        public string dependency;
        
        // [SerializeField, ContentValue("Description")]
        // public string description;
    }
}
