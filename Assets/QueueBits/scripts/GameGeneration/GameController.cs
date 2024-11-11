using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using QueueBits;
using TMPro;
using System;
using Mono.Cecil.Cil;

//using System;
//using MySql.Data.MySqlClient;
//using Data;

namespace QueueBits
{
	public class GameController : MonoBehaviour
	{
		public int LEVEL_NUMBER;

		// Static variables referenced in GameModes and cpuAI
		public int numRows = 6;
		public int numColumns = 7;
		public int numPiecesToWin = 4;
		public float dropTime = 1f;

		// Create new Data object when loaded
		public Data myData = new Data();

		// References to AI, Display, and Stars
		public CPUBrain cpuAI;
		public DisplayManager DM;
		public StarDisplay starDisplay;

		// Holder for Board and all tokens
		public GameObject fieldObject;

		// Prefilled Board objects and name
		[Header("Prefilled Boards")]
		public PrefilledBoards PB;
		public List<(Piece, int, int, int)> prefilledBoard = new List<(Piece piece, int col, int row, int prob)>();
		public string boardName;

		[Header("GameModes")]
		public GameMode1 GM1;
		public GameMode2 GM2;
		public GameMode3 GM3;

		[Header("Meter")]
		public GameObject meter;

		[Header("Tutorial")]
		public GameObject pointer;
		public GameObject tutorialPane;
		public TextMeshProUGUI tutorialText;
		public Image tutorialImage;
		public Sprite[] tutorialImages;

		public GameObject tutorialStepButton;

		public GameObject tutorialPieceCounter;

		private HashSet<int> tutorialLevels = new HashSet<int> { 1, 3, 6, 11 };

		private int tutorialStep;

		// Initializes game based on level from static GameManager
		void Start()
		{
			// pointer.SetActive(true);

			// Debug.Log(tutorialLevels.Contains(LEVEL_NUMBER));
			// Set all inactive as a precaution
			GM1.gameObject.SetActive(false);
			GM2.gameObject.SetActive(false);
			GM3.gameObject.SetActive(false);

			// Get level number
			LEVEL_NUMBER = GameManager.LEVEL;
			DM.initDisplay(LEVEL_NUMBER);
			tutorialStep = 0;
			if (tutorialLevels.Contains(LEVEL_NUMBER))
			{
				// pointer.SetActive(true);
				tutorialPane.SetActive(true);
				tutorialPieceCounter.SetActive(false);
				updateTutorial();
			}
			else{
			pointer.SetActive(false);
			tutorialPane.SetActive(false);
			// Choose GameMode based on level number
			if (LEVEL_NUMBER < 6)
			{
				GM1.gameObject.SetActive(true);
				GM1.StartGame();
				meter.SetActive(false);
			}
			else if (LEVEL_NUMBER < 11)
			{
				GM2.gameObject.SetActive(true);
			}
			else
			{
				GM3.gameObject.SetActive(true);
			}
			}
		}

		private void tutorialToggle(string s){
			tutorialText.SetText(s);
			tutorialImage.gameObject.SetActive(false);
		}

		private void tutorialToggle(string s, int i){
			Debug.Log(s);
			tutorialText.SetText(s);
			tutorialImage.sprite = tutorialImages[i];
			tutorialImage.gameObject.SetActive(true);
		}

