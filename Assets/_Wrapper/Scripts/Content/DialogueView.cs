using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BeauRoutine;
using System.Collections;

namespace Wrapper
{
    public class DialogueView : MonoBehaviour
    {
        #region Variables

        [Header("DialogueView Parts")]
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI dialogueBody;
        [SerializeField] private TextMeshProUGUI dupePage;
        [SerializeField] private Image contextImage; //todo: might have to work with container object instead of image

        [Space(10)]
        [SerializeField] private GameObject charMountLeft;
        [SerializeField] private Image charNameBGLeft;
        [SerializeField] private TextMeshProUGUI charNameTextLeft;

        [Space(10)]
        [SerializeField] private GameObject charMountRight;
        [SerializeField] private Image charNameBGRight;
        [SerializeField] private TextMeshProUGUI charNameTextRight;

        [Header("Character Prefabs")]
        [SerializeField] private GameObject mollyPrefab;
        [SerializeField] private GameObject tanglePrefab;
        [SerializeField] private GameObject bytePrefab;
        [SerializeField] private GameObject wolfiePrefab;
        [SerializeField] private GameObject twinAPrefab;
        [SerializeField] private GameObject twinBPrefab;
        [SerializeField] private GameObject twinsBothPrefab;
        [SerializeField] private GameObject chefPrefab;

        private Dictionary<Character, GameObject> charDictionary; // todo: what about Speaker.None?
        private Color nameTextColorSpeaker = Color.white;
        private Color nameTextColorListener;

        private Color nameBGColorSpeaker = Color.white;
        private Color nameBGColorListener;

        private Character charLeft;
        private Expression expressionLeft;
        private Animator animatorCharLeft;

        private Character charRight;
        private Expression expressionRight;
        private Animator animatorCharRight;
        
        private string contextImagePath;
        private string tempDialogueText; //todo: refactor this?

        //On main animator
        private const int contextImagePosLayerIndex = 2;
        private const int contextImageShakeLayerIndex = 3;
        private const int leftCharAnimLayerIndex = 5;
        private const int rightCharAnimLayerIndex = 6;
        
        //On character animators
        private const int charAnimExpressionLayerIndex = 2;
        private const int charAnimSpeakingLayerIndex = 3;

        #endregion

        #region Unity Functions, Opening/Closing

        private void Awake()
        {
            SetupListenerColors();
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
            Routine.Start(InitCharacters(dialogue));
            UpdateView(dialogue);
        }

        private void Close()
        {
            Routine.Start(CloseRoutine());
        }

        private IEnumerator CloseRoutine()
        {
            animator.SetBool("View/On", false);
            yield return animator.WaitToCompleteAnimation();
            gameObject.SetActive(false);
        }

        #endregion

        #region Initialization

        private IEnumerator InitCharacters(Dialogue dialogue)
        {
            // if switching characters mid-sequence
            if (animator.GetBool("CharacterLeft/On") && animator.GetBool("CharacterRight/On"))
            {
                animator.SetBool("CharacterLeft/On", false);
                animator.SetBool("CharacterRight/On", false);

                yield return animator.WaitToCompleteAnimation(leftCharAnimLayerIndex);
                yield return animator.WaitToCompleteAnimation(rightCharAnimLayerIndex);
            }

            //on init, start with initial speaker on the left, initial listener on the right
            charLeft = dialogue.speaker;
            charRight = dialogue.listener;

            ClearChildren(charMountLeft);
            ClearChildren(charMountRight);

            charNameTextLeft.text = dialogue.speaker.ToString();
            charNameTextRight.text = dialogue.listener.ToString();

            GameObject charLeftGO = Instantiate(charDictionary[dialogue.speaker], charMountLeft.transform);
            GameObject charRightGO = Instantiate(charDictionary[dialogue.listener], charMountRight.transform);

            animator.SetBool("CharacterLeft/On", true);
            animator.SetBool("CharacterRight/On", true);

            animatorCharLeft = charLeftGO.GetComponentInChildren<Animator>();
            animatorCharRight = charRightGO.GetComponentInChildren<Animator>();

            animatorCharLeft.SetBool("Speaking", false);
            animatorCharRight.SetBool("Speaking", false);
            animatorCharRight.SetBool("CharacterRight", true);
        }

        private void InitCharacterDictionary()
        {
            charDictionary = new Dictionary<Character, GameObject>
            {
                { Character.Molly,      mollyPrefab },
                { Character.Tangle,     tanglePrefab },
                { Character.Byte,       bytePrefab },
                { Character.Wolfie,     wolfiePrefab },
                { Character.TwinA,      twinAPrefab },
                { Character.TwinB,      twinBPrefab },
                { Character.TwinsBoth,  twinsBothPrefab },
                { Character.Chef,       chefPrefab }
            };
        }

        private void SetupListenerColors()
        {
            ColorUtility.TryParseHtmlString("#DAD5E0", out nameTextColorListener);
            ColorUtility.TryParseHtmlString("#BCB4C5", out nameBGColorListener);
        }

        #endregion

        #region Update Elements

        private void UpdateView(Dialogue dialogue, int step = 1)
        {
            Routine.Start(UpdateViewRoutine(dialogue, step));
        }

        private IEnumerator UpdateViewRoutine(Dialogue dialogue, int step = 1)
        {
            //todo: rethink closing here
            if (dialogue == null)
            {
                Close();
                yield break;
            }

            yield return UpdateCharacters(dialogue);
            InitiateDialogueAnimation(dialogue, step);
            yield return Routine.Combine
            (
                UpdateExpressions(dialogue),
                UpdateContextImage(dialogue),
                UpdateSpeakerHighlight(dialogue)
            );
        }

