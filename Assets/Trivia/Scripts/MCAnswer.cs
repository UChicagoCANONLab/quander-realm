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
                answerImage.sprite = Resources.LoadAll<Sprite>(imageName)[i];
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