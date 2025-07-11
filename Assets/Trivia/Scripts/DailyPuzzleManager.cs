// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using BeauRoutine;
using Wrapper;
using System.IO;
using System;
using System.Linq;



namespace Trivia 
{
    public class DailyPuzzleManager : MonoBehaviour
    {
        [SerializeField] private DailyPuzzleAsset currPuzzle;
        [SerializeField] private Animator animator;
        //[SerializeField] private Button nextButton;

        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Image questionImage;
        [SerializeField] private Image sideImage;
        [SerializeField] private MCAnswer[] MCAnswerObjs;
        [SerializeField] private TextMeshProUGUI explanationText;
        [SerializeField] private Image explanationImage;
        [SerializeField] private TextMeshProUGUI presenterFeedback;
        [SerializeField] private TextMeshProUGUI finalScoreNumText;
        [SerializeField] private TextMeshProUGUI finalScoreWinnerText;


        [SerializeField] private GameObject cardMount;

        [SerializeField] private GameObject cardHolder;
        [SerializeField] private GameObject sideImageHolder;

        private string prefix = "Trivia/BGCCPuzzles";
        private GameObject cardGO;
        private int seq = 0;
        private string[] questionSequence = {
            "LR_20","LR_21","LR_22","LR_23","LR_24","LR_25", "LR_40", "LR_41", "LR_42", "LR_43" };
        private int numCorrect = 0;

        private bool demo = false;

        private string[] demoQuestionSequence = 
            {"NA1_1", "NA1_2", "SP1_1", "SP1_3", "SP1_4", "C1_1"};
        private string[] demoWinnerText = 
            {"Padawan", "Intern", "Apprentice", "Assistant", "Expert", "Black Belt", "The GOAT"};


        private void Awake() {
            animator.SetBool("StartOn", true);
            animator.SetBool("TriviaOn", false);
            animator.SetBool("FeedbackOn", false);
            animator.SetBool("EndOn", false);
        }


