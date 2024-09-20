using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BlackBox 
{
    public class HintPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hintTextObj;
        [SerializeField] private string[] hintTexts = {
            "You want a hint? Okay, fine...",
            "Click on a space. If it glows yellow, a lantern goes there!",
            "If it stays purple, too bad! Keep looking.",
            "Be careful! You lose a star each time you use a hint."
        };

        // [SerializeField] private LineRenderer hintLine;
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject lineContainer;
        // [SerializeField] private Vector3[] positions; // start, turn-point, end

        // Hint data
        // [SerializeField] private string[] possibleHints = new string[] {};
        // [SerializeField] private List<string> possibleHints;
        [SerializeField] private List<Vector3Int[]> hintPairs = new List<Vector3Int[]>();
        
        private GridSize size;
        private int maxSize;

        private int[] gridSizeValues = new int[3] { 5, 6, 7 };  // Copied from BBGameManager
        private float[] nodeCellSizeValues = new float[3] { 200f, 166.66f, 142.86f }; // Copied from BBGameManager

        // if small -->     max=900;min=0; normal= coor*(900/size = 180) + (900/(size*2) = 90)
        // if medium -->    max=900;min=0; normal= coor*(900/size = 150) + (900/(size*2) = 75)

        private Animator HintAnimator = null;
        private int seqCounter = 0;
        private int hintCounter = 0;

        private void Awake() 
        {
            HintAnimator = GetComponent<Animator>();
            hintTextObj.text = hintTexts[0];
        }

        private void OnEnable() 
        {
            BBEvents.ShowHint += GiveHint;
            BBEvents.AppendHint += AppendHintCoor;
            BBEvents.ClearHints += ClearHintLines;
        }

        private void OnDisable() 
        {
            BBEvents.ShowHint -= GiveHint;
            BBEvents.AppendHint -= AppendHintCoor;
            BBEvents.ClearHints -= ClearHintLines;
        }

        public void GiveHint() 
        {
            /* seqCounter = 0;
            HintAnimator.SetBool("IsOn", true);

            hintTextObj.text = hintTexts[0];
            HintAnimator.SetInteger("TextSeq", 0);
            // make the thing click turn on */

            if (hintPairs.Count <= hintCounter) { return; }

            Vector3 start = (Vector3)hintPairs[hintCounter][0];
            Vector3 end = (Vector3)hintPairs[hintCounter][1];
            Vector3 turn = new Vector3(start.x, end.y, 0);
            if (start.x==-1 || start.x==maxSize) {
                turn.x = end.x; turn.y = start.y;
            }
            Vector3[] positions = new Vector3[] {start, turn, end};
            
            for (int i=0; i<3; i++) {
                Vector3 pos = positions[i];
                switch (pos.x) {
                    case -1:        positions[i].x=0; break;
                    case var value when value == maxSize:   positions[i].x=900; break;
                    default:        positions[i].x= pos.x*(900/maxSize)+(900/(maxSize*2)); break;
                } switch (pos.y) {
                    case -1:        positions[i].y=0; break;
                    case var value when value == maxSize:   positions[i].y=900; break;
                    default:        positions[i].y= pos.y*(900/maxSize)+(900/(maxSize*2)); break;
                }
            }

            GameObject currLine = Instantiate(linePrefab, lineContainer.transform);
            currLine.GetComponent<LineRenderer>().SetPositions(positions);
            currLine.GetComponent<Animator>().SetBool("IsOn", true);
            
            hintCounter++;
        }

        public void ClearHintLines() {
            foreach (Transform transform in lineContainer.transform) {
                UnityEngine.Object.Destroy(transform.gameObject);
            }
        }

        public void NextSeqButton()
        {
            seqCounter++;
            
            if (seqCounter > 3) {
                EndHint();
                return;
            }

            HintAnimator.SetInteger("TextSeq", seqCounter);
            hintTextObj.text = hintTexts[seqCounter];
        }

        public void PrevSeqButton()
        {
            seqCounter--;

            HintAnimator.SetInteger("TextSeq", seqCounter);
            hintTextObj.text = hintTexts[seqCounter];
        }

        public void EndHint() 
        {
            HintAnimator.SetBool("IsOn", false);
            // make the click thing turn off
        }


        public void calculateLine() {
            // if direction == Top -->  y = size+1
            // if direction == Bot -->  y = -1
            // if direction == Left --> x = -1
            // if direction == Right -> x = size+1
        }

        public void AppendHintCoor(Vector3Int orig, Dir origDir, Vector3Int dest, Dir destDir) {
            Vector3Int[] pair = new Vector3Int[] {orig, dest};
            Dir[] dirPair = new Dir[] {origDir, destDir};

            size = BBEvents.GetLevel.Invoke().gridSize;
            maxSize = gridSizeValues[(int)size];

            for (int i=0; i<2; i++) {
                switch(dirPair[i]) {
                    case Dir.Top:   pair[i].y = maxSize;    break;
                    case Dir.Bot:   pair[i].y = -1;         break;
                    case Dir.Left:  pair[i].x = -1;         break;
                    case Dir.Right: pair[i].x = maxSize;    break;
                }
            }
            Debug.Log(string.Join("; ", pair));
            hintPairs.Add(pair);
        }

    }
}