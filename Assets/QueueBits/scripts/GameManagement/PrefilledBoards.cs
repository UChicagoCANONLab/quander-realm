using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QueueBits;

namespace QueueBits {
    public class PrefilledBoards : MonoBehaviour
    {
        /* public enum Piece
		{
			Empty = 0,
			Blue = 1,
			Red = 2
		} */

		public (string, List<(Piece, int, int, int)>) getRandomBoard(int level) 
		{
			string boardName = boardsPerLevel[level][Random.Range(0,8)];
			List<(Piece, int, int, int)> board = boards[boardName];

			return (boardName, board);
		}

		public List<(Piece, int, int, int)> getSpecificBoard(string name)
		{
			List<(Piece, int, int, int)> board = boards[name];
			return board;
		}

        Dictionary<int, string[]> boardsPerLevel = new Dictionary<int, string[]> 
		{
            {3,   new string[] {"5.2F",  "5.1",   "5.2",   "5.3",   "5.4",   "5.5",   "5.6",   "5.3F"}},
            {4,   new string[] {"5.6F",  "5.7F",  "5.9",   "5.8",   "5.4",   "5.5",   "5.6",   "5.7"}},
            {5,   new string[] {"5.10F", "5.9F",  "5.12",  "5.11",  "5.10",  "5.9",   "5.8",   "5.7"}},
            {6,   new string[] {"6.1",   "6.3",   "6.6",   "6.7",   "6.9",   "6.10",  "6.11",  "6.12"}},
            {7,   new string[] {"7.1",   "7.2",   "7.3",   "7.4",   "7.5",   "7.6",   "7.7",   "7.8"}},
            {8,   new string[] {"7.9F",  "7.10F", "7.3F",  "7.4F",  "7.5F",  "7.6F",  "7.7F",  "7.8F"}},
            {9,   new string[] {"7.9",   "7.10",  "7.11",  "7.12",  "8.5",   "8.6",   "8.7",   "8.8"}},
            {10,  new string[] {"8.9",   "8.10",  "7.11F", "7.12F", "8.5F",  "8.6F",  "8.7F",  "8.8F"}},
            {11,  new string[] {"6.1F",  "6.3F",  "6.6F",  "6.7F",  "6.9F",  "6.10F", "6.11F", "6.12F"}},
            {12,  new string[] {"7.1",   "7.2",   "7.3",   "7.4",   "7.5",   "7.6",   "7.7",   "7.8"}},
            {13,  new string[] {"7.9F",  "7.10F", "7.3F",  "7.4F",  "7.5F",  "7.6F",  "7.7F",  "7.8F"}},
            {14,  new string[] {"7.9",   "7.10",  "7.11",  "7.12",  "8.5",   "8.6",   "8.7",   "8.8"}},
            {15,  new string[] {"8.9",   "8.10",  "7.11F", "7.12F", "8.5F",  "8.6F",  "8.7F",  "8.8F"}}
        };

