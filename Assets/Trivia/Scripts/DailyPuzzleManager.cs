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
using DependencySystem.Graph;
using DependencySystem.Models;
using DependencySystem.Dependencies;
using static DependencySystem.Dependencies.Dep;



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
        [SerializeField] private GameObject flipButton;

        [SerializeField] private GameObject confirmFlipButton;
        [SerializeField] private GameObject submitButton;
        [SerializeField] private GameObject bestGuessButton;

        private string prefix = "Trivia/Questions";
        private GameObject cardGO;
        private int seq = 0;
        private int FLIP_COST = 100;
        private int BANK_SIZE = 10;


        private string[] questionSequence = { };
        private string[] questionBank = {
            "BK1",
            "V1b",
            "V1b_2",
            "C1",
            "A1",
            "BK2a",
            // "BK2a_2",
            "BK2a_3",
            // "BK2a_4",
            "BK3",
            "BK3_2",
            "C2",
            "A2",
            "A2_2",
            "V2b",
            "C3",
            "A3",
            "A3_2"

        };
        private string[] questionFiller = {"LR_20",
            "LR_21",
            "LR_22",
            "LR_23",
            "LR_24",
            "LR_25",
            "LR_40",
            "LR_41",
            "LR_42",
            "LR_43"};

        private Dictionary<Question, IDependency> deps = new Dictionary<Question, IDependency>
        {
            [new Question("A1")] = E("C1"),
            [new Question("C1")] = Or(E("V1a"), E("V1b")),
            [new Question("V1a")] = E("QB1"),
            [new Question("V1b-1")] = E("BK1"),
            [new Question("V1b-2")] = E("BK1"),
            [new Question("QB1")] = Or(),
            [new Question("BK1")] = Or(),

            [new Question("BK2a-1")] = And(E("BK1"), E("A1")),
            [new Question("BK2a-2")] = And(E("BK1"), E("A1")),
            [new Question("BK2a-3")] = And(E("BK1"), E("A1")),
            [new Question("BK2b-1")] = And(E("BK1"), E("A1")),
            [new Question("BK2b-2")] = And(E("BK1"), E("A1")),
            [new Question("BK3-1")] = And(E("BK2b"), E("A1")),
            [new Question("BK3-2")] = And(E("BK2b"), E("A1")),
            [new Question("QB2")] = And(E("QB1"), E("A1")),
            [new Question("QB3-1")] = And(E("QB2"), E("A1")),
            [new Question("QB3-2")] = And(E("QB2"), E("A1")),
            [new Question("C2")] = And(Or(E("BK3"), E("QB3")), E("A1")),
            [new Question("A2-1")] = And(E("C2"), E("A1")),
            [new Question("A2-2")] = And(E("C2"), E("A1")),
            [new Question("V2a")] = And(E("QB2"), E("A1")),
            [new Question("V2b")] = And(E("BK2b"), E("A1")),
            [new Question("C3")] = And(Or(E("V2a"), E("V2b")), E("C1"), E("A1")),
            [new Question("A3-1")] = And(E("C3"), E("A1")),
            [new Question("A3-2")] = And(E("C3"), E("A1"))
        };

        private int numCorrect = 0;

        private bool demo = false;

        private string[] demoQuestionSequence =
            {"NA1_1", "NA1_2", "SP1_1", "SP1_3", "SP1_4", "C1_1"};
        private string[] demoWinnerText =
            {"Padawan", "Intern", "Apprentice", "Assistant", "Expert", "Black Belt", "The GOAT"};


        private void Awake()
        {
            animator.SetBool("StartOn", true);
            animator.SetBool("TriviaOn", false);
            animator.SetBool("FeedbackOn", false);
            animator.SetBool("EndOn", false);
        }


        private void setQuestion()
        {
            Debug.Log(questionSequence[seq]);
            presenterFeedback.text = "";
            flipButton.SetActive(true);
            // Load next puzzle in sequence
            if (demo)
            {
                currPuzzle = Resources.Load<DailyPuzzleAsset>($"{prefix}/{demoQuestionSequence[seq]}");
            }
            else
            {
                currPuzzle = Resources.Load<DailyPuzzleAsset>($"{prefix}/{questionSequence[seq]}");
            }

            Debug.Log("Starting Trivia!");
            //  bool rewardAdded = Events.AddReward?.Invoke(levelReward.rewardID) ?? false;


            // Set question and question image
            questionText.text = currPuzzle.question;
            if (currPuzzle.questionImageName != "")
            {
                questionImage.sprite = Resources.Load<Sprite>($"{prefix}_Images/{currPuzzle.questionImageName}");
                animator.SetBool("QuestionImageOn", true);
                // questionImage.gameObject.GetComponent<Button>().onClick.AddListener(ToggleQuestionImageBig);
            }
            else
            {
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
                    float maxWidth = Mathf.Min(675f, sprite.rect.width * 2f);
                    float maxHeight = Mathf.Min(930f, sprite.rect.height * 2f);

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

        public void StartTrivia()
        {
            seq = 0; numCorrect = 0;
            filterQuestions();
            ShuffleQuestions();
            setQuestion();

            animator.SetBool("EndOn", false);
            animator.SetBool("StartOn", false);
            animator.SetBool("TriviaOn", true);
        }

        public void flipCard(bool confirmed)
        {

            if (!confirmed)
            {
                presenterFeedback.text = "It costs 100 coins to flip this card!\n(You can view your cards for free in the rewards center!)";
                confirmFlipButton.SetActive(true);
            }
            else
            {
                if (Events.GetUserSaveTotalCoins() < FLIP_COST)
                {
                    presenterFeedback.text = "Uh Oh! You don't have enough coins!\n You can check cards for free in the rewards center";
                    confirmFlipButton.SetActive(false);
                    return;
                }
                presenterFeedback.text = "First make your best guess! Then I'll flip the card for you";
                submitButton.SetActive(false);
                confirmFlipButton.SetActive(false);
                bestGuessButton.SetActive(true);
                flipButton.SetActive(false);
                Events.UpdateUserSaveTotalCoins.Invoke(-1 * FLIP_COST); // must be negative

            }
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

        public void makeGuess()
        {
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

            string selectedAnswers = questionSequence[seq] + "GUESS\n";
            foreach (MCAnswer ans in MCAnswerObjs)
            {
                if (ans.toggle.isOn)
                {
                    selectedAnswers += ans.answerID + "\n";
                }
            }
            Debug.Log(selectedAnswers);
            TriviaResearchData rd = new TriviaResearchData();
            rd.Username = Wrapper.Events.GetPlayerResearchCode?.Invoke();
            rd.SaveData = selectedAnswers;
            Wrapper.Events.SaveMinigameResearchData?.Invoke(Wrapper.Game.Trivia, rd);


            var front = cardGO.transform.Find("Container").Find("Front").gameObject;
            var back = cardGO.transform.Find("Container").Find("Back").gameObject;
            Debug.Log("Flipped!");
            front.SetActive(back.activeSelf);
            back.SetActive(!front.activeSelf);
            bestGuessButton.SetActive(false);
            submitButton.SetActive(true);
            presenterFeedback.text = "Here's the back of the card!\nNow you can answer the question!";

            foreach (MCAnswer ans in MCAnswerObjs)
            {
                ans.toggle.isOn = false;
            }



        }

        public void CheckAnswer()
        {
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
            explanationText.text = currPuzzle.explanation.Replace("<br>", "\n"); ;
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


            Debug.Log("Answer Texts!");
            string selectedAnswers = questionSequence[seq] + "\n";
            foreach (MCAnswer ans in MCAnswerObjs)
            {
                if (ans.toggle.isOn)
                {
                    selectedAnswers += ans.answerID + "\n";
                }
            }
            Debug.Log(selectedAnswers);
            TriviaResearchData rd = new TriviaResearchData();
            rd.Username = Wrapper.Events.GetPlayerResearchCode?.Invoke();
            rd.SaveData = selectedAnswers;
            Wrapper.Events.SaveMinigameResearchData?.Invoke(Wrapper.Game.Trivia, rd);

            bool triviaLogged;
            foreach (MCAnswer ans in MCAnswerObjs)
            {
                if (ans.toggle.isOn != ans.correctAnswer)
                {
                    animator.SetBool("AnswerCorrect", false);
                    animator.SetBool("FeedbackOn", true);
                    triviaLogged = Events.AddTriviaAnswer?.Invoke(questionSequence[seq], false) ?? false;
                    return;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                MCAnswer ans = MCAnswerObjs[i];
                if (ans.toggle.isOn != ans.correctAnswer)
                {
                    animator.SetBool("AnswerCorrect", false);
                    animator.SetBool("FeedbackOn", true);
                    triviaLogged = Events.AddTriviaAnswer?.Invoke(questionSequence[seq], false) ?? false;
                    return;
                }
            }

            numCorrect++;
            triviaLogged = Events.AddTriviaAnswer?.Invoke(questionSequence[seq], true) ?? false;
            animator.SetBool("AnswerCorrect", true);
            animator.SetBool("FeedbackOn", true);
            Events.UpdateUserSaveTotalCoins.Invoke(5);
        }

        public void NextQuestion()
        {
            animator.SetBool("FeedbackOn", false);

            if ((demo && (seq < demoQuestionSequence.Length - 1))
            || (!demo && (seq < questionSequence.Length - 1)))
            {
                seq++; setQuestion();
                animator.SetBool("TriviaOn", true);
            }
            else
            {
                EndTrivia();
            }
        }

        public void EndTrivia()
        {
            animator.SetBool("EndOn", true);

            if (demo)
            {
                finalScoreNumText.text = $"{numCorrect} / {demoQuestionSequence.Length}";
                finalScoreWinnerText.text = demoWinnerText[numCorrect];
            }
            else
            {
                // float percent = 
                finalScoreNumText.text = $"{numCorrect}";
                finalScoreWinnerText.text = "Correct:";

                Events.UpdateUserSaveTotalCoins.Invoke(2 * numCorrect);
                Events.UpdateUserSaveTotalCoins.Invoke(2 * numCorrect);
                // Events.CollectAndDisplayBadge?.Invoke(Game.Trivia, 1, 0);
            }

        }

        public void ToggleQuestionImageBig()
        {
            bool active = animator.GetBool("QuestionImageBig");
            animator.SetBool("QuestionImageBig", !active);
        }

        public void ExitTrivia()
        { // Copied from BackButton.cs
            Wrapper.Events.ScreenFadeMidAction?.Invoke(() =>
                { SceneManager.LoadScene(0); Wrapper.Events.MinigameClosed?.Invoke(); }, 0.1F);
        }

        public void filterQuestions()
        {


            List<string> triviaLog = Events.GetTriviaLog?.Invoke() ?? new List<string>();
            Dictionary<string, (bool correct, long latestUT)> triviaStats = new();
            foreach (string entry in triviaLog)
            {
                // Example: "q1-1-1721350000"
                var parts = entry.Split('-');
                if (parts.Length != 3)
                    continue; // Skip malformed entries

                string id = parts[0];
                bool correct = parts[1] == "1";

                if (!long.TryParse(parts[2], out long ut))
                    continue; // Skip invalid timestamp

                if (triviaStats.TryGetValue(id, out var existing))
                {
                    // Only replace if this UT is newer
                    if (ut > existing.latestUT)
                    {
                        triviaStats[id] = (correct, ut);
                    }
                }
                else
                {
                    // First time seeing this ID
                    triviaStats[id] = (correct, ut);
                }
            }

            long currentUT = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            List<(string title, long score)> novel = new List<(string, long)>();
            List<(string title, long score)> repeats = new List<(string, long)>();
            foreach (string question in questionBank)
            {
                if (triviaStats.TryGetValue(question, out var existing))
                {
                    Debug.Log(currentUT - existing.latestUT);
                    if (currentUT - existing.latestUT > 500)
                    {
                        if (existing.correct)
                        {
                            repeats.Add((question, currentUT - existing.latestUT));
                        }
                        else
                        {
                            novel.Add((question, currentUT - existing.latestUT));
                        }

                    }
                }
                else
                {
                    if (QuestionUnlocked(question))
                    {
                        novel.Add((question, 0));
                    }
                }
            }
            List<string> selectedNovels = novel.OrderBy(n => n.score).Take(BANK_SIZE).Select(n => n.title).ToList();
            if (selectedNovels.Count() < BANK_SIZE)
            {
                int n_missing = BANK_SIZE - selectedNovels.Count();

                System.Random rng = new System.Random();
                List<string> selectedRepeats = repeats.OrderBy(n => n.score).Take(n_missing * 2).Select(n => n.title).OrderBy(x => rng.Next()).Take(n_missing).ToList();
                selectedNovels = selectedNovels.Concat(selectedRepeats).ToList();
            }

            if (selectedNovels.Count() < 5)
            {
                int n_missing = 5 - selectedNovels.Count();

                System.Random rng = new System.Random();
                List<string> selectedFiller = questionFiller.OrderBy(x => rng.Next()).Take(n_missing).ToList();
                selectedNovels = selectedNovels.Concat(selectedFiller).ToList();
            }


            questionSequence = selectedNovels.ToArray();


            Debug.Log(string.Join("\n", questionSequence));

        }

        public bool QuestionUnlocked(string questionID)
        {
            return true;
        }

        public void ShuffleQuestions()
        {

            System.Random rng = new System.Random();
            questionSequence = questionSequence.OrderBy(x => rng.Next()).ToArray();

        }

    }
}