using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlackBox
{
    public class NodeCell : Cell
    {
        private bool hasNode = false;
        private bool hasFlag = false;
        private bool debug = false; //todo: debug, delete later

        [SerializeField] private LanternMount lanternMount = null;
        [SerializeField] private GameObject nodeObj = null;
        [SerializeField] private Button[] buttons; //todo: debug, delete later
        [SerializeField] private TextMeshProUGUI text;

        protected override void Start()
        {
            base.Start();
            lanternMount.SetGridPosition(gridPosition);
            lanternMount.EvaluateEmpty();

            SetupDebug();
        }

        public override void Interact()
        {
            if (cellType == CellType.EdgeNode)
                return;

            hasNode = !hasNode;

            //todo: debug, delete later
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

        public void ToggleFlag(bool toggle)
        {
            hasFlag = toggle;
        }

        //todo: debug, delete later
        private void SetupDebug()
        {
            text.text = gridPosition.x.ToString() + ", " + gridPosition.y.ToString();
            GameEvents.ToggleDebug.AddListener(ToggleDebug);

            foreach (Button button in buttons)
                button.onClick.AddListener(() => { if (debug) Interact(); });
        }

        //todo: debug, delete later 
        private void ToggleDebug()
        {
            debug = !(text.gameObject.activeInHierarchy);
            text.gameObject.SetActive(debug);

            if (!hasNode)
                return;

            nodeObj.SetActive(debug);
        }
    }
}
