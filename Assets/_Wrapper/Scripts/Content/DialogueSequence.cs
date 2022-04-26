using System.Collections.Generic;

namespace Wrapper
{
    public class DialogueSequence
    {
        public List<Dialogue> Nodes { get => nodes; }
        private List<Dialogue> nodes;

        private int currentLineNumber = 0;

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

        public Dialogue GetLine(int step)
        {
            if (SteppingOutOfBounds(step))
                return null;

            currentLineNumber += step;

            return nodes[currentLineNumber];
        }

        private void SortByLineNumber()
        {
            nodes.Sort((x, y) => x.num.CompareTo(y.num));
        }

        private bool SteppingOutOfBounds(int step)
        {
            bool result = false;

            if (step < 0 && currentLineNumber == 0)
                result = true;
            else if (step > 0 && currentLineNumber == nodes.Count - 1)
                result = true;

            return result;
        }
    }
}
