using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class EndPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private Button keepPlayingButton;

        private void Awake()
        {
            keepPlayingButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public void UpdatePanel(WinState winState)
        {
            if (winState.levelWon)
            {
                header.text = "You Won!";
                Debug.Log("Win");
            }
            else
            {
                header.text = "You found " + winState.numCorrect + " out of " + winState.numNodes + " nodes.";
                Debug.LogFormat("Lose: {0}/{1}", winState.numCorrect, winState.numNodes);
            }
        }
    }
}
