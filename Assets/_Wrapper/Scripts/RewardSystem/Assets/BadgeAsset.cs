using Filament.Content;
using UnityEngine;

namespace Wrapper
{
    public class BadgeAsset : ContentAsset
    {
        [SerializeField, ContentValue("Game")]
        public Game game;

        [SerializeField, ContentValue("Difficulty")]
        public int difficulty;

        [SerializeField, ContentValue("Title")]
        public string title;

        /* [SerializeField, ContentValue("Card Type")]
        public CardType cardType;

        [SerializeField, ContentValue("Sticker Title")]
        public string title;

        [SerializeField, ContentValue("Back Text")]
        public string backText;

        [SerializeField, ContentValue("Front Flavor Text")]
        public string flavorText;

        [SerializeField, ContentValue("Front Image Path")]
        public string imagePath; */
    }
}
