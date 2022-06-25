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
        private Toggle tab;

        public List<JournalPage> pages;

        public JournalSection(GameObject tabGO)
        {
            Events.UpdateTab += UpdateTab;
            Events.ResetPageNumbers += ResetPageNumbers;

            InitSectionTab(tabGO);
            pages = new List<JournalPage>();
        }

        ~JournalSection()
        {
            Events.UpdateTab -= UpdateTab;
            Events.ResetPageNumbers -= ResetPageNumbers;
        }

        private void InitSectionTab(GameObject tabGO)
        {
            tab = tabGO.GetComponent<Toggle>();
            tab.onValueChanged.AddListener(OpenSection);
        }

        private void OpenSection(bool isOn)
        {
            try
            {
                if (isOn)
                    pages.First().ClickNavDot();
            }
            catch(Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        private void UpdateTab(JournalPage currentPage)
        {
            foreach (JournalPage page in pages)
                if (page.pageNumber == currentPage.pageNumber)
                    tab.SetIsOnWithoutNotify(true);
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
