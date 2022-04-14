using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class LanternMount : MonoBehaviour, IDropHandler, IPointerExitHandler
    {
        private Vector3Int gridPosition = Vector3Int.back;
        public bool isEmpty = true;

        public GameObject[] colliders;

        private void Start()
        {
            CheckIfEmpty();
        }

        private void CheckIfEmpty()
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Lantern>() != null)
                {
                    isEmpty = false;
                    break;
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject lanternGO = eventData.pointerDrag;

            if (isEmpty)
            {
                lanternGO.transform.SetParent(this.transform);
                lanternGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                if (gridPosition != Vector3Int.back) // using "back" or (0, 0, -1) as default. Z will be 0 if this mount belongs to a cell
                    GameEvents.ToggleFlag.Invoke(gridPosition, true);

                isEmpty = false;
            }
            else
                GameEvents.ReturnToHome?.Invoke(lanternGO);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag?.GetComponent<Lantern>() != null) // check if lantern is being held by the mouse pointer
            {
                GameEvents.ToggleFlag.Invoke(gridPosition, false);
                isEmpty = true;
            }
        }

        public void SetGridPosition(Vector3Int position)
        {
            gridPosition = position;
        }

        public void SetColliderActive(GridSize gridSize)
        {
            foreach (GameObject GO in colliders)
                GO.SetActive(false);

            colliders[(int)gridSize].SetActive(true);
        }
    }
}
