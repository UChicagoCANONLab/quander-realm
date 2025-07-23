using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class MinigameMapIcon : MonoBehaviour
    {
        [SerializeField] private MinigameButton button;
        [SerializeField] private Image mapIcon;
        [SerializeField] private Sprite[] icons;
        [SerializeField] private Game game;

        private int[] upgradeCost = {-150, -300};


        public void SetMapIcon(int level)
        {
            if (level >= icons.Length) return;
            mapIcon.sprite = icons[level];
        }

        public void SetInteractable(bool active)
        {
            button.interactable = active;
        }

        public void UpgradeIcon()
        {
            // Animation FadeOut
            Events.UpgradeMinigameMapIcon.Invoke(game, cost[Events.GetMinigameMapIcon.Invoke(game)]);
            SetMapIcon(Events.GetMinigameMapIcon.Invoke(game));
            // Animation FadeIn
        }
    }
}