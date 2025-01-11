// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using BeauRoutine;


namespace Wrapper 
{
    public class DailyPuzzleManager : MonoBehaviour
    {
        [SerializeField] private DailyPuzzle currPuzzle;
        [SerializeField] private Animator animator;
        [SerializeField] private Button nextButton;

        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Image questionImage;
        [SerializeField] private MCAnswer[] MCAnswerObjs;
        [SerializeField] private TextMeshProUGUI explanationText;
        [SerializeField] private Image explanationImage;
        [SerializeField] private TextMeshProUGUI finalScoreNumText;
        [SerializeField] private TextMeshProUGUI finalScoreWinnerText;

        private int seq = 0;
        private string[] questionSequence = {
            "E1_1", "E1_2",
            "NA1_1", "NA1_2",
            "SP1_1", "SP1_2", "SP1_3", "SP1_4",
            "C1_1"
        };
        private int numCorrect = 0;

        private bool demo = true;
        private string[] demoQuestionSequence = 
            {"NA1_1", "NA1_2", "SP1_1", "SP1_3", "SP1_4", "C1_1"};
        private string[] demoWinnerText = 
            {"Padawan", "Intern", "Assistant", "Associate", "Expert", "Black Belt", "The GOAT"};


        private void Awake() {
            animator.SetBool("StartOn", true);
            animator.SetBool("TriviaOn", false);
            animator.SetBool("FeedbackOn", false);
            animator.SetBool("EndOn", false);
        }


        private void setQuestion() {
            // Load next puzzle in sequence
            if (demo) {
                currPuzzle = Resources.Load<DailyPuzzle>($"_Wrapper/Incentives/DailyPuzzles/{demoQuestionSequence[seq]}");
            } else {
                currPuzzle = Resources.Load<DailyPuzzle>($"_Wrapper/Incentives/DailyPuzzles/{questionSequence[seq]}");
            }
            

            // Set question and question image
            questionText.text = currPuzzle.question;
            if (currPuzzle.questionImagePath != "") {
                questionImage.sprite = Resources.Load<Sprite>(currPuzzle.questionImagePath);
                animator.SetBool("QuestionImageOn", true);
                // questionImage.gameObject.GetComponent<Button>().onClick.AddListener(ToggleQuestionImageBig);
            } else {
                animator.SetBool("QuestionImageOn", false);
            }

            // Set answers and answer images
            if (currPuzzle.questionType == QuestionType.MC) {
                for (int i=0; i<4; i++) {
                    string tempImagePath = "";
                    if (currPuzzle.answersImagePath != "") {
                       tempImagePath = $"{currPuzzle.answersImagePath}";
                    }
                    MCAnswerObjs[i].SetMCAnswer(tempImagePath, i, currPuzzle.answers[i], 
                        (currPuzzle.answers[i]==currPuzzle.correctAnswer));
                    MCAnswerObjs[i].toggle.onValueChanged.AddListener(
                        delegate {nextButton.interactable = true;});
                }
            }
            nextButton.interactable = false;
        }

        // Button Functionality

        public void StartTrivia() {
            seq = 0; numCorrect = 0;
            ShuffleQuestions();
            setQuestion();

            animator.SetBool("EndOn", false);
            animator.SetBool("StartOn", false);
            animator.SetBool("TriviaOn", true);
        }

        public void CheckAnswer() {
            animator.SetBool("TriviaOn", false);
            explanationText.text = currPuzzle.explanation;

            if (currPuzzle.explanationImagePath != "") {
                explanationImage.sprite = Resources.Load<Sprite>(currPuzzle.explanationImagePath);
                animator.SetBool("FeedbackImage", true);
            } else {
                animator.SetBool("FeedbackImage", false);
            }

            foreach(MCAnswer ans in MCAnswerObjs) {
                if (ans.toggle.isOn != ans.correctAnswer) {
                    animator.SetBool("AnswerCorrect", false);
                    animator.SetBool("FeedbackOn", true);
                    return;
                }
            } 
            numCorrect++;            
            animator.SetBool("AnswerCorrect", true);
            animator.SetBool("FeedbackOn", true);
        }

        public void NextQuestion() {
            animator.SetBool("FeedbackOn", false);

            if ((demo && (seq < demoQuestionSequence.Length -1))
            || (!demo && (seq < questionSequence.Length -1))) {
                seq++; setQuestion();
                animator.SetBool("TriviaOn", true);
            } else {
                EndTrivia();
            }
        }
        
        public void EndTrivia() {
            animator.SetBool("EndOn", true);

            if (demo) {
                finalScoreNumText.text = $"{numCorrect} / {demoQuestionSequence.Length}";
                finalScoreWinnerText.text = demoWinnerText[numCorrect];
            } else {
                finalScoreNumText.text = $"{numCorrect/questionSequence.Length}%";
                finalScoreWinnerText.text = "Tangle's Assistant!";
            }

        }

        public void ToggleQuestionImageBig() {
            bool active = animator.GetBool("QuestionImageBig");
            animator.SetBool("QuestionImageBig", !active);
        }

        public void ExitTrivia() { // Copied from BackButton.cs
            Events.ScreenFadeMidAction?.Invoke(() =>
                { SceneManager.LoadScene(0); Events.MinigameClosed?.Invoke();}, 0.1F);
        }

        public void ShuffleQuestions() {
            // Knuth shuffle algorithm
            if (demo) {
                for(int i=0; i<demoQuestionSequence.Length; i++) {
                    string temp = demoQuestionSequence[i];
                    int j = Random.Range(i, demoQuestionSequence.Length);
                    demoQuestionSequence[i] = demoQuestionSequence[j];
                    demoQuestionSequence[j] = temp;
                }
            }
        }

    }
}