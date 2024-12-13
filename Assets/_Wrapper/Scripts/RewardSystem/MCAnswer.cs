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

        public void SetMCAnswer(string imagePath, int i, string text, bool correct) 
        {  
            toggle.isOn = false;

            if (imagePath != "") {
                answerImage.sprite = Resources.LoadAll<Sprite>(imagePath)[i];
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