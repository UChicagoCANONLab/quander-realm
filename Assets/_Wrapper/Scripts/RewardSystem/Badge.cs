using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wrapper
{
    public class Badge : MonoBehaviour
    {
        [SerializeField] public Animator animator;
        [SerializeField] public Game game;
        [SerializeField] public int difficulty;
        
        [SerializeField] public TextMeshProUGUI titleText;
        [SerializeField] public TextMeshProUGUI descriptionText;


        public void InitBadge(string badgeAssetPath) 
        {
            
        }
    
    }
}