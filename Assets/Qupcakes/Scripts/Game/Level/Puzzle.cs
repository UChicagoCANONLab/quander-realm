using System;
using System.Collections.Generic;
using UnityEngine;
using SysDebug = System.Diagnostics.Debug;

/*
 * Puzzle batch info: sets of (cake type, customer order) pair
 * Interfacing with game setup modules
 */
namespace Qupcakery
{
    // A puzzle batch with a list of entries (the order matters!)
    public class Puzzle
    {
        public int Size { get; private set; }
        public (int, int)[] entries { get; private set; }

        // Constructor
        public Puzzle(int maxEntryCnt)
        {
            entries = new (int, int)[maxEntryCnt];
            Size = maxEntryCnt;
        }

        public void UpdatePuzzle(int cakeType, int orderCakeType)
        {
            entries[0] = (cakeType, orderCakeType);
            Size = 1;
        }

        public void UpdatePuzzle(int cakeType0, int orderCakeType0,
            int cakeType1, int orderCakeType1)
        {
            entries[0] = (cakeType0, orderCakeType0);
            entries[1] = (cakeType1, orderCakeType1);
            Size = 2;
        }

        public void UpdatePuzzle(int cakeType0, int orderCakeType0,
           int cakeType1, int orderCakeType1,
           int cakeType2, int orderCakeType2)
        {
            entries[0] = (cakeType0, orderCakeType0);
            entries[1] = (cakeType1, orderCakeType1);
            entries[2] = (cakeType2, orderCakeType2);
            Size = 3;
        }

        public override string ToString()
        {
            string ret = "";
            for (int i = 0; i < Size; i++)
            {
                ret += "[";
                ret += entries[i].Item1.ToString();
                ret += " ,";
                ret += entries[i].Item2.ToString();
                ret += "] \n";
            }
            return ret;
        }
    }
}
