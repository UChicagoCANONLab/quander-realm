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
        private Animator tabAnimator;
        private const string stateNormal = "Normal";
        private const string stateSelected = "Selected";

        public List<JournalPage> pages;

        public JournalSection(GameObject tabGO)
        {
            pages = new List<JournalPage>();
            InitSectionTab(tabGO);
            Events.ResetPageNumbers += ResetPageNumbers;
        }

        ~JournalSection()
        {
            Events.ResetPageNumbers -= ResetPageNumbers;
        }

        private void ResetPageNumbers()
        {
            nextPageNumber = 0;
        }

        private void InitSectionTab(GameObject tabGO)
        {
            tabAnimator = tabGO.GetComponent<Animator>();
            tab = tabGO.GetComponent<Toggle>();
            tab.onValueChanged.AddListener((isOn) => OpenSection(isOn));
        }

        private void OpenSection(bool isOn)
        {
            if (isOn)
            {
                //Events.OpenSection?.Invoke(pages.First().pageNumber);
                Events.OpenJournalPage?.Invoke(pages.First());
            }
        }

        public void ToggleTabAnim(bool isOn)
        {
            Debug.LogFormat("toggleTabAnim {0}: {1}", pages.First().cardList.First().GetComponent<Reward>().game, isOn);
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
                pages.Add(new JournalPage());
                pages.Last().pageNumber = nextPageNumber;
                nextPageNumber++;
            }
        }
    }
}
