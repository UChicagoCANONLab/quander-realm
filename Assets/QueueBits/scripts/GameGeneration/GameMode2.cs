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
	public class GameMode2 : MonoBehaviour
	{
		private int LEVEL_NUMBER;
		private int probability;

		[Header("External Objects")]
		// public StarDisplay starDisplay;
		public GameController GC;

		[Header("CPU Pieces")]
		public TokenCounter tokenCounterCPU;
		public GameObject pieceCPU100;
		public GameObject pieceCPU75; 
		public GameObject pieceCPU50; 

		[Header("Player Pieces")]
		public TokenCounter tokenCounterPlayer;
		public GameObject piecePlayer100;
		public GameObject piecePlayer75;
		public GameObject piecePlayer50;

		// Dictionaries for token counts
		private Dictionary<int, int> CPUProbs = new Dictionary<int, int>();
		private Dictionary<int, int> playerProbs = new Dictionary<int, int>();

		// Prefilled board initialized in GameController
		private List<(Piece, int, int, int)> prefilledBoard = new List<(Piece piece, int col, int row, int prob)>();
		// Dictionary of each piece's probability and location
		private Dictionary<int, (int, (int, int))> probDict = new Dictionary<int, (int, (int, int))>();
		
		// temporary gameobject, holds the piece at mouse position until the mouse has clicked
		private GameObject gameObjectTurn;

		// The Game field. 0 = Empty, 1 = Player, 2 = CPU
		public int[,] field;
		public int[,] probField;
		private (int, int)[] dropOrder = new (int, int)[42];
		private GameObject[] pieces = new GameObject[42];
		
		// Counters for game management
		private int numSuperpositionPieces = 0;
		private int probCounter = 0;
		private int turnCounter = 14; // GameController counts the prefilledBoard pieces

		// Booleans for game management
		private bool isPlayersTurn = true;
		private bool isDropping = false;
		private bool isCheckingForWinner = false;
		private bool gameOver = false;
		private bool revealingProbs = false;


		// Use this for initialization
		public void Start()
		{
			GC.StartGame();
			
			// Sync with GameController
			LEVEL_NUMBER = GC.LEVEL_NUMBER;
			prefilledBoard = GC.prefilledBoard;
			
			// Set AI difficulty
			GC.cpuAI.difficulty = 2;

			// init Player token counter
			playerProbs = tokenCounterPlayer.getCounterDict(LEVEL_NUMBER);
			tokenCounterPlayer.initCounter(LEVEL_NUMBER);
			// init CPU token counter
			CPUProbs = tokenCounterCPU.getCounterDict(LEVEL_NUMBER);
			tokenCounterCPU.initCounter(LEVEL_NUMBER);

			CreateField();

			isPlayersTurn = false;
		}


		// Initializes Field with prefilled board tokenss
		public void CreateField()
		{
			GC.DM.SwitchPlayer(true);

			// create an empty field and instantiate the cells
			field = new int[GC.numColumns, GC.numRows];
			probField = new int[GC.numColumns, GC.numRows];

			// initialize field for pieces
			for (int x = 0; x < GC.numColumns; x++) {
				for (int y = 0; y < GC.numRows; y++) {
					field[x, y] = (int)Piece.Empty;
					probField[x, y] = -1;
				}
			}

			// initialize prefilled board
			for (int i = 0; i < prefilledBoard.Count; i++)
            {
				probCounter++;
				if (prefilledBoard[i].Item4 == 100) 
				{
					field[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = (int)prefilledBoard[i].Item1;
					if (prefilledBoard[i].Item1 == Piece.Player) {
						GameObject obj = Instantiate(piecePlayer100, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
					}
					else {
						GameObject obj = Instantiate(pieceCPU100, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
					}
				}
				else
				{
					field[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = (int)Piece.Unknown;
					GameObject obj;
					if (prefilledBoard[i].Item1 == Piece.Player)
					{
						probField[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = prefilledBoard[i].Item4;
						if (prefilledBoard[i].Item4 == 75)
                        {
							obj = Instantiate(piecePlayer75, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
						}
						else 
                        {
							obj = Instantiate(piecePlayer50, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
						}
					}
					else
					{
						probField[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = 100 - prefilledBoard[i].Item4;
						if (prefilledBoard[i].Item4 == 75)
						{
							obj = Instantiate(pieceCPU75, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
						}
						else
						{
							obj = Instantiate(pieceCPU50, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
						}
					}
					Color c = obj.GetComponent<MeshRenderer>().material.color;
					c.a = 0.5f;
					obj.GetComponent<MeshRenderer>().material.color = c;

					dropOrder[numSuperpositionPieces] = (prefilledBoard[i].Item2, prefilledBoard[i].Item3);
					pieces[numSuperpositionPieces] = obj;
					numSuperpositionPieces++;
				}
			}
		}


		// Update is called once per frame
		public void Update()
		{
			if (isCheckingForWinner || gameOver)
				return;

			if (revealingProbs) {
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
						GC.fieldObject.transform.position.y + 1, 0);

					// click the left mouse button to drop the piece into the selected column
					if (Input.GetMouseButtonDown(0) && !isDropping)
					{
						StartCoroutine(dropPiece(gameObjectTurn, probability));
					}
				}
			}
			else
			{
				if (gameObjectTurn == null)
				{
					(gameObjectTurn, probability) = SpawnPiece(-1); 
				}
				else
				{
					if (!isDropping)
					{
						// Task.Delay(2000);
						Thread.Sleep(1000);
						StartCoroutine(dropPiece(gameObjectTurn, probability));
					}
				}
			}
		}

		// New funtion to spawn piece when clicking buttons on TokenSelector
		public void tokenSelectedByButton(int prob) {
			(gameObjectTurn, probability) = SpawnPiece(prob);
		}

		
		// Spawns a piece at mouse position above the first row
		public (GameObject, int) SpawnPiece(int prob)
		{
			Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			GameObject pieceTemp = piecePlayer100;

			if (isPlayersTurn)
			{
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

				for (int i = 0; i < GC.numColumns; i++) {
					for (int j = 0; j < GC.numRows; j++) {
						if (field[i, j] != 0)
						{
							GC.cpuAI.colPointers[i] = j - 1;
							break;
						}
					}
				}

				if (GC.FieldContainsUnknownCell(field))
				{
					int column = GC.cpuAI.findBestMove(GC.cpuAI.colPointers);
					spawnPos = new Vector3(column, 0, 0);
				}
			}

			GameObject g = Instantiate(pieceTemp,
					new Vector3(
					Mathf.Clamp(spawnPos.x, 0, GC.numColumns - 1),
					GC.fieldObject.transform.position.y + 1, 0), // spawn it above the first row
					Quaternion.identity) as GameObject;

			return (g, prob);
		}


		// This method searches for a empty cell and lets the object fall down into this cell
		public IEnumerator dropPiece(GameObject gObject, int probability)
		{
			isDropping = true;
			Vector3 startPosition = gObject.transform.position;
			Vector3 endPosition = new Vector3();

			// round to a grid cell
			int x = Mathf.RoundToInt(startPosition.x);
			startPosition = new Vector3(x, startPosition.y, startPosition.z);

			// is there a free cell in the selected column?
			bool foundFreeCell = false;
			(int, int) tempLocation = (-1, -1);

			for (int i = GC.numRows - 1; i >= 0; i--)
			{
				if (field[x, i] == 0)
				{
					turnCounter++;
					int index = i * GC.numColumns + x;

					GC.myData.placement_order[index] = turnCounter;
					GC.myData.superposition[index] = probability;
					GC.cpuAI.superpositionArray = GC.myData.superposition;

					foundFreeCell = true;

					if (isPlayersTurn) {
						probField[x,i] = probability; // probability of being Player piece
						GC.cpuAI.playMove(x, "1");
						if (probability == 100) {
							GC.myData.outcome[index] = 1;
							field[x, i] = 1;
						} else {
							field[x, i] = 3;
						}
					} else {
						probField[x,i] = 100 - probability; // probability of being Player piece
						GC.cpuAI.playMove(x, "2");
						if (probability == 100) {
							GC.myData.outcome[index] = 2;
							field[x, i] = 2;
						} else {
							field[x, i] = 3;
						}
					}

					tempLocation = (x, i);
					if (probability != 100) {
						dropOrder[numSuperpositionPieces] = (x, i);
					}
					endPosition = new Vector3(x, i * -1, startPosition.z);
					break;
				}
			}

			if (foundFreeCell)
			{
				// Instantiate a new Piece, disable the temporary
				GameObject g = Instantiate(gObject) as GameObject;
				gameObjectTurn.GetComponent<Renderer>().enabled = false;

				if (probability != 100) {
					Color c = g.GetComponent<MeshRenderer>().material.color;
					c.a = 0.5f;
					g.GetComponent<MeshRenderer>().material.color = c; 
				}

				float distance = Vector3.Distance(startPosition, endPosition);

				float t = 0;
				while (t < 1)
				{
					t += Time.deltaTime * GC.dropTime * ((GC.numRows - distance) + 1);

					g.transform.position = Vector3.Lerp(startPosition, endPosition, t);
					yield return null;
				}

				g.transform.parent = GC.fieldObject.transform;

				if (isPlayersTurn) {
					probDict.Add(g.transform.GetInstanceID(), (probability, tempLocation));
				} else {
					probDict.Add(g.transform.GetInstanceID(), (100 - probability, tempLocation));
				}

				if (probability != 100) {
					pieces[numSuperpositionPieces] = g;
					numSuperpositionPieces++;
				}

				// remove the temporary gameobject
				DestroyImmediate(gameObjectTurn);

				probCounter++;

				// run coroutine to check if someone has won
				StartCoroutine(Won());

				// wait until winning check is done
				while (isCheckingForWinner)
					yield return null;

				if (probCounter == 42) {
					revealingProbs = true;
					StartCoroutine(revealProbabilities());
				} 

				isPlayersTurn = !isPlayersTurn;
				GC.DM.SwitchPlayer(isPlayersTurn);
			}
			isDropping = false;
			yield return 0;
		}


		public IEnumerator revealProbabilities()
		{
			int x, y;
			//GameObject piece;
			for (int i = 0; i < numSuperpositionPieces; i++)
			{
				yield return new WaitForSeconds(1);
				//piece = pieces[i];
				(x, y) = dropOrder[i];
				//Data Collection
				int index = y * GC.numColumns + x;
				GC.myData.reveal_order[index] = i + 1;

				GameObject finalColor = null;
				int probability = probField[x, y];
				int p = Random.Range(1, 101);
				if (p < probability)
				{
					if (pieces[i] != null)
					{
						Vector3 pos = pieces[i].transform.position;
						finalColor = Instantiate(
							piecePlayer100,
							new Vector3(pos.x, pos.y, 0),
							Quaternion.identity, GC.fieldObject.transform) as GameObject;
						DestroyImmediate(pieces[i]);

						//Data Collection
						GC.myData.outcome[index] = 1;
						field[x, y] = 1;
					}
				}
				else
				{
					if (pieces[i] != null)
					{
						Vector3 pos = pieces[i].transform.position;
						finalColor = Instantiate(
							pieceCPU100,
							new Vector3(pos.x, pos.y, 0),
							Quaternion.identity, GC.fieldObject.transform) as GameObject;
						DestroyImmediate(pieces[i]);
						
						//Data Collection
						GC.myData.outcome[index] = 2;
						field[x, y] = 2;
					}
				}
				isPlayersTurn = !isPlayersTurn;

				StartCoroutine(Won());

				while (isCheckingForWinner)
					yield return null;

				GC.DM.SwitchPlayer(isPlayersTurn);
				if (gameOver)
					break;

				yield return new WaitForSeconds(1);
			}
			yield return 0;
		}

		// Checks for winner
		public IEnumerator Won()
		{
			isCheckingForWinner = true;
			Results winCode = Results.Lose;
			// bool gameOver = false;

			for (int x = 0; x < GC.numColumns; x++)
			{
				for (int y = 0; y < GC.numRows; y++)
				{
					//if somebody won, gameOver = true;
					int color = field[x, y];
					if (color != 0 && color != 3)
					{
						//check up
						if (y >= 3 && field[x, y - 1] == color && field[x, y - 2] == color && field[x, y - 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
							revealingProbs = false;
						}

						//check down
						else if (y <= GC.numRows - 4 && field[x, y + 1] == color && field[x, y + 2] == color && field[x, y + 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
							revealingProbs = false;
						}

						//check left
						else if (x >= 3 && field[x - 1, y] == color && field[x - 2, y] == color && field[x - 3, y] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
							revealingProbs = false;
						}

						//check right
						else if (x <= GC.numColumns - 4 && field[x + 1, y] == color && field[x + 2, y] == color && field[x + 3, y] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
							revealingProbs = false;
						}

						//check upper left diagonal
						else if (y >= 3 && x >= 3 && field[x - 1, y - 1] == color && field[x - 2, y - 2] == color && field[x - 3, y - 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
							revealingProbs = false;
						}

						// check upper right diagonal
						else if (y >= 3 && x <= GC.numColumns - 4 && field[x + 1, y - 1] == color && field[x + 2, y - 2] == color && field[x + 3, y - 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
							revealingProbs = false;
						}

						// check lower left diagonal
						else if (x >= 3 && y <= GC.numRows - 4 && field[x - 1, y + 1] == color && field[x - 2, y + 2] == color && field[x - 3, y + 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
							revealingProbs = false;
						}

						// check lower right diagonal
						else if (x <= GC.numColumns - 4 && y <= GC.numRows - 4 && field[x + 1, y + 1] == color && field[x + 2, y + 2] == color && field[x + 3, y + 3] == color)
						{
							if (color == 1) { winCode = Results.Win; }
							gameOver = true;
							revealingProbs = false;
						}

						// check if it's a tie
						else if (!GC.FieldContainsUnknownCell(field))
						{
							gameOver = true;
							winCode = Results.Draw;
						}
					}
					yield return null;
				}
				yield return null;
			}

			if (gameOver == true) {
				GC.EndGame(winCode);
			}

			isCheckingForWinner = false;
			yield return 0;
		}
	}
}