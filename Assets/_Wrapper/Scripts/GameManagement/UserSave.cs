using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    [System.Serializable]
    public class UserSave
    {
        public string id = string.Empty;
        public string[] minigameSaves;
        public List<string> rewards;
        public bool introDialogueSeen = false;

        public UserSave(string idString = "", string rewardID = "")
        {
            rewards = new List<string>();
            minigameSaves = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

            if (!(idString.Equals(string.Empty)))
                id = idString.Trim();

            AddReward(rewardID);
        }

        public bool AddReward(string rewardID)
        {
            if (rewardID.Equals(string.Empty))
                return false;

            string formatted = FormatString(rewardID);
            if (rewards.Contains(formatted))
                return false;

            rewards.Add(formatted);
            return true;
        }

        public bool HasReward(string rewardID)
        {
            if (rewards == null)
                return false;

            return rewards.Contains(FormatString(rewardID));
        }

        public void UpdateMinigameSave(Game game, object data)
        {
            //todo: error catching?
            minigameSaves[(int)game] = JsonUtility.ToJson(data);
        }

        public string GetMinigameSave(Game game)
        {
            //todo: error catching?
            string saveString = minigameSaves[(int)game];
            if (saveString.Equals(string.Empty))
                return null;

            return saveString;
        }

        private string FormatString(string rawString)
        {
            return rawString.Trim().ToLower();
        }
    }
}
