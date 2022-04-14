using System;
using UnityEngine;

namespace BlackBox
{
    public class NodeCell : Cell
    {
        private bool hasNode = false;
        private bool hasFlag = false;

        [SerializeField] private LanternMount lanternMount = null;

        public GameObject nodeObj = null;

        protected override void Start()
        {
            base.Start();
            lanternMount.SetGridPosition(gridPosition);
        }

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

        public bool HasFlag()
        {
            return hasFlag;
        }

        public void ToggleFlag(bool toggle)
        {
            hasFlag = toggle;
        }
    }
}
