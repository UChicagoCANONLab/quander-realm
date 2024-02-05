using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using QueueBits;

namespace QueueBits
{
	public class GameMode3 : MonoBehaviour
	{
		private int LEVEL_NUMBER;
		private int probability;

		[Header("External Objects")]
		// public StarDisplay starDisplay;
		public DisplayManager DM;
		public GameController GC;
		public CPUBrain cpuAI;
		private Data mydata; // = new Data();

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


		private Dictionary<int, int> CPUProbs = new Dictionary<int, int>();
		private Dictionary<int, int> playerProbs = new Dictionary<int, int>();

		private List<(Piece, int, int, int)> prefilledBoard = new List<(Piece piece, int col, int row, int prob)>();

		private Dictionary<int, (int, (int, int), (float, float), GameObject)> probDict = new Dictionary<int, (int, (int, int), (float, float), GameObject)>();

		
		// temporary gameobject, holds the piece at mouse position until the mouse has clicked
		private GameObject gameObjectTurn;

		/// <summary>
		/// The Game field. 0 = Empty, 1 = Player, 2 = CPU
		/// </summary>
		public int[,] field;
		public int[,] probField;
		(int, int)[] dropOrder = new (int, int)[42];
		GameObject[] pieces = new GameObject[42];

		int numSuperpositionPieces = 0;
		int probCounter = 0;
		bool revealingProbs = false;
		int revealturn = 0;
		int turn = 0;

		bool isPlayersTurn = true;
		private bool isDropping = false;
		private bool isCheckingForWinner = false;
		private bool gameOver = false;
		private GameObject finalColor = null;		


