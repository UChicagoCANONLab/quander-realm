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
	public class Level3 : MonoBehaviour
	{
		public int LEVEL_NUMBER = 3;

		[Range(3, 8)]
		private int numRows = 6;
		[Range(3, 8)]
		private int numColumns = 7;
		[Tooltip("How many pieces have to be connected to win.")]
		private int numPiecesToWin = 4;
		[Tooltip("Allow diagonally connected Pieces?")]
		private bool allowDiagonally = true;

		private float dropTime = 4f;

		public int probability;

		public CPUBrain cpuAI;
		
		[Header("Display Objects")]
		public GameObject displayHolder;
		public GameObject turnSign;
		public StarDisplay starDisplay;
		public TokenCounter tokenCounterBlue;
		public TokenCounter tokenCounterRed;
		public GameOverScreen resultDisplay;
		public TokenSelector tokenSelector;

		// Gameobjects 
		[Header("Red Pieces")]
		public GameObject pieceRed;
		public GameObject piece25red_turn; //75%
		public GameObject piece50red_turn; //50%

		[Header("Blue Pieces")]
		public GameObject pieceBlue;
		public GameObject piece50;
		public GameObject piece75;

		[Header("GameObjects")]
		public GameObject pieceTemp;
		public GameObject finalColor;
		public GameObject fieldObject;

		Dictionary<int, int> redProbs = new Dictionary<int, int>();
		Dictionary<int, int> blueProbs = new Dictionary<int, int>();

		[Header("Prefilled Boards")]
		public PrefilledBoards PB;
		public List<(Piece, int, int, int)> prefilledBoard = new List<(Piece piece, int col, int row, int prob)>();
		public string boardName;

		
		int choice;

		// temporary gameobject, holds the piece at mouse position until the mouse has clicked
		GameObject gameObjectTurn;

		/// <summary>
		/// The Game field.
		/// 0 = Empty, 1 = Blue, 2 = Red
		/// </summary>
		int[,] field;

		// FROM LEVEL 6
		/* int probCounter = 0;
		int numSuperpositionPieces = 0;
		bool revealingProbs = false;
		bool currentlyRevealing = false; //Shivani: previously choosingreveal
		bool revealAuto = false;
		bool revealManual = false; */

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
			mydata.placement_order = new int[numColumns * numRows];
			mydata.superposition = new int[numColumns * numRows];
			mydata.reveal_order = new int[numColumns * numRows];
			mydata.outcome = new int[numColumns * numRows];
			for (int i = 0; i < numColumns * numRows; i++)
			{
				mydata.placement_order[i] = 0;
				mydata.superposition[i] = 0;
				mydata.reveal_order[i] = 0;
				mydata.outcome[i] = 0;
			}

			foreach ((Piece pi, int c, int r, int pr) in prefilledBoard)
			{
				turn++;
				int index = r * numColumns + c;
				mydata.placement_order[index] = turn;
				
				mydata.reveal_order[index] = turn;
				mydata.superposition[index] = pr;
				if (pi == Piece.Blue)//if Yellow
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

			redProbs.Add(75, 7);
			redProbs.Add(100, 7);

			blueProbs.Add(75, 7);
			blueProbs.Add(100, 7);

			int max = Mathf.Max(numRows, numColumns);

			if (numPiecesToWin > max)
				numPiecesToWin = max;

			CreateField();

			isPlayersTurn = false;
			turnSign.SetActive(isPlayersTurn);
			tokenSelector.switchTurns(isPlayersTurn);
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
			turnSign.SetActive(true);
			tokenSelector.switchTurns(true);

			isLoading = true;

			// create an empty field and instantiate the cells
			field = new int[numColumns, numRows];

			// initialize field for pieces
			for (int x = 0; x < numColumns; x++) {
				for (int y = 0; y < numRows; y++) {
					field[x, y] = (int)Piece.Empty;
				}
			}

			// initialize prefilled board
			for (int i = 0; i < prefilledBoard.Count; i++)
            {
				field[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = (int)prefilledBoard[i].Item1;
				if (prefilledBoard[i].Item1 == Piece.Blue) {
					GameObject obj = Instantiate(pieceBlue, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, fieldObject.transform) as GameObject;
				}
				else {
					GameObject obj = Instantiate(pieceRed, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, fieldObject.transform) as GameObject;
				}

				// FROM LEVEL 6
				/* else // if (prefilledBoard[i].Item4 != 100)
                {
					field[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = (int)Piece.Unknown;
					GameObject obj;
					if (prefilledBoard[i].Item1 == Piece.Blue)
					{
						probField[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = prefilledBoard[i].Item4;
						if (prefilledBoard[i].Item4 == 75) {
							obj = Instantiate(piece75, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity) as GameObject;
						}
						else {
							obj = Instantiate(piece50, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity) as GameObject;
						}
					}
					else
					{
						probField[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = 100 - prefilledBoard[i].Item4;
						if (prefilledBoard[i].Item4 == 75) {
							obj = Instantiate(piece25red_turn, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity) as GameObject;
						}
						else {
							obj = Instantiate(piece50red_turn, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity) as GameObject;
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
			// tokenCounterBlue.setCounter(100, blueProbs[100]);
			// tokenCounterBlue.setCounter(75, blueProbs[75]);
			// tokenCounterBlue.disable(50);
			tokenCounterBlue.initCounter(LEVEL_NUMBER);

			// tokenCounterRed.setCounter(100, redProbs[100]);
			// tokenCounterRed.setCounter(75, redProbs[75]);
			// tokenCounterRed.disable(50);
			tokenCounterRed.initCounter(LEVEL_NUMBER);
		}

		/// <summary>
		/// Gets all the possible moves.
		/// </summary>
		/// <returns>The possible moves.</returns>
		public List<int> GetPossibleMoves()
		{
			List<int> possibleMoves = new List<int>();
			for (int x = 0; x < numColumns; x++)
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

			if (isPlayersTurn)
			{
				prob = choice;
				int freq = blueProbs[prob];

				// delete probability from player's list
				blueProbs[prob] -= 1;

				if (prob == 100) {
					pieceTemp = pieceBlue;
					tokenCounterBlue.setCounter(100, blueProbs[100]);
				}
				else if (prob == 75) {
					pieceTemp = piece75;
					tokenCounterBlue.setCounter(75, blueProbs[75]);
				}
				else if (prob == 50) {
					pieceTemp = piece50;
					tokenCounterBlue.setCounter(50, blueProbs[50]);
				}

				if (blueProbs[prob] == 0) {
					blueProbs.Remove(prob);
				}
			}

			else
			{
				int ind = Random.Range(0, redProbs.Keys.Count);
				List<int> keyList = new List<int>(redProbs.Keys);
				prob = keyList[ind];
				int freq = redProbs[prob];

				// delete probability from player's list
				redProbs[prob] -= 1;

				if (prob == 100) {
					pieceTemp = pieceRed;
					tokenCounterRed.setCounter(100, redProbs[100]);
				}
				else if (prob == 75) {
					pieceTemp = piece25red_turn;
					tokenCounterRed.setCounter(75, redProbs[75]); }
				else if (prob == 50) {
					pieceTemp = piece50red_turn;
					tokenCounterRed.setCounter(50, redProbs[50]);
				}

				if (redProbs[prob] == 0) {
					redProbs.Remove(prob);
				}

			// IN LEVEL 6 the ELSE ends here

				List<int> moves = GetPossibleMoves();

				// FROM LEVEL 4, 5, 6
				/* for (int i = 0; i < numColumns; i++) {
					for (int j = 0; j < numRows; j++) {
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
					Mathf.Clamp(spawnPos.x, 0, numColumns - 1),
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
						Mathf.Clamp(pos.x, 0, numColumns - 1),
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
			for (int j = 0; j< numRows; j++)
			{
				for (int p = 0; p < numColumns; p++)
				{
					s += (int)field[p, j];
				}
				s += cpuAI.state.Substring(j * numColumns, numColumns);
				s += "\n";
			}
			//Debug.Log(s); */

			// round to a grid cell
			int x = Mathf.RoundToInt(startPosition.x);
			startPosition = new Vector3(x, startPosition.y, startPosition.z);

			// is there a free cell in the selected column?
			bool foundFreeCell = false;
			(int, int) tempLocation = (-1, -1);

			for (int i = numRows - 1; i >= 0; i--)
			{
				if (field[x, i] == 0)
				{
					foundFreeCell = true;
					GameObject pieceColorObject = pieceBlue;
					int numOutcome = 1;

					int p = Random.Range(1, 101);
					if ((p < probability && isPlayersTurn) || (p >= probability && !isPlayersTurn)) {
						pieceColorObject = pieceBlue;
						numOutcome = (int)Piece.Blue;
					} else if ((p >= probability && isPlayersTurn) || (p < probability && !isPlayersTurn)){
						pieceColorObject = pieceRed;
						numOutcome = (int)Piece.Red;
					}

					Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					finalColor = Instantiate(
						pieceColorObject, // is players turn = spawn blue, else spawn red
						new Vector3(Mathf.Clamp(pos.x, 0, numColumns - 1),
						fieldObject.transform.position.y + 1, 0), // spawn it above the first row
						Quaternion.identity, fieldObject.transform) as GameObject;
					field[x, i] = numOutcome;
					//Shivani Puli data collection
					int r = cpuAI.colPointers[x];
					int index = r * numColumns + x;
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
					t += Time.deltaTime * dropTime * ((numRows - distance) + 1);

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
				turnSign.SetActive(isPlayersTurn);
				tokenSelector.switchTurns(isPlayersTurn);

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

			for (int x = 0; x < numColumns; x++)
			{
				for (int y = 0; y < numRows; y++)
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
						else if (y <= numRows - 4 && field[x, y + 1] == color && field[x, y + 2] == color && field[x, y + 3] == color)
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
						else if (x <= numColumns - 4 && field[x + 1, y] == color && field[x + 2, y] == color && field[x + 3, y] == color)
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
						else if (y >= 3 && x <= numColumns - 4 && field[x + 1, y - 1] == color && field[x + 2, y - 2] == color && field[x + 3, y - 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						// check lower left diagonal
						else if (x >= 3 && y <= numRows - 4 && field[x - 1, y + 1] == color && field[x - 2, y + 2] == color && field[x - 3, y + 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
						}

						// check lower right diagonal
						else if (x <= numColumns - 4 && y <= numRows - 4 && field[x + 1, y + 1] == color && field[x + 2, y + 2] == color && field[x + 3, y + 3] == color)
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
				// if (blueWon)
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
				displayHolder.SetActive(false);
				resultDisplay.gameObject.SetActive(true);
				
				resultDisplay.GameOver(winCode);

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
			for (int x = 0; x < numColumns; x++)
			{
				for (int y = 0; y < numRows; y++)
				{
					if (field[x, y] == (int)Piece.Empty)
						return true;
				}
			}
			return false;
		}
	}
}
