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

		public int numRows = 6; 	// [Range(3, 8)]
		public int numColumns = 7; 	// [Range(3, 8)]
		public int numPiecesToWin = 4;
		public bool allowDiagonally = true;

		public float dropTime = 4f;

		Data myData = new Data();

		void Start() 
		{

		}

	}
}
