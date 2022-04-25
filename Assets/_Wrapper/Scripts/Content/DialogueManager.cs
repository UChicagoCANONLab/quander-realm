using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class DialogueManager : MonoBehaviour
    {
        public DialogueView dialogueView = null;

        private const string dialoguePath = "_Wrapper/Dialogue";
        private Dictionary<string, DialogueSequence> dialogueDictionary;

        private DialogueSequence currentSequence = null;

        #region Unity Functions

        private void Awake()
        {
            InitDictionary();
        }

        private void OnEnable()
        {
            Events.PrintDialogue += PrintSequence;
            Events.StartDialogueSequence += OpenDialogueView;
            Events.ChangeDialogue += UpdateDialogueNumber;
            //Events.DialoguePrevious += Previous;
        }

        private void OnDisable()
        {
            Events.PrintDialogue -= PrintSequence;
            Events.StartDialogueSequence -= OpenDialogueView;
            Events.ChangeDialogue -= UpdateDialogueNumber;
            //Events.DialoguePrevious -= Previous;
        }

        #endregion

        private void InitDictionary()
        {
            dialogueDictionary = new Dictionary<string, DialogueSequence>();
            Dialogue[] allDialogue = Resources.LoadAll<Dialogue>(dialoguePath);

            foreach (Dialogue node in allDialogue)
            {
                if (dialogueDictionary.ContainsKey(node.sequenceID))
                    dialogueDictionary[node.sequenceID].Add(node);
                else
                    dialogueDictionary.Add(node.sequenceID, new DialogueSequence(node));
            }

            Events.SortSequences?.Invoke();
        }

        private void OpenDialogueView(string sequenceID)
        {
            if (!(dialogueDictionary.ContainsKey(sequenceID)))
            {
                Debug.LogFormat("Could not find dialogue sequence with ID {0}", sequenceID);
                return;
            }

            dialogueView.gameObject.SetActive(true);
            currentSequence = dialogueDictionary[sequenceID];
            Events.OpenDialogueView?.Invoke(currentSequence.GetLine(0));
        }

        private void UpdateDialogueNumber(int step)
        {
            //get next dialogue from sequence, send to DialogueView
            Events.UpdateDialogueView?.Invoke(currentSequence.GetLine(step));
        }

        private void PrintSequence(string sequenceID)
        {
            foreach (Dialogue node in dialogueDictionary[sequenceID].Nodes)
                Debug.LogFormat("Num: {0} Text: {1}", node.num, node.text);
        }
    } 
}
