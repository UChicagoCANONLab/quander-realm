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

        public void SetMapIcon(int level)
        {
            if (level >= icons.Length) return;
            mapIcon.sprite = icons[level];
        }

        public void SetInteractable(bool active)
        {
            button.interactable = active;
        }
    }
}