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
        public Speaker speaker = Speaker.None;
        
        [SerializeField, ContentValue("Speaker Expression")]
        public Expression speakerExpression = Expression.Default;

        [SerializeField, ContentValue("Listener")]
        public Speaker listener = Speaker.None;

        [SerializeField, ContentValue("Listener Expression")]
        public Expression listenerExpression = Expression.Default;

        public enum Speaker
        {
            None,
            Char1,
            Char2,
            Char3,
            Char4,
            Char5,
            Char6
        }

        public enum Expression
        {
            Default,
            Positive,
            Negative,
            Confused
        }
    }
}
