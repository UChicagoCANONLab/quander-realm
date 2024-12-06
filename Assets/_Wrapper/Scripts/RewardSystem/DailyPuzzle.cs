using Filament.Content;
using UnityEngine;

namespace Wrapper
{
    public class DailyPuzzle : ContentAsset
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

        [SerializeField, ContentValue("Question Image Path")]
        public string questionImagePath;

        [SerializeField, ContentValue("Answers")]
        public string[] answers;

        [SerializeField, ContentValue("Answers Image Path")]
        public string answersImagePath;

        [SerializeField, ContentValue("Correct Answer")]
        public string correctAnswer;

        // Dependencies

        [SerializeField, ContentValue("Game Dependency")]
        public string gameDependency;

        [SerializeField, ContentValue("Question Dependency")]
        public string questionDependency;  
    }
}
