
namespace BlackBox
{
    [System.Serializable]
    public class BBResearchData
    {
        public string Username = Wrapper.Events.GetPlayerResearchCode?.Invoke();
        public string BBSaveDataString = string.Empty;

        public float timePlayed;
        public bool wonLevel;

        public int hintsUsed = 0;
        public int starsWon = 3; 
    }
}
