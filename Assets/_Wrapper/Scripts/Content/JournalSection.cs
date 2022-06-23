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
        //private Toggle tab;
        private Button tab;
        private Animator tabAnimator;
        private const string stateNormal = "Normal";
        private const string stateSelected = "Selected";

        public List<JournalPage> pages;

        public JournalSection(GameObject tabGO)
        {
            pages = new List<JournalPage>();
            InitSectionTab(tabGO);

            //Events.SwitchPage += UpdateTab;
            Events.ResetPageNumbers += ResetPageNumbers;
        }

        ~JournalSection()
        {
            //Events.SwitchPage -= UpdateTab;
            Events.ResetPageNumbers -= ResetPageNumbers;
        }

        private void InitSectionTab(GameObject tabGO)
        {
            tab = tabGO.GetComponent<Button>();
            //tab.onClick.AddListener(OpenSection);
            tabAnimator = tabGO.GetComponent<Animator>();
        }

        private void OpenSection()
        {
            pages.First().ClickNavDot();
        }

        public void ToggleTabAnim(bool isOn)
        {
            Debug.LogFormat("toggleTabAnim {0}: {1}", pages.First().cardList.First().GetComponent<Reward>().game, isOn);
            tabAnimator.SetTrigger(stateNormal);

            if (isOn)
                tabAnimator.SetTrigger(stateSelected);
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

        private void UpdateTab(JournalPage currentPage)
        {
            foreach (JournalPage page in pages)
                if (page.pageNumber == currentPage.pageNumber)
                    ToggleTabAnim(true);
        }

        private void ResetPageNumbers()
        {
            nextPageNumber = 0;
        }
    }
}
