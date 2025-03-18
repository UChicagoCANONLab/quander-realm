using System.Collections;
using UnityEngine;

namespace Wrapper
{
    public class BadgePopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject badgeContainer;
        [SerializeField] private QButton okButton;
        [SerializeField] private QButton backgroundButton;

        private void Awake()
        {
            okButton.onClick.AddListener(() => ToggleDisplay(false));
            backgroundButton.onClick.AddListener(() => ToggleDisplay(false));
        }

        public IEnumerator DisplayBadge(GameObject badgeGO)
        {
            Events.PlaySound?.Invoke("W_Reward");
            badgeGO.transform.SetParent(badgeContainer.transform);
            badgeGO.GetComponent<Transform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);

            ToggleDisplay(true);

            while (!(badgeGO.activeInHierarchy))
                yield return null;
            badgeGO.GetComponent<Animator>().SetBool("Mini", false);
        }

        public GameObject GetContainerMount()
        {
            return badgeContainer;
        }

        private void ToggleDisplay(bool isOn)
        {
            animator.SetBool("PopupOn", isOn);
        }
    }
}
