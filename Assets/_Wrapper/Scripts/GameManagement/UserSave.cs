using System.Collections.Generic;

namespace Wrapper
{
    public class UserSave
    {
        public string id = string.Empty;
        public List<string> rewards;

        public UserSave()
        {
            rewards = new List<string>();
        }

        public UserSave(string id)
        {
            rewards = new List<string>();
            this.id = id.Trim().ToLower();
        }

        public UserSave(string id, string rewardID)
        {
            rewards = new List<string>();
            this.id = id.Trim().ToLower();
            string formattedReward = rewardID.Trim().ToLower();

            if (!(rewards.Contains(formattedReward)))
                rewards.Add(formattedReward);
        }

        //UpdateProgress(minigame, level, rewardID)
    }
}
