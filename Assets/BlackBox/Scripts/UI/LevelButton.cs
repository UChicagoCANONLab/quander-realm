using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Wrapper;

namespace BlackBox
{
    public class LevelButton : QButton
    {
        [SerializeField]
        int levelID = -1;
        [SerializeField]
        Animator buttonAnim;

        public void SetButtonState(int maxLevel, int numStars)
        {
            if (maxLevel >= levelID)
            {
                interactable = true;
                buttonAnim.SetBool("LevelLocked", false);
                if (maxLevel > levelID) {
                    buttonAnim.SetBool("LevelCompleted", true);
                    buttonAnim.SetInteger("StarsWon", numStars);
                }
            }
            else
            {
                interactable = false;
                buttonAnim.SetBool("LevelLocked", true);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable) return;

            base.OnPointerClick(eventData);

            // if (levelID < 1 || levelID > 15)
            if (levelID < 1 || levelID > 24)
            {
                Debug.LogError("Invalid level ID: " + levelID);
                return;
            }

            BBEvents.PlayLevel?.Invoke(BBGameManager.ParseLevelID(levelID));
        }

    }
}