
namespace Wrapper
{
    [System.Serializable]
    public class RewardSaveObject
    {
        public string Username = Events.GetPlayerResearchCode?.Invoke();

        public string currentCard = string.Empty;
        public string game = string.Empty;
        public string timeClicked = string.Empty;
        public string displayType = string.Empty;
        public string description = string.Empty;
        
        public int[] cardsPerGame = new int[] { 0, 0, 0, 0, 0 };
        public int totalCards = 0;
    }
}