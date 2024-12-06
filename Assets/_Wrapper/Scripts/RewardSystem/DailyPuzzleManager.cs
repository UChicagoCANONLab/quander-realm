using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BeauRoutine;
using System;

namespace Wrapper 
{
    public class DailyPuzzleManager : MonoBehaviour
    {
        [SerializeField] private DailyPuzzle currPuzzle;
        [SerializeField] private Animator animator;
        [SerializeField] private Button nextButton;

        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private MCAnswer[] MCAnswerObjs;
    }
}