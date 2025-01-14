using Filament.Content;
using UnityEngine;

namespace Wrapper
{
    public class DailyPuzzleAsset : ContentAsset
    {
        // Identifying information

        [SerializeField, ContentValue("Name")]
        public string name;

        [SerializeField, ContentValue("Game")]
        public Game game;

        [SerializeField, ContentValue("Category")]
        public Category category;

        [SerializeField, ContentValue("Question Type")]
        public QuestionType questionType;

        [SerializeField, ContentValue("Card Type")]
        public CardType cardType;

        // Puzzle content

        [SerializeField, ContentValue("Question")]
        public string question;

        [SerializeField, ContentValue("Question Image Name")]
        public string questionImageName;

        [SerializeField, ContentValue("Answers")]
        public string[] answers;

        [SerializeField, ContentValue("Answers Image Name")]
        public string answersImageName;

        [SerializeField, ContentValue("Correct Answer")]
        public string correctAnswer;

        // Dependencies

        [SerializeField, ContentValue("Game Dependency")]
        public string gameDependency;

        [SerializeField, ContentValue("Question Dependency")]
        public string questionDependency;  

        // Feedback
        [SerializeField, ContentValue("Explanation")]
        public string explanation;
        
        [SerializeField, ContentValue("Explanation Image Name")]
        public string explanationImageName;
    }
}
