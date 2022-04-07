using UnityEngine;

namespace BlackBox
{
    public class NodeCell : Cell
    {
        private bool hasNode = false;
        public GameObject nodeObj;

        public override void Interact()
        {
            hasNode = !hasNode;
            nodeObj.SetActive(hasNode);
        }

        public override void SetValue(string value) {}

        public override bool HasNode()
        {
            return hasNode;
        }
    }
}
