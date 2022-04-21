using UnityEngine;
using Filament.Content;

namespace Wrapper
{
    public class DialogueNode : ContentAsset
    {
        [SerializeField, ContentValue("SequenceID")]
        public string sequenceID = string.Empty;

        [SerializeField, ContentValue("Number")]
        public int num = 0;
        
        [SerializeField, ContentValue("Text")]
        public string text = string.Empty;
        
        [SerializeField, ContentValue("Speaker")]
        public string speaker = string.Empty;
        
        [SerializeField, ContentValue("Speaker Expression")]
        public string speakerExpression = string.Empty;

        [SerializeField, ContentValue("Listener")]
        public string listener = string.Empty;

        [SerializeField, ContentValue("Listener Expression")]
        public string listenerExpression = string.Empty;
    }
}
