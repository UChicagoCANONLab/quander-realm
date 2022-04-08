using UnityEngine;

namespace BlackBox
{
    public class NodeCell : Cell
    {
        private bool hasNode = false;
        public GameObject nodeObj;

        public override void Interact()
        {
            if (cellType == CellType.EdgeNode)
                return;

            hasNode = !hasNode;
            nodeObj.SetActive(hasNode);
        }

        public override bool HasNode()
        {
            return hasNode;
        }
    }
}
