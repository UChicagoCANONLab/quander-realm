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
            Events.PrintDialogue += PrintSequence;
            InitDictionary();
        }

        private void OnDisable()
        {
            Events.PrintDialogue -= PrintSequence;
        }

        private void InitDictionary()
        {
            dialogueDict = new Dictionary<string, DialogueSequence>();
            Dialogue[] allDialogue = Resources.LoadAll<Dialogue>(dialoguePath);

            foreach (Dialogue node in allDialogue)
            {
                if (dialogueDict.ContainsKey(node.sequenceID))
                    dialogueDict[node.sequenceID].Add(node);
                else
                    dialogueDict.Add(node.sequenceID, new DialogueSequence(node));
            }

            Events.SortSequences?.Invoke();
        }

        private void PrintSequence(string sequenceID)
        {
            foreach (Dialogue node in dialogueDict[sequenceID].Nodes)
                Debug.LogFormat("Num: {0} Text: {1}", node.num, node.text);
        }
    } 
}
