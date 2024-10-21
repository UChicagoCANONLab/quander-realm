using System;
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
        public int[][] Gates { get; private set; }
        public int Height;

        // Constructor
        public Solution()
        {
        }

        /* Updates the solution to the level. 
         * Takes as input:
         * h - the number of rows in the level/solution
         * solutionTuple - a tuple with solution data, parsed as:
         *      solutionCode - the code for each row group
         *      flip - if the tallest group's solution is flipped (vertically)
         */
        public void UpdateSolution(int h, Tuple<int[], bool> solutionTuple)
        {


            int[] solutionCode = solutionTuple.Item1;
            bool flip = solutionTuple.Item2;

            Gates = new int[h][];
            Height = h;

            int currRow = 0;
            // Iterate through the groups
            for (int groupNumber = 0; groupNumber < solutionCode.Length; groupNumber++)
            {
                // Store values to help with flipping
                int groupHeight = solutionCode[groupNumber] > 100 ? 3 :
                    (solutionCode[groupNumber] > 10 ? 2 : 1);
                int groupStart = currRow;

 
                // For each group, fill in the corresponding gates.
                foreach (int[] row in SolutionManual[solutionCode[groupNumber]])
                {
                    if (flip)
                    {
                        // (top row to fill in) - (number filled in already)
                        int newRow = (groupHeight + groupStart - 1) - (currRow - groupStart);
                        Gates[newRow] = row;
                    }
                    else
                    {
                        Gates[currRow] = row;
                    }

                    currRow++;
                }
            }
        }

        /* A dictionary containing all possible solutions to levels in the game.
         * Note that row 0 is the top row in the grid.
         * In all CNOT gates, CNOT is on top.
         */
        private static readonly Dictionary<int, int[][]> SolutionManual = new Dictionary<int, int[][]>
        {
            // One row
            {0, new int[][] { new int[] {(int)GateType.None} } },
            {1, new int[][] { new int[] {(int)GateType.NOT} } },
            {2, new int[][] { new int[] {(int)GateType.H} } },
            {3, new int[][] { new int[] {(int)GateType.Z} } },
            {4, new int[][] { new int[] {(int)GateType.H, (int) GateType.NOT} } },
            {5, new int[][] { new int[] {(int)GateType.Z, (int) GateType.H} } },
            {6, new int[][] { new int[] {(int)GateType.H, (int) GateType.Z, (int) GateType.H} } },

            // Two rows
            {11, new int[][] { new int[] {(int)GateType.SWAP},
                                new int[] { (int)GateType.SWAP } } },
            {12, new int[][] { new int[] {(int)GateType.CNOT},
                                new int[] { (int)GateType.CNOT } } },
            {13, new int[][] { new int[] {(int)GateType.CNOT },
                                new int[] {(int)GateType.CNOT, (int)GateType.NOT} } },
            {14, new int[][] { new int[] {(int)GateType.H, (int)GateType.SWAP},
                                new int[] {(int)GateType.H, (int)GateType.SWAP} } },
            {15, new int[][] { new int[] {(int)GateType.SWAP, (int)GateType.H},
                                new int[] {(int)GateType.SWAP, (int)GateType.H} } },
            {16, new int[][] { new int[] {(int)GateType.SWAP, (int)GateType.H},
                                new int[] { (int)GateType.SWAP} } },
            {17, new int[][] { new int[] {(int)GateType.None, (int)GateType.CNOT},
                                new int[] {(int)GateType.H, (int)GateType.CNOT} } },
            {18, new int[][] { new int[] {(int)GateType.NOT, (int)GateType.CNOT },
                                new int[] {(int)GateType.H, (int)GateType.CNOT} } },
            {19, new int[][] { new int[] {(int)GateType.CNOT},
                                new int[] {(int)GateType.CNOT, (int)GateType.NOT} } },
            {20, new int[][] { new int[] {(int)GateType.None, (int)GateType.CNOT},
                                new int[] {(int)GateType.NOT, (int)GateType.CNOT} } },

            // Three rows
            {101, new int[][] { new int[] { (int)GateType.None, (int)GateType.SWAP},
                                new int[] { (int)GateType.SWAP, (int)GateType.SWAP},
                                new int[] { (int)GateType.SWAP, (int)GateType.None } } },
            {102, new int[][] { new int[] { (int)GateType.CNOT},
                                new int[] { (int)GateType.CNOT, (int)GateType.SWAP},
                                new int[] { (int)GateType.None, (int)GateType.SWAP} } },
            {103, new int[][] { new int[] { (int)GateType.None, (int)GateType.CNOT},
                                new int[] { (int)GateType.SWAP, (int)GateType.CNOT},
                                new int[] { (int)GateType.SWAP } } },
            {104, new int[][] { new int[] { (int)GateType.CNOT},
                                new int[] { (int)GateType.CNOT, (int)GateType.CNOT},
                                new int[] { (int)GateType.None, (int)GateType.CNOT } } },
            {105, new int[][] { new int[] { (int)GateType.None, (int)GateType.None, (int)GateType.SWAP},
                                new int[] { (int)GateType.NOT, (int)GateType.CNOT, (int)GateType.SWAP},
                                new int[] { (int)GateType.H, (int)GateType.CNOT } } },
            {106, new int[][] { new int[] { (int)GateType.NOT, (int)GateType.CNOT},
                                new int[] { (int)GateType.H, (int)GateType.CNOT, (int)GateType.SWAP},
                                new int[] { (int)GateType.None, (int)GateType.None, (int)GateType.SWAP } } },
            {107, new int[][] { new int[] { (int)GateType.None, (int)GateType.None, (int)GateType.SWAP},
                                new int[] { (int)GateType.None, (int)GateType.CNOT, (int)GateType.SWAP},
                                new int[] { (int)GateType.H, (int)GateType.CNOT } } },
            {108, new int[][] { new int[] { (int)GateType.None, (int)GateType.CNOT},
                                new int[] { (int)GateType.H, (int)GateType.CNOT, (int)GateType.SWAP},
                                new int[] { (int)GateType.None, (int)GateType.None, (int)GateType.SWAP } } },
            {109, new int[][] { new int[] { (int)GateType.SWAP},
                                new int[] { (int)GateType.SWAP, (int)GateType.CNOT},
                                new int[] { (int)GateType.H, (int)GateType.CNOT } } },
            {110, new int[][] { new int[] { (int)GateType.None, (int)GateType.None, (int)GateType.SWAP},
                                new int[] { (int)GateType.NOT, (int)GateType.CNOT, (int)GateType.SWAP},
                                new int[] { (int)GateType.H, (int)GateType.CNOT} } }
        };


        public override string ToString()
        {
            string ret = "";

            foreach (int[] row in Gates)
            {
                ret += "[";
                foreach (int gate in row)
                {
                    ret += ((GateType)gate).ToString();
                    ret += ",";               
                }
                ret = ret.Remove(ret.Length - 1);
                ret += "] \n";
            }

            return ret;
        }
    }
}