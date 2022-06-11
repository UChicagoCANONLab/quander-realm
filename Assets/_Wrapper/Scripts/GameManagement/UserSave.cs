using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    [System.Serializable]
    public class UserSave
    {
        public string id = string.Empty;

        private string[] minigameSaves;
        private List<string> rewards;

        public UserSave(string idString = "", string rewardID = "")
        {
            rewards = new List<string>();
            minigameSaves = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

            if (!(idString.Equals(string.Empty)))
                id = FormatString(idString);

            AddReward(rewardID);
        }

        public void AddReward(string rewardID)
        {
            if (rewardID.Equals(string.Empty))
                return;

            string formatted = FormatString(rewardID);
            if (rewards.Contains(formatted))
                return;

            rewards.Add(formatted);
            Events.UpdateRemoteSave?.Invoke();
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
