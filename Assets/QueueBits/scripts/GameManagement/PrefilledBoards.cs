using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QueueBits {
    public class PrefilledBoards : MonoBehaviour
    {
        enum Piece
		{
			Empty = 0,
			Blue = 1,
			Red = 2
		}

        Dictionary<int, List<string>> boardsPerLevel = new Dictionary<int, List<string>> {
            {3,   {"5.2F",  "5.1",   "5.2",   "5.3",   "5.4",   "5.5",   "5.6",   "5.3F"}},
            {4,   {"5.6F",  "5.7F",  "5.9",   "5.8",   "5.4",   "5.5",   "5.6",   "5.7"}},
            {5,   {"5.10F", "5.9F",  "5.12",  "5.11",  "5.10",  "5.9",   "5.8",   "5.7"}},
            {6,   {"6.1",   "6.3",   "6.6",   "6.7",   "6.9",   "6.10",  "6.11",  "6.12"}},
            {7,   {"7.1",   "7.2",   "7.3",   "7.4",   "7.5",   "7.6",   "7.7",   "7.8"}},
            {8,   {"7.9F",  "7.10F", "7.3F",  "7.4F",  "7.5F",  "7.6F",  "7.7F",  "7.8F"}},
            {9,   {"7.9",   "7.10",  "7.11",  "7.12",  "8.5",   "8.6",   "8.7",   "8.8"}},
            {10,  {"8.9",   "8.10",  "7.11F", "7.12F", "8.5F",  "8.6F",  "8.7F",  "8.8F"}},
            {11,  {"6.1F",  "6.3F",  "6.6F",  "6.7F",  "6.9F",  "6.10F", "6.11F", "6.12F"}},
            {12,  {"7.1",   "7.2",   "7.3",   "7.4",   "7.5",   "7.6",   "7.7",   "7.8"}},
            {13,  {"7.9F",  "7.10F", "7.3F",  "7.4F",  "7.5F",  "7.6F",  "7.7F",  "7.8F"}},
            {14,  {"7.9",   "7.10",  "7.11",  "7.12",  "8.5",   "8.6",   "8.7",   "8.8"}},
            {15,  {"8.9",   "8.10",  "7.11F", "7.12F", "8.5F",  "8.6F",  "8.7F",  "8.8F"}}
        };


        // define prefilled board
		List<(Piece, int, int)> prefilledBoard = new List<(Piece piece, int col, int row)>();

		Dictionary<int, List<(Piece, int, int)>> prefilledBoardList_Lev3 
            = new Dictionary<int, List<(Piece, int, int)>> {
		{0, new List<(Piece piece, int col, int row)> // 5.2 flipped
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 2, 5),
				(Piece.Blue, 4, 5),
				(Piece.Red, 3, 3),
				(Piece.Blue, 4, 4),
				(Piece.Red, 4, 3),
				(Piece.Blue, 2, 4),
				(Piece.Red, 6, 5),
				(Piece.Blue, 6, 4),
				(Piece.Red, 3, 2),
				(Piece.Blue, 6, 3),
				(Piece.Red, 3, 1),
				(Piece.Blue, 3, 0)
			}
		},
		{1, new List<(Piece piece, int col, int row)> //5.1
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 2, 5),
				(Piece.Blue, 4, 5),
				(Piece.Red, 2, 4),
				(Piece.Blue, 2, 3),
				(Piece.Red, 1, 5),
				(Piece.Blue, 0, 5),
				(Piece.Red, 0, 4),
				(Piece.Blue, 1, 4),
				(Piece.Red, 0, 3),
				(Piece.Blue, 0, 2),
				(Piece.Red, 4, 4),
				(Piece.Blue, 2, 2)
			}
		},
		{2, new List<(Piece piece, int col, int row)> //5.2
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 4, 5),
				(Piece.Blue, 2, 5),
				(Piece.Red, 3, 3),
				(Piece.Blue, 2, 4),
				(Piece.Red, 2, 3),
				(Piece.Blue, 4, 4),
				(Piece.Red, 0, 5),
				(Piece.Blue, 0, 4),
				(Piece.Red, 3, 2),
				(Piece.Blue, 0, 3),
				(Piece.Red, 3, 1),
				(Piece.Blue, 3, 0)
			}
		},
		{3, new List<(Piece piece, int col, int row)> //5.3
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 3, 1),
				(Piece.Blue, 1, 5),
				(Piece.Red, 1, 4),
				(Piece.Blue, 1, 3),
				(Piece.Red, 2, 5),
				(Piece.Blue, 4, 5),
				(Piece.Red, 1, 2),
				(Piece.Blue, 4, 4),
				(Piece.Red, 4, 3),
				(Piece.Blue, 4, 2)
			}
		},
		{4, new List<(Piece piece, int col, int row)> //5.4
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 2, 5),
				(Piece.Blue, 4, 5),
				(Piece.Red, 0, 5),
				(Piece.Blue, 1, 5),
				(Piece.Red, 4, 4),
				(Piece.Blue, 2, 4),
				(Piece.Red, 1, 4),
				(Piece.Blue, 3, 3),
				(Piece.Red, 1, 3),
				(Piece.Blue, 5, 5),
				(Piece.Red, 5, 4),
				(Piece.Blue, 0, 4)
			}
		},
		{5, new List<(Piece piece, int col, int row)> //5.5
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 4, 5),
				(Piece.Red, 2, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 2, 4),
				(Piece.Blue, 3, 3),
				(Piece.Red, 1, 5),
				(Piece.Blue, 0, 5),
				(Piece.Red, 2, 3),
				(Piece.Blue, 2, 2),
				(Piece.Red, 3, 2),
				(Piece.Blue, 0, 4),
				(Piece.Red, 4, 4),
				(Piece.Blue, 3, 1)
			}
		},
		{6, new List<(Piece piece, int col, int row)> //5.6
			{
				(Piece.Red, 4, 5),
				(Piece.Blue, 1, 5),
				(Piece.Red, 3, 5),
				(Piece.Blue, 5, 5),
				(Piece.Red, 1, 4),
				(Piece.Blue, 4, 4),
				(Piece.Red, 5, 4),
				(Piece.Blue, 4, 3),
				(Piece.Red, 4, 2),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 1, 3),
				(Piece.Blue, 3, 1)
			}
		},
		{7, new List<(Piece piece, int col, int row)> //5.3 flipped
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 3, 1),
				(Piece.Blue, 5, 5),
				(Piece.Red, 5, 4),
				(Piece.Blue, 5, 3),
				(Piece.Red, 4, 5),
				(Piece.Blue, 2, 5),
				(Piece.Red, 5, 2),
				(Piece.Blue, 2, 4),
				(Piece.Red, 2, 3),
				(Piece.Blue, 2, 2)
			}
		}
	    };


        Dictionary<int, List<(Piece, int, int)>> prefilledBoardList_Lev4 
            = new Dictionary<int, List<(Piece, int, int)>> {
		{0, new List<(Piece piece, int col, int row)> //5.6 flipped
			{
				(Piece.Red, 2, 5),
				(Piece.Blue, 5, 5),
				(Piece.Red, 3, 5),
				(Piece.Blue, 1, 5),
				(Piece.Red, 5, 4),
				(Piece.Blue, 2, 4),
				(Piece.Red, 1, 4),
				(Piece.Blue, 2, 3),
				(Piece.Red, 2, 2),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 5, 3),
				(Piece.Blue, 3, 1)
			}
		},
		{1, new List<(Piece piece, int col, int row)> //5.7 flipped
			{
				(Piece.Red, 4, 5),
				(Piece.Blue, 3, 5),
				(Piece.Red, 3, 4),
				(Piece.Blue, 3, 3),
				(Piece.Red, 3, 2),
				(Piece.Blue, 1, 5),
				(Piece.Red, 5, 5),
				(Piece.Blue, 3, 1),
				(Piece.Red, 0, 5),
				(Piece.Blue, 1, 4),
				(Piece.Red, 5, 4),
				(Piece.Blue, 6, 5),
				(Piece.Red, 4, 4),
				(Piece.Blue, 6, 4)
			}
		},
		{2, new List<(Piece piece, int col, int row)> //5.9
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 2, 5),
				(Piece.Red, 2, 4),
				(Piece.Blue, 5, 5),
				(Piece.Red, 4, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 4, 4),
				(Piece.Red, 0, 5),
				(Piece.Blue, 3, 2),
				(Piece.Red, 2, 3),
				(Piece.Blue, 2, 2),
				(Piece.Red, 4, 3),
				(Piece.Blue, 0, 4)
			}
		},
			{3, new List<(Piece piece, int col, int row)> //5.8
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 5, 5),
				(Piece.Red, 2, 5),
				(Piece.Blue, 1, 5),
				(Piece.Red, 5, 4),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 2, 4),
				(Piece.Blue, 2, 3),
				(Piece.Red, 1, 4),
				(Piece.Blue, 1, 3),
				(Piece.Red, 5, 3),
				(Piece.Blue, 3, 1)
			}
		},
			{4, new List<(Piece piece, int col, int row)> //5.4
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 2, 5),
				(Piece.Blue, 4, 5),
				(Piece.Red, 0, 5),
				(Piece.Blue, 1, 5),
				(Piece.Red, 4, 4),
				(Piece.Blue, 2, 4),
				(Piece.Red, 1, 4),
				(Piece.Blue, 3, 3),
				(Piece.Red, 1, 3),
				(Piece.Blue, 5, 5),
				(Piece.Red, 5, 4),
				(Piece.Blue, 0, 4)
			}
		},
		{5, new List<(Piece piece, int col, int row)> //5.5
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 4, 5),
				(Piece.Red, 2, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 2, 4),
				(Piece.Blue, 3, 3),
				(Piece.Red, 1, 5),
				(Piece.Blue, 0, 5),
				(Piece.Red, 2, 3),
				(Piece.Blue, 2, 2),
				(Piece.Red, 3, 2),
				(Piece.Blue, 0, 4),
				(Piece.Red, 4, 4),
				(Piece.Blue, 3, 1)
			}
		},
		{6, new List<(Piece piece, int col, int row)> //5.6
			{
				(Piece.Red, 4, 5),
				(Piece.Blue, 1, 5),
				(Piece.Red, 3, 5),
				(Piece.Blue, 5, 5),
				(Piece.Red, 1, 4),
				(Piece.Blue, 4, 4),
				(Piece.Red, 5, 4),
				(Piece.Blue, 4, 3),
				(Piece.Red, 4, 2),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 1, 3),
				(Piece.Blue, 3, 1)
			}
		},
		{7, new List<(Piece piece, int col, int row)> //5.7
			{
				(Piece.Red, 2, 5),
				(Piece.Blue, 3, 5),
				(Piece.Red, 3, 4),
                (Piece.Blue, 3, 3),
				(Piece.Red, 3, 2),
				(Piece.Blue, 5, 5),
				(Piece.Red, 1, 5),
				(Piece.Blue, 3, 1),
				(Piece.Red, 6, 5),
				(Piece.Blue, 5, 4),
				(Piece.Red, 1, 4),
				(Piece.Blue, 0, 5),
				(Piece.Red, 2, 4),
				(Piece.Blue, 0, 4)
			}
		}
	    };

        Dictionary<int, List<(Piece, int, int)>> prefilledBoardList_Lev5 
            = new Dictionary<int, List<(Piece, int, int)>> {
		{0, new List<(Piece piece, int col, int row)> //5.10 flipped
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 4, 5),
				(Piece.Red, 4, 4),
				(Piece.Blue, 2, 5),
				(Piece.Red, 4, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 3, 1),
				(Piece.Blue, 2, 4),
				(Piece.Red, 2, 3),
				(Piece.Blue, 2, 2),
				(Piece.Red, 1, 5),
				(Piece.Blue, 4, 2)
			}
		},
		{1, new List<(Piece piece, int col, int row)> //5.9 flipped
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 4, 5),
				(Piece.Red, 4, 4),
				(Piece.Blue, 1, 5),
				(Piece.Red, 2, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 2, 4),
				(Piece.Red, 6, 5),
				(Piece.Blue, 3, 2),
				(Piece.Red, 4, 3),
				(Piece.Blue, 4, 2),
				(Piece.Red, 2, 3),
				(Piece.Blue, 6, 4)
			}
		},
		{2, new List<(Piece piece, int col, int row)> //5.12
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 4, 5),
				(Piece.Blue, 2, 5),
				(Piece.Red, 3, 3),
				(Piece.Blue, 4, 4),
				(Piece.Red, 2, 4),
				(Piece.Blue, 1, 5),
				(Piece.Red, 4, 3),
				(Piece.Blue, 2, 3),
				(Piece.Red, 3, 2),
				(Piece.Blue, 4, 2),
				(Piece.Red, 2, 2),
				(Piece.Blue, 3, 1)
			}
		},
        {3, new List<(Piece piece, int col, int row)> //5.11
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 3, 1),
				(Piece.Blue, 4, 5),
				(Piece.Red, 4, 4),
				(Piece.Blue, 2, 5),
				(Piece.Red, 4, 3),
				(Piece.Blue, 5, 5),
				(Piece.Red, 5, 4),
				(Piece.Blue, 2, 4),
				(Piece.Red, 0, 5),
				(Piece.Blue, 0, 4)
			}
		},
		{4, new List<(Piece piece, int col, int row)> //5.10
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 2, 5),
				(Piece.Red, 2, 4),
				(Piece.Blue, 4, 5),
				(Piece.Red, 2, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 3, 1),
				(Piece.Blue, 4, 4),
				(Piece.Red, 4, 3),
				(Piece.Blue, 4, 2),
				(Piece.Red, 5, 5),
				(Piece.Blue, 2, 2)
			}
		},
		{5, new List<(Piece piece, int col, int row)> //5.9
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 2, 5),
				(Piece.Red, 2, 4),
				(Piece.Blue, 5, 5),
				(Piece.Red, 4, 5),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 4, 4),
				(Piece.Red, 0, 5),
				(Piece.Blue, 3, 2),
				(Piece.Red, 2, 3),
				(Piece.Blue, 2, 2),
				(Piece.Red, 4, 3),
				(Piece.Blue, 0, 4)
			}
		},
        {6, new List<(Piece piece, int col, int row)> //5.8
			{
				(Piece.Red, 3, 5),
				(Piece.Blue, 5, 5),
				(Piece.Red, 2, 5),
				(Piece.Blue, 1, 5),
				(Piece.Red, 5, 4),
				(Piece.Blue, 3, 4),
				(Piece.Red, 3, 3),
				(Piece.Blue, 3, 2),
				(Piece.Red, 2, 4),
				(Piece.Blue, 2, 3),
				(Piece.Red, 1, 4),
				(Piece.Blue, 1, 3),
				(Piece.Red, 5, 3),
				(Piece.Blue, 3, 1)
			}
		},
		{7, new List<(Piece piece, int col, int row)> //5.7
			{
				(Piece.Red, 2, 5),
				(Piece.Blue, 3, 5),
				(Piece.Red, 3, 4),
				(Piece.Blue, 3, 3),
				(Piece.Red, 3, 2),
				(Piece.Blue, 5, 5),
				(Piece.Red, 1, 5),
				(Piece.Blue, 3, 1),
				(Piece.Red, 6, 5),
				(Piece.Blue, 5, 4),
				(Piece.Red, 1, 4),
				(Piece.Blue, 0, 5),
				(Piece.Red, 2, 4),
				(Piece.Blue, 0, 4)
			}
		}
		};

        List<(Piece, int, int, int)> prefilledBoard_prob = new List<(Piece piece, int col, int row, int prob)>();

        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev6 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
		    {0, new List<(Piece piece, int col, int row, int prob)> //6.1
			    {
		    	(Piece.Red, 3, 5, 75),
	    		(Piece.Blue, 3, 4, 75),
    			(Piece.Red, 2, 5, 75),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 100),
				(Piece.Red, 0, 4, 75),
				(Piece.Blue, 1, 4, 75),
				(Piece.Red, 0, 3, 75),
				(Piece.Blue, 0, 2, 75),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 2, 2, 100)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //6.3
				{
					(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 75),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 1, 3, 75),
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 1, 2, 75),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 4, 2, 75)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //6.6
				{
					(Piece.Red, 4, 5, 75),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 4, 3, 75),
				(Piece.Red, 4, 2, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 1, 3, 75),
				(Piece.Blue, 3, 1, 75)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //6.7
			{
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 3, 5, 100),
				(Piece.Red, 3, 4, 75),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 3, 1, 75),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 5, 4, 75),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 0, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 0, 4, 75)
			}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //6.9
			{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 2, 2, 100),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 0, 4, 100)
			}
		},
			{5, new List<(Piece piece, int col, int row, int prob)> //6.10
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 4, 2, 75),
				(Piece.Red, 5, 5, 75),
				(Piece.Blue, 2, 2, 75)
			}
		},
			{6, new List<(Piece piece, int col, int row, int prob)> //6.11
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 2, 4, 75),
				(Piece.Red, 0, 5, 75),
				(Piece.Blue, 0, 4, 100)
			}
		},
			{7, new List<(Piece piece, int col, int row, int prob)> //6.12
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 4, 2, 75),
				(Piece.Red, 2, 2, 75),
				(Piece.Blue, 3, 1,100)
			}
		}
		};

        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev7 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //7.1
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 100),
				(Piece.Red, 0, 4, 75),
				(Piece.Blue, 1, 4, 75),
				(Piece.Red, 0, 3, 50),
				(Piece.Blue, 0, 2, 50),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 2, 2, 100)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //7.2
				{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 0, 4, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 0, 3, 50),
				(Piece.Red, 0, 2, 50),
				(Piece.Blue, 3, 1, 75)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //7.3
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 75),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 1, 3, 75),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 1, 2, 75),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 4, 3, 50),
				(Piece.Blue, 4, 2, 50)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //7.4
			{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 4, 5, 50),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 4, 75),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 1, 3, 100),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 5, 4, 50),
				(Piece.Blue, 0, 4, 100)
			}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //7.5
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 4, 50),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 50),
				(Piece.Red, 0, 4, 100),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 2, 2, 100),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 3, 1, 100)
			}
		    },
			{5, new List<(Piece piece, int col, int row, int prob)> //7.6
			{
				(Piece.Red, 4, 5, 40),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 4, 50),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 4, 3, 75),
				(Piece.Red, 4, 2, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 1, 3, 100),
				(Piece.Blue, 3, 1, 50)
			}
	    	},
    		{6, new List<(Piece piece, int col, int row, int prob)> //7.7
			{
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 3, 5, 100),
				(Piece.Red, 3, 4, 50),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 3, 1, 100),
				(Piece.Red, 6, 5, 50),
				(Piece.Blue, 5, 4, 50),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 0, 5, 75),
				(Piece.Red,0, 4, 75),
				(Piece.Blue, 1, 4, 100)
			}
    		},
			{7, new List<(Piece piece, int col, int row, int prob)> //7.8
			{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 4, 50),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 1, 3, 50),
				(Piece.Red, 5, 3, 75),
				(Piece.Blue, 3, 1, 100)
			}
		}
		};

        
        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev8 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //7.9 flipped
				{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 4, 2, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 6, 4, 100)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //7.10 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 4, 3, 50),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 2, 4, 75),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 2, 2, 50),
				(Piece.Red, 1, 5, 75),
				(Piece.Blue, 4, 2, 75)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //7.3 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 75),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 5, 3, 75),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 5, 2, 75),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 2, 2, 50)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //7.4 flipped
			{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 2, 5, 50),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 5, 3, 100),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 1, 4, 50),
				(Piece.Blue, 6, 4, 100)
			}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //7.5 flipped
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 4, 4, 50),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 6, 5, 50),
				(Piece.Red, 6, 4, 100),
				(Piece.Blue, 4, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 4, 2, 100),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 3, 1, 100)
			}
		},
			{5, new List<(Piece piece, int col, int row, int prob)> //7.6 flipped
			{
				(Piece.Red, 2, 5, 40),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 4, 50),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 2, 2, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 5, 3, 100),
				(Piece.Blue, 3, 1, 50)
			}
		},
			{6, new List<(Piece piece, int col, int row, int prob)> //7.7 flipped
			{
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 3, 5, 100),
				(Piece.Red, 3, 4, 50),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 3, 1, 100),
				(Piece.Red, 0, 5, 50),
				(Piece.Blue, 1, 4, 50),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 6, 5, 75),
				(Piece.Red,6, 4, 75),
				(Piece.Blue, 5, 4, 100)
			}
		},
			{7, new List<(Piece piece, int col, int row, int prob)> //7.8 flipped
			{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 4, 4, 50),
				(Piece.Blue, 4, 3, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 5, 3, 50),
				(Piece.Red, 1, 3, 75),
				(Piece.Blue, 3, 1, 100)
			}
		}
		};


        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev9 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //7.9
				{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 2, 2, 100),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 0, 4, 100)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //7.10
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 4, 2, 50),
				(Piece.Red, 5, 5, 75),
				(Piece.Blue, 2, 2, 75)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //7.11
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 5, 50),
				(Piece.Red, 4, 4, 50),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 4, 3, 50),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 0, 5, 75),
				(Piece.Blue, 0, 4, 100)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //7.12
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 3, 3, 50),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 4, 3, 50),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 4, 2, 75),
				(Piece.Red, 2, 2, 75),
				(Piece.Blue, 3, 1, 100)
				}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //8.5
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 3, 3, 50),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 50),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 2, 2, 50),
				(Piece.Blue, 0, 4, 100),
				(Piece.Red, 4, 4, 50),
				(Piece.Blue, 3, 1, 100)
				}
			},
			{5, new List<(Piece piece, int col, int row, int prob)> //8.6
				{
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 3, 5, 100),
                (Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 4, 3, 75),
				(Piece.Red, 4, 2, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 1, 3, 50),
				(Piece.Blue, 3, 1, 50)
				}
			},
			{6, new List<(Piece piece, int col, int row, int prob)> //8.7
				{
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 3, 5, 50),
				(Piece.Red, 3, 4, 50),
				(Piece.Blue, 3, 3, 100),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 3, 1, 75),
				(Piece.Red, 6, 5, 50),
				(Piece.Blue, 5, 4, 75),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 0, 5, 75),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 0, 4, 50)
				}
			},
			{7, new List<(Piece piece, int col, int row, int prob)> //8.8
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 5, 4, 50),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 2, 3, 50),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 1, 3, 75),
				(Piece.Red, 5, 3, 75),
				(Piece.Blue, 3, 1, 100)
				}
			}
		};


        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev10 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //8.9
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 50),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 2, 2, 50),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 0, 4, 50)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //8.10
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 4, 2, 50),
				(Piece.Red, 5, 5, 75),
				(Piece.Blue, 2, 2, 50)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //7.11 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 2, 5, 50),
				(Piece.Red, 2, 4, 50),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 6, 5, 75),
				(Piece.Blue, 6, 4, 100)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //7.12 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 3, 3, 50),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 4, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 2, 2, 75),
				(Piece.Red, 4, 2, 75),
				(Piece.Blue, 3, 1, 100)
				}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //8.5 flipped
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 3, 3, 50),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 6, 5, 50),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 4, 3, 75),
				(Piece.Red, 4, 2, 50),
				(Piece.Blue, 6, 4, 100),
				(Piece.Red, 2, 4, 50),
				(Piece.Blue, 3, 1, 100)
				}
			},
			{5, new List<(Piece piece, int col, int row, int prob)> //8.6 flipped
				{
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 2, 2, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 5, 3, 50),
				(Piece.Blue, 3, 1, 50)
				}
			},
			{6, new List<(Piece piece, int col, int row, int prob)> //8.7 flipped
				{
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 3, 5, 50),
				(Piece.Red, 3, 4, 50),
				(Piece.Blue, 3, 3, 100),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 3, 1, 75),
				(Piece.Red, 0, 5, 50),
				(Piece.Blue, 1, 4, 75),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 6, 5, 75),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 6, 4, 50)
				}
			},
			{7, new List<(Piece piece, int col, int row, int prob)> //8.8 flipped
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 1, 4, 50),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 4, 3, 50),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 5, 3, 75),
				(Piece.Red, 1, 3, 75),
				(Piece.Blue, 3, 1, 100)
				}
			}
		};


        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev11 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //6.1 flipped
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 4, 3, 75),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 6, 5, 100),
				(Piece.Red, 6, 4, 75),
				(Piece.Blue, 5, 4, 75),
				(Piece.Red, 6, 3, 75),
				(Piece.Blue, 6, 2, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 4, 2, 100)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //6.3 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 75),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 5, 3, 75),
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 5, 2, 75),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 2, 2, 75)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //6.6 flipped
				{
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 2, 2, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 5, 3, 75),
				(Piece.Blue, 3, 1, 75)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //6.7 flipped
			{
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 3, 5, 100),
				(Piece.Red, 3, 4, 75),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 3, 1, 75),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 1, 4, 75),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 6, 5, 75),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 6, 4, 75)
			}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //6.9 flipped
			{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 2, 4, 75),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 4, 2, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 6, 4, 100)
			}
		},
			{5, new List<(Piece piece, int col, int row, int prob)> //6.10 flipped
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 2, 2, 75),
				(Piece.Red, 1, 5, 75),
				(Piece.Blue, 4, 2, 75)
			}
		},
			{6, new List<(Piece piece, int col, int row, int prob)> //6.11 flipped
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 6, 5, 75),
				(Piece.Blue, 6, 4, 100)
			}
		},
			{7, new List<(Piece piece, int col, int row, int prob)> //6.12 flipped
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 2, 4, 75),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 4, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 2, 2, 75),
				(Piece.Red, 4, 2, 75),
				(Piece.Blue, 3, 1,100)
			}
		}
		};


        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev12 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //7.1
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 100),
				(Piece.Red, 0, 4, 75),
				(Piece.Blue, 1, 4, 75),
				(Piece.Red, 0, 3, 50),
				(Piece.Blue, 0, 2, 50),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 2, 2, 100)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //7.2
				{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 0, 4, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 0, 3, 50),
				(Piece.Red, 0, 2, 50),
				(Piece.Blue, 3, 1, 75)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //7.3
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 75),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 1, 3, 75),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 1, 2, 75),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 4, 3, 50),
				(Piece.Blue, 4, 2, 50)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //7.4
			{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 4, 5, 50),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 4, 75),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 1, 3, 100),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 5, 4, 50),
				(Piece.Blue, 0, 4, 100)
			}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //7.5
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 4, 50),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 50),
				(Piece.Red, 0, 4, 100),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 2, 2, 100),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 3, 1, 100)
			}
		},
			{5, new List<(Piece piece, int col, int row, int prob)> //7.6
			{
				(Piece.Red, 4, 5, 40),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 4, 50),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 4, 3, 75),
				(Piece.Red, 4, 2, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 1, 3, 100),
				(Piece.Blue, 3, 1, 50)
			}
		},
			{6, new List<(Piece piece, int col, int row, int prob)> //7.7
			{
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 3, 5, 100),
				(Piece.Red, 3, 4, 50),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 3, 1, 100),
				(Piece.Red, 6, 5, 50),
				(Piece.Blue, 5, 4, 50),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 0, 5, 75),
				(Piece.Red,0, 4, 75),
				(Piece.Blue, 1, 4, 100)
			}
		},
			{7, new List<(Piece piece, int col, int row, int prob)> //7.8
			{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 4, 50),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 1, 3, 50),
				(Piece.Red, 5, 3, 75),
				(Piece.Blue, 3, 1, 100)
			}
		}
		};


        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev13 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //7.9 flipped
				{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 4, 2, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 6, 4, 100)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //7.10 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 4, 3, 50),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 2, 4, 75),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 2, 2, 50),
				(Piece.Red, 1, 5, 75),
				(Piece.Blue, 4, 2, 75)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //7.3 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 75),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 5, 3, 75),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 5, 2, 75),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 2, 2, 50)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //7.4 flipped
			{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 2, 5, 50),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 5, 3, 100),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 1, 4, 50),
				(Piece.Blue, 6, 4, 100)
			}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //7.5 flipped
			{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 4, 4, 50),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 6, 5, 50),
				(Piece.Red, 6, 4, 100),
				(Piece.Blue, 4, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 4, 2, 100),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 3, 1, 100)
			}
		},
			{5, new List<(Piece piece, int col, int row, int prob)> //7.6 flipped
			{
				(Piece.Red, 2, 5, 40),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 4, 50),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 2, 2, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 5, 3, 100),
				(Piece.Blue, 3, 1, 50)
			}
		},
			{6, new List<(Piece piece, int col, int row, int prob)> //7.7 flipped
			{
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 3, 5, 100),
				(Piece.Red, 3, 4, 50),
				(Piece.Blue, 3, 3, 75),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 3, 1, 100),
				(Piece.Red, 0, 5, 50),
				(Piece.Blue, 1, 4, 50),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 6, 5, 75),
				(Piece.Red,6, 4, 75),
				(Piece.Blue, 5, 4, 100)
			}
		},
			{7, new List<(Piece piece, int col, int row, int prob)> //7.8 flipped
			{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 4, 4, 50),
				(Piece.Blue, 4, 3, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 5, 3, 50),
				(Piece.Red, 1, 3, 75),
				(Piece.Blue, 3, 1, 100)
			}
		}
		};


        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev14 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //7.9
				{
				(Piece.Red, 3, 5, 50),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 3, 4, 50),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 2, 2, 100),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 0, 4, 100)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //7.10
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 4, 2, 50),
				(Piece.Red, 5, 5, 75),
				(Piece.Blue, 2, 2, 75)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //7.11
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 5, 50),
				(Piece.Red, 4, 4, 50),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 4, 3, 50),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 0, 5, 75),
				(Piece.Blue, 0, 4, 100)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //7.12
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 3, 3, 50),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 4, 3, 50),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 4, 2, 75),
				(Piece.Red, 2, 2, 75),
				(Piece.Blue, 3, 1, 100)
				}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //8.5
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 4, 5, 75),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 3, 3, 50),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 50),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 2, 2, 50),
				(Piece.Blue, 0, 4, 100),
				(Piece.Red, 4, 4, 50),
				(Piece.Blue, 3, 1, 100)
				}
			},
			{5, new List<(Piece piece, int col, int row, int prob)> //8.6
				{
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 4, 3, 75),
				(Piece.Red, 4, 2, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 1, 3, 50),
				(Piece.Blue, 3, 1, 50)
				}
			},
			{6, new List<(Piece piece, int col, int row, int prob)> //8.7
				{
				(Piece.Red, 2, 5, 75),
				(Piece.Blue, 3, 5, 50),
				(Piece.Red, 3, 4, 50),
				(Piece.Blue, 3, 3, 100),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 3, 1, 75),
				(Piece.Red, 6, 5, 50),
				(Piece.Blue, 5, 4, 75),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 0, 5, 75),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 0, 4, 50)
				}
			},
			{7, new List<(Piece piece, int col, int row, int prob)> //8.8
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 5, 4, 50),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 2, 3, 50),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 1, 3, 75),
				(Piece.Red, 5, 3, 75),
				(Piece.Blue, 3, 1, 100)
				}
			}
		};


        Dictionary<int, List<(Piece, int, int, int)>> prefilledBoardList_Lev15 
            = new Dictionary<int, List<(Piece, int, int, int)>> {
			{0, new List<(Piece piece, int col, int row, int prob)> //8.9
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 50),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 3, 75),
				(Piece.Blue, 2, 2, 50),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 0, 4, 50)
				}
			},
			{1, new List<(Piece piece, int col, int row, int prob)> //8.10
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 2, 4, 75),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 4, 75),
				(Piece.Red, 4, 3, 75),
				(Piece.Blue, 4, 2, 50),
				(Piece.Red, 5, 5, 75),
				(Piece.Blue, 2, 2, 50)
				}
			},
			{2, new List<(Piece piece, int col, int row, int prob)> //7.11 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 75),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 2, 5, 50),
				(Piece.Red, 2, 4, 50),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 1, 5, 75),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 4, 4, 50),
				(Piece.Red, 6, 5, 75),
				(Piece.Blue, 6, 4, 100)
				}
			},
			{3, new List<(Piece piece, int col, int row, int prob)> //7.12 flipped
				{
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 3, 3, 50),
				(Piece.Blue, 2, 4, 50),
				(Piece.Red, 4, 4, 75),
				(Piece.Blue, 5, 5, 50),
				(Piece.Red, 2, 3, 50),
				(Piece.Blue, 4, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 2, 2, 75),
				(Piece.Red, 4, 2, 75),
				(Piece.Blue, 3, 1, 100)
				}
			},
			{4, new List<(Piece piece, int col, int row, int prob)> //8.5 flipped
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 2, 5, 75),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 3, 3, 50),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 6, 5, 50),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 4, 3, 75),
				(Piece.Red, 4, 2, 50),
				(Piece.Blue, 6, 4, 100),
				(Piece.Red, 2, 4, 50),
				(Piece.Blue, 3, 1, 100)
				}
			},
			{5, new List<(Piece piece, int col, int row, int prob)> //8.6 flipped
				{
				(Piece.Red, 2, 5, 50),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 4, 75),
                (Piece.Blue, 2, 4, 50),
				(Piece.Red, 1, 4, 75),
				(Piece.Blue, 2, 3, 75),
				(Piece.Red, 2, 2, 75),
				(Piece.Blue, 3, 4, 75),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 5, 3, 50),
				(Piece.Blue, 3, 1, 50)
				}
			},
			{6, new List<(Piece piece, int col, int row, int prob)> //8.7 flipped
				{
				(Piece.Red, 4, 5, 75),
				(Piece.Blue, 3, 5, 50),
				(Piece.Red, 3, 4, 50),
				(Piece.Blue, 3, 3, 100),
				(Piece.Red, 3, 2, 75),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 3, 1, 75),
				(Piece.Red, 0, 5, 50),
				(Piece.Blue, 1, 4, 75),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 6, 5, 75),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 6, 4, 50)
				}
			},
			{7, new List<(Piece piece, int col, int row, int prob)> //8.8 flipped
				{
				(Piece.Red, 3, 5, 75),
				(Piece.Blue, 1, 5, 50),
				(Piece.Red, 4, 5, 50),
				(Piece.Blue, 5, 5, 75),
				(Piece.Red, 1, 4, 50),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 75),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 4, 3, 50),
				(Piece.Red, 5, 4, 75),
				(Piece.Blue, 5, 3, 75),
				(Piece.Red, 1, 3, 75),
				(Piece.Blue, 3, 1, 100)
				}
			}
		};

    }
}