		public void updateTutorial(){
			tutorialStep+=1;
			Debug.Log("update Tutorial!");
			if(LEVEL_NUMBER == 1){
				GM1.gameObject.SetActive(true);
				switch (tutorialStep)
				{
					case 1:
						tutorialToggle("Thanks for helping me beat Byte! He's my sworn nemesis!");
						break;
					case 2:
						tutorialToggle("The rules of the game are simple. All you need to do is make a row of 4 tokens before Byte does!");
						break;
					case 3:
						tutorialToggle("These 4 tokens can be arranged horizontally...\n\n\n\n\n\n", 0);
						break;
					case 4:
						tutorialToggle("Vertically...\n\n\n\n\n\n", 1);
						break;
					case 5:
						tutorialToggle("Even as a diagonal!\n\n\n\n\n\n", 2);
						break;
					case 6:
						tutorialToggle("It is Byte's turn first... Did you see how they just placed a red token?" );
						GM1.gameObject.SetActive(true);
						GM1.StartGame(this);
						tutorialStepButton.SetActive(false);
						break;
					case 7:
						tutorialStepButton.SetActive(true);
						break;
					case 8:
						tutorialToggle("Now that it's our turn we first need to grab a token!");
						tutorialStepButton.SetActive(false);
						tutorialPieceCounter.SetActive(true);
						pointer.SetActive(true);
						break;
					case 9:
						tutorialToggle("Great job!\n\nNow click where you want to place your token.");
						tutorialStepButton.SetActive(false);
						break;		
					case 10:
						tutorialToggle("Amazing!\n\nLooks like you got the hang of it! If you ever need a reminder of what to do be sure to click on the hint button: \n\n\n\n\n\n\n", 3);
						tutorialStepButton.SetActive(true);
						GM1.disconnectTutorial();
						break;
					case 11:
						tutorialPane.SetActive(false);
						break;			
					
					
					
					default:
						Debug.Log("TutorialFallThrough");
						return;
				}
			} else if(LEVEL_NUMBER==3){
				
				GM1.gameObject.SetActive(true);

				switch (tutorialStep){
					case 1:
						tutorialToggle("Thanks for all your help!\n\n\nThis level things are going to get a little bit different!");
						GM1.StartGame();
						break;
					case 2:
						tutorialToggle("We have new tokens that are in superposition!\n\n That means the tokens are BOTH yellow and red at the same time!\n\n\n\n\n\n\n\n\n", 4);
						break;
					case 3:
						tutorialToggle("Here's the trick though:\n\nWhen you play a superposition token it will become either yellow OR red!!");
						break;
					case 4:
						tutorialToggle("Superposition tokens that have more yellow will end up becoming yellow very often.\n\n\n\n\n\n\n", 5);
						break;
					case 5:
						tutorialToggle("If they don't have that much yellow they will become yellow less often.\n\n\n\n\n\n\n\n", 6);
						break;
					case 6 :
						tutorialToggle("Ready to play?");
						break;
					case 7:
						tutorialPane.SetActive(false);
						tutorialPieceCounter.SetActive(true);
						break;			
					default:
						Debug.Log("TutorialFallThrough");
						return;
				}
			} else if(LEVEL_NUMBER==6){
				
				GM2.gameObject.SetActive(true);

				switch (tutorialStep){
					case 1:
						tutorialToggle("Hi Again! This time we are changing the rules a little bit.");
						break;
					case 2:
						tutorialToggle("From now on the tokens will remain in superposition UNTIL the entire board is filled.");
						break;
					case 3:
						tutorialToggle("After that, the tokens will be automatically measured one by one!\n\n\nReady to play??");
						break;
					case 4:
						tutorialPane.SetActive(false);
						tutorialPieceCounter.SetActive(true);
						break;			
					default:
						Debug.Log("TutorialFallThrough");
						return;
				}
			}else if(LEVEL_NUMBER==11){
				
				GM3.gameObject.SetActive(true);

				switch (tutorialStep){
					case 1:
						tutorialToggle("Almost done! Thanks to your help Byte will think twice before challenging us again!");
						break;
					case 2:
						tutorialToggle("There is one final change...\n\n\n This time you get to picl the order in which the tokens are measured at the end of the game!");
						break;
					case 3:
						tutorialToggle("Fill the entire board then simply click on a superposition token to measure it!\n\n\nLets go!");
						break;
					case 4:
						tutorialPane.SetActive(false);
						tutorialPieceCounter.SetActive(true);
						break;			
					default:
						Debug.Log("TutorialFallThrough");
						return;
				}
			}

		}
		// Called from GameMode#, sets up dialogue, data, and prefilled board
		public void StartGame()
		{
			// Debug.Log("Not starting!");
			// return;
			if (GameManager.saveData.dialogueSystem[LEVEL_NUMBER])
			{
				// dialoguePhase = true;
				Wrapper.Events.StartDialogueSequence?.Invoke($"QB_Level{LEVEL_NUMBER}");
				GameManager.saveData.dialogueSystem[LEVEL_NUMBER] = false;
				GameManager.Save();
				// Wrapper.Events.DialogueSequenceEnded += updateDialoguePhase;
			}

			initMyData();

			if (LEVEL_NUMBER > 2)
			{
				(boardName, prefilledBoard) = PB.getRandomBoard(LEVEL_NUMBER);
				initPrefilledBoard();

				myData.prefilledBoard = boardName;
			}
		}

		public void GetHint()
		{
			Debug.Log("Hint provided!");
		}

		// Ends game, sets display and saves relevant data
		public void EndGame(Results result)
		{
			// Save data
			myData.winner = (int)result;
			saveData.Save(myData);

			// Manage stars awarded
			int starsWon = starDisplay.getResults(result);
			if (GameManager.saveData.starSystem[LEVEL_NUMBER] <= starsWon)
			{
				GameManager.saveData.starSystem[LEVEL_NUMBER] = starsWon;
				GameManager.Save();
			}

			// Update display
			fieldObject.SetActive(false);
			DM.GameOver(result);

			// Check if there's a reward card
			if (GameManager.rewardSystem[LEVEL_NUMBER])
			{
				Wrapper.Events.CollectAndDisplayReward?.Invoke(Wrapper.Game.QueueBits, LEVEL_NUMBER);
			}
		}

		// Helper to initialize myData
		public void initMyData()
		{
			myData.level = LEVEL_NUMBER;
			myData.userID = Wrapper.Events.GetPlayerResearchCode?.Invoke();
			myData.placement_order = new int[numColumns * numRows];
			myData.superposition = new int[numColumns * numRows];
			myData.reveal_order = new int[numColumns * numRows];
			myData.outcome = new int[numColumns * numRows];
			for (int i = 0; i < numColumns * numRows; i++)
			{
				myData.placement_order[i] = 0;
				myData.superposition[i] = 0;
				myData.reveal_order[i] = 0;
				myData.outcome[i] = 0;
			}
			cpuAI.superpositionArray = myData.superposition;
		}

		// dialogue, UNSURE IF NEEDED
		/* void updateDialoguePhase()
		{
			// dialoguePhase = false;
			Wrapper.Events.DialogueSequenceEnded -= updateDialoguePhase;
		} */

		// New funtion to spawn piece when clicking buttons on TokenSelector
		public void tokenSelectedByButton(int prob)
		{
			if (LEVEL_NUMBER < 6)
			{
				GM1.tokenSelectedByButton(prob);
			}
			else if (LEVEL_NUMBER < 11)
			{
				GM2.tokenSelectedByButton(prob);
			}
			else
			{
				GM3.tokenSelectedByButton(prob);
			}
		}

		// Initializes array that contains Prefilled Board
		public void initPrefilledBoard()
		{
			int turn = 0;
			foreach ((Piece pi, int c, int r, int pr) in prefilledBoard)
			{
				turn++;
				int index = r * numColumns + c;
				myData.placement_order[index] = turn;

				// GameMode2 tokens are displayed in order of placement
				if (LEVEL_NUMBER > 5 && LEVEL_NUMBER < 11)
				{
					myData.reveal_order[index] = turn;
				}

				myData.superposition[index] = pr;
				if (pi == Piece.Player)
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

		// Checks if the field contains an empty cell
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
