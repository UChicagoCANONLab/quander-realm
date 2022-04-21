using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class DialogueSequence
    {
        public List<DialogueNode> nodes;

        public DialogueSequence()
        {
            nodes = new List<DialogueNode>();
        }

        public DialogueSequence(DialogueNode node)
        {
            nodes = new List<DialogueNode>();
            nodes.Add(node);
        }

        public void Add(DialogueNode node)
        {
            nodes.Add(node);
        }

        //public IEnumerator GetNodesInOrder()
        //{

        //}

        //public IEnumerator GetEnumerator()
        //{
        //    using IEnumerator ie = base.GetEnumerator();
        //    while (ie.MoveNext())
        //    {
        //        yield return Path.Combine(baseDirectory, ie.Current);
        //    }
        //}

        //public DialogueSequence(List<DialogueNode> nodeRange)
        //{
        //    nodes = new List<DialogueNode>();
        //    nodes.AddRange(nodeRange);
        //}
    }
}
