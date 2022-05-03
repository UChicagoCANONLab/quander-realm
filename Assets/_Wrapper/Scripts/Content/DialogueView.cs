using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class DialogueView : MonoBehaviour
    {
        [Header("DialogueView Parts")]
        [SerializeField] private Animator animator = null;
        [SerializeField] private TextMeshProUGUI dialogueBody = null;
        [SerializeField] private TextMeshProUGUI dupePage = null;
        [SerializeField] private GameObject characterMountLeft = null;
        [SerializeField] private GameObject characterMountRight = null;
        [SerializeField] private Image contextImage = null; //todo: might have to work with container object instead of image

        [Header("Character Prefabs")]
        [SerializeField] private GameObject char1Prefab = null;
        [SerializeField] private GameObject char2Prefab = null;
        [SerializeField] private GameObject char3Prefab = null;
        [SerializeField] private GameObject char4Prefab = null;
        [SerializeField] private GameObject char5Prefab = null;
        [SerializeField] private GameObject char6Prefab = null;

        private Dictionary<Dialogue.Speaker, GameObject> characterDictionary; // todo: what about Speaker.None?
        private Dialogue.Speaker characterLeft = Dialogue.Speaker.None;
        private Dialogue.Speaker characterRight = Dialogue.Speaker.None;
        private Dialogue.Expression ExpressionLeft = Dialogue.Expression.Default;
        private Dialogue.Expression ExpressionRight = Dialogue.Expression.Default;
        private string contextImagePath = string.Empty;
        private string tempDialogueText = string.Empty; //todo: refactor this?

        private void Awake()
        {
            InitCharacterDictionary();
        }

        private void OnEnable()
        {
            Events.OpenDialogueView += Open;
            Events.CloseDialogueView += Close;
            Events.UpdateDialogueView += UpdateView;
            Events.SwitchNextButton += SwitchNextButton;
        }

        private void OnDisable()
        {
            Events.OpenDialogueView -= Open;
            Events.CloseDialogueView -= Close;
            Events.UpdateDialogueView -= UpdateView;
            Events.SwitchNextButton -= SwitchNextButton;
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
            UpdateContextImage(dialogue);
            UpdateCharacters(dialogue);
            UpdateExpressions(dialogue);
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
                Instantiate(characterDictionary[dialogue.speaker], characterMountLeft.transform);
            }

            if (characterRight != dialogue.listener)
            {
                characterRight = dialogue.listener;
                ClearChildren(characterMountRight);
                Instantiate(characterDictionary[dialogue.listener], characterMountRight.transform);
            }
        }

        private void UpdateExpressions(Dialogue dialogue)
        {
            if (ExpressionLeft != dialogue.speakerExpression)
            {
                //do something
            }

            if (ExpressionRight != dialogue.listenerExpression)
            {
                //do something
            }
        }

        private void UpdateContextImage(Dialogue dialogue)
        {
            if (dialogue.contextImagePath != string.Empty)
                return;

            if (dialogue.contextImagePath == contextImagePath)
                return;

            Image image = Resources.Load<Image>(dialogue.contextImagePath);
            if (image == null)
            {
                Debug.LogFormat("Could not find image at path: {0}", dialogue.contextImagePath);
                return;
            }

            contextImagePath = dialogue.contextImagePath;

        }

        /// <summary>
        /// Switch the Next button image between "dismiss" and "next",
        /// depending on the current dialogue being the last or not
        /// </summary>
        private void SwitchNextButton(bool isLast)
        {
            animator.SetBool("DialogueLast", isLast);
        }

#endregion

        private void ClearChildren(GameObject mount)
        {
            foreach(Transform child in mount.transform)
                Destroy(child.gameObject);
        }

        private void InitCharacterDictionary()
        {
            characterDictionary = new Dictionary<Dialogue.Speaker, GameObject>
            {
                { Dialogue.Speaker.Char1, char1Prefab },
                { Dialogue.Speaker.Char2, char2Prefab },
                { Dialogue.Speaker.Char3, char3Prefab },
                { Dialogue.Speaker.Char4, char4Prefab },
                { Dialogue.Speaker.Char5, char5Prefab },
                { Dialogue.Speaker.Char6, char6Prefab }
            };
        }
    }
}
