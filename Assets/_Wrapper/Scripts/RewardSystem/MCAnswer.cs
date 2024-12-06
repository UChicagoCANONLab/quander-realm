using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Wrapper
{
    public class MCAnswer : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image answerImage;
        [SerializeField] private TextMeshProUGUI answerText;
        [SerializeField] private bool correctAnswer;

        private void SetMCAnswer(string imagePath, string text, bool correct) 
        {  
            if (imagePath != "") {
                answerImage = Resources.Load<Image>(imagePath);
                answerText.enabled = false;
            } else {
                answerImage.enabled = false;
                answerText.text = text;
            }
            correctAnswer = correct;
        }


    }
}