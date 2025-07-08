using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Wrapper
{
    public class RewardData : MonoBehaviour
    {
        public string Username = Events.GetPlayerResearchCode?.Invoke();
        public string RewardResearchData = string.Empty;

        public void UpdateRewardData(System.Object data)
        {
            Username = Events.GetPlayerResearchCode?.Invoke();
            RewardResearchData = JsonUtility.ToJson(data);
        }
    }
}