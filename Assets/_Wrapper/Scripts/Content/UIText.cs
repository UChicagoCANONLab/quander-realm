using UnityEngine;
using Filament.Content;

namespace Wrapper
{
    public class UIText : ContentAsset
    {
        [SerializeField, ContentValue("Name")]
        public string ID = string.Empty;

        [SerializeField, ContentValue("Text")]
        public string text = string.Empty;

        
    }
}
