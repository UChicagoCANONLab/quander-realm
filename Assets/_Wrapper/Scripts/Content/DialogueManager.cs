using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class DialogueManager : MonoBehaviour
    {
        private const string dialoguePath = "_Wrapper/Dialogue";
        private Dictionary<string, DialogueSequence> dialogueDict;

        private void Awake()
        {
            Events.PrintDialogue.AddListener((sID) => PrintSequence(sID));

            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            dialogueDict = new Dictionary<string, DialogueSequence>();
            DialogueNode[] allDialogue = Resources.LoadAll<DialogueNode>(dialoguePath);

            foreach (DialogueNode node in allDialogue)
            {
                if (dialogueDict.ContainsKey(node.sequenceID))
                    dialogueDict[node.sequenceID].Add(node);
                else
                    dialogueDict.Add(node.sequenceID, new DialogueSequence(node));
            }
        }

        private void PrintSequence(string sequenceID)
        {
            foreach (DialogueNode node in dialogueDict[sequenceID].nodes)
                Debug.LogFormat("Num: {0} Text: {1}", node.num, node.text);
        }
    } 
}