		// Use this for initialization
		public void Start()
		{
			GC.StartGame();
			
			// Sync with GameController
			LEVEL_NUMBER = GC.LEVEL_NUMBER;
			mydata = GC.myData;
			cpuAI = GC.cpuAI;
			prefilledBoard = GC.prefilledBoard;
			
			// Setting CPU difficulty
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


		// Initializes Field
		public void CreateField()
		{
			DM.SwitchPlayer(true);

			// create an empty field and instantiate the cells
			field = new int[GC.numColumns, GC.numRows];
			probField = new int[GC.numColumns, GC.numRows];

			// initialize field for pieces
			for (int x = 0; x < GC.numColumns; x++) {
				for (int y = 0; y < GC.numRows; y++) {
					field[x, y] = (int)Piece1.Empty;
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
					// if (prefilledBoard[i].Item1 == Piece1.Player) {
					if ((int)prefilledBoard[i].Item1 == (int)Piece1.Player) {
						GameObject obj = Instantiate(piecePlayer100, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
					}
					else {
						GameObject obj = Instantiate(pieceCPU100, new Vector3(prefilledBoard[i].Item2, -prefilledBoard[i].Item3, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
					}
				}
				else
				{
					field[prefilledBoard[i].Item2, prefilledBoard[i].Item3] = (int)Piece1.Unknown;
					GameObject obj;
					if ((int)prefilledBoard[i].Item1 == (int)Piece1.Player)
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
					(int, int) tempLocation = (prefilledBoard[i].Item2, prefilledBoard[i].Item3);
					(float, float) piecePos = (obj.transform.position.x, obj.transform.position.y);
					probDict.Add(obj.transform.GetInstanceID(), (probability, tempLocation, piecePos, obj));
					
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
				StartCoroutine(revealProbabilitiesThroughClick());
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

				// FROM LEVEL 4, 5, 6
				for (int i = 0; i < GC.numColumns; i++) {
					for (int j = 0; j < GC.numRows; j++) {
						if (field[i, j] != 0)
						{
							cpuAI.colPointers[i] = j - 1;
							break;
						}
					}
				}

				if (GC.FieldContainsUnknownCell(field))
				{
					int column = cpuAI.findBestMove(cpuAI.colPointers);
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
					turn++;
					int index = i * GC.numColumns + x;

					mydata.placement_order[index] = turn;
					mydata.superposition[index] = probability;
					cpuAI.superpositionArray = mydata.superposition;

					foundFreeCell = true;
					// GameObject pieceColorObject = piecePlayer100;
					// int numOutcome = 1;

					/* int p = Random.Range(1, 101);
					if ((p < probability && isPlayersTurn) || (p >= probability && !isPlayersTurn)) {
						pieceColorObject = piecePlayer100;
						numOutcome = (int)Piece1.Player;
					} else if ((p >= probability && isPlayersTurn) || (p < probability && !isPlayersTurn)){
						pieceColorObject = pieceCPU100;
						numOutcome = (int)Piece1.CPU;
					} */

					if (isPlayersTurn) {
						probField[x,i] = probability; // probability of being Player piece
						cpuAI.playMove(x, "1");
						if (probability == 100) {
							mydata.outcome[index] = 1;
							field[x, i] = 1;
						} else {
							field[x, i] = 3;
						}
					} else {
						probField[x,i] = 100 - probability; // probability of being Player piece
						cpuAI.playMove(x, "2");
						if (probability == 100) {
							mydata.outcome[index] = 2;
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

					// Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					// finalColor = Instantiate(
					// 	pieceColorObject, // isPlayersTurn = spawn player, else spawn CPU
					// 	new Vector3(Mathf.Clamp(pos.x, 0, GC.numColumns - 1),
					// 	GC.fieldObject.transform.position.y + 1, 0), // spawn it above the first row
					// 	Quaternion.identity, GC.fieldObject.transform) as GameObject;
					// field[x, i] = numOutcome;
					// //Shivani Puli data collection
					// int r = cpuAI.colPointers[x];
					
					// // Update myData here
					// mydata.reveal_order[index] = turn;
					// mydata.outcome[index] = numOutcome;

					// cpuAI.superpositionArray = mydata.superposition;
					// cpuAI.playMove(x, $"{numOutcome}");

					// endPosition = new Vector3(x, i * -1, startPosition.z);
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
				
				if (probability != 100) {
					(float, float) piecePos = (g.transform.position.x, g.transform.position.y);
					if (isPlayersTurn) {
						probDict.Add(g.transform.GetInstanceID(), (probability, tempLocation, piecePos, g));
					} else {
						probDict.Add(g.transform.GetInstanceID(), (100 - probability, tempLocation, piecePos, g));
					}
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
					StartCoroutine(revealProbabilitiesThroughClick());
				} 

				isPlayersTurn = !isPlayersTurn;
				DM.SwitchPlayer(isPlayersTurn);
			}
			isDropping = false;
			yield return 0;
		}


		public IEnumerator revealProbabilitiesThroughClick()
		{
			if (isPlayersTurn)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;

					if (Physics.Raycast(ray, out hit))
					{
						GameObject piece = hit.transform.gameObject;
						int clickedObjectID = piece.GetInstanceID();

						if (probDict.ContainsKey(clickedObjectID - 2))
						{
							(int probability, (int coord_x, int coord_y), (float x, float y), GameObject token) = probDict[clickedObjectID - 2];
							int p = Random.Range(1, 101);
							if (p < probability)
							{
								Vector3 pos = piece.transform.position;
								finalColor = Instantiate(
									piecePlayer100,
									new Vector3(pos.x, pos.y, 0),
									Quaternion.identity, GC.fieldObject.transform) as GameObject;
								DestroyImmediate(piece);
								//Data Collection
								int index = coord_y * GC.numColumns + coord_x;
								revealturn++;
								mydata.reveal_order[index] = revealturn;
								mydata.outcome[index] = 1;
								field[coord_x, coord_y] = 1;
							}
							else
							{
								Vector3 pos = piece.transform.position;
								finalColor = Instantiate(
									pieceCPU100,
									new Vector3(pos.x, pos.y, 0),
									Quaternion.identity, GC.fieldObject.transform) as GameObject;
								DestroyImmediate(piece);
								//Data Collection
								int index = coord_y * GC.numColumns + coord_x;
								revealturn++;
								mydata.reveal_order[index] = revealturn;
								mydata.outcome[index] = 2;
								field[coord_x, coord_y] = 2;
							}
							isPlayersTurn = !isPlayersTurn;
							probDict.Remove(clickedObjectID - 2);
						}
						StartCoroutine(Won());
					}
				}
			}
			else
			{
				Thread.Sleep(1000);
				int chosenObjectID = probDict.ElementAt(Random.Range(0, probDict.Count)).Key;
				(int probability, (int coord_x, int coord_y), (float x, float y), GameObject token) = probDict[chosenObjectID];
				
				int p = Random.Range(1, 101);
				if (p < probability)
				{
					finalColor = Instantiate(piecePlayer100, new Vector3(x, y, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
					DestroyImmediate(token);
					//Data Collection
					int index = coord_y * GC.numColumns + coord_x;
					revealturn++;
					mydata.reveal_order[index] = revealturn;
					mydata.outcome[index] = 1;
					field[coord_x, coord_y] = 1;
				}
				else
				{
					finalColor = Instantiate(pieceCPU100, new Vector3(x, y, 0), Quaternion.identity, GC.fieldObject.transform) as GameObject;
					DestroyImmediate(token);
					//Data Collection
					int index = coord_y * GC.numColumns + coord_x;
					revealturn++;
					mydata.reveal_order[index] = revealturn;
					mydata.outcome[index] = 2;
					field[coord_x, coord_y] = 2;
				}

				isPlayersTurn = !isPlayersTurn;
				probDict.Remove(chosenObjectID);

				StartCoroutine(Won());
			}
			
			DM.SwitchPlayer(isPlayersTurn);
			if (gameOver) {
				revealingProbs = false;
			}
			yield return 0;
		}

		/* public IEnumerator revealProbabilities()
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
				mydata.reveal_order[index] = i + 1;

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
						mydata.outcome[index] = 1;
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
						mydata.outcome[index] = 2;
						field[x, y] = 2;
					}
				}
				isPlayersTurn = !isPlayersTurn;

				StartCoroutine(Won());

				while (isCheckingForWinner)
					yield return null;

				if (gameOver)
					break;

				yield return new WaitForSeconds(1);
			}
			yield return 0;
		} */

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
