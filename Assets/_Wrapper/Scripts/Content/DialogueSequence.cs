using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class DialogueSequence
    {
        public List<Dialogue> Nodes { get => nodes; }
        private List<Dialogue> nodes;

        public DialogueSequence(Dialogue node)
        {
            Events.SortSequences += SortByLineNumber;
            nodes = new List<Dialogue> { node };
        }

        ~DialogueSequence()
        {
            Events.SortSequences -= SortByLineNumber;
        }

        public void Add(Dialogue node)
        {
            nodes.Add(node);
        }

        private void SortByLineNumber()
        {
            Debug.Log("Sorted");
            nodes.Sort((x, y) => x.num.CompareTo(y.num));
        }
    }
}
