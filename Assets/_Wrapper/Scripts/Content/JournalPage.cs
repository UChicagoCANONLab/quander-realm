using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class JournalPage
    {
        private const int maxListSize = 4;
        private List<GameObject> cardList;

        public JournalPage()
        {
            cardList = new List<GameObject>();
        }

        public bool IsFull()
        {
            return cardList.Count == maxListSize;
        }

        public void Add(GameObject rewardGO)
        {
            cardList.Add(rewardGO);
        }
    }
}
