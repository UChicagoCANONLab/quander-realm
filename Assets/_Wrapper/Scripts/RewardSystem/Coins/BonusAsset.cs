using Filament.Content;
using UnityEngine;

namespace Wrapper
{
    public class BonusAsset : ContentAsset
    {
        [SerializeField, ContentValue("Title")]
        public string title;

        [SerializeField, ContentValue("Cost")]
        public int cost;

        [SerializeField, ContentValue("Effect")]
        public string effect;

        [SerializeField, ContentValue("Unlock Criteria")]
        public string unlockCriteria;
        
        [SerializeField, ContentValue("Dependency")]
        public string dependency;
    }
}
