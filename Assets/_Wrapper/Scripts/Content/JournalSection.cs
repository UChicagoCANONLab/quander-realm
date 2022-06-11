using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class JournalSection
    {
        private List<JournalPage> pages;
        private Toggle tab;
        private Animator tabAnimator;

        public JournalSection(GameObject tabGO)
        {
            pages = new List<JournalPage>();

            tab = tabGO.GetComponent<Toggle>();
            tabAnimator = tabGO.GetComponent<Animator>();
        }
        
        public void AddCard(GameObject rewardGO)
        {
            AddPageIfNeeded();
            pages.Last().Add(rewardGO);
        }

        private void AddPageIfNeeded()
        {
            if (pages.Count == 0 || pages.Last().IsFull())
                pages.Add(new JournalPage());
        }

        public JournalPage GetFirstPage()
        {
            return pages.First();
        }   

        public void ToggleTab(bool isOn)
        {
            tab.isOn = isOn;
            tabAnimator.SetTrigger(isOn ? "Normal" : "Selected");
        }
    }
}