        private void setQuestion() {
            presenterFeedback.text = "";
            // Load next puzzle in sequence
            if (demo) {
                currPuzzle = Resources.Load<DailyPuzzleAsset>($"{prefix}/{demoQuestionSequence[seq]}");
            } else {
                currPuzzle = Resources.Load<DailyPuzzleAsset>($"{prefix}/{questionSequence[seq]}");
            }
            

            // Set question and question image
            questionText.text = currPuzzle.question;
            if (currPuzzle.questionImageName != "") {
                questionImage.sprite = Resources.Load<Sprite>($"{prefix}_Images/{currPuzzle.questionImageName}");
                animator.SetBool("QuestionImageOn", true);
                // questionImage.gameObject.GetComponent<Button>().onClick.AddListener(ToggleQuestionImageBig);
            } else {
                animator.SetBool("QuestionImageOn", false);
            }

            // Set answers and answer images
            if (currPuzzle.questionType == QuestionType.MC)
            {
                for (int i = 0; i < 4; i++)
                {
                    string tempImageName = "";
                    if (currPuzzle.imageAnswers)
                    {
                        tempImageName = $"{prefix}_Images/{currPuzzle.name}";
                        
                    }
                    MCAnswerObjs[i].SetMCAnswer(tempImageName, i, currPuzzle.answers[i],
                        (currPuzzle.correctAnswer.Contains(currPuzzle.answers[i])));
                    
                    //MCAnswerObjs[i].toggle.onValueChanged.AddListener(
                    //    delegate {nextButton.interactable = true;});
                }
                cardHolder.SetActive(false);
                sideImageHolder.SetActive(false);
                if (currPuzzle.questionCard != "")
                {
                    cardHolder.SetActive(true);
                    string featuredCardID = currPuzzle.questionCard;
                    RewardAsset rAsset = Resources.Load<RewardAsset>(Path.Combine(GameManager.Instance.rewardsPath, featuredCardID));


                    //GameObject cardGO;
                    cardGO = Events.CreatRewardCard?.Invoke(rAsset, cardMount, DisplayType.CardPopup);
                    Destroy(cardGO.GetComponent<Animator>());
                    //cardGO.transform.SetParent(cardMount.transform);
                    //s

                    Routine.Start(cardGO.GetComponent<Reward>().SelectCard());

                }
                else if (currPuzzle.sideImage)
                {
                    sideImageHolder.SetActive(true);
                    Sprite sprite = Resources.LoadAll<Sprite>($"{prefix}_Images/{currPuzzle.name}/Side")[0];


                    // Get the original size of the sprite (in Unity units)
                    float spriteWidth = sprite.rect.width / sprite.pixelsPerUnit;
                    float spriteHeight = sprite.rect.height / sprite.pixelsPerUnit;

                    // Set your max dimensions
                    float maxWidth = Mathf.Min(675f, sprite.rect.width*2f);
                    float maxHeight = Mathf.Min(930f, sprite.rect.height*2f);

                    // Assign the sprite first
                    sideImage.sprite = sprite;

                    // Calculate scale factors for width and height
                    float scaleWidth = maxWidth / spriteWidth;
                    float scaleHeight = maxHeight / spriteHeight;

                    // Choose the smaller scale to ensure both width and height fit
                    float scale = Mathf.Min(scaleWidth, scaleHeight);

                    // Apply scaled size to RectTransform
                    RectTransform rt = sideImage.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(spriteWidth * scale, spriteHeight * scale);

                    
                }

            }
            //nextButton.interactable = false;
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

        public void flipCard() {


            presenterFeedback.text = "Uh Oh! You don't have enough coins to flip the card!";
            return;
            //cardGO?.GetComponent<Reward>().FlipCard();
            var front = cardGO.transform.Find("Container").Find("Front").gameObject;
            var back = cardGO.transform.Find("Container").Find("Back").gameObject;
            Debug.Log("Flipped!");
            front.SetActive(back.activeSelf);
            back.SetActive(!front.activeSelf);

            //animator.SetBool("TriviaOn", false);

            //Debug.Log(c.name);
        }

        public void CheckAnswer() {
            bool oneSelected = false;
            foreach (MCAnswer ans in MCAnswerObjs)
            {
                if (ans.toggle.isOn)
                {
                    oneSelected = true;
                    break;
                }
            }

            if (!oneSelected)
            {
                // Debug.Log("Pick an answer!");
                presenterFeedback.text = "You need to pick at least one option!";
                return;
            }

            animator.SetBool("TriviaOn", false);
            explanationText.text = currPuzzle.explanation.Replace("<br>", "\n");;
            // explanationText.text = tex

            if (currPuzzle.explanationImageName != "")
            {
                explanationImage.sprite = Resources.Load<Sprite>($"{prefix}_Images/{currPuzzle.explanationImageName}");
                animator.SetBool("FeedbackImage", true);
            }
            else
            {
                animator.SetBool("FeedbackImage", false);
            }

            foreach(MCAnswer ans in MCAnswerObjs) {
                if (ans.toggle.isOn != ans.correctAnswer) {
                    animator.SetBool("AnswerCorrect", false);
                    animator.SetBool("FeedbackOn", true);
                    return;
                }
            } 

            for (int i = 0; i < 4; i++)
            {
                MCAnswer ans = MCAnswerObjs[i];
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
                // float percent = 
                finalScoreNumText.text = $"{(float)numCorrect / questionSequence.Length:P0}";
                finalScoreWinnerText.text = "Score:";
            }

        }

        public void ToggleQuestionImageBig() {
            bool active = animator.GetBool("QuestionImageBig");
            animator.SetBool("QuestionImageBig", !active);
        }

        public void ExitTrivia() { // Copied from BackButton.cs
            Wrapper.Events.ScreenFadeMidAction?.Invoke(() =>
                { SceneManager.LoadScene(0); Wrapper.Events.MinigameClosed?.Invoke();}, 0.1F);
        }

        public void ShuffleQuestions()
        {

                System.Random rng = new System.Random();
                questionSequence = questionSequence.OrderBy(x => rng.Next()).ToArray();

        }

    }
}