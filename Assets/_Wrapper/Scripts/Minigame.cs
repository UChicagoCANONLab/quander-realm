using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    [CreateAssetMenu(fileName = "Minigame", menuName = "Minigame")]
    public class Minigame : ScriptableObject
    {
        public string StartScene;
        public new string name; 
    }
}