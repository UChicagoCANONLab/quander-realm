
namespace BlackBox
{
    public class WinState
    {
        public bool levelWon = false;
        public int numNodes = 0;
        public int numCorrect = 0;
        public string rewardID = string.Empty;

        public WinState(int totalNodes, int correctFlags, string rewardID = "")
        {
            numNodes = totalNodes;
            numCorrect = correctFlags;
            this.rewardID = rewardID;

            levelWon = numNodes == numCorrect;
        }
    }
}
