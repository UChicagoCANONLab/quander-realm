using Filament.Content;
using UnityEngine;

namespace Wrapper
{
    public class BadgeAsset : ContentAsset
    {
        [SerializeField, ContentValue("Title")]
        public string title;

        [SerializeField, ContentValue("Game")]
        public Game game;

        [SerializeField, ContentValue("Description")]
        public string description;

        [SerializeField, ContentValue("Star Levels")]
        public int starLevels;

        [SerializeField, ContentValue("Criteria Type")]
        public CriteriaType criteriaType;

        [SerializeField, ContentValue("Criteria")]
        public int[] criteria;

        [SerializeField, ContentValue("Icon Name")]
        public string iconName;
        
        [SerializeField, ContentValue("Criteria Description")]
        public string criteriaDescription;
    }
}
