using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class DialogueView : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private TextMeshProUGUI dialogueBody = null;
        [SerializeField] private GameObject characterMountLeft = null;
        [SerializeField] private GameObject characterMountRight = null;
        [SerializeField] private Image contextImage = null; //todo: might have to work with container object instead of image

        private void Awake()
        {
            Events.OpenDialogueView += Open;
        }

        private void OnEnable()
        {
            Events.UpdateDialogueView += UpdateView;
        }

        private void OnDisable()
        {
            Events.UpdateDialogueView -= UpdateView;
            Events.OpenDialogueView -= Open;
        }

        private void Open(Dialogue dialogue)
        {
            animator.SetBool("View/On", true);
            UpdateView(dialogue);
        }

        private void UpdateView(Dialogue dialogue)
        {
            if (dialogue == null)
                CloseView();

            //update the different parts of the view (if needed)
            dialogueBody.text = dialogue.text;
        }

        private void CloseView()
        {
            animator.SetBool("View/On", false);
            //todo: wait for animation to complete
            gameObject.SetActive(false);
        }

        //listen for next/prev and change everything based on node
    }
}
