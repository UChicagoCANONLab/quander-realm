using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BlackBox 
{
    public class HintPopup : MonoBehaviour
    {
        [SerializeField] private Animator WolfieAnimator;
        /* [SerializeField] private string[] hintTexts = {
            "You need to play more before I can give you a hint!",
            "Remember, if you use too many hints, you'll lose a star..."
        }; */

        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject lineContainer;
    
        [SerializeField] private List<Vector3Int[]> hintPairs = new List<Vector3Int[]>();
        
        private GridSize size;
        private int[] gridSizeValues = new int[3] { 5, 6, 7 };  // Copied from BBGameManager

        private int maxSize;
        private int hintCounter = 0;
    

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
            if (hintPairs.Count <= hintCounter) { 
                WolfieAnimator.SetBool("IsOn", true);
                return;    
            }

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
            GameObject corner = currLine.transform.GetChild(0).gameObject;
            corner.transform.localPosition += positions[1];

            currLine.GetComponent<Animator>().SetBool("IsOn", true);            
            hintCounter++;
        }

        public void ClearHintLines() {
            foreach (Transform transform in lineContainer.transform) {
                UnityEngine.Object.Destroy(transform.gameObject);
            }
            hintCounter = 0;
            hintPairs.Clear();
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
            // Debug.Log(string.Join("; ", pair));
            hintPairs.Add(pair);
        }

        public void ExitWolfiePopup() {
            WolfieAnimator.SetBool("IsOn", false);
        }

    }
}