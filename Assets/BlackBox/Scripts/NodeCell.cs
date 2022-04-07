using UnityEngine;

namespace BlackBox
{
    public class NodeCell : Cell
    {
        private bool hasNode = false;
        public GameObject nodeObj; 

        public override bool HasNode()
        {
            return hasNode;
        }

        public override void Interact()
        {
            hasNode = !hasNode;
            nodeObj.SetActive(hasNode);
        }

        public override void SetValue(string value)
        {
        }
    }
}
