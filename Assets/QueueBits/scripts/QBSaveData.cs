using System;
namespace QueueBits
{
    [System.Serializable]
    public class QBSaveData
    {
        public Wrapper.Game gameID = Wrapper.Game.QueueBits;
        public bool[] playDialogue = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
        public int[] levelStarCount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }
}
