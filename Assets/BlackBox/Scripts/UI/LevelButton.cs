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

        public void SetButtonState(int currentLevel, int numStars)
        {
            if (currentLevel >= levelID)
            {
                interactable = true;
                if (currentLevel > levelID) buttonAnim.SetBool("LevelCompleted", true);
            }
            else
            {
                interactable = false;
                buttonAnim.SetBool("LevelLocked", true);
            }
            
            string prefix = "TrashPile/StarSystem/Star_";
            for (int i=0; i<numStars; i++) {
                gameObject.transform.Find($"{prefix}{i}/Star{i}").gameObject.SetActive(true);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable) return;

            base.OnPointerClick(eventData);

            if (levelID < 1 || levelID > 15)
            {
                Debug.LogError("Invalid level ID: " + levelID);
                return;
            }

            BBEvents.PlayLevel?.Invoke(BBGameManager.ParseLevelID(levelID));
        }
    }
}