        private IEnumerator UpdateCharacters(Dialogue dialogue)
        {
            // If both characters are already present, return
            if (IsCharacterInView(dialogue.speaker) != Side.None && IsCharacterInView(dialogue.listener) != Side.None)
                yield break;

            // Otherwise, we need to replace one or both characters
            if (IsCharacterInView(dialogue.speaker) == Side.None && IsCharacterInView(dialogue.listener) == Side.None) // replace both
                yield return InitCharacters(dialogue);
            else
            {
                if (IsCharacterInView(dialogue.speaker) == Side.Left) // speaker is on the left
                    yield return NewCharacter(dialogue.listener, Side.Right); // new char on right

                else if (IsCharacterInView(dialogue.speaker) == Side.Right) // speaker is on the right
                    yield return NewCharacter(dialogue.listener, Side.Left); // new char on left

                else if (IsCharacterInView(dialogue.listener) == Side.Left) // listener is on the left
                    yield return NewCharacter(dialogue.speaker, Side.Right); // new char on the right
                
                else if (IsCharacterInView(dialogue.listener) == Side.Right) // listener is on the right
                    yield return NewCharacter(dialogue.speaker, Side.Left); // new char on the left
            }
        }

        private IEnumerator NewCharacter(Character character, Side side)
        {
            if (side == Side.Left)
            {
                animator.SetBool("CharacterLeft/On", false);
                yield return animator.WaitToCompleteAnimation(leftCharAnimLayerIndex);

                charLeft = character;
                ClearChildren(charMountLeft);
                charNameTextLeft.text = character.ToString();
                GameObject charLeftGO = Instantiate(charDictionary[character], charMountLeft.transform);
                animatorCharLeft = charLeftGO.GetComponentInChildren<Animator>();
                
                animator.SetBool("CharacterLeft/On", true);
                animatorCharLeft.SetBool("Speaking", false);
            }
            else
            {
                animator.SetBool("CharacterRight/On", false);
                yield return animator.WaitToCompleteAnimation(rightCharAnimLayerIndex);

                charRight = character;
                ClearChildren(charMountRight);
                charNameTextRight.text = character.ToString();
                GameObject charRightGO = Instantiate(charDictionary[character], charMountRight.transform);
                animatorCharRight = charRightGO.GetComponentInChildren<Animator>();
                
                animator.SetBool("CharacterRight/On", true);
                animatorCharRight.SetBool("Speaking", false);
                animatorCharRight.SetBool("CharacterRight", true);
            }
        }

        private IEnumerator UpdateSpeakerHighlight(Dialogue dialogue)
        {
            if (dialogue.speaker == charLeft)
            {
                animatorCharLeft.SetBool("Speaking", true);
                charNameTextLeft.color = nameTextColorSpeaker;
                charNameBGLeft.color = nameBGColorSpeaker;
                
                animatorCharRight.SetBool("Speaking", false);
                charNameTextRight.color = nameTextColorListener;
                charNameBGRight.color = nameBGColorListener;          
            }
            else
            {
                animatorCharRight.SetBool("Speaking", true);
                charNameTextRight.color = nameTextColorSpeaker;
                charNameBGRight.color = nameBGColorSpeaker;

                animatorCharLeft.SetBool("Speaking", false);
                charNameTextLeft.color = nameTextColorListener;
                charNameBGLeft.color = nameBGColorListener;
            }

            yield return animatorCharLeft.WaitToCompleteAnimation(charAnimSpeakingLayerIndex);
            yield return animatorCharRight.WaitToCompleteAnimation(charAnimSpeakingLayerIndex);            
        }

        private IEnumerator UpdateExpressions(Dialogue dialogue)
        {
            if (dialogue.speaker == charLeft)
            {
                animatorCharLeft.SetInteger("Expression", (int)dialogue.speakerExpression);
                expressionLeft = dialogue.speakerExpression;

                animatorCharRight.SetInteger("Expression", (int)dialogue.listenerExpression);
                expressionRight = dialogue.listenerExpression;
            }
            else
            {
                animatorCharLeft.SetInteger("Expression", (int)dialogue.listenerExpression);
                expressionLeft = dialogue.listenerExpression;

                animatorCharRight.SetInteger("Expression", (int)dialogue.speakerExpression);
                expressionRight = dialogue.speakerExpression;
            }

            yield return animatorCharLeft.WaitToCompleteAnimation(charAnimExpressionLayerIndex);
            yield return animatorCharRight.WaitToCompleteAnimation(charAnimExpressionLayerIndex);
        }

        private IEnumerator UpdateContextImage(Dialogue dialogue)
        {
            if (dialogue.contextImagePath == string.Empty)
            {
                animator.SetBool("ContextImage/On", false);
                yield break;
            }

            Sprite sprite = Resources.Load<Sprite>(dialogue.contextImagePath);
            if (sprite == null)
            {
                Debug.LogFormat("Could not find image at path: {0}", dialogue.contextImagePath);
                yield break;
            }

            contextImage.sprite = sprite;

            animator.SetBool("ContextImage/On", true);
            yield return Routine.Combine
            (
                animator.WaitToCompleteAnimation(contextImagePosLayerIndex),
                animator.WaitToCompleteAnimation(contextImageShakeLayerIndex)
            );
        }

        /// <summary>
        /// Start the first step of the dialogue snimation, the second step and dialogue body text switching are 
        /// triggered by an animation event. 
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

        private Side IsCharacterInView(Character character)
        {
            Side side = Side.None;

            if (character == charLeft)
                side = Side.Left;
            else if (character == charRight)
                side = Side.Right;

            return side;
        }
    }
}
