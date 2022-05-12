
namespace BlackBox
{
    public class WinState
    {
        public bool levelWon = false;
        public int numNodes = 0;
        public int numCorrect = 0;
        public int livesRemaining = 0;
        public string rewardID = string.Empty;

        public WinState(int totalNodes, int correctFlags, bool levelWon, int livesRemaining, string rewardID = "")
        {
            numNodes = totalNodes;
            numCorrect = correctFlags;
            this.livesRemaining = livesRemaining;
            this.levelWon = levelWon;
            this.rewardID = rewardID;
        }
    }
}
