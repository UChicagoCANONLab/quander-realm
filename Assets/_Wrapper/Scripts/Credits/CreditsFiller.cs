using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Wrapper
{
    public class CreditsFiller : MonoBehaviour
    {
        bool loaded = false;

        [SerializeField]
        TextMeshProUGUI orgContainer;
        [SerializeField]
        UnityEngine.UI.Button backButton;

        private void Awake()
        {
            orgContainer.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            backButton.onClick.AddListener(CloseCredits);
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(CloseCredits);
        }

        public void LoadCredits(Credits credits)
        {
            if (loaded) return;

            TextMeshProUGUI currentOrg = null;
            CreditsLineItem nameTitlePair = null;

            foreach (var entry in credits.Entries)
            {
                if (string.IsNullOrEmpty(entry.Title))
                {
                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        Debug.LogWarning("No data set for this line");
                        continue;
                    }

                    // organization entry

                    currentOrg = Instantiate(orgContainer, orgContainer.transform.parent, false);
                    nameTitlePair = currentOrg.gameObject.GetComponentInChildren<CreditsLineItem>(true);
                    currentOrg.gameObject.SetActive(true);
                    
                    currentOrg.text = entry.Name;
                }
                else
                {
                    // person entry

                    if (currentOrg == null || nameTitlePair == null)
                    {
                        Debug.LogError("Cannot load names, need to provide organization first");
                        return;
                    }

                    var newLine = Instantiate(nameTitlePair, nameTitlePair.transform.parent, false);
                    newLine.gameObject.SetActive(true);
                    newLine.LoadText(entry.Name, entry.Title);
                }
            }

            loaded = true;
        }

        void CloseCredits()
        {
            gameObject.SetActive(false);
        }
    }
}