		Dictionary<string, List<(Piece, int, int, int)>> boards = new Dictionary<string, List<(Piece, int, int, int)>> 
		{
			{"5.1", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 100),
				(Piece.Red, 0, 4, 100),
				(Piece.Blue, 1, 4, 100),
				(Piece.Red, 0, 3, 100),
				(Piece.Blue, 0, 2, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 2, 100)
			}},
			{"5.2", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 0, 4, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 0, 3, 100),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 3, 0, 100)
			}},
			{"5.2F", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 6, 4, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 6, 3, 100),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 3, 0, 100)
			}},
			{"5.3", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 1, 3, 100),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 1, 2, 100),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 4, 2, 100)
			}},
			{"5.3F", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 5, 3, 100),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 5, 2, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 2, 2, 100)
			}},
			{"5.4", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 3, 3, 100),
				(Piece.Red, 1, 3, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 0, 4, 100)
			}},
			{"5.5", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 3, 3, 100),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 0, 5, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 2, 2, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 0, 4, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 3, 1, 100)
			}},
			{"5.6", new List<(Piece, int, int, int)> {
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 4, 3, 100),
				(Piece.Red, 4, 2, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 1, 3, 100),
				(Piece.Blue, 3, 1, 100)
			}},
			{"5.6F", new List<(Piece, int, int, int)> {
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 2, 2, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 5, 3, 100),
				(Piece.Blue, 3, 1, 100)
			}},
			{"5.7", new List<(Piece, int, int, int)> {
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 3, 5, 100),
				(Piece.Red, 3, 4, 100),
                (Piece.Blue, 3, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 3, 1, 100),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 5, 4, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 0, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 0, 4, 100)
			}},
			{"5.7F", new List<(Piece, int, int, int)> {
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 3, 5, 100),
				(Piece.Red, 3, 4, 100),
				(Piece.Blue, 3, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 3, 1, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 1, 4, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 6, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 6, 4, 100)
			}},
			{"5.8", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 1, 4, 100),
				(Piece.Blue, 1, 3, 100),
				(Piece.Red, 5, 3, 100),
				(Piece.Blue, 3, 1, 100)
			}},
			{"5.9", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 2, 2, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 0, 4, 100)
			}},
			{"5.9F", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 2, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 6, 5, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 4, 2, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 6, 4, 100)
			}},
			{"5.10", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 4, 2, 100),
				(Piece.Red, 5, 5, 100),
				(Piece.Blue, 2, 2, 100)
			}},
			{"5.10F", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 2, 3, 100),
				(Piece.Blue, 2, 2, 100),
				(Piece.Red, 1, 5, 100),
				(Piece.Blue, 4, 2, 100)
			}},
			{"5.11", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 3, 2, 100),
				(Piece.Red, 3, 1, 100),
				(Piece.Blue, 4, 5, 100),
				(Piece.Red, 4, 4, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 5, 5, 100),
				(Piece.Red, 5, 4, 100),
				(Piece.Blue, 2, 4, 100),
				(Piece.Red, 0, 5, 100),
				(Piece.Blue, 0, 4, 100)
			}},
			{"5.12", new List<(Piece, int, int, int)> {
				(Piece.Red, 3, 5, 100),
				(Piece.Blue, 3, 4, 100),
				(Piece.Red, 4, 5, 100),
				(Piece.Blue, 2, 5, 100),
				(Piece.Red, 3, 3, 100),
				(Piece.Blue, 4, 4, 100),
				(Piece.Red, 2, 4, 100),
				(Piece.Blue, 1, 5, 100),
				(Piece.Red, 4, 3, 100),
				(Piece.Blue, 2, 3, 100),
				(Piece.Red, 3, 2, 100),
				(Piece.Blue, 4, 2, 100),
				(Piece.Red, 2, 2, 100),
				(Piece.Blue, 3, 1, 100)
			}},
			{"6.1", new List<(Piece, int, int, int)> {
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
			}},
			{"6.1F", new List<(Piece, int, int, int)> {
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
			}},
			{"6.3", new List<(Piece, int, int, int)> {
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
			}},
			{"6.3F", new List<(Piece, int, int, int)> {
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
			}},
			{"6.6", new List<(Piece, int, int, int)> {
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
			}},
			{"6.6F", new List<(Piece, int, int, int)> {
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
			}},
			{"6.7", new List<(Piece, int, int, int)> {
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
			}},
			{"6.7F", new List<(Piece, int, int, int)> {
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
			}},
			{"6.9", new List<(Piece, int, int, int)> {
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
			}},
			{"6.9F", new List<(Piece, int, int, int)> {
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
			}},
			{"6.10", new List<(Piece, int, int, int)> {
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
			}},
			{"6.10F", new List<(Piece, int, int, int)> {
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
			}},
			{"6.11", new List<(Piece, int, int, int)> {
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
			}},
			{"6.11F", new List<(Piece, int, int, int)> {
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
			}},
			{"6.12", new List<(Piece, int, int, int)> {
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
			}},
			{"6.12F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.1", new List<(Piece, int, int, int)> {
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
			}},
			{"7.2", new List<(Piece, int, int, int)> {
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
			}},
			{"7.3", new List<(Piece, int, int, int)> {
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
			}},
			{"7.3F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.4", new List<(Piece, int, int, int)> {
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
			}},
			{"7.4F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.5", new List<(Piece, int, int, int)> {
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
			}},
			{"7.5F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.6", new List<(Piece, int, int, int)> {
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
			}},
			{"7.6F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.7", new List<(Piece, int, int, int)> {
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
			}},
			{"7.7F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.8", new List<(Piece, int, int, int)> {
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
			}},
			{"7.8F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.9", new List<(Piece, int, int, int)> {
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
			}},
			{"7.9F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.10", new List<(Piece, int, int, int)> {
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
			}},
			{"7.10F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.11", new List<(Piece, int, int, int)> {
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
			}},
			{"7.11F", new List<(Piece, int, int, int)> {
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
			}},
			{"7.12", new List<(Piece, int, int, int)> {
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
			}},
			{"7.12F", new List<(Piece, int, int, int)> {
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
			}},
			{"8.5", new List<(Piece, int, int, int)> {
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
			}},
			{"8.5F", new List<(Piece, int, int, int)> {
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
			}},
			{"8.6", new List<(Piece, int, int, int)> {
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
			}},
			{"8.6F", new List<(Piece, int, int, int)> {
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
			}},
			{"8.7", new List<(Piece, int, int, int)> {
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
			}},
			{"8.7F", new List<(Piece, int, int, int)> {
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
			}},
			{"8.8", new List<(Piece, int, int, int)> {
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
			}},
			{"8.8F", new List<(Piece, int, int, int)> {
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
			}},
			{"8.9", new List<(Piece, int, int, int)> {
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
			}},
			{"8.10", new List<(Piece, int, int, int)> {
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
			}}
		};

    }
}