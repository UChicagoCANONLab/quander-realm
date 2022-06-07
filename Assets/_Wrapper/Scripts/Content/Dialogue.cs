using UnityEngine;
using Filament.Content;

namespace Wrapper
{
    public class Dialogue : ContentAsset
    {
        [SerializeField, ContentValue("SequenceID")]
        public string sequenceID = string.Empty;

        [SerializeField, ContentValue("Number")]
        public int num = 0;
        
        [SerializeField, ContentValue("Text")]
        public string text = string.Empty;
        
        [SerializeField, ContentValue("Speaker")]
        public Character speaker = Character.None;
        
        [SerializeField, ContentValue("Speaker Expression")]
        public Expression speakerExpression = Expression.Default;

        [SerializeField, ContentValue("Listener")]
        public Character listener = Character.None;

        [SerializeField, ContentValue("Listener Expression")]
        public Expression listenerExpression = Expression.Default;

        [SerializeField, ContentValue("Context Image Path")]
        public string contextImagePath = string.Empty;
    }
}
