using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;
using Qupcakery;

namespace Wrapper
{
    public class BackButton : QButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (SceneManager.GetActiveScene().name == "QU_Level")
            {
                GameUtilities.UnpauseGame();
                GameManagement.Instance.game.gameStat.SetLevelResultAndSave(GameStat.LevelResult.QUIT);
                GameObjectsManagement.ResetAllGameObjects();
                GameObjectsManagement.DeactiveAllGameObjects();
            }

            if (SceneManager.GetActiveScene().buildIndex == 0)
                return;

            SceneManager.LoadScene(0);
            Events.MinigameClosed?.Invoke();
        }
    }
}