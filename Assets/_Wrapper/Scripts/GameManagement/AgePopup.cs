using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Wrapper;
using UnityEngine.UI;
using TMPro;

namespace Wrapper
{
    public class AgePopup : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject gameContainer;
        [SerializeField] private QButton okButton;
        public TMP_Dropdown ageSelector;
        public string selection;
        public string[] ageGroups = {"8-10", "11-13", "13-18", "over 18"};

        private void Awake()
        {
            okButton.onClick.AddListener(() => StartCoroutine(SetAge()));
        }

        public IEnumerator DisplayAgePopup()
        {
            okButton.gameObject.SetActive(false);
            Events.PlaySound?.Invoke("W_Reward");

            ageSelector.onValueChanged.AddListener(delegate { dropdownValueChanged(ageSelector); });
            
            ToggleDisplay(true);

            yield return null;
        }

        private void ToggleDisplay(bool isOn) {
            animator.SetBool("PopupOn", isOn);
        }

        // Setting age in database
        public IEnumerator SetAge() {
            ToggleDisplay(false);

            // Sending new age verification to database
            string userAgeJson = JsonUtility.ToJson(new UserAge(Events.GetPlayerResearchCode(), selection));
            byte[] ageBytes = new System.Text.UTF8Encoding().GetBytes(userAgeJson);

            string url = "https://backend-quantime.link/set_research_code_age";
            using (UnityWebRequest www = new UnityWebRequest (url, "POST"))
            {
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(ageBytes);
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Could not send age verification to aws: " + www.error);
                }
            }
        }

        public void dropdownValueChanged(TMP_Dropdown selector) {
            selection = ageGroups[selector.value];
            okButton.gameObject.SetActive(true);
            // Debug.Log(selection);
        }
    }

    [System.Serializable]
    public class UserAge
    {
        public string Username = string.Empty;
        public string ageGroup;
        public string timestamp;

        public UserAge(string name, string group) {
            Username = name;
            ageGroup = group;
            timestamp = DateTime.Now.ToString();
        }
    }


    [System.Serializable]
    public class UserCode
    {
        public string Username = string.Empty;

        public UserCode(string name) {
            Username = name;
        }
    }

}
