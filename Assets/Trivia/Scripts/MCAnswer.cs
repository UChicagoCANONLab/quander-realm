using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Trivia
{
    public class MCAnswer : MonoBehaviour
    {
        [SerializeField] public Toggle toggle;
        [SerializeField] public Image answerImage;
        [SerializeField] public TextMeshProUGUI answerText;
        [SerializeField] public bool correctAnswer;

        public void SetMCAnswer(string imageName, int i, string text, bool correct) 
        {  
            toggle.isOn = false;

            // imageName already includes prefix from DailyPuzzleManager
            if (imageName != "") {
                Sprite sprite = Resources.LoadAll<Sprite>(imageName)[i];
                // Set your max dimensions
                float maxWidth = 450f;
                float maxHeight = 340f;

                // Assign the sprite first
                answerImage.sprite = sprite;

                // Get the original size of the sprite (in Unity units)
                float spriteWidth = sprite.rect.width / sprite.pixelsPerUnit;
                float spriteHeight = sprite.rect.height / sprite.pixelsPerUnit;

                // Calculate scale factors for width and height
                float scaleWidth = maxWidth / spriteWidth;
                float scaleHeight = maxHeight / spriteHeight;

                // Choose the smaller scale to ensure both width and height fit
                float scale = Mathf.Min(scaleWidth, scaleHeight);

                // Apply scaled size to RectTransform
                RectTransform rt = answerImage.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(spriteWidth * scale, spriteHeight * scale);

                answerImage.enabled = true;
                answerText.enabled = false;
            } else {
                answerImage.enabled = false;
                answerText.text = text;
                answerText.enabled = true;
            }
            correctAnswer = correct;
        }


    }
}