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

        [SerializeField] private LineRenderer hintLine;
        [SerializeField] private Vector3[] positions; // start, turn-point, end

        // Hint data
        // [SerializeField] private string[] possibleHints = new string[] {};
        // [SerializeField] private List<string> possibleHints;
        [SerializeField] private List<Vector3Int[]> hintPairs = new List<Vector3Int[]>();
        
        private GridSize size;
        private int[] gridSizeValues = new int[3] { 5, 6, 7 };  // Copied from BBGameManager
        private float[] nodeCellSizeValues = new float[3] { 200f, 166.66f, 142.86f }; // Copied from BBGameManager

        // if small --> max=900;min=0; normal= coor*(900/size = 180) + 90

        private Animator HintAnimator = null;
        private int seqCounter = 0;

        private void Awake() 
        {
            HintAnimator = GetComponent<Animator>();
            hintTextObj.text = hintTexts[0];
        }

        private void OnEnable() 
        {
            BBEvents.ShowHint += GiveHint;
            BBEvents.AppendHint += AppendHintCoor;
        }

        private void OnDisable() 
        {
            BBEvents.ShowHint -= GiveHint;
            BBEvents.AppendHint -= AppendHintCoor;
        }

        public void GiveHint() 
        {
            seqCounter = 0;
            HintAnimator.SetBool("IsOn", true);

            hintTextObj.text = hintTexts[0];
            HintAnimator.SetInteger("TextSeq", 0);
            // make the thing click turn on
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
            int maxSize = gridSizeValues[(int)size];

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