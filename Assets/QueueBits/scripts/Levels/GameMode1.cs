using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;
using TMPro;
using QueueBits;

namespace QueueBits
{
	public class GameMode1 : MonoBehaviour
	{
		public int LEVEL_NUMBER;

		// [Range(3, 8)]
		// private int numRows = 6;
		// // [Range(3, 8)]
		// private int numColumns = 7;
		// [Tooltip("How many pieces have to be connected to win.")]
		// private int numPiecesToWin = 4;
		// [Tooltip("Allow diagonally connected Pieces?")]
		// private bool allowDiagonally = true;

		// private float dropTime = 4f;

		public int probability;

		public CPUBrain cpuAI;
		
		[Header("Display Objects")]
		// public GameObject displayHolder;
		// public GameObject turnSign;
		public StarDisplay starDisplay;
		// public GameOverScreen resultDisplay;
		// public TokenSelector tokenSelector;
		public DisplayManager DM;
		public GameController GC;

		// Gameobjects 
		[Header("CPU Pieces")]
		public TokenCounter tokenCounterCPU;
		public GameObject pieceCPU100;
		public GameObject pieceCPU75; 
		public GameObject pieceCPU50; 

		[Header("Player Pieces")]
		public TokenCounter tokenCounterPlayer;
		public GameObject piecePlayer100;
		public GameObject piecePlayer50;
		public GameObject piecePlayer75;

		[Header("GameObjects")]
		// public GameObject pieceTemp;
		// public GameObject finalColor;
		public GameObject fieldObject;

		Dictionary<int, int> CPUProbs = new Dictionary<int, int>();
		Dictionary<int, int> playerProbs = new Dictionary<int, int>();

		[Header("Prefilled Boards")]
		public PrefilledBoards PB;
		public List<(Piece, int, int, int)> prefilledBoard = new List<(Piece piece, int col, int row, int prob)>();
		public string boardName;

		
		int choice;

		// temporary gameobject, holds the piece at mouse position until the mouse has clicked
		GameObject gameObjectTurn;

		/// <summary>
		/// The Game field.
		/// 0 = Empty, 1 = Player, 2 = CPU
		/// </summary>
		int[,] field;

		bool isPlayersTurn = true;
		bool isLoading = true;
		bool isDropping = false;
		// bool mouseButtonPressed = false;
		bool gameOver = false;
		bool isCheckingForWinner = false;
		bool starUpdated = false;
		bool SelectMenuGenerated = false;
	

		// dialogue
		bool dialoguePhase = false;

		// Shivani Puli Data Collection
		int turn = 0;
		Data mydata = new Data();

		// Use this for initialization
		void Start()
		{
			ShowStarSystem();

			// dialogue
			if (GameManager.saveData.dialogueSystem[LEVEL_NUMBER])
			{
				dialoguePhase = true;
				Wrapper.Events.StartDialogueSequence?.Invoke($"QB_Level{LEVEL_NUMBER}");
				GameManager.saveData.dialogueSystem[LEVEL_NUMBER] = false;
				GameManager.Save();
				Wrapper.Events.DialogueSequenceEnded += updateDialoguePhase;
			}

			// int board_num = Random.Range(0, prefilledBoardList.Keys.Count);
            // prefilledBoard = prefilledBoardList[board_num];
			(boardName, prefilledBoard) = PB.getRandomBoard(LEVEL_NUMBER);

			//Shivani Puli Data Collection
			mydata.level = LEVEL_NUMBER;
			mydata.userID = Wrapper.Events.GetPlayerResearchCode?.Invoke();
			// mydata.prefilledBoard = board_num;
			mydata.newPrefilledBoard = boardName;
			mydata.placement_order = new int[GC.numColumns * GC.numRows];
			mydata.superposition = new int[GC.numColumns * GC.numRows];
			mydata.reveal_order = new int[GC.numColumns * GC.numRows];
			mydata.outcome = new int[GC.numColumns * GC.numRows];
			for (int i = 0; i < GC.numColumns * GC.numRows; i++)
			{
				mydata.placement_order[i] = 0;
				mydata.superposition[i] = 0;
				mydata.reveal_order[i] = 0;
				mydata.outcome[i] = 0;
			}

			foreach ((Piece pi, int c, int r, int pr) in prefilledBoard)
			{
				turn++;
				int index = r * GC.numColumns + c;
				mydata.placement_order[index] = turn;
				
				mydata.reveal_order[index] = turn;
				mydata.superposition[index] = pr;
				//if (pi == Piece1.Player)//if Yellow
				if ((int)pi == (int)Piece1.Player)//if Yellow
				{
					cpuAI.playMove(c, "1");
					mydata.outcome[index] = 1;
					// FROM LEVEL 6
					/* if (pr == 100)
						mydata.outcome[index] = 1; */
				}
				else
				{
					cpuAI.playMove(c, "2");
					mydata.outcome[index] = 2;
					// FROM LEVEL 6
					/* if (pr == 100)
						mydata.outcome[index] = 2; */
				}
			}
			//Shivani Puli Data collection

			// CPUProbs.Add(75, 7);
			// CPUProbs.Add(100, 7);
			CPUProbs = tokenCounterCPU.getCounterDict(LEVEL_NUMBER);

			// playerProbs.Add(75, 7);
			// playerProbs.Add(100, 7);
			playerProbs = tokenCounterPlayer.getCounterDict(LEVEL_NUMBER);

			int max = Mathf.Max(GC.numRows, GC.numColumns);

			if (GC.numPiecesToWin > max)
				GC.numPiecesToWin = max;

			CreateField();

			isPlayersTurn = false;
			// turnSign.SetActive(isPlayersTurn);
			// tokenSelector.switchTurns(isPlayersTurn);
			DM.SwitchPlayer(isPlayersTurn);
		}

