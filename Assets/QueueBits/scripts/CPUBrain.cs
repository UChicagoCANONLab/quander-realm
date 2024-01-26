using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;

namespace QueueBits {
    public class CPUBrain : MonoBehaviour
    {
        // From Level3.cs
        // Lines 250-600ish (AFTER heavy editing)

        public string state = "000000000000000000000000000000000000000000";
		public int[] colPointers = { 5, 5, 5, 5, 5, 5, 5 };
		public HashSet<(int, int)> visited = new HashSet<(int, int)>();


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
		/* public void printState()
		{
			string p_state = "";
			for (int r = 0; r < 6; r++)
			{
				int i = index(r, 0);
				p_state += state.Substring(i, 7) + "\n";
			}
			Debug.Log(p_state);
		} */

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
				if ((i % 7) == 6)
					i = state.Length;
			}
			ld_counter = Mathf.Min(4, ld_counter);
			num_connected[ld_counter - 1]++;

			int score = 100 * num_connected[3] + 20 * num_connected[2] + 3 * num_connected[1] + 10 * hasCenter;
			return score;
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

			if (i == 0)
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

		public int checkForWinner(int r, int c)
        {
			int i = index(r, c);
			char color = state[i];

			//horizontal win
			int horizontal = 1;
			int searchInd = i + 1;
			//look right
			while ((searchInd % 7)!=0 && state[searchInd]==color)
			{
				horizontal++;
				searchInd++;
			}
			//look left
			searchInd = i - 1;
			while ((searchInd % 7) != 6 && state[searchInd] == color)
			{
				horizontal++;
				searchInd--;
			}
			if (horizontal == 4)
			{
				if (color == '2')
					return 1;
				else
					return -1;
			}

			// look down
			int vertical = 1;
			searchInd = i - 7;
			while (searchInd >= 0 && state[searchInd] == color)
			{
				vertical++;
				searchInd -= 7;
			}
			//look up
			searchInd = i + 7;
			while (searchInd < state.Length && state[searchInd] == color)
			{
				vertical++;
				searchInd += 7;
			}
			if (vertical == 4)
			{
				if (color == '2')
					return 1;
				else
					return -1;
			}

			// look diagonal right-down
			searchInd = i+8;
			int diagright = 1;
			while (searchInd < state.Length && state[searchInd]==color)
			{
				diagright++;
				searchInd += 8;
			}
			//look diagonal up-left
			searchInd = i - 8;
			while (searchInd >= 0 && state[searchInd] == color)
			{
				diagright++;
				searchInd -= 8;
			}
			if (diagright == 4)
			{
				if (color == '2')
					return 1;
				else
					return -1;
			}

			// look diagonal left-down
			searchInd = i + 6;
			int leftdown = 1;
			while (searchInd < state.Length && state[searchInd] == color)
			{
				leftdown++;
				i += 6;
			}
			//look diagonal up-right
			searchInd = i - 6;
			while ((searchInd % 7) !=0 && state[searchInd] == color)
			{
				leftdown++;
				i -= 6;
			}
			if (leftdown == 4)
			{
				if (color == '2')
					return 1;
				else
					return -1;
			}
			return 0;
		}

		public int findBestMove(int[] cols)
		{
			int bestVal = int.MinValue;
			int bestMove = -1;

			List<int> moves = getMoves(cols);
			foreach (int column in moves)
			{
				playMove(column, "2");
				int value = minimax(0, 3, false);
				Debug.Log("Column " + column + ": " + value);
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
			{
				return evaluateState();
			}
			if (isMaximizing)
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
