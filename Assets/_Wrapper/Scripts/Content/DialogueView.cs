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
        [SerializeField] private TextMeshProUGUI dupePage = null;
        [SerializeField] private GameObject characterMountLeft = null;
        [SerializeField] private GameObject characterMountRight = null;
        [SerializeField] private Image contextImage = null; //todo: might have to work with container object instead of image

        private Dialogue.Speaker characterLeft = Dialogue.Speaker.None;
        private Dialogue.Speaker characterRight = Dialogue.Speaker.None;
        private string tempDialogueText = string.Empty;

        private void OnEnable()
        {
            Events.OpenDialogueView += Open;
            Events.CloseDialogueView += Close;
            Events.UpdateDialogueView += UpdateView;
        }

        private void OnDisable()
        {
            Events.OpenDialogueView -= Open;
            Events.CloseDialogueView -= Close;
            Events.UpdateDialogueView -= UpdateView;
        }

        private void Open(Dialogue dialogue)
        {
            dialogueBody.text = "";
            animator.SetBool("View/On", true);
            UpdateView(dialogue);
        }

        private void Close()
        {
            animator.SetBool("View/On", false);
            //todo: wait for animation to complete
            gameObject.SetActive(false);
        }

#region Update Elements

        /// <summary>
        /// 
        /// </summary>
        private void UpdateView(Dialogue dialogue, int step = 1)
        {
            if (dialogue == null)
            {
                Close();
                return;
            }

            InitiateDialogueAnimation(dialogue, step);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        private void InitiateDialogueAnimation(Dialogue dialogue, int step)
        {
            if (step > 0) //Next
            {
                dupePage.text = dialogue.text;
                animator.SetTrigger("DialogueNext");
            }
            else
            {
                dupePage.text = dialogueBody.text;
                tempDialogueText = dialogue.text;
                animator.SetTrigger("DialoguePrevious");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SwitchDialogueBody(int step)
        {
            if (step > 0) //Next
            {
                dialogueBody.text = dupePage.text;
                animator.SetTrigger("DialogueNext");
            }
            else
            {
                dialogueBody.text = tempDialogueText;
                animator.SetTrigger("DialoguePrevious");
            }
        }

        private void UpdateCharacters(Dialogue dialogue)
        {
            //if it's different
            if (characterLeft != dialogue.speaker)
            {
                characterLeft = dialogue.speaker;
                ClearChildren(characterMountLeft);
                //instantiate new character
            }
        }
        
#endregion

        private void ClearChildren(GameObject mount)
        {
            foreach(Transform child in mount.transform)
                Destroy(child.gameObject);
        }

        //listen for next/prev and change everything based on node
    }
}
