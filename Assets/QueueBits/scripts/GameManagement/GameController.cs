﻿using UnityEngine;
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
		public int difficulty;

		public int numRows = 6; 	// [Range(3, 8)]
		public int numColumns = 7; 	// [Range(3, 8)]
		public int numPiecesToWin = 4;
		// public bool allowDiagonally = true;
		public float dropTime = 4f;

		public Data myData = new Data();
		public int turn;

		public CPUBrain cpuAI;
		public DisplayManager DM;
		public StarDisplay starDisplay;

		public GameObject fieldObject;

		[Header("Prefilled Boards")]
		public PrefilledBoards PB;
		public List<(Piece, int, int, int)> prefilledBoard = new List<(Piece piece, int col, int row, int prob)>();
		public string boardName;

		[Header("GameModes")]
		public GameMode1 GM1;
		public GameMode2 GM2;
		public GameMode3 GM3;

		// [Header("Booleans")]
		// public bool isPlayersTurn;

		void Start() {
			GM1.gameObject.SetActive(false);
			GM2.gameObject.SetActive(false);
			GM3.gameObject.SetActive(false);

			// GM1.enabled = false;
			// GM2.enabled = false;
			// GM3.enabled = false;

			LEVEL_NUMBER = GameManager.LEVEL;

			if (LEVEL_NUMBER < 6) {
				GM1.gameObject.SetActive(true);
			} else if (LEVEL_NUMBER < 11) {
				GM2.gameObject.SetActive(true);
			} else {
				GM3.gameObject.SetActive(true);
			}
		}


		public void StartGame() 
		{
			if (GameManager.saveData.dialogueSystem[LEVEL_NUMBER])
			{
				// dialoguePhase = true;
				Wrapper.Events.StartDialogueSequence?.Invoke($"QB_Level{LEVEL_NUMBER}");
				GameManager.saveData.dialogueSystem[LEVEL_NUMBER] = false;
				GameManager.Save();
				Wrapper.Events.DialogueSequenceEnded += updateDialoguePhase;
			}

			initMyData();
			DM.initDisplay(LEVEL_NUMBER);

			if (LEVEL_NUMBER > 2) {
				(boardName, prefilledBoard) = PB.getRandomBoard(LEVEL_NUMBER);
				initPrefilledBoard();

				// myData.prefilledBoard = board_num;
				myData.newPrefilledBoard = boardName;
			}
		}


		public void EndGame(Results result, Data finalData)
		{
			myData = finalData;
			myData.winner = (int)result;
			saveData.Save(myData);

			int starsWon = starDisplay.getResults(result);
			if (GameManager.saveData.starSystem[LEVEL_NUMBER] <= starsWon) {
				GameManager.saveData.starSystem[LEVEL_NUMBER] = starsWon;
				GameManager.Save();
			}

			fieldObject.SetActive(false);
			DM.GameOver(result);

			if (GameManager.rewardSystem[LEVEL_NUMBER]) {
				Wrapper.Events.CollectAndDisplayReward?.Invoke(Wrapper.Game.QueueBits, LEVEL_NUMBER);
			}
		}


		public void initMyData() 
		{
			myData.level = LEVEL_NUMBER;
			myData.userID = Wrapper.Events.GetPlayerResearchCode?.Invoke();
			myData.placement_order = new int[numColumns * numRows];
			myData.superposition = new int[numColumns * numRows];
			myData.reveal_order = new int[numColumns * numRows];
			myData.outcome = new int[numColumns * numRows];
			for (int i = 0; i < numColumns * numRows; i++) {
				myData.placement_order[i] = 0;
				myData.superposition[i] = 0;
				myData.reveal_order[i] = 0;
				myData.outcome[i] = 0;
			}
			cpuAI.superpositionArray = myData.superposition;
		}

		// dialogue
		void updateDialoguePhase()
		{
			// dialoguePhase = false;
			Wrapper.Events.DialogueSequenceEnded -= updateDialoguePhase;
		}

		// New funtion to spawn piece when clicking buttons on TokenSelector
		public void tokenSelectedByButton(int prob) {
			if (LEVEL_NUMBER < 6) {
				GM1.tokenSelectedByButton(prob);
			} else if (LEVEL_NUMBER < 11) {
				GM2.tokenSelectedByButton(prob);
			} else {
				GM3.tokenSelectedByButton(prob);
			}
			
		}

		public void initPrefilledBoard() 
		{
			foreach ((Piece pi, int c, int r, int pr) in prefilledBoard)
			{
				turn++;
				int index = r * numColumns + c;
				myData.placement_order[index] = turn;
				
				if (LEVEL_NUMBER > 5 && LEVEL_NUMBER < 11) {
					myData.reveal_order[index] = turn;
				}
				myData.superposition[index] = pr;
				//if (pi == Piece1.Player)//if Yellow
				if ((int)pi == (int)Piece1.Player)//if Yellow
				{
					cpuAI.playMove(c, "1");
					if (pr == 100)
						myData.outcome[index] = 1;
				}
				else
				{
					cpuAI.playMove(c, "2");
					if (pr == 100)
						myData.outcome[index] = 2;
				}
			}
		}

		/// <summary>
		/// check if the field contains an empty cell
		/// </summary>
		/// <returns><c>true</c>, if it contains empty cell, <c>false</c> otherwise.</returns>
		public bool FieldContainsUnknownCell(int[,] field)
		{
			for (int x = 0; x < numColumns; x++)
			{
				for (int y = 0; y < numRows; y++)
				{
					if (field[x, y] == 3 || field[x, y] == 0)
						return true;
				}
			}
			return false;
		}

	}
}
