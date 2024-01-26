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
		public GameObject pieceField;
		public GameObject finalColor;
		public GameObject gameObjectField;


		Dictionary<int, int> redProbs = new Dictionary<int, int>();
		Dictionary<int, int> blueProbs = new Dictionary<int, int>();

		[Header("Prefilled Boards")]
		public PrefilledBoards PB;
		public List<(Piece, int, int, int)> prefilledBoard = new List<(Piece piece, int col, int row, int prob)>();
		public string boardName;

		// Select Tokens
		GameObject choice50;
		GameObject choice75;
		GameObject choice100;
		int choice;

		// temporary gameobject, holds the piece at mouse position until the mouse has clicked
		GameObject gameObjectTurn;

		/// <summary>
		/// The Game field.
		/// 0 = Empty, 1 = Blue, 2 = Red
		/// </summary>
		int[,] field;

		bool isPlayersTurn = true;
		bool isLoading = true;
		bool isDropping = false;
		bool mouseButtonPressed = false;
		bool gameOver = false;
		bool isCheckingForWinner = false;
		bool starUpdated = false;
		bool SelectMenuGenerated = false;
		
		// For the AI 
		// string state = "000000000000000000000000000000000000000000";
		// int[] colPointers = { 5, 5, 5, 5, 5, 5, 5 };
		// HashSet<(int, int)> visited = new HashSet<(int, int)>();

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

			foreach ((Piece pi, int c, int r, int p) in prefilledBoard)
			{
				turn++;
				int index = r * numColumns + c;
				mydata.placement_order[index] = turn;
				
				mydata.reveal_order[index] = turn;
				mydata.superposition[index] = p;
				if (pi == Piece.Blue)//if Yellow
				{
					mydata.outcome[index] = 1;
					cpuAI.playMove(c, "1");
				}
				else
				{
					mydata.outcome[index] = 2;
					cpuAI.playMove(c, "2");
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

			/* if (isPlayersTurn)
			{
				playerTurnObject = Instantiate(pieceBlue, new Vector3(numColumns - 1.75f, -6.3f, -1), Quaternion.identity) as GameObject;
				playerTurnObject.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
			}
			else
			{
				playerTurnObject = Instantiate(pieceRed, new Vector3(numColumns - 1.75f, -6.3f, -1), Quaternion.identity) as GameObject;
				playerTurnObject.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
			} */
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

			// initialize the wood board image
			// GameObject g = Instantiate(pieceField, new Vector3(3, -2.8f, 1), Quaternion.identity) as GameObject;
			// g.transform.parent = gameObjectField.transform;

			// initialize field for pieces
			for (int x = 0; x < numColumns; x++)
			{
				for (int y = 0; y < numRows; y++)
				{
					field[x, y] = (int)Piece.Empty;
				}
			}

			// initialize prefilled board
			for (int i = 0; i < prefilledBoard.Count; i++)
            {
				field[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = (int)prefilledBoard[i].Item1;
				if (prefilledBoard[i].Item1 == Piece.Blue)
                {
					GameObject obj = Instantiate(pieceBlue, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, gameObjectField.transform) as GameObject;
				}
				else
                {
					GameObject obj = Instantiate(pieceRed, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, gameObjectField.transform) as GameObject;
				}
			}

			isLoading = false;
			gameOver = false;

			// center camera
			Camera.main.transform.position = new Vector3(
				(numColumns - 1) / 2.0f, -((numRows - 1) / 2.0f), Camera.main.transform.position.z);


			// Piece Count Displays
			tokenCounterBlue.setCounter(100, blueProbs[100]);
			tokenCounterBlue.setCounter(75, blueProbs[75]);
			tokenCounterBlue.disable50();

			tokenCounterRed.setCounter(100, redProbs[100]);
			tokenCounterRed.setCounter(75, redProbs[75]);
			tokenCounterRed.disable50();
		}


/* 
		int index(int r, int c)
		{
			int i = r * 7 + c;
			return i;
		}

		(int, int) reverse_index(int i)
		{
			int r = i / 7;
			int c = i % 7;
			return (r, c);
		}

		// DEBUGGING
		void printState()
		{
			string p_state = "";
			for (int r = 0; r < 6; r++)
			{
				int i = index(r, 0);
				p_state += state.Substring(i, 7) + "\n";
			}
			Debug.Log(p_state);
		}

		int evaluateState()
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

		int evaluatePosition(int r, int c, string color)
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

		List<int> getMoves(int[] cols)
		{
			List<int> possCols = new List<int>();
			for (int i = 0; i < cols.Length; i++)
			{
				if (cols[i] != -1)
					possCols.Add(i);
			}
			return possCols;
		}

		void playMove(int column, string color)
		{
			int r = colPointers[column];
			colPointers[column] -= 1;

			int i = index(r, column);

			if (i == 0)
				state = color + state.Substring(1); //prevents substring with length 0 error
			else
				state = state.Substring(0, i) + color + state.Substring(i + 1);
		}

		void reverseMove(int column)
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

		int checkForWinner(int r, int c)
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

		int findBestMove(int[] cols)
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

		int minimax(int depth, int maxDepth, bool isMaximizing)
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
		} */

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

				if (prob == 100)
				{
					pieceTemp = pieceBlue;
					tokenCounterBlue.setCounter(100, blueProbs[100]);
				}
				else if (prob == 75)
				{
					pieceTemp = piece75;
					tokenCounterBlue.setCounter(75, blueProbs[75]);
				}
				else if (prob == 50)
				{
					pieceTemp = piece50;
					tokenCounterBlue.setCounter(50, blueProbs[50]);
				}

				if (blueProbs[prob] == 0)
				{
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

				if (prob == 100)
				{
					pieceTemp = pieceRed;
					tokenCounterRed.setCounter(100, redProbs[100]);
				}
				else if (prob == 75)
				{
					pieceTemp = piece25red_turn;
					tokenCounterRed.setCounter(75, redProbs[75]);
				}
				else if (prob == 50)
				{
					pieceTemp = piece50red_turn;
					tokenCounterRed.setCounter(50, redProbs[50]);
				}

				if (redProbs[prob] == 0)
				{
					redProbs.Remove(prob);
				}


				List<int> moves = GetPossibleMoves();

				/*for (int i = 0; i < numColumns; i++)
				{
					for (int j = 0; j < numRows; j++)
					{
						if (field[i, j] != 0)
						{
							colPointers[i] = j - 1;
							break;
						}
					}
				}*/

				if (moves.Count > 0)
				{
					int column = cpuAI.findBestMove(cpuAI.colPointers);
					spawnPos = new Vector3(column, 0, 0);
				}
			}

			GameObject g = Instantiate(pieceTemp,
					new Vector3(
					Mathf.Clamp(spawnPos.x, 0, numColumns - 1),
					gameObjectField.transform.position.y + 1, 0), // spawn it above the first row
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
				gameObjectField.SetActive(false);
				displayHolder.SetActive(false);
				resultDisplay.gameObject.SetActive(true);

				return;
			}

			if (isPlayersTurn)
			{
				if (gameObjectTurn == null)
				{
					if (!SelectMenuGenerated)
					{
						if (blueProbs.ContainsKey(100) && blueProbs[100] > 0)
                        {
							choice100 = Instantiate(pieceBlue, new Vector3(-1f, 1, -1), Quaternion.identity) as GameObject;
						}
						if (blueProbs.ContainsKey(75) && blueProbs[75] > 0)
						{
							choice75 = Instantiate(piece75, new Vector3(3, 1, -1), Quaternion.identity) as GameObject;
						}
						if (blueProbs.ContainsKey(50) && blueProbs[50] > 0)
						{
							choice50 = Instantiate(piece50, new Vector3(6.6f, 1, -1), Quaternion.identity) as GameObject;
						}
						SelectMenuGenerated = true;
					}

					if (Input.GetMouseButtonDown(0))
                    {
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						RaycastHit hit;

						if (Physics.Raycast(ray, out hit))
						{
							GameObject piece = hit.transform.gameObject;
							int clickedObjectID = piece.GetInstanceID();
							if (blueProbs.ContainsKey(100) && blueProbs[100] > 0 && clickedObjectID == choice100.GetInstanceID())
                            {
								choice = 100;
								DestroyImmediate(choice100);
								if (blueProbs.ContainsKey(75) && blueProbs[75] > 0)
								{
									DestroyImmediate(choice75);
								}
								if (blueProbs.ContainsKey(50) && blueProbs[50] > 0)
								{
									DestroyImmediate(choice50);
								}
								(gameObjectTurn, probability) = SpawnPiece(choice);
								SelectMenuGenerated = false;
							}
							if (blueProbs.ContainsKey(75) && blueProbs[75] > 0 && clickedObjectID == choice75.GetInstanceID())
							{
								choice = 75;
								if (blueProbs.ContainsKey(100) && blueProbs[100] > 0)
								{
									DestroyImmediate(choice100);
								}
								DestroyImmediate(choice75);
								if (blueProbs.ContainsKey(50) && blueProbs[50] > 0)
								{
									DestroyImmediate(choice50);
								}
								(gameObjectTurn, probability) = SpawnPiece(choice);
								SelectMenuGenerated = false;
							}
							if (blueProbs.ContainsKey(50) && blueProbs[50] > 0 && clickedObjectID == choice50.GetInstanceID())
							{
								choice = 50;
								if (blueProbs.ContainsKey(100) && blueProbs[100] > 0)
								{
									DestroyImmediate(choice100);
								}
								if (blueProbs.ContainsKey(75) && blueProbs[75] > 0)
								{
									DestroyImmediate(choice75);
								}
								DestroyImmediate(choice50);
								(gameObjectTurn, probability) = SpawnPiece(choice);
								SelectMenuGenerated = false;
							}
						}
					}
				}
				else
				{
					// update the objects position
					Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					gameObjectTurn.transform.position = new Vector3(
						Mathf.Clamp(pos.x, 0, numColumns - 1),
						gameObjectField.transform.position.y + 1, 0);

					// click the left mouse button to drop the piece into the selected column
					if (Input.GetMouseButtonDown(0) && !mouseButtonPressed && !isDropping)
					{
						mouseButtonPressed = true;

						StartCoroutine(dropPiece(gameObjectTurn, probability));
					}
					else
					{
						mouseButtonPressed = false;
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
						//Task.Delay(2000);
						Thread.Sleep(1000);
						// Debug.Log(gameObjectTurn.transform.position.ToString());
						StartCoroutine(dropPiece(gameObjectTurn, probability));
					}
				}
			}
		}

		/// <summary>
		/// This method searches for a empty cell and lets 
		/// the object fall down into this cell
		/// </summary>
		/// <param name="gObject">Game Object.</param>
		IEnumerator dropPiece(GameObject gObject, int probability)
		{
			isDropping = true;
			string s = "";

			for (int j = 0; j< numRows; j++)
			{
				/*for (int p = 0; p < numColumns; p++)
			{
				
					s += (int)field[p, j];
				}*/
				s += cpuAI.state.Substring(j * numColumns, numColumns);
				s += "\n";
			}
			//Debug.Log(s);

			Vector3 startPosition = gObject.transform.position;
			Vector3 endPosition = new Vector3();

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
					if (isPlayersTurn)
					{
						int p = Random.Range(1, 101);
						if (p < probability)
						{
							Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
							finalColor = Instantiate(
								pieceBlue, // is players turn = spawn blue, else spawn red
								new Vector3(Mathf.Clamp(pos.x, 0, numColumns - 1),
								gameObjectField.transform.position.y + 1, 0), // spawn it above the first row
								Quaternion.identity, gameObjectField.transform) as GameObject;
							field[x, i] = (int)Piece.Blue;
							//Shivani Puli data collection
							int r = cpuAI.colPointers[x];
							int index = r * numColumns + x;
							turn++;
							mydata.placement_order[index] = turn;
							mydata.superposition[index] = probability;
							mydata.reveal_order[index] = turn;
							mydata.outcome[index] = 1;
							// data collection
							cpuAI.playMove(x, "1");
						}
						else
						{
							Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
							finalColor = Instantiate(
								pieceRed, // is players turn = spawn blue, else spawn red
								new Vector3(Mathf.Clamp(pos.x, 0, numColumns - 1),
								gameObjectField.transform.position.y + 1, 0), // spawn it above the first row
								Quaternion.identity, gameObjectField.transform) as GameObject;
							field[x, i] = (int)Piece.Red;
							//Shivani Puli data collection
							int r = cpuAI.colPointers[x];
							int index = r * numColumns + x;
							turn++;
							mydata.placement_order[index] = turn;
							mydata.superposition[index] = probability;
							mydata.reveal_order[index] = turn;
							mydata.outcome[index] = 2;
							// data collection
							cpuAI.playMove(x, "2");
						}
					}
					else // CPU's turn
					{
						int p = Random.Range(1, 101);
						if (p < probability)
						{
							Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
							finalColor = Instantiate(
								pieceRed, // is players turn = spawn blue, else spawn red
								new Vector3(Mathf.Clamp(pos.x, 0, numColumns - 1),
								gameObjectField.transform.position.y + 1, 0), // spawn it above the first row
								Quaternion.identity, gameObjectField.transform) as GameObject;
							field[x, i] = (int)Piece.Red;
							//Shivani Puli data collection
							int r = cpuAI.colPointers[x];
							int index = r * numColumns + x;
							turn++;
							mydata.placement_order[index] = turn;
							mydata.superposition[index] = probability;
							mydata.reveal_order[index] = turn;
							mydata.outcome[index] = 2;
							// data collection
							cpuAI.playMove(x, "2");
						}
						else
						{
							Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
							finalColor = Instantiate(
								pieceBlue, // is players turn = spawn blue, else spawn red
								new Vector3(Mathf.Clamp(pos.x, 0, numColumns - 1),
								gameObjectField.transform.position.y + 1, 0), // spawn it above the first row
								Quaternion.identity, gameObjectField.transform) as GameObject;
							field[x, i] = (int)Piece.Blue;
							//Shivani Puli data collection
							int r = cpuAI.colPointers[x];
							int index = r * numColumns + x;
							turn++;
							mydata.placement_order[index] = turn;
							mydata.superposition[index] = probability;
							mydata.reveal_order[index] = turn;
							mydata.outcome[index] = 1;
							// data collection
							cpuAI.playMove(x, "1");
						}
					}

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

				g.transform.parent = gameObjectField.transform;


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

				/* if (isPlayersTurn)
				{
					playerTurnObject = Instantiate(pieceBlue, new Vector3(numColumns - 1.75f, -6.3f, -1), Quaternion.identity, gameObjectField.transform) as GameObject;
					playerTurnObject.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
				}
				else
				{
					playerTurnObject = Instantiate(pieceRed, new Vector3(numColumns - 2.25f, -6.3f, -1), Quaternion.identity, gameObjectField.transform) as GameObject;
					playerTurnObject.transform.localScale -= new Vector3(0.5f, 0.5f, 0);

				} */
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
							if (color == 1)
								// blueWon = true;
								winCode = Results.Win;
							gameOver = true;
						}

						//check down
						if (y <= numRows - 4 && field[x, y + 1] == color && field[x, y + 2] == color && field[x, y + 3] == color)
						{
							if (color == 1)
								// blueWon = true;
								winCode = Results.Win;
							gameOver = true;
						}

						//check left
						if (x >= 3 && field[x - 1, y] == color && field[x - 2, y] == color && field[x - 3, y] == color)
						{
							if (color == 1)
								// blueWon = true;
								winCode = Results.Win;
							gameOver = true;
						}

						//check right
						if (x <= numColumns - 4 && field[x + 1, y] == color && field[x + 2, y] == color && field[x + 3, y] == color)
						{
							if (color == 1)
								// blueWon = true;
								winCode = Results.Win;
							gameOver = true;
						}

						//check upper left diagonal
						if (y >= 3 && x >= 3 && field[x - 1, y - 1] == color && field[x - 2, y - 2] == color && field[x - 3, y - 3] == color)
						{
							if (color == 1)
								// blueWon = true;
								winCode = Results.Win;
							gameOver = true;
						}

						// check upper right diagonal
						if (y >= 3 && x <= numColumns - 4 && field[x + 1, y - 1] == color && field[x + 2, y - 2] == color && field[x + 3, y - 3] == color)
						{
							if (color == 1)
								// blueWon = true;
								winCode = Results.Win;
							gameOver = true;
						}

						// check lower left diagonal
						if (x >= 3 && y <= numRows - 4 && field[x - 1, y + 1] == color && field[x - 2, y + 2] == color && field[x - 3, y + 3] == color)
						{
							if (color == 1)
								// blueWon = true;
								winCode = Results.Win;
							gameOver = true;
						}

						// check lower right diagonal
						if (x <= numColumns - 4 && y <= numRows - 4 && field[x + 1, y + 1] == color && field[x + 2, y + 2] == color && field[x + 3, y + 3] == color)
						{
							if (color == 1)
								// blueWon = true;
								winCode = Results.Win;
							gameOver = true;
						}

						// check if it's a tie
						if (!FieldContainsEmptyCell())
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

					// if (winCode == 1) { // If player wins => 3 stars
					// 	starsWon = 3;
					// } else if (winCode == 2) { // If player loses => 1 star
					// 	starsWon = 1;
					// } else { // If there's a tie => 2 stars
					// 	starsWon = 2;
					// }

					if (GameManager.saveData.starSystem[LEVEL_NUMBER] <= starsWon) {
						GameManager.saveData.starSystem[LEVEL_NUMBER] = starsWon;
						GameManager.Save();
					}

					// if (GameManager.saveData.starSystem[LEVEL_NUMBER] + 1 <= 3)
					// {
					// 	GameManager.saveData.starSystem[LEVEL_NUMBER] = GameManager.saveData.starSystem[LEVEL_NUMBER] + 1;
					// 	GameManager.Save();
					// }
				}

				// StarSystem
				// DestroyImmediate(Star1);
				// DestroyImmediate(Star2);
				// DestroyImmediate(Star3);
				ShowStarSystem();

				// if (winCode == 1) {
				// 	resultWon.SetActive(true);
				// } else if (winCode == 2) {
				// 	resultLose.SetActive(true);
				// } else {
				// 	resultDraw.SetActive(true);
				// }

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
			// Star System
			/* if (GameManager.saveData.starSystem[LEVEL_NUMBER] == 0)
			{
				Star1 = Instantiate(starEmpty, new Vector3(-3.3f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
				Star2 = Instantiate(starEmpty, new Vector3(-2.4f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
				Star3 = Instantiate(starEmpty, new Vector3(-1.5f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
			}
			else if (GameManager.saveData.starSystem[LEVEL_NUMBER] == 1)
			{
				Star1 = Instantiate(starFilled, new Vector3(-3.3f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
				Star2 = Instantiate(starEmpty, new Vector3(-2.4f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
				Star3 = Instantiate(starEmpty, new Vector3(-1.5f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
			}
			else if (GameManager.saveData.starSystem[LEVEL_NUMBER] == 2)
			{
				Star1 = Instantiate(starFilled, new Vector3(-3.3f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
				Star2 = Instantiate(starFilled, new Vector3(-2.4f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
				Star3 = Instantiate(starEmpty, new Vector3(-1.5f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
			}
			else if (GameManager.saveData.starSystem[LEVEL_NUMBER] == 3)
			{
				Star1 = Instantiate(starFilled, new Vector3(-3.3f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
				Star2 = Instantiate(starFilled, new Vector3(-2.4f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
				Star3 = Instantiate(starFilled, new Vector3(-1.5f, -6.9f, 1), Quaternion.identity, starHolder.transform) as GameObject;
			} */
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
