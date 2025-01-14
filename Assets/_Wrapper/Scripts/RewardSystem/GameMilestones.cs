using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class GameMilestones : MonoBehaviour
    {
        public List<string> CompletedMilestones = new List<string> {""}; // want to add empty stringn as null co-criteria
        
        Dictionary<(int,int),string[]> QupcakeryMilestones = new Dictionary<(int,int),string[]>
        {   //  (Level Criteria, Star Criteria); Name, Co-Criteria, Description
            {(1, 0),    new string[] {"MS_QU0",  "",         "NOT Gate"}},
            {(3, 0),    new string[] {"MS_QU1",  "",         "SWAP Gate"}},
            {(8, 0),    new string[] {"MS_QU2",  "",         "CNOT Gate"}},
            {(9, 0),    new string[] {"MS_QU3",  "",         "CNOT Gate flip"}},
            {(13, 0),   new string[] {"MS_QU4",  "",         "H-Gate / Mystery boxes"}},
            {(14, 0),   new string[] {"MS_QU5",  "",         "H-Gate reverse"}},
            {(16, 0),   new string[] {"MS_QU6",  "",         "Z-Gate"}},
            {(23, 0),   new string[] {"MS_QU7",  "",         "Same-Entangled boxes"}},
            {(24, 0),   new string[] {"MS_QU8",  "",         "Opposite-Entangled boxes"}},
            {(27, 0),   new string[] {"MS_QU9",  "",         "Completed game"}},
            {(0, 81),   new string[] {"MS_QU10", "MS_QU9",   "Win all stars"}},
            {(0, 27),   new string[] {"MS_QU11", "",         "Unlock Tangle's Lair"}},
            {(0, 10),   new string[] {"MS_QU12", "MS_LA5",   "Unlock Queuebits"}}    
        };
        
        
        Dictionary<(int,int),string[]> TwintanglementMilestones = new Dictionary<(int,int),string[]>
        {   //  (Level Criteria, Star Criteria); Name, Co-Criteria, Description
            {(1, 0),    new string[] {"MS_LA0", "",         "Same-Entangled twins"}},
            {(6, 0),    new string[] {"MS_LA1", "",         "Opposite-Entangled twins"}},
            {(11, 0),   new string[] {"MS_LA2", "",         "Funky-Entangled twins"}},
            {(15, 0),   new string[] {"MS_LA3", "",         "Completed game"}},
            {(0, 45),   new string[] {"MS_LA4", "MS_LA3",   "Win all stars"}},
            {(0, 10),   new string[] {"MS_LA5", "MS_QU12",  "Unlock Queuebits"}}
        };


        Dictionary<(int,int),string[]> TanglesLairMilestones = new Dictionary<(int,int),string[]>
        {   //  (Level Criteria, Star Criteria); Name, Co-Criteria, Description
            {(1, 0),    new string[] {"MS_CT0",  "",        "H-Gate"}},
            {(3, 0),    new string[] {"MS_CT1",  "",        "Not Gate / HXH -> Z"}},
            {(6, 0),    new string[] {"MS_CT2",  "",        "HZH -> X"}},
            {(9, 0),    new string[] {"MS_CT3",  "",        "Two Not Gates -> Nothing"}},
            {(14, 0),   new string[] {"MS_CT4",  "",        "CNOT / CZ"}},
            {(18, 0),   new string[] {"MS_CT5",  "",        "H-CNOT-H -> CZ"}},
            {(22, 0),   new string[] {"MS_CT6",  "",        "SWAP Gate / HHCXHH"}},
            {(25, 0),   new string[] {"MS_CT7",  "",        "Completed game"}},
            {(0, 75),   new string[] {"MS_CT8",  "MS_CT7",  "Win all stars"}}
        };
        
        
        Dictionary<(int,int),string[]> QueueBitsMilestones = new Dictionary<(int,int),string[]>
        {   //  (Level Criteria, Star Criteria); Name, Co-Criteria, Description
            {(1, 0),    new string[] {"MS_QB0",  "",        "Played one normal game"}},
            {(3, 0),    new string[] {"MS_QB1",  "",        "Superposition tokens (75/25)"}},
            {(4, 0),    new string[] {"MS_QB2",  "",        "Superposition tokens (50/50)"}},
            {(6, 0),    new string[] {"MS_QB3",  "",        "Reveal tokens at end, in order"}},
            {(11, 0),   new string[] {"MS_QB4",  "",        "Select tokens to reveal"}},
            {(15, 0),   new string[] {"MS_QB5",  "",        "Completed game"}},
            {(0, 45),   new string[] {"MS_QB6",  "MS_QB5",  "Win all stars"}}
        };
        
        
        Dictionary<(int,int),string[]> BuriedTreasureMilestones = new Dictionary<(int,int),string[]>
        {   //  (Level Criteria, Star Criteria); Name, Co-Criteria, Description
            {(1, 0),    new string[] {"MS_BB0",  "",        "Miss, Hit, and Detour unlocked"}},
            {(7, 0),    new string[] {"MS_BB1",  "",        "Fog rolls in"}},
            {(10, 0),   new string[] {"MS_BB2",  "",        "Reflect unlocked"}},
            {(13, 0),   new string[] {"MS_BB3",  "",        "Multiple Detours unlocked"}},
            {(24, 0),   new string[] {"MS_BB4",  "",        "Completed game"}},
            {(0, 72),   new string[] {"MS_BB6",  "MS_BB4",  "Win all stars"}}
        };


        public void CheckMilestone(Game game, int level, int totalStars) {
            (int, int) tempKey = (level, totalStars);
            switch(game) 
            {
                case Game.Qupcakes:
                    if (QupcakeryMilestones.ContainsKey(tempKey)) {
                        if (CompletedMilestones.Contains(QupcakeryMilestones[tempKey][1])) {
                            CompletedMilestones.Add(QupcakeryMilestones[tempKey][0]);
                        }
                    } break;
                case Game.Labyrinth:
                    if (TwintanglementMilestones.ContainsKey(tempKey)) {
                        if (CompletedMilestones.Contains(TwintanglementMilestones[tempKey][1])) {
                            CompletedMilestones.Add(TwintanglementMilestones[tempKey][0]);
                        }
                    } break;
                case Game.Circuits:
                    if (TanglesLairMilestones.ContainsKey(tempKey)) {
                        if (CompletedMilestones.Contains(TanglesLairMilestones[tempKey][1])) {
                            CompletedMilestones.Add(TanglesLairMilestones[tempKey][0]);
                        }
                    } break;
                case Game.QueueBits:
                    if (QueueBitsMilestones.ContainsKey(tempKey)) {
                        if (CompletedMilestones.Contains(QueueBitsMilestones[tempKey][1])) {
                            CompletedMilestones.Add(QueueBitsMilestones[tempKey][0]);
                        }
                    } break;
                case Game.BlackBox:
                    if (BuriedTreasureMilestones.ContainsKey(tempKey)) {
                        if (CompletedMilestones.Contains(BuriedTreasureMilestones[tempKey][1])) {
                            CompletedMilestones.Add(BuriedTreasureMilestones[tempKey][0]);
                        }
                    } break;
                case Game.Rewards:
                    break;

            }
        }


    }
}
