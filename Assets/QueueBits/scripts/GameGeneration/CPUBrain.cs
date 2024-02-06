using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;

namespace QueueBits 
{
	// AI for CPU actions, minimax algorithm predominantly
    public class CPUBrain : MonoBehaviour
    {
        // Origninally copied from Level3.cs, compared to other levels 

        public int difficulty = 0;
        // Difficulty 0: Level 1, 2, 3
        // Difficulty 1: Level 4, 5
        // Difficulty 2: Level 6+ (when measurement is no longer upon drop)

        public string state = "000000000000000000000000000000000000000000";
		public int[] colPointers = { 5, 5, 5, 5, 5, 5, 5 };
		public HashSet<(int, int)> visited = new HashSet<(int, int)>();

        public int[] superpositionArray;


		public int index(int r, int c)
		{
			int i = r * 7 + c;
			return i;
		}

		public (int, int) reverse_index(int i)
		{
			int r = i / 7;
			int c = i % 7;
			return (r, c);
		}

		// DEBUGGING
		public void printState()
		{
			string p_state = "";
			for (int r = 0; r < 6; r++)
			{
				int i = index(r, 0);
				p_state += state.Substring(i, 7) + "\n";
			}
			Debug.Log(p_state);
		}

		public int evaluateState()
		{
			int score = 0;
			visited.Clear();

			for (int r = 0; r < 6; r++)
			{
				for (int c = 0; c < 7; c++)
				{
					int i = index(r, c);
					if (state.Substring(i, 1).Equals("2")) // && !visited.Contains((r,c)) -- adding this causes missed connections, but speed?
					{
						score += evaluatePosition(r, c, "2");
					}
					else if (state.Substring(i, 1).Equals("1"))
					{
						score -= evaluatePosition(r, c, "1");
                        // In level 1 this is -= 2 * evaluatePosition
					}
				}
			}
			return score;
		}

		public int evaluatePosition(int r, int c, string color)
		{
			int i = index(r, c);
			(int, int) pos;

			int[] num_connected = new int[4];
			int hasCenter = 0;

			// look right
			int r_counter = 0;
			while (i < state.Length && state.Substring(i, 1).Equals(color))
			{
				pos = reverse_index(i);
				visited.Add(pos);

				(int row, int col) = pos;
				if (col >= 2 && col <= 4)
					hasCenter = 1;

				r_counter++;
				i++;

                // Below not in level 1
				if ((i % 7) == 0)
					i = state.Length;
			}
			r_counter = Mathf.Min(4, r_counter); // if 5 or more connected, goes to 4.
			num_connected[r_counter - 1]++;

			// look down
			i = index(r, c);
			int d_counter = 0;
			while (i < state.Length && state.Substring(i, 1).Equals(color))
			{
				pos = reverse_index(i);
				visited.Add(pos);

				(int row, int col) = pos;
				if (col >= 2 && col <= 4)
					hasCenter = 1;

				d_counter++;
				i += 7;
			}
			d_counter = Mathf.Min(4, d_counter);
			num_connected[d_counter - 1]++;

			// look diagonal right-down
			i = index(r, c);
			int rd_counter = 0;
			while (i < state.Length && state.Substring(i, 1).Equals(color))
			{
				pos = reverse_index(i);
				visited.Add(pos);

				(int row, int col) = pos;
				if (col >= 2 && col <= 4)
					hasCenter = 1;

				rd_counter++;
				i += 8;

                // Below not in level 1
				if ((i % 7) == 0)
					i = state.Length;
			}
			rd_counter = Mathf.Min(4, rd_counter);
			num_connected[rd_counter - 1]++;

			// look diagonal left-down
			i = index(r, c);
			int ld_counter = 0;
			while (i < state.Length && state.Substring(i, 1).Equals(color))
			{
				pos = reverse_index(i);
				visited.Add(pos);

				(int row, int col) = pos;
				if (col >= 2 && col <= 4)
					hasCenter = 1;

				ld_counter++;
				i += 6;

                // Below not in level 1
				if ((i % 7) == 6)
					i = state.Length;
			}
			ld_counter = Mathf.Min(4, ld_counter);
			num_connected[ld_counter - 1]++;

			int score = 100 * num_connected[3] + 20 * num_connected[2] + 3 * num_connected[1] + 10 * hasCenter;
			return score;
		}

