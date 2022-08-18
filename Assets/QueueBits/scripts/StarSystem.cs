using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QueueBits
{
    public class StarSystem : MonoBehaviour
    {
        // star system
        public static Dictionary<int, int> levelStarCount;

        public void updateLevelStarCount (Dictionary<int, int> newLevelStarCount)
        {
            levelStarCount = newLevelStarCount;
        }
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}