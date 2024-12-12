using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Wrapper
{
    public class MCAnswer : MonoBehaviour
    {
        [SerializeField] public Toggle toggle;
        [SerializeField] public Image answerImage;
        [SerializeField] public TextMeshProUGUI answerText;
        [SerializeField] public bool correctAnswer;

        public void SetMCAnswer(string imagePath, string text, bool correct) 
        {  
            if (imagePath != "") {
                answerImage.sprite = Resources.Load<Sprite>(imagePath);
                answerText.enabled = false;
            } else {
                answerImage.enabled = false;
                answerText.text = text;
            }
            correctAnswer = correct;

            // toggle.onValueChanged.AddListener();
        }


    }
}