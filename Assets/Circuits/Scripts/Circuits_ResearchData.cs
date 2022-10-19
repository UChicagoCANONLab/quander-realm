using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Circuits
{
    [System.Serializable]
	public class Circuits_ResearchData
    {
        public Wrapper.Game gameID = Wrapper.Game.Circuits;
        public string user;
        public int currLevel = 0;
        public bool[] completedLevels = new bool[CTConstants.N_LEVELS];
        public string log;
    }
}

