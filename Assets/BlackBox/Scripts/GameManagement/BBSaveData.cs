
namespace BlackBox
{
    [System.Serializable]
    public class BBSaveData
    {
        public Wrapper.Game gameID = Wrapper.Game.BlackBox;
        public string currentLevelID = string.Empty;
        public bool[] tutorialsSeen = new bool[] { false, false, false, false, false };
        public bool completed = false;
        public int[] livesPerLevel = new int[15];
        public int totalStars = 0;
    }
}
