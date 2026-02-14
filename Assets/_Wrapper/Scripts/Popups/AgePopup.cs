using System;
using System.Collections;
using System.Collections.Generic; // Required for List<>
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
        public string[] ageGroups = { "8-10", "11-13", "13-18", "over 18" };
        
        // Define your placeholder text here
        private const string PLACEHOLDER_TEXT = "Select your age";

        private void Awake()
        {
            okButton.onClick.AddListener(() => StartCoroutine(SetAge()));
        }

        public IEnumerator DisplayAgePopup()
        {
            okButton.gameObject.SetActive(false);
            Events.PlaySound?.Invoke("W_Reward");

            // --- STEP 1: SETUP DROPDOWN ---
            ageSelector.ClearOptions();

            // Create a temporary list that starts with the placeholder
            List<string> options = new List<string>();
            options.Add(PLACEHOLDER_TEXT);
            options.AddRange(ageGroups); // Add the actual ages after

            ageSelector.AddOptions(options);
            
            // Set selection to 0 (The placeholder)
            ageSelector.value = 0;
            
            // Clear old listeners to prevent stacking if this popup is opened multiple times
            ageSelector.onValueChanged.RemoveAllListeners();
            ageSelector.onValueChanged.AddListener(delegate { dropdownValueChanged(ageSelector); });
            
            ToggleDisplay(true);

            yield return null;
        }

        public void dropdownValueChanged(TMP_Dropdown selector)
        {
            // --- STEP 2: CHECK FOR PLACEHOLDER ---
            // If the first option is still the placeholder...
            if (selector.options.Count > 0 && selector.options[0].text == PLACEHOLDER_TEXT)
            {
                // If they clicked the placeholder again, do nothing (keep OK disabled)
                if (selector.value == 0)
                {
                    okButton.gameObject.SetActive(false);
                    return; 
                }

                // If they picked a valid age (Index > 0)...
                // We remove the placeholder so it disappears from the list
                selector.options.RemoveAt(0);

                // IMPORTANT: Since we removed item 0, the list shifted up. 
                // The item at index 2 is now at index 1. We must correct the value.
                selector.value -= 1; 
                
                // Force the dropdown to refresh its visual caption immediately
                selector.RefreshShownValue(); 
            }

            // --- STEP 3: SET DATA ---
            // Now that the list is "clean" (placeholder gone), the indices match your array perfectly
            selection = ageGroups[selector.value];
            okButton.gameObject.SetActive(true);
            
            // Debug.Log("Selected: " + selection);
        }

        private void ToggleDisplay(bool isOn)
        {
            animator.SetBool("PopupOn", isOn);
        }

        // Setting age in database
        public IEnumerator SetAge()
        {
            ToggleDisplay(false);

            // Sending new age verification to database
            string userAgeJson = JsonUtility.ToJson(new UserAge(Events.GetPlayerResearchCode(), selection));
            byte[] ageBytes = new System.Text.UTF8Encoding().GetBytes(userAgeJson);

            string url = "https://backend-quantime.link/set_research_code_age";
            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
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
    }

    [System.Serializable]
    public class UserAge
    {
        public string Username = string.Empty;
        public string ageGroup;
        public string timestamp;

        public UserAge(string name, string group)
        {
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
