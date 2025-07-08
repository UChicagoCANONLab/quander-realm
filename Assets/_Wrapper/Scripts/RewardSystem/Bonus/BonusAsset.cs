using Filament.Content;
using UnityEngine;

namespace Wrapper
{
    public class BonusAsset : ContentAsset
    {
        [SerializeField, ContentValue("ID")]
        public string ID;
        
        [SerializeField, ContentValue("Title")]
        public string title;

        [SerializeField, ContentValue("Maximum")]
        public int maximum;
        
        [SerializeField, ContentValue("Cost")]
        public int[] cost;

        [SerializeField, ContentValue("Sequential")]
        public bool inSequence;

        [SerializeField, ContentValue("Effect")]
        public string effect;

        [SerializeField, ContentValue("Usable Scenes")]
        public string[] usableScenes;

        [SerializeField, ContentValue("Unlock Criteria")]
        public string[] unlockCriteria;
        
        [SerializeField, ContentValue("Dependency")]
        public string dependency;
        
        // [SerializeField, ContentValue("Description")]
        // public string description;
    }
}
