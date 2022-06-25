using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class DialogueView : MonoBehaviour
    {
        [Header("DialogueView Parts")]
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI dialogueBody;
        [SerializeField] private TextMeshProUGUI dupePage;
        [SerializeField] private GameObject characterMountLeft;
        [SerializeField] private GameObject characterMountRight;
        [SerializeField] private Image contextImage; //todo: might have to work with container object instead of image

        [Header("Character Prefabs")]
        [SerializeField] private GameObject mollyPrefab;
        [SerializeField] private GameObject tanglePrefab;
        [SerializeField] private GameObject bytePrefab;
        [SerializeField] private GameObject wolfiePrefab;
        [SerializeField] private GameObject battyPrefab;
        [SerializeField] private GameObject twinAPrefab;
        [SerializeField] private GameObject twinBPrefab;
        [SerializeField] private GameObject chefPrefab;

        private Dictionary<Character, GameObject> characterDictionary; // todo: what about Speaker.None?
        private Character characterLeft;
        private Character characterRight;
        private Expression ExpressionLeft;
        private Expression ExpressionRight;
        private Animator animatorCharacterLeft;
        private Animator animatorCharacterRight;
        
        private string contextImagePath;
        private string tempDialogueText; //todo: refactor this?

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
            InitCharacters(dialogue);
            UpdateView(dialogue);
        }

        private void Close()
        {
            animator.SetBool("View/On", false);
            //todo: wait for animation to complete
            gameObject.SetActive(false);
        }

        private void InitCharacters(Dialogue dialogue)
        {
            characterLeft = dialogue.speaker;
            characterRight = dialogue.listener;

            ClearChildren(characterMountLeft);
            ClearChildren(characterMountRight);

            //start with initial speaker on the left, initial listener on the right
            GameObject characterLeftGO = Instantiate(characterDictionary[dialogue.speaker], characterMountLeft.transform);
            GameObject characterRightGO = Instantiate(characterDictionary[dialogue.listener], characterMountRight.transform);

            animator.SetBool("CharacterLeft/On", true);
            animator.SetBool("CharacterRight/On", true);

            animatorCharacterLeft = characterLeftGO.GetComponentInChildren<Animator>();
            animatorCharacterRight = characterRightGO.GetComponentInChildren<Animator>();

            animatorCharacterLeft.SetBool("Speaking", false);
            animatorCharacterRight.SetBool("Speaking", false);
            animatorCharacterRight.SetBool("CharacterRight", true);
        }

        #region Update Elements

        private void UpdateView(Dialogue dialogue, int step = 1)
        {
            //todo: rethink closing here
            if (dialogue == null)
            {
                Close();
                return;
            }

            InitiateDialogueAnimation(dialogue, step);
            UpdateSpeakerHighlight(dialogue);
            UpdateExpressions(dialogue);
            UpdateContextImage(dialogue);
        }

        /// <summary>
        /// Start the first step of the dialogue snimation, the second step and dialogue body text switching are 
        /// triggered and called by an animation event. 
        /// </summary>
        /// <param name="step">positive = moving forward, negative = moving backward - in the dialogue sequence</param>
        private void InitiateDialogueAnimation(Dialogue dialogue, int step)
        {
            if (step > 0)
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
        /// Triggered by an animation event, this method switches dialogue text on the UI and triggeres the second part
        /// of the animation
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

        private void UpdateSpeakerHighlight(Dialogue dialogue)
        {
            if (dialogue.speaker == characterLeft)
            {
                animatorCharacterLeft.SetBool("Speaking", true);
                animatorCharacterRight.SetBool("Speaking", false);
            }
            else
            {
                animatorCharacterRight.SetBool("Speaking", true);
                animatorCharacterLeft.SetBool("Speaking", false);
            }
        }

        private void UpdateExpressions(Dialogue dialogue)
        {
            //todo: refactor
            if (characterLeft == dialogue.speaker)
            {
                if (ExpressionLeft != dialogue.speakerExpression)
                    animatorCharacterLeft.SetInteger("Expression", (int)dialogue.speakerExpression);

                if (ExpressionRight != dialogue.listenerExpression)
                    animatorCharacterRight.SetInteger("Expression", (int)dialogue.listenerExpression);
            }
            else
            {
                if (ExpressionLeft != dialogue.listenerExpression)
                    animatorCharacterLeft.SetInteger("Expression", (int)dialogue.listenerExpression);

                if (ExpressionRight != dialogue.speakerExpression)
                    animatorCharacterRight.SetInteger("Expression", (int)dialogue.speakerExpression);
            } 
        }

        private void UpdateContextImage(Dialogue dialogue)
        {
            if (dialogue.contextImagePath == string.Empty)
            {
                animator.SetBool("ContextImage/On", false);
                return;
            }

            //if (dialogue.contextImagePath == contextImagePath)
            //    return;

            Sprite sprite = Resources.Load<Sprite>(dialogue.contextImagePath);
            if (sprite == null)
            {
                Debug.LogFormat("Could not find image at path: {0}", dialogue.contextImagePath);
                return;
            }

            contextImage.sprite = sprite;
            //contextImagePath = dialogue.contextImagePath;
            animator.SetBool("ContextImage/On", true);
        }

        /// <summary>
        /// Switch the Next button image to either "dismiss" or "next", depending on the current dialogue being the 
        /// last or not
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
            characterDictionary = new Dictionary<Character, GameObject>
            {
                { Character.Molly, mollyPrefab },
                { Character.Tangle, tanglePrefab },
                { Character.Byte, bytePrefab },
                { Character.Wolfie, wolfiePrefab },
                { Character.Batty, battyPrefab },
                { Character.TwinA, twinAPrefab },
                { Character.TwinB, twinBPrefab },
                { Character.Chef, chefPrefab }
            };
        }
    }
}
