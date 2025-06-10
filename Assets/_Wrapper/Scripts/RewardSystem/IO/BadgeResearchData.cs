
namespace Wrapper
{
    [System.Serializable]
    public class BadgeResearchData
    {
        public string Username = Events.GetPlayerResearchCode?.Invoke();
        public string Header = "Badge Data";

        // Fill in as research data determined
    }
}