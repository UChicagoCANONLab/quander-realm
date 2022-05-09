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
            keepPlayingButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OnEnable()
        {
            BlackBoxEvents.SetEndPanelText += SetInfo;
        }

        private void OnDisable()
        {
            BlackBoxEvents.SetEndPanelText -= SetInfo;
        }

        public void SetInfo(string text)
        {
            header.text = text;
        }
    }
}
