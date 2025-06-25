

namespace Wrapper 
{
    [System.Serializable]
    public class CardResearchData
    {
        public string Header = "Reward Card Data";

        public string currentCard = string.Empty;
        public string game = string.Empty;
        public string timeClicked = string.Empty;
        public string displayType = string.Empty;
        public string description = string.Empty;
        
        // Doesn't update all games, so total and array total can be different
        // public int[] cardsPerGame = new int[] { 0, 0, 0, 0, 0 };
        public int totalCards = 0;        
    }
}