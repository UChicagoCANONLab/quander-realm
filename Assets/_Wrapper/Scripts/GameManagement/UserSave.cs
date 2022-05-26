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

        public UserSave(string idString = "", string rewardID = "")
        {
            Events.AddReward += AddReward;
            Events.GetMinigameSaveData += GetMinigameSaveData;
            Events.UpdateMinigameSaveData += UpdateMinigameSaveData;

            rewards = new List<string>();
            minigameSaves = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

            if (!(idString.Equals(string.Empty)))
                id = FormatString(idString);

            AddReward(rewardID);
        }

        ~UserSave()
        {
            Events.AddReward -= AddReward;
            Events.GetMinigameSaveData -= GetMinigameSaveData;
            Events.UpdateMinigameSaveData -= UpdateMinigameSaveData;
        }

        private void AddReward(string rewardID)
        {
            if (rewardID.Equals(string.Empty))
                return;

            string formatted = FormatString(rewardID);
            if (rewards.Contains(formatted))
                return;

            rewards.Add(formatted);
            Events.UpdateRemoteSave?.Invoke();
        }

        private void UpdateMinigameSaveData(Game game, object data)
        {
            //todo: error catching?
            minigameSaves[(int)game] = JsonUtility.ToJson(data);
            Events.UpdateRemoteSave?.Invoke();
        }

        private object GetMinigameSaveData(Game game)
        {
            //todo: error catching?
            string saveString = minigameSaves[(int)game];

            if (saveString.Equals(string.Empty))
                return null;

            return JsonUtility.FromJson<object>(saveString);
        }

        private string FormatString(string rawString)
        {
            return rawString.Trim().ToLower();
        }
    }
}
