using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using QueueBits;

//using System;
//using MySql.Data.MySqlClient;
//using Data;

namespace QueueBits
{
	public class GameController : MonoBehaviour 
	{
		public int LEVEL_NUMBER;

		// [Range(3, 8)]
		public int numRows = 6;
		// [Range(3, 8)]
		public int numColumns = 7;
		// [Tooltip("How many pieces have to be connected to win.")]
		public int numPiecesToWin = 4;
		// [Tooltip("Allow diagonally connected Pieces?")]
		public bool allowDiagonally = true;

		public float dropTime = 4f;


	}
}
