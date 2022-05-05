using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class EndPanel : MonoBehaviour
    {
        public TextMeshProUGUI header;
        public Button keepPlayingButton;

        private void Awake()
        {
            GameEvents.SetEndPanelText.AddListener((text) => SetInfo(text));
            keepPlayingButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public void SetInfo(string text)
        {
            header.text = text;
        }
    }
}
