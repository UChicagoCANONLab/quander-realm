using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class JournalSection
    {
        private List<JournalPage> pages;
        private GameObject rewardPrefab;

        public JournalSection(GameObject prefab)
        {
            pages = new List<JournalPage>();
            rewardPrefab = prefab;
        }

        public void AddReward(RewardAsset asset)
        {
            //if current page doesn't have room
            //create page
            //set page's game variable
            //set isFirst
            //add to pageList

            //create reward
            //add to page
        }
    }
}
