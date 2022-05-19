using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class NodeCell : Cell
    {
        private bool hasNode = false;
        private bool hasFlag = false;
        private bool debug = false; // Debug

        [SerializeField] private LanternMount lanternMount = null;
        [SerializeField] private GameObject nodeObj = null;
        [SerializeField] private List<Button> buttons = null; // Debug
        [SerializeField] private TextMeshProUGUI text = null;

        protected override void Start()
        {
            base.Start();
            lanternMount.SetGridPosition(gridPosition);
            lanternMount.EvaluateEmpty();

            SetupDebug();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            BBEvents.ToggleDebug += ToggleDebug;
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            BBEvents.ToggleDebug -= ToggleDebug;
        }
        public override void Interact()
        {
            if (cellType == CellType.EdgeNode)
                return;

            hasNode = !hasNode;

            // Debug
            if (debug)
                nodeObj.SetActive(hasNode);
        }

        //todo: make these properties?
        public override bool HasNode()
        {
            return hasNode;
        }

        public bool HasFlag()
        {
            return hasFlag;
        }

        public void ToggleFlag(bool isOn)
        {
            hasFlag = isOn;
        }

        #region Debug

        private void SetupDebug()
        {
            debug = (bool)BBEvents.IsDebug?.Invoke();

            text.text = gridPosition.x.ToString() + ", " + gridPosition.y.ToString();
            buttons = new List<Button>();
            foreach (Button button in buttons)
                button.onClick.AddListener(() => { if (debug) Interact(); });

            if (debug)
                ToggleDebug();
        }

        private void ToggleDebug()
        {
            debug = !(text.gameObject.activeInHierarchy);
            text.gameObject.SetActive(debug);

            if (!hasNode)
                return;

            nodeObj.SetActive(debug);
        }

        #endregion
    }
}