		// dialogue
		void updateDialoguePhase()
		{
			dialoguePhase = false;
			Wrapper.Events.DialogueSequenceEnded -= updateDialoguePhase;
		}

		/// <summary>
		/// Creates the field.
		/// </summary>
		void CreateField()
		{
			// turnSign.SetActive(true);
			// tokenSelector.switchTurns(true);
			DM.SwitchPlayer(true);

			isLoading = true;

			// create an empty field and instantiate the cells
			field = new int[GC.numColumns, GC.numRows];

			// initialize field for pieces
			for (int x = 0; x < GC.numColumns; x++) {
				for (int y = 0; y < GC.numRows; y++) {
					field[x, y] = (int)Piece1.Empty;
				}
			}

			// initialize prefilled board
			for (int i = 0; i < prefilledBoard.Count; i++)
            {
				field[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = (int)prefilledBoard[i].Item1;
				// if (prefilledBoard[i].Item1 == Piece1.Player) {
				if ((int)prefilledBoard[i].Item1 == (int)Piece1.Player) {
					GameObject obj = Instantiate(piecePlayer100, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, fieldObject.transform) as GameObject;
				}
				else {
					GameObject obj = Instantiate(pieceCPU100, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, fieldObject.transform) as GameObject;
				}

				// FROM LEVEL 6
				/* else // if (prefilledBoard[i].Item4 != 100)
                {
					field[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = (int)Piece.Unknown;
					GameObject obj;
					if (prefilledBoard[i].Item1 == Piece1.Player)
					{
						probField[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = prefilledBoard[i].Item4;
						if (prefilledBoard[i].Item4 == 75) {
							obj = Instantiate(piecePlayer75, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity) as GameObject;
						}
						else {
							obj = Instantiate(piecePlayer50, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity) as GameObject;
						}
					}
					else
					{
						probField[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = 100 - prefilledBoard[i].Item4;
						if (prefilledBoard[i].Item4 == 75) {
							obj = Instantiate(pieceCPU75, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity) as GameObject;
						}
						else {
							obj = Instantiate(pieceCPU50, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity) as GameObject;
						}
					}
					Color c = obj.GetComponent<MeshRenderer>().material.color;
					c.a = 0.5f;
					obj.GetComponent<MeshRenderer>().material.color = c;

					dropOrder[numSuperpositionPieces] = (prefilledBoard[i].Item2, prefilledBoard[i].Item3);
					pieces[numSuperpositionPieces] = obj;
					numSuperpositionPieces++;
				} */
			}

			isLoading = false;
			gameOver = false;

			// Piece Count Displays
			// tokenCounterPlayer.setCounter(100, playerProbs[100]);
			// tokenCounterPlayer.setCounter(75, playerProbs[75]);
			// tokenCounterPlayer.disable(50);
			tokenCounterPlayer.initCounter(LEVEL_NUMBER);

			// tokenCounterCPU.setCounter(100, CPUProbs[100]);
			// tokenCounterCPU.setCounter(75, CPUProbs[75]);
			// tokenCounterCPU.disable(50);
			tokenCounterCPU.initCounter(LEVEL_NUMBER);
		}

		/// <summary>
		/// Gets all the possible moves.
		/// </summary>
		/// <returns>The possible moves.</returns>
		public List<int> GetPossibleMoves()
		{
			List<int> possibleMoves = new List<int>();
			for (int x = 0; x < GC.numColumns; x++)
			{
				if (field[x, 0] == 0)
				{
					possibleMoves.Add(x);
				}
			}
			return possibleMoves;
		}

		/// <summary>
		/// Spawns a piece at mouse position above the first row
		/// </summary>
		/// <returns>The piece.</returns>
		public (GameObject, int) SpawnPiece(int choice)
		{
			Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int prob = 0;
			GameObject pieceTemp = piecePlayer100;

			if (isPlayersTurn)
			{
				prob = choice;
				int freq = playerProbs[prob];

				// delete probability from player's list
				playerProbs[prob] -= 1;

				if (prob == 100) {
					pieceTemp = piecePlayer100;
					tokenCounterPlayer.setCounter(100, playerProbs[100]);
				}
				else if (prob == 75) {
					pieceTemp = piecePlayer75;
					tokenCounterPlayer.setCounter(75, playerProbs[75]);
				}
				else if (prob == 50) {
					pieceTemp = piecePlayer50;
					tokenCounterPlayer.setCounter(50, playerProbs[50]);
				}

				if (playerProbs[prob] == 0) {
					playerProbs.Remove(prob);
				}
			}

			else
			{
				int ind = Random.Range(0, CPUProbs.Keys.Count);
				List<int> keyList = new List<int>(CPUProbs.Keys);
				prob = keyList[ind];
				int freq = CPUProbs[prob];

				// delete probability from player's list
				CPUProbs[prob] -= 1;

				if (prob == 100) {
					pieceTemp = pieceCPU100;
					tokenCounterCPU.setCounter(100, CPUProbs[100]);
				}
				else if (prob == 75) {
					pieceTemp = pieceCPU75;
					tokenCounterCPU.setCounter(75, CPUProbs[75]); }
				else if (prob == 50) {
					pieceTemp = pieceCPU50;
					tokenCounterCPU.setCounter(50, CPUProbs[50]);
				}

				if (CPUProbs[prob] == 0) {
					CPUProbs.Remove(prob);
				}

			// IN LEVEL 6 the ELSE ends here

				List<int> moves = GetPossibleMoves();

				// FROM LEVEL 4, 5, 6
				/* for (int i = 0; i < GC.numColumns; i++) {
					for (int j = 0; j < GC.numRows; j++) {
						if (field[i, j] != 0)
						{
							colPointers[i] = j - 1;
							break;
						}
					}
				} */

				if (moves.Count > 0)
				{
					int column = cpuAI.findBestMove(cpuAI.colPointers);
					spawnPos = new Vector3(column, 0, 0);
				}
			}

			GameObject g = Instantiate(pieceTemp,
					new Vector3(
					Mathf.Clamp(spawnPos.x, 0, GC.numColumns - 1),
					fieldObject.transform.position.y + 1, 0), // spawn it above the first row
					Quaternion.identity) as GameObject;

			return (g, prob);
		}

		// Update is called once per frame
		void Update()
		{
			if (isLoading)
				return;

			if (dialoguePhase)
				return;

			if (isCheckingForWinner)
				return;

			if (gameOver)
			{
				// fieldObject.SetActive(false);
				// displayHolder.SetActive(false);
				// resultDisplay.gameObject.SetActive(true);

				return;
			}

			if (isPlayersTurn)
			{
				if (gameObjectTurn != null)
				{
					// update the objects position
					Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					gameObjectTurn.transform.position = new Vector3(
						Mathf.Clamp(pos.x, 0, GC.numColumns - 1),
						fieldObject.transform.position.y + 1, 0);

					// click the left mouse button to drop the piece into the selected column
					if (Input.GetMouseButtonDown(0) && !isDropping)
					{
						// mouseButtonPressed = true;
						StartCoroutine(dropPiece(gameObjectTurn, probability));
					}
					else
					{
						// mouseButtonPressed = false;
					}
				}
			}
			else
			{
				if (gameObjectTurn == null)
				{
					(gameObjectTurn, probability) = SpawnPiece(choice);
				}
				else
				{
					if (!isDropping)
					{
						// Task.Delay(2000);
						Thread.Sleep(1000);
						// Debug.Log(gameObjectTurn.transform.position.ToString());
						StartCoroutine(dropPiece(gameObjectTurn, probability));
					}
				}
			}
		}

		// New funtion to spawn piece when clicking buttons on TokenSelector
		public void tokenSelectedByButton(int prob) {
			choice = prob;
			(gameObjectTurn, probability) = SpawnPiece(choice);
		}

		/// <summary>
		/// This method searches for a empty cell and lets 
		/// the object fall down into this cell
		/// </summary>
		/// <param name="gObject">Game Object.</param>
		IEnumerator dropPiece(GameObject gObject, int probability)
		{
			isDropping = true;
			Vector3 startPosition = gObject.transform.position;
			Vector3 endPosition = new Vector3();

			/* string s = "";
			for (int j = 0; j< GC.numRows; j++)
			{
				for (int p = 0; p < GC.numColumns; p++)
				{
					s += (int)field[p, j];
				}
				s += cpuAI.state.Substring(j * GC.numColumns, GC.numColumns);
				s += "\n";
			}
			//Debug.Log(s); */

			// round to a grid cell
			int x = Mathf.RoundToInt(startPosition.x);
			startPosition = new Vector3(x, startPosition.y, startPosition.z);

			// is there a free cell in the selected column?
			bool foundFreeCell = false;
			(int, int) tempLocation = (-1, -1);

			GameObject finalColor = null;

			for (int i = GC.numRows - 1; i >= 0; i--)
			{
				if (field[x, i] == 0)
				{
					foundFreeCell = true;
					GameObject pieceColorObject = piecePlayer100;
					int numOutcome = 1;

					int p = Random.Range(1, 101);
					if ((p < probability && isPlayersTurn) || (p >= probability && !isPlayersTurn)) {
						pieceColorObject = piecePlayer100;
						numOutcome = (int)Piece1.Player;
					} else if ((p >= probability && isPlayersTurn) || (p < probability && !isPlayersTurn)){
						pieceColorObject = pieceCPU100;
						numOutcome = (int)Piece1.CPU;
					}

					Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					finalColor = Instantiate(
						pieceColorObject, // is players turn = spawn player, else spawn CPU
						new Vector3(Mathf.Clamp(pos.x, 0, GC.numColumns - 1),
						fieldObject.transform.position.y + 1, 0), // spawn it above the first row
						Quaternion.identity, fieldObject.transform) as GameObject;
					field[x, i] = numOutcome;
					//Shivani Puli data collection
					int r = cpuAI.colPointers[x];
					int index = r * GC.numColumns + x;
					
					turn++;
					mydata.placement_order[index] = turn;
					mydata.superposition[index] = probability;
					mydata.reveal_order[index] = turn;
					mydata.outcome[index] = numOutcome;
					// data collection
					cpuAI.superpositionArray = mydata.superposition;
					cpuAI.playMove(x, $"{numOutcome}");

					endPosition = new Vector3(x, i * -1, startPosition.z);
					break;
				}
			}

			if (foundFreeCell)
			{
				// Instantiate a new Piece, disable the temporary
				GameObject g = Instantiate(finalColor) as GameObject;
				gameObjectTurn.GetComponent<Renderer>().enabled = false;
				finalColor.GetComponent<Renderer>().enabled = false;

				float distance = Vector3.Distance(startPosition, endPosition);

				float t = 0;
				while (t < 1)
				{
					t += Time.deltaTime * GC.dropTime * ((GC.numRows - distance) + 1);

					g.transform.position = Vector3.Lerp(startPosition, endPosition, t);
					yield return null;
				}

				g.transform.parent = fieldObject.transform;


				// remove the temporary gameobject
				DestroyImmediate(gameObjectTurn);

				// run coroutine to check if someone has won
				StartCoroutine(Won());

				// wait until winning check is done
				while (isCheckingForWinner)
					yield return null;

				isPlayersTurn = !isPlayersTurn;
				// turnSign.SetActive(isPlayersTurn);
				// tokenSelector.switchTurns(isPlayersTurn);
				DM.SwitchPlayer(isPlayersTurn);

				// DestroyImmediate(playerTurnObject);
			}

			isDropping = false;
			yield return 0;
		}

		/// <summary>
		/// Check for Winner
		/// </summary>
		IEnumerator Won()
		{
			isCheckingForWinner = true;

			Results winCode = Results.Lose;

			for (int x = 0; x < GC.numColumns; x++)
			{
				for (int y = 0; y < GC.numRows; y++)
				{
					//if somebody won, gameOver = true;
					int color = field[x, y];
					if (color != 0)
					{
						//check up
						if (y >= 3 && field[x, y - 1] == color && field[x, y - 2] == color && field[x, y - 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						//check down
						else if (y <= GC.numRows - 4 && field[x, y + 1] == color && field[x, y + 2] == color && field[x, y + 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						//check left
						else if (x >= 3 && field[x - 1, y] == color && field[x - 2, y] == color && field[x - 3, y] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						//check right
						else if (x <= GC.numColumns - 4 && field[x + 1, y] == color && field[x + 2, y] == color && field[x + 3, y] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						//check upper left diagonal
						else if (y >= 3 && x >= 3 && field[x - 1, y - 1] == color && field[x - 2, y - 2] == color && field[x - 3, y - 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						// check upper right diagonal
						else if (y >= 3 && x <= GC.numColumns - 4 && field[x + 1, y - 1] == color && field[x + 2, y - 2] == color && field[x + 3, y - 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						// check lower left diagonal
						else if (x >= 3 && y <= GC.numRows - 4 && field[x - 1, y + 1] == color && field[x - 2, y + 2] == color && field[x - 3, y + 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						// check lower right diagonal
						else if (x <= GC.numColumns - 4 && y <= GC.numRows - 4 && field[x + 1, y + 1] == color && field[x + 2, y + 2] == color && field[x + 3, y + 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						// check if it's a tie
						else if (!FieldContainsEmptyCell())
						{
							gameOver = true;
							winCode = Results.Draw;
						}
					}
					yield return null;
				}
				yield return null;
			}

			if (gameOver == true)
			{
				//Shivani Puli Data Collection -> store winner
				// if (playerWon)
				// 	mydata.winner = 1;
				// else 
				// 	mydata.winner = 2;
				mydata.winner = (int)winCode;
				saveData.Save(mydata);
				//Data Collection

				// star system
				if (!starUpdated)
				{
					starUpdated = true;
					int starsWon = starDisplay.getResults(winCode);

					if (GameManager.saveData.starSystem[LEVEL_NUMBER] <= starsWon) {
						GameManager.saveData.starSystem[LEVEL_NUMBER] = starsWon;
						GameManager.Save();
					}
				}

				// StarSystem
				ShowStarSystem();

				fieldObject.SetActive(false);
				// displayHolder.SetActive(false);
				// resultDisplay.gameObject.SetActive(true);
				// resultDisplay.GameOver(winCode);
				DM.GameOver(winCode);

				// Reward System
				if (GameManager.rewardSystem[LEVEL_NUMBER]) {
					Wrapper.Events.CollectAndDisplayReward?.Invoke(Wrapper.Game.QueueBits, 3);
				}
			}

			isCheckingForWinner = false;

			yield return 0;
		}

		void ShowStarSystem()
		{
			starDisplay.resetStars();
			starDisplay.setDisplay(GameManager.saveData.starSystem[LEVEL_NUMBER]);
		}

		/// <summary>
		/// check if the field contains an empty cell
		/// </summary>
		/// <returns><c>true</c>, if it contains empty cell, <c>false</c> otherwise.</returns>
		bool FieldContainsEmptyCell()
		{
			for (int x = 0; x < GC.numColumns; x++)
			{
				for (int y = 0; y < GC.numRows; y++)
				{
					if (field[x, y] == (int)Piece1.Empty)
						return true;
				}
			}
			return false;
		}
	}
}
