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
            InitSectionTab(tabGO);
        }

        private void InitSectionTab(GameObject tabGO)
        {
            tabAnimator = tabGO.GetComponent<Animator>();
            tab = tabGO.GetComponent<Toggle>();
            tab.onValueChanged.AddListener((isOn) => OpenSection(isOn));
        }

        private void OpenSection(bool isOn)
        {
            ToggleTab(isOn);

            if (isOn)
                Events.OpenJournalPage(pages.First());
        }

        public void AddCard(GameObject rewardGO)
        {
            AddPageIfNeeded();
            pages.Last().Add(rewardGO);
        }

        #region Helpers

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
            tabAnimator.SetTrigger(isOn ? "Selected" : "Normal");
        }

        #endregion
    }
}
