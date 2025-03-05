
namespace BlackBox
{
    [System.Serializable]
    public class BBResearchData
    {
        public string Username = Wrapper.Events.GetPlayerResearchCode?.Invoke();
        public string BBSaveDataString = string.Empty;
        public string winStateString = string.Empty;

        public float timePlayed;
        public int hintsUsed = 0; 
    }
}
