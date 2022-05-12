using UnityEngine;
using UnityEngine.EventSystems;

namespace BlackBox
{
    public class LanternMount : MonoBehaviour, IDropHandler, IPointerExitHandler
    {
        private Vector3Int gridPosition = Vector3Int.back;
        private GameObject mountedLantern = null;

        public bool isEmpty = true;
        public GameObject[] colliders;

        [SerializeField] private Animator nodeCellAnimator = null;

        private void Start()
        {
            EvaluateEmpty();   
        }

        public void EvaluateEmpty()
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Lantern>() != null)
                {
                    isEmpty = false;
                    break;
                }
            }

            if (!isEmpty)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Lantern>() != null)
                    {
                        mountedLantern = child.gameObject;
                        break;
                    }
                }
            }
        }

        public void SetColliderActive(GridSize gridSize)
        {
            foreach (GameObject GO in colliders)
                GO.SetActive(false);

            colliders[(int)gridSize].SetActive(true);
        }

        public void SetGridPosition(Vector3Int position)
        {
            gridPosition = position;
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject droppedLantern = eventData.pointerDrag;

            if (isEmpty)
            {
                droppedLantern.transform.SetParent(this.transform);
                droppedLantern.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                if (gridPosition != Vector3Int.back) // using "back" or (0, 0, -1) as default. Z will be 0 instead if this mount belongs to a cell
                    BBEvents.ToggleFlag.Invoke(gridPosition, true);

                mountedLantern = droppedLantern;
                isEmpty = false;

                if (nodeCellAnimator != null)
                    nodeCellAnimator.SetBool("NodeCell/Flagged", true);
            }
            else
            {
                BBEvents.ReturnLanternHome?.Invoke(droppedLantern);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isEmpty) return;
            if (eventData.pointerDrag == null) return;
            if (eventData.pointerDrag.GetComponent<Lantern>() == null) return;
            if (eventData.pointerDrag != mountedLantern) return;

            BBEvents.ToggleFlag.Invoke(gridPosition, false);
            isEmpty = true;

            if (nodeCellAnimator != null)
                nodeCellAnimator.SetBool("NodeCell/Flagged", false);
        }
    }
}
