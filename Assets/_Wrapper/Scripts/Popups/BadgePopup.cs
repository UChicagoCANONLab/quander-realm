using System.Collections;
using UnityEngine;
using TMPro;

namespace Wrapper
{
    public class BadgePopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject badgeContainer;
        [SerializeField] private QButton okButton;
        [SerializeField] private QButton backgroundButton;

        [Header("Badge Display Objects")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Star[] stars;

        private void Awake()
        {
            okButton.onClick.AddListener(() => ToggleDisplay(false));
            backgroundButton.onClick.AddListener(() => ToggleDisplay(false));
        }

        public IEnumerator DisplayBadge(GameObject badgeGO)
        {
            // Events.PlaySound?.Invoke("W_Reward");
            // badgeGO.transform.SetParent(badgeContainer.transform);
            // badgeGO.GetComponent<Transform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
            // badgeGO.GetComponent<Animator>().SetBool("Mini", false);

            title.text = badgeGO.GetComponent<Badge>().titleText.text;
            description.text = badgeGO.GetComponent<Badge>().descriptionText.text;
            for(int i=0; i<5; i++) {
                stars[i].SetStar(
                    (i <= badgeGO.GetComponent<Badge>().starLevel - 1), 
                    (i <= badgeGO.GetComponent<Badge>().starStatus - 1)
                );
            }
            badgeGO.SetActive(false);
            
            ToggleDisplay(true);
            yield return 5f;
            ToggleDisplay(false);

            // while (!(badgeGO.activeInHierarchy))
            //     yield return null;
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
