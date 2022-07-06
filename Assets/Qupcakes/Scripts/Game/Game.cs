using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Game info: all avaliable levels  
 */
namespace Qupcakery
{
    public class Game
    {
        public GameStat gameStat;
        public int CurrLevelInd { get; private set; } = 0;

        public int MaxLevelCnt { get; } = 25;
        public int maxPuzzleCnt { get; } = 10;
        const int maxGateCnt = 5; // gate type

        private Level level;

        // Constructor 
        public Game()
        {
            level = new Level(maxPuzzleCnt, maxGateCnt);
            gameStat = new GameStat(MaxLevelCnt);
        }

        // Update level
        public void UpdateLevel(int levelInd)
        {
            level.Update(levelInd);
            CurrLevelInd = levelInd;
        }

        // Get current level
        public Level GetCurrLevel()
        {
            return level;
        }
    }

}
