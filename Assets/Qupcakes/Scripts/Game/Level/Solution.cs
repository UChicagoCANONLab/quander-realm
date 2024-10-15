using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Solution constructor for a given puzzle. Solutions are constructed by
 * row. 
 * 
 * From GateType: None, NOT = 0, CNOT = 1, SWAP = 2, H = 3, Z = 4
 */

namespace Qupcakery
{
    // A puzzle batch with a list of entries (the order matters!)
    public class Solution
    {

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[][] Gates { get; private set; }


        // Constructor
        public Solution()
        {
            Width = 0;
            Height = 0;
        }

        public void UpdateSolution(int w, int h, int[] split, int[] solutionCode)
        {
            Width = w;
            Height = h;
            Gates = new int[Height][];

            int currRow = 0;
            // Iterate through the rows by group
            for (int groupNumber = 0; groupNumber < split.Length; groupNumber++)
            {
                // For each group, find the corresponding solution
                switch(split[groupNumber])
                {
                    case 1:
                        // Assign the row accordingly
                        Gates[currRow] = SolutionManual[solutionCode[groupNumber]][0];
                        // Increment current row.
                        currRow++;
                        break;
                    case 2:
                        for (int i = 0; i < Width; i++)
                        {
                            Gates[currRow];
                        }
                        break;

                }


            }
        }

        // Note to self - reassign to have all solutions just in this one dictionary! 
        private static readonly Dictionary<int, int[][]> SolutionManual = new Dictionary<int, int[][]>
        {
            {0, new int[][] { new int[] {(int) GateType.None} } },
            {1, new int[][] { new int[] {(int) GateType.NOT} } },
            {2, new int[][] { new int[] {(int) GateType.H} } },
            {3, new int[][] { new int[] {(int) GateType.Z} } },
            {4, new int[][] { new int[] {(int) GateType.H, (int) GateType.NOT} } },
            {5, new int[][] { new int[] {(int) GateType.Z, (int) GateType.H} } },
            {6, new int[][] { new int[] {(int)GateType.H, (int) GateType.Z, (int) GateType.H} } }
        };


        public override string ToString()
        {
            string ret = "";
            /*
            for (int i = 0; i < Size; i++)
            {
                ret += "[";
                ret += entries[i].Item1.ToString();
                ret += " ,";
                ret += entries[i].Item2.ToString();
                ret += "] \n";
            }
            */
            return ret;
        }

        
    }


}