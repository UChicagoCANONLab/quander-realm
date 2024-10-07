using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

namespace Wrapper
{
    public class QButton : Button
    {
        [SerializeField] protected string audioName = string.Empty;
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            Events.PlaySound?.Invoke(audioName.Equals(string.Empty)? "ButtonClick" : audioName);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            // If clicking reward card, then save card info
            // Doing this here even though it would be better in Reward.cs becauses idk how to do it there
            if (eventData.pointerCurrentRaycast.gameObject.name == "Hitbox" && SceneManager.GetActiveScene().name == "RewardCollection") {
                // Debug.Log(eventData.pointerPress);
                Reward currCard = eventData.pointerPress.GetComponent<Reward>();

                RewardResearchData.Instance.currentCard = currCard.titleFront.text; //name of card
                RewardResearchData.Instance.timeClicked = DateTime.Now.ToString("HH:mm:ss tt"); //when they clicked it
                RewardResearchData.Instance.displayType = currCard.displayType.ToString(); //InJournal=select new, Featured=flip
                RewardSave.Instance.SaveRewardResearchData();
            }

            base.OnPointerClick(eventData);
        }

        //public override void OnPointerUp(PointerEventData eventData)
        //{
        //    base.OnPointerUp(eventData);
        //    OnUp.Invoke();
        //}
    }
}