        bool isWin(int r, int c, string color)
		{
			int i = index(r, c);
			// look right
			int r_counter = 0;
			while (i < state.Length && state.Substring(i, 1).Equals(color))
			{
				r_counter++;
				i++;
				if ((i % 7) == 0)
					i = state.Length;
                if (difficulty > 1) {
                    if (color == "1" && i < state.Length && superpositionArray[i] != 100)
					    i = state.Length; }
			}

			i = index(r, c);
			while (i >= 0 && state.Substring(i, 1).Equals(color))
			{
				r_counter++;
				i--;
				if ((i % 7) == 6)
					i = -1;
                if (difficulty > 1) {
                    if (color == "1" && i >= 0 && superpositionArray[i] != 100)
                        i = -1; }
			}
			r_counter--; // center val counted twice
			if (r_counter >= 4)
				return true;

			// look down
			i = index(r, c);
			int d_counter = 0;
			while (i < state.Length && state.Substring(i, 1).Equals(color))
			{
				d_counter++;
				i += 7;
                if (difficulty > 1) {
                    if (color == "1" && i < state.Length && superpositionArray[i] != 100)
                        i = state.Length;
				}
			}
            // Level 6+ says below section not needed, vertical case redundant
			// i = index(r, c);
			// while (i >= 0 && state.Substring(i, 1).Equals(color))
			// {
			// 	d_counter++;
			// 	i -= 7;
			// }
			// d_counter--; //center val counted twice

			if (d_counter >= 4)
				return true;

			// look diagonal right-down
			i = index(r, c);
			int rd_counter = 0;
			while (i < state.Length && state.Substring(i, 1).Equals(color))
			{
				rd_counter++;
				i += 8;
				if ((i % 7) == 0)
					i = state.Length;
                if (difficulty > 1) {
                    if (color == "1" && i < state.Length && superpositionArray[i] != 100)
                        i = state.Length; }
			}
			i = index(r, c);
			while (i >= 0 && state.Substring(i, 1).Equals(color))
			{
				rd_counter++;
				i -= 8;
				if ((i % 7) == 6)
					i = -1;
                if (difficulty > 1) {
                    if (color == "1" && i >= 0 && superpositionArray[i] != 100)
                        i = -1; }
			}
			rd_counter--;
			if (rd_counter >= 4)
				return true;

			// look diagonal left-down
			i = index(r, c);
			int ld_counter = 0;
			while (i < state.Length && state.Substring(i, 1).Equals(color))
			{
				ld_counter++;
				i += 6;
				if ((i % 7) == 6)
					i = state.Length;
                if (difficulty > 1) {
                    if (color == "1" && i < state.Length && superpositionArray[i] != 100)
                        i = state.Length; }
			}
			i = index(r, c);
			while (i >= 0 && state.Substring(i, 1).Equals(color))
			{
				ld_counter++;
				i -= 6;
				if ((i % 7) == 0)
					i = -1;
                if (difficulty > 1) {
                    if (color == "1" && i >= 0 && superpositionArray[i] != 100)
                        i = -1; }
			}
			ld_counter--;
			if (ld_counter >= 4)
				return true;

			return false;
		}

		public List<int> getMoves(int[] cols)
		{
			List<int> possCols = new List<int>();
			for (int i = 0; i < cols.Length; i++)
			{
				if (cols[i] != -1)
					possCols.Add(i);
			}
			return possCols;
		}

		public void playMove(int column, string color)
		{
			int r = colPointers[column];
			colPointers[column] -= 1;

			int i = index(r, column);

			if (i <= 0)
				state = color + state.Substring(1); //prevents substring with length 0 error
			else
				state = state.Substring(0, i) + color + state.Substring(i + 1);
		}

		public void reverseMove(int column)
		{
			colPointers[column] += 1;
			int r = colPointers[column];

			int i = index(r, column);

			if (i == 0)
				state = "0" + state.Substring(1); //prevents substring with length 0 error
			else
			{
				state = state.Substring(0, i) + "0" + state.Substring(i + 1);
			}

		}

		public int findBestMove(int[] cols)
		{
			int bestVal = int.MinValue;
			int bestMove = -1;

			List<int> moves = getMoves(cols);

            if (difficulty > 1) { //checking for all 100% yellow win
                foreach (int column in moves)
			    {
				    playMove(column, "1");
				    if (isWin(colPointers[column] + 1, column, "1"))
                    {
                        reverseMove(column);
                        return column;
                    }
                    reverseMove(column);
			    }
            }

			foreach (int column in moves)
			{
				playMove(column, "2");

                if (difficulty > 0) {
                    if (isWin(colPointers[column] + 1, column, "2"))
                    {
                        reverseMove(column);
                        return column;
                    }
                }

				int value = minimax(0, 3, false);

				if (value > bestVal)
				{
					bestVal = value;
					bestMove = column;
				}
				reverseMove(column);
			}
			return bestMove;
		}

		public int minimax(int depth, int maxDepth, bool isMaximizing)
		{
			List<int> moves = getMoves(colPointers);
			int bestVal;

			if (moves.Count == 0 || depth == maxDepth)
				return evaluateState();

			if (isMaximizing) // None of them are ever maximizing
			{
				bestVal = int.MinValue;
				foreach (int column in moves)
				{
					playMove(column, "2");
					int value = minimax(depth + 1, maxDepth, !isMaximizing);
					bestVal = Mathf.Max(bestVal, value);
					reverseMove(column);
				}
			}

			else
			{
				bestVal = int.MaxValue;
				foreach (int column in moves)
				{
					playMove(column, "1");
					int value = minimax(depth + 1, maxDepth, !isMaximizing);
					bestVal = Mathf.Min(bestVal, value);
					reverseMove(column);
				}
			}
			return bestVal;
		}

    }
}
