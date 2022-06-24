using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class JournalSection
    {
        private static int nextPageNumber = 0;
        private const string stateNormal = "Normal";
        private const string stateSelected = "Selected";
        private Toggle tab;
        private Animator tabAnimator;

        public List<JournalPage> pages;

        public JournalSection(GameObject tabGO)
        {
            Events.SwitchPage += UpdateTab;
            Events.ResetPageNumbers += ResetPageNumbers;

            InitSectionTab(tabGO);
            pages = new List<JournalPage>();
        }

        ~JournalSection()
        {
            Events.SwitchPage -= UpdateTab;
            Events.ResetPageNumbers -= ResetPageNumbers;
        }

        private void InitSectionTab(GameObject tabGO)
        {
            tab = tabGO.GetComponent<Toggle>();
            tabAnimator = tabGO.GetComponent<Animator>();
            tab.onValueChanged.AddListener(OpenSection);
        }

        private void OpenSection(bool isOn)
        {
            if (!(isOn))
                return;

            pages.First().ClickNavDot();
        }

        private void UpdateTab(JournalPage currentPage)
        {
            foreach (JournalPage page in pages)
                if (page.pageNumber == currentPage.pageNumber)
                    ToggleTabAnim(true);
        }

        public void ToggleTabAnim(bool isOn)
        {
            tab.SetIsOnWithoutNotify(isOn);
            tabAnimator.SetTrigger(isOn ? stateSelected : stateNormal);
        }

        public void AddCard(GameObject rewardGO)
        {
            AddPageIfNeeded();
            pages.Last().Add(rewardGO);
        }

        private void AddPageIfNeeded()
        {
            if (pages.Count == 0 || pages.Last().IsFull())
            {
                pages.Add(new JournalPage(nextPageNumber));
                nextPageNumber++;
            }
        }

        private void ResetPageNumbers()
        {
            nextPageNumber = 0;
        }
    }
}
