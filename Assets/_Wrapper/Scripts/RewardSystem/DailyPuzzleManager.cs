using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BeauRoutine;
using System;

namespace Wrapper 
{
    public class DailyPuzzleManager : MonoBehaviour
    {
        [SerializeField] private DailyPuzzle currPuzzle;
        [SerializeField] private Animator animator;
        // [SerializeField] private Button nextButton;

        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Image questionImage;
        [SerializeField] private MCAnswer[] MCAnswerObjs;
        [SerializeField] private TextMeshProUGUI explanationText;

        private int seq = 0;
        private string[] questionSequence = {
            // "E1_1",
            // "E1_2",
            "NA1_1",
            "NA1_2",
            "SP1_1",
            "SP1_2",
            "SP1_3",
            "SP1_4"
        };
        private int numCorrect = 0;


        private void Awake() {
            animator.SetBool("StartOn", true);
            animator.SetBool("TriviaOn", false);
            animator.SetBool("FeedbackOn", false);
        }


        private void setQuestion() {
            // Load next puzzle in sequence
            currPuzzle = Resources.Load<DailyPuzzle>($"_Wrapper/Incentives/DailyPuzzles/{questionSequence[seq]}");

            // Set question and question image
            questionText.text = currPuzzle.question;
            if (currPuzzle.questionImagePath != "") {
                questionImage.sprite = Resources.Load<Sprite>(currPuzzle.questionImagePath);
                Debug.Log(currPuzzle.questionImagePath);
                animator.SetBool("QuestionImageOn", true);
            } else {
                animator.SetBool("QuestionImageOn", false);
            }

            // Set answers and answer images
            if (currPuzzle.questionType == QuestionType.MC) {
                for (int i=0; i<4; i++) {
                    string tempImagePath = "";
                    if (currPuzzle.answersImagePath != "") {
                       tempImagePath = $"{currPuzzle.answersImagePath}_{i}";
                    }
                    Debug.Log(tempImagePath);
                    MCAnswerObjs[i].SetMCAnswer(tempImagePath, currPuzzle.answers[i], 
                        (currPuzzle.answers[i]==currPuzzle.correctAnswer));
                }
            }
            

        }

        // Button Functionality

        public void StartTrivia() {
            seq = 0; setQuestion();

            animator.SetBool("StartOn", false);
            animator.SetBool("TriviaOn", true);
        }

        public void CheckAnswer() {
            animator.SetBool("TriviaOn", false);

            foreach(MCAnswer ans in MCAnswerObjs) {
                if (ans.toggle.isOn != ans.correctAnswer) {
                    animator.SetBool("AnswerCorrect", false);
                    animator.SetBool("FeedbackOn", true);
                    return;
                }
            } numCorrect++;
            
            animator.SetBool("AnswerCorrect", true);
            animator.SetBool("FeedbackOn", true);
        }

        public void NextQuestion() {
            animator.SetBool("FeedbackOn", false);

            if (seq < questionSequence.Length) {
                seq++; setQuestion();
                animator.SetBool("TriviaOn", true);
            } else {
                //do ending screen
            }
            
        }

    }
}