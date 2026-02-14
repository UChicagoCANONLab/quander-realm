using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BeauRoutine;


namespace Wrapper
{
    public class MinigameMapIcon : MonoBehaviour
    {
        [SerializeField] private MinigameButton button;
        [SerializeField] private Image mapIcon;
        [SerializeField] private Sprite[] icons;
        [SerializeField] public Game game;
        [SerializeField] private GameObject costIcon;

        private int[] upgradeCost = {150, 300, 0};
        private int iconStatus = 0;


        public void SetMapIcon(int level)
        {
            if (level >= icons.Length) return;
            mapIcon.sprite = icons[level];
            
            iconStatus = level;
            if (level == 2)
            {
                costIcon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "MAX";
                costIcon.transform.GetChild(0).gameObject.SetActive(false);
            }
            else costIcon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{upgradeCost[level]}";
        }

        public void SetInteractable(bool active)
        {
            button.interactable = active;
        }

        public void UpgradeIcon()
        {
            if (iconStatus == 2) return;
            Routine.Start(UpgradeIconRoutine());
        }

        public IEnumerator UpgradeIconRoutine()
        {
            this.GetComponent<Animator>().SetTrigger("FadeOut");

            // Events.UpgradeMinigameMapIcon.Invoke(game, upgradeCost[Events.GetMinigameMapIcon.Invoke(game)]);
            Events.UpgradeMinigameMapIcon.Invoke(game, -1 * upgradeCost[iconStatus]);
            SetMapIcon(Events.GetMinigameMapIcon.Invoke(game));
            
            this.GetComponent<Animator>().SetTrigger("FadeIn");
            Events.ToggleUpgradable.Invoke(false);
            yield return 0f;
        }


        public void ToggleIconUpgradable(bool enabled)
        {
            button.upgradeMode = enabled;
            costIcon.SetActive(enabled);

            if (enabled) button.onClick.AddListener(UpgradeIcon);
            else button.onClick.RemoveListener(UpgradeIcon);
        }
    }
}