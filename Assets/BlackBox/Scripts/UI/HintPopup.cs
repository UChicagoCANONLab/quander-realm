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

        // [SerializeField] private GameObject yellowGlow;
        // [SerializeField] private GameObject purpleGlow;

        // [SerializeField] private GameObject forwardButton;
        // [SerializeField] private GameObject backwardButton;

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
        }

        private void OnDisable() 
        {
            BBEvents.ShowHint -= GiveHint;
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

    }
}