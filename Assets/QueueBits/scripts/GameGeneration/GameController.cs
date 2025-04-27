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

		private Coroutine columnHighlightCoroutine;
        private GameObject activeColumnHighlight;

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
			if (tutorialStepButton != null)
			{
				Button button = tutorialStepButton.GetComponent<Button>();
				if (button != null)
				{
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(() => {
						if (tutorialLevels.Contains(LEVEL_NUMBER) && tutorialStep != -1)
						{
							tutorialStep++; // Increment tutorialStep directly here
							updateTutorial();
						}
						else if (tutorialPane.activeSelf)
						{
							InfoNextStep();
						}
					});
				}
			}
			if (tutorialLevels.Contains(LEVEL_NUMBER))
			{
				// pointer.SetActive(true);
				tutorialPane.SetActive(true);
				tutorialPieceCounter.SetActive(false);
				updateTutorial(1);

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

		public void updateTutorial(int i = 0){
			Debug.Log("update Tutorial! " + i);
			if (i == 1){tutorialStep+=1;}
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
						// tutorialToggle("Amazing!\n\nLooks like you got the hang of it! If you ever need a reminder of what to do be sure to click on the hint button: \n\n\n\n\n\n\n", 3);
						tutorialToggle("Amazing!\n\nLooks like you got the hang of it!\n\nHave fun!");
						tutorialStepButton.SetActive(true);
						GM1.disconnectTutorial();
						break;
					case 11:
						tutorialStep = -1;
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
						tutorialStep = -1;
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
						tutorialStep = -1;
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
						tutorialStep = -1;
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
			// Removed dialogue from levels -- just intro and end now
			/* if (GameManager.saveData.dialogueSystem[LEVEL_NUMBER])
			{
				// dialoguePhase = true;
				Wrapper.Events.StartDialogueSequence?.Invoke($"QB_Level{LEVEL_NUMBER}");
				GameManager.saveData.dialogueSystem[LEVEL_NUMBER] = false;
				GameManager.Save();
				// Wrapper.Events.DialogueSequenceEnded += updateDialoguePhase;
			} */

			initMyData();

			if (LEVEL_NUMBER > 2)
			{
				(boardName, prefilledBoard) = PB.getRandomBoard(LEVEL_NUMBER);
				initPrefilledBoard();

				myData.prefilledBoard = boardName;
			}
		}


		private List<string> infoSections = new List<string>();
		private int infoStep = 0;

		public void GetInfo()
		{
			Debug.Log("Showing game information");

			// Cancel any existing highlights
			CancelHighlights();

			// Reset info step
			infoStep = 0;

			// Set up the information sections
			PrepareInfoSections();

			// Show the pane
			tutorialPane.SetActive(true);

			// Set the initial info text
			if (tutorialText != null)
			{
				tutorialText.SetText(infoSections[infoStep]);
			}

			// Hide the tutorial image if it's showing
			if (tutorialImage != null)
				tutorialImage.gameObject.SetActive(false);

			// Show the tutorial step button with "Next" text
			if (tutorialStepButton != null) {
				tutorialStepButton.SetActive(true);
				TextMeshProUGUI buttonText = tutorialStepButton.GetComponentInChildren<TextMeshProUGUI>();
				if (buttonText != null)
					buttonText.text = "Next";
			}
		}

		private void PrepareInfoSections()
		{
			infoSections.Clear();

			// Basic game rules for all levels
			infoSections.Add("Connect 4 tokens in a row (horizontal, vertical, or diagonal) to win before Byte does!");

			// Level-specific information
			if (LEVEL_NUMBER < 3)
			{
				infoSections.Add("• Click a token below to select it\n• Click a column to drop your token\n• First to connect 4 tokens wins!");
			}
			else if (LEVEL_NUMBER < 6)
			{
				infoSections.Add("• Tokens in superposition have both yellow and red states\n• When played, they instantly become either yellow or red\n• Higher probability tokens (75%, 100%) are more likely to be yellow");
			}
			else if (LEVEL_NUMBER < 11)
			{
				infoSections.Add("• Tokens stay in superposition until the board is filled\n• After the board is full, the tokens will change one by one\n• Plan ahead! Consider what might happen when tokens are measured");
			}
			else
			{
				infoSections.Add("• Tokens stay in superposition until the board is filled\n• After the board is full, YOU choose which tokens to measure and in what order\n• Choose strategically! Measure tokens that could create winning connections");
			}

			// Add token explanation for levels 3+
			if (LEVEL_NUMBER >= 3) {
				infoSections.Add("• 100% token: Will always be yellow\n• 75% token: Has a 75% chance of being yellow\n• 50% token: Equal chance of being yellow or red");
			}

			// Final page: Make a move to continue
			infoSections.Add("Click the Hint button for suggestions on your next move!\n\nMake a move to continue playing");
		}

		// This function will be called when the "Next" button is clicked during info display
		public void InfoNextStep()
		{
			infoStep++;
			if (infoStep < infoSections.Count)
			{
				// Show next info section
				if (tutorialText != null)
				{
					tutorialText.SetText(infoSections[infoStep]);
				}

				// Update button text on last page
				if (tutorialStepButton != null && infoStep == infoSections.Count - 1)
				{
					TextMeshProUGUI buttonText = tutorialStepButton.GetComponentInChildren<TextMeshProUGUI>();
					if (buttonText != null)
						buttonText.text = "Got it!";
				}
			}
			else
			{
				// Close the info pane when we've gone through all sections
				tutorialPane.SetActive(false);
			}
		}
		public void GetHint()
		{
			Debug.Log("Hint provided!");
			// Create temp AI
			GameObject tempObject = new GameObject("TempAI");
    		CPUBrain tempAI = tempObject.AddComponent<CPUBrain>();
			tempAI.state = cpuAI.state;
			tempAI.colPointers = new int[cpuAI.colPointers.Length];
			System.Array.Copy(cpuAI.colPointers, tempAI.colPointers, cpuAI.colPointers.Length);
			tempAI.superpositionArray = cpuAI.superpositionArray;
			tempAI.difficulty = cpuAI.difficulty;

			// Find best move from temp AI
			(int bestMove, int difference) = tempAI.findBestMoveandDifference(tempAI.colPointers, 4);
			Destroy(tempObject);
			(int recommendedProbability, string explanation) = CalculateRecommendedProbability(tempAI, bestMove, difference);
			Debug.Log("Difference: " + difference);
			// Check if there are still tokens of given probability
			//Debug.Log("Recommended probability: " + recommendedProbability);
			DisplayHint(recommendedProbability, bestMove, explanation);
			//Debug.Log("Best move: " + bestMove);
		}


		private void DisplayHint(int recommendedProbability, int bestMove, string explanation)
		{
			Debug.Log($"Showing hint: Use {recommendedProbability}% token in column {bestMove}");

			// Highlight the recommended token based on game mode
			if (LEVEL_NUMBER < 6)
			{
				GM1.tokenCounterPlayer.HighlightToken(recommendedProbability);
			}
			else if (LEVEL_NUMBER < 11)
			{
				GM2.tokenCounterPlayer.HighlightToken(recommendedProbability);
			}
			else
			{
				GM3.tokenCounterPlayer.HighlightToken(recommendedProbability);
			}
			// Highlight the column where the token should be played
			HighlightRecommendedColumn(bestMove);
			ShowHintExplanation(explanation);
		}

		private void ShowHintExplanation(string explanation)
		{
			// Use the existing tutorial panel instead of creating a new one
			if (tutorialPane != null)
			{
				// Store the original text to restore later
				string originalText = "";
				if (tutorialText != null)
					originalText = tutorialText.text;

				// Show the tutorial pane if it's not already active
				tutorialPane.SetActive(true);

				// Set the explanation text using the same method as tutorial text
				if (tutorialText != null)
				{
					tutorialText.SetText(explanation);
				}

				// Hide the tutorial image if it's showing
				if (tutorialImage != null)
					tutorialImage.gameObject.SetActive(false);

				// Hide the tutorial step button if it exists
				if (tutorialStepButton != null)
					tutorialStepButton.SetActive(false);

				// Start a coroutine to hide the explanation after a delay
				StartCoroutine(HideHintExplanation(tutorialPane, 7.0f, originalText));
			}
			else
			{
				Debug.LogWarning("Tutorial pane not found. Cannot display hint explanation.");
			}
		}

		private IEnumerator HideHintExplanation(GameObject panel, float delay, string originalText)
		{
			yield return new WaitForSeconds(delay);

			// Restore original text if in a tutorial level
			if (tutorialLevels.Contains(LEVEL_NUMBER) && tutorialText != null)
			{
				tutorialText.SetText(originalText);
			}
			else
			{
				// If not in tutorial, just hide the panel
				panel.SetActive(false);
			}
		}

		private void HighlightRecommendedColumn(int column)
		{
			Debug.Log("Highlighting column: " + column);
			// Cancel any existing highlight
			if (columnHighlightCoroutine != null)
			{
				StopCoroutine(columnHighlightCoroutine);
				if (activeColumnHighlight != null)
					Destroy(activeColumnHighlight);
			}

			// Create a downward-pointing arrow
			activeColumnHighlight = new GameObject("ColumnHighlightArrow");
			activeColumnHighlight.transform.SetParent(fieldObject.transform);

			// Calculate position based on board layout - make sure it's actually above the board
			float columnWidth = 1.0f;
			float startX = -((numColumns - 1) * columnWidth / 2.0f);
			Vector3 position = new Vector3(startX + (column * columnWidth)+ 3.0f, 1.5f, -0.1f);

			activeColumnHighlight.transform.position = position;

			// Create the arrow mesh directly (simpler approach)
			GameObject arrowObject = new GameObject("ArrowMesh");
			arrowObject.transform.parent = activeColumnHighlight.transform;
			arrowObject.transform.localPosition = Vector3.zero;

			MeshFilter meshFilter = arrowObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = arrowObject.AddComponent<MeshRenderer>();

			// Define points for a down-pointing arrow (simplified)
			Vector3[] vertices = new Vector3[7];
			vertices[0] = new Vector3(0f, -0.5f, 0f);    // Arrow tip (down)
			vertices[1] = new Vector3(0.3f, -0.1f, 0f);  // Bottom right corner
			vertices[2] = new Vector3(0.15f, -0.1f, 0f); // Right inner corner
			vertices[3] = new Vector3(0.15f, 0.5f, 0f);  // Top right corner
			vertices[4] = new Vector3(-0.15f, 0.5f, 0f); // Top left corner
			vertices[5] = new Vector3(-0.15f, -0.1f, 0f);// Left inner corner
			vertices[6] = new Vector3(-0.3f, -0.1f, 0f); // Bottom left corner

			// Create the mesh with triangles
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.triangles = new int[] {
				0, 1, 2,  // Right triangle of arrowhead
				0, 2, 5,  // Middle of arrowhead
				0, 5, 6,  // Left triangle of arrowhead
				2, 3, 4,  // Rectangle of shaft
				2, 4, 5   // Rectangle of shaft
			};
			mesh.RecalculateNormals();

			// Apply the mesh and set material
			meshFilter.mesh = mesh;
			meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
			meshRenderer.material.color = new Color(0.7f, 0.3f, 1.0f); // Brighter purple
			// Start animation coroutine
			columnHighlightCoroutine = StartCoroutine(AnimateColumnHighlight(activeColumnHighlight));
		}

		private IEnumerator AnimateColumnHighlight(GameObject highlight)
		{
			float duration = 7.0f;
			float elapsed = 0f;
			Vector3 originalPosition = highlight.transform.position;
			Vector3 targetPosition = originalPosition - new Vector3(0f, 0.2f, 0f); // Move down a small amount

			while (elapsed < duration)
			{
				// Only move up and down, no alpha pulsing
				float t = Mathf.PingPong(elapsed, 1f);
				highlight.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);

				elapsed += Time.deltaTime;
				yield return null;
			}

			Destroy(highlight);
			activeColumnHighlight = null;
			columnHighlightCoroutine = null;
		}

		public void CancelHighlights()
		{
			// Cancel column highlight if active
			if (columnHighlightCoroutine != null)
			{
				StopCoroutine(columnHighlightCoroutine);
				if (activeColumnHighlight != null)
					Destroy(activeColumnHighlight);
				activeColumnHighlight = null;
				columnHighlightCoroutine = null;
			}

			// Cancel token highlight if active (call method in TokenCounter)
			if (LEVEL_NUMBER < 6)
			{
				GM1.tokenCounterPlayer.CancelHighlight();
			}
			else if (LEVEL_NUMBER < 11)
			{
				GM2.tokenCounterPlayer.CancelHighlight();
			}
			else
			{
				GM3.tokenCounterPlayer.CancelHighlight();
			}

			// Hide the tutorial pane if it's showing a hint
			if (tutorialPane.activeSelf && !tutorialLevels.Contains(LEVEL_NUMBER))
			{
				tutorialPane.SetActive(false);
			}
		}

		private (int, string) CalculateRecommendedProbability(CPUBrain ai, int column, int difference)
		{
			string originalState = ai.state;
			int[] originalPointers = new int[ai.colPointers.Length];
			System.Array.Copy(ai.colPointers, originalPointers, ai.colPointers.Length);

			// Get current game phase based on how full the board is
			GamePhase currentPhase = DetermineGamePhase(ai);

			// Get token inventory
			int tokens100 = GetRemainingTokenCount(100);
			int tokens75 = GetRemainingTokenCount(75);
			int tokens50 = GetRemainingTokenCount(50);

			// Perform critical checks
			int row = originalPointers[column];
			// CASE 1: Check if we can win immediately with this move
			ai.playMove(column, "1");
			bool playerWinsWithMove = ai.isWin(row, column, "1");

			// Restore state
			ai.state = originalState;
			System.Array.Copy(originalPointers, ai.colPointers, originalPointers.Length);

			// CASE 2: Check if we're blocking CPU's win
			ai.playMove(column, "2");
			bool blockingCpuWin = ai.isWin(row, column, "2");

			// Restore state
			ai.state = originalState;
			System.Array.Copy(originalPointers, ai.colPointers, originalPointers.Length);

			// CASE 3: Check if our move becomes a threat if it turns red
			ai.playMove(column, "1");
			int index = ai.index(row, column);
			string modifiedState = ai.state.Substring(0, index) + "2" + ai.state.Substring(index + 1);
			ai.state = modifiedState;
			bool createsRiskIfRed = ai.isWin(row, column, "2");

			// Restore state
			ai.state = originalState;
			System.Array.Copy(originalPointers, ai.colPointers, originalPointers.Length);

			// CASE 4: Check if we're creating a strategic threat
			ai.playMove(column, "1");
			bool createsThreatForUs = CheckForThreats(ai, row, column, "1");

			// Restore state
			ai.state = originalState;
			System.Array.Copy(originalPointers, ai.colPointers, originalPointers.Length);

			// CASE 5: Check for opponent threats
			bool opponentHasThreat = false;
			List<int> validMoves = GetValidMoves(ai.colPointers);

			foreach (int col in validMoves)
			{
				if (col == column) continue;
				int opponentRow = ai.colPointers[col];
				if (opponentRow < 0) continue;

				ai.playMove(col, "2");
				if (CheckForThreats(ai, opponentRow, col, "2"))
				{
					opponentHasThreat = true;
					break;
				}

				// Restore state
				ai.state = originalState;
				System.Array.Copy(originalPointers, ai.colPointers, originalPointers.Length);
			}

			// Calculate strategic value of the position
			int strategicValue = CalculateStrategicValue(column, currentPhase);
			string explanation;

			// Critical situations - must use highest probability available
			if (playerWinsWithMove || blockingCpuWin) {
				if (playerWinsWithMove) {
					explanation = "Wow! You have a likely win right now! Use your highest probability token here to try to guarantee victory!";
				}
				else {
					explanation = "Watch out! Byte is about to win! Use your highest probability token to block this move or you might lose!";
				}
				if (tokens100 > 0)
					return (100, explanation);
				else if (tokens75 > 0)
					return (75, explanation);
				else
					return (50, explanation);
			}

			// High risk situations - prefer 75% when available but only when truly necessary
			if (createsRiskIfRed) {
				explanation = "Careful! If this token turns red, it could help Byte win. A medium (75%) token would be a good balance of safety and efficiency!";
				// Use 75% for high-risk situations, but consider the game phase
				if (currentPhase == GamePhase.Late && tokens75 > 0)
					return (75, explanation);
				else if (tokens100 > 0)
					return (100, explanation);
				else if (tokens75 > 0)
					return (75, explanation);
				else
					return (50, explanation);
			}

			// Strategic positions - more selective use of 75%
			if (createsThreatForUs || (opponentHasThreat && column == GetBestBlockingMove(ai, validMoves))) {
				// For threats, consider the game phase and strategic value
				if (currentPhase == GamePhase.Late) {
					explanation = "This is a key move that could help you win! A high probability token gives you the best chance to secure this position!";
					if (tokens100 > 0)
						return (100, explanation);
					else if (tokens75 > 0)
						return (75, explanation);
					else
						return (50, explanation);
				}
				else if (strategicValue >= 8) {
					explanation = "Smart move! This creates a trap for Byte in a highly strategic position. A medium (75%) token gives you a good balance of safety and efficiency!";
					if (tokens75 > 0)
						return (75, explanation);
					else if (tokens50 > 0)
						return (50, explanation);
					else
						return (100, explanation);
				}
				else {
					explanation = "This creates a potential opportunity! Use a low probability token to save your stronger tokens for more critical positions.";
					if (tokens50 > 0)
						return (50, explanation);
					else if (tokens75 > 0)
						return (75, explanation);
					else
						return (100, explanation);
				}
			}

			// Early game center columns - be more selective
			if (currentPhase == GamePhase.Early && strategicValue >= 8) {
				explanation = "Great spot! The central columns give you more ways to win later. A medium token could help control this important position!";
				// Only use 75% for the absolute center in early game
				if (column == numColumns/2 && tokens75 > 0 && Random.Range(0, 100) < 50)
					return (75, explanation);
				else
					return (50, explanation);
			}

			// Mid-game positions with high minimax values - more selective with 75%
			if (difference > 15) {
				explanation = "This is a strong move that keeps you in control of the game. A medium token could be ideal for this promising position!";
				if (currentPhase == GamePhase.Mid && strategicValue >= 7 && tokens75 > 0 && Random.Range(0, 100) < 40)
					return (75, explanation);
				else if (tokens50 > 0)
					return (50, explanation);
				else if (tokens75 > 0)
					return (75, explanation);
				else
					return (100, explanation);
			}

			// Even for non-critical moves, recommend 75% very selectively
			if (currentPhase == GamePhase.Late && strategicValue >= 8 && tokens75 > 0 && Random.Range(0, 100) < 30) {
				explanation = "This position could become important soon! A medium token gives you a balanced chance without wasting your best tokens.";
				return (75, explanation);
			}

			// Non-critical moves - default to 50%
			explanation = "This is a safe move, but not critical yet. Save your best tokens for later and use a low probability token here!";
			if (tokens50 > 0)
				return (50, explanation);
			else if (tokens75 > 0)
				return (75, explanation);
			else
				return (100, explanation);
		}

		private enum GamePhase
		{
			Early,
			Mid,
			Late
		}

		private GamePhase DetermineGamePhase(CPUBrain ai)
		{
			// Count total pieces on board
			int piecesOnBoard = 0;
			foreach (char c in ai.state)
			{
				if (c != '0')
					piecesOnBoard++;
			}

			int totalPositions = numRows * numColumns;
			float fillPercentage = (float)piecesOnBoard / totalPositions;

			if (fillPercentage < 0.33f)
				return GamePhase.Early;
			else if (fillPercentage < 0.66f)
				return GamePhase.Mid;
			else
				return GamePhase.Late;
		}

		private int GetRemainingTokenCount(int probability)
		{
			// Return number of tokens of given probability based on current game mode
			if (LEVEL_NUMBER < 6)
			{
				return GM1.tokenCounterPlayer.getCounter(probability);
			}
			else if (LEVEL_NUMBER < 11)
			{
				return GM2.tokenCounterPlayer.getCounter(probability);
			}
			else
			{
				return GM3.tokenCounterPlayer.getCounter(probability);
			}
		}

		// Calculate strategic value of a position (0-10 scale)
		private int CalculateStrategicValue(int column, GamePhase phase)
		{
			// Center columns are more valuable
			int centerValue = 10 - Math.Abs(column - (numColumns / 2)) * 2;

			// In early game, center is most important
			if (phase == GamePhase.Early)
				return centerValue;

			// In mid/late game, use column height to evaluate position
			int height = cpuAI.colPointers[column];
			int heightValue = 5 + (numRows - height);

			// Combined value weighted by game phase
			if (phase == GamePhase.Mid)
				return (centerValue + heightValue) / 2;
			else // Late game
				return heightValue;
		}

		// Check if a move creates a threat (3 in a row with an open spot to make 4)
		private bool CheckForThreats(CPUBrain ai, int row, int column, string player)
		{
			// Count consecutive pieces in all directions
			int[] connected = new int[4];
			int[] open = new int[4]; // Track open spaces at ends

			// Horizontal check
			int leftCount = CountConsecutive(ai, row, column, 0, -1, player);
			int rightCount = CountConsecutive(ai, row, column, 0, 1, player);
			connected[0] = leftCount + rightCount + 1;
			open[0] = CountOpenEnds(ai, row, column, 0, -1, player) +
					CountOpenEnds(ai, row, column, 0, 1, player);

			// Vertical check
			int downCount = CountConsecutive(ai, row, column, 1, 0, player);
			int upCount = CountConsecutive(ai, row, column, -1, 0, player);
			connected[1] = downCount + upCount + 1;
			open[1] = CountOpenEnds(ai, row, column, 1, 0, player) +
					CountOpenEnds(ai, row, column, -1, 0, player);

			// Diagonal down-right
			int downRightCount = CountConsecutive(ai, row, column, 1, 1, player);
			int upLeftCount = CountConsecutive(ai, row, column, -1, -1, player);
			connected[2] = downRightCount + upLeftCount + 1;
			open[2] = CountOpenEnds(ai, row, column, 1, 1, player) +
					CountOpenEnds(ai, row, column, -1, -1, player);

			// Diagonal down-left
			int downLeftCount = CountConsecutive(ai, row, column, 1, -1, player);
			int upRightCount = CountConsecutive(ai, row, column, -1, 1, player);
			connected[3] = downLeftCount + upRightCount + 1;
			open[3] = CountOpenEnds(ai, row, column, 1, -1, player) +
					CountOpenEnds(ai, row, column, -1, 1, player);

			// Check if we have a threat (3 connected with at least one open end)
			for (int i = 0; i < 4; i++)
			{
				if (connected[i] >= 3 && open[i] >= 1)
					return true;
			}

			return false;
		}

		// Count consecutive pieces in a direction
		private int CountConsecutive(CPUBrain ai, int row, int col, int rowDelta, int colDelta, string player)
		{
			int count = 0;
			row += rowDelta;
			col += colDelta;

			while (IsValidPosition(row, col))
			{
				int index = ai.index(row, col);
				if (index >= 0 && index < ai.state.Length && ai.state[index].ToString() == player)
				{
					count++;
					row += rowDelta;
					col += colDelta;
				}
				else
					break;
			}

			return count;
		}

		// Count open spaces at the end of a line
		private int CountOpenEnds(CPUBrain ai, int row, int col, int rowDelta, int colDelta, string player)
		{
			// First count consecutive pieces in this direction
			int piecesCount = CountConsecutive(ai, row, col, rowDelta, colDelta, player);

			// Move to the position just after the last consecutive piece
			row += rowDelta * (piecesCount + 1);
			col += colDelta * (piecesCount + 1);

			// Check if this position is valid and empty
			if (IsValidPosition(row, col))
			{
				int index = ai.index(row, col);
				if (index >= 0 && index < ai.state.Length && ai.state[index] == '0')
				{
					// Verify there's a piece below (or it's the bottom row)
					if (row == numRows - 1 || (row + 1 < numRows && ai.index(row + 1, col) >= 0 &&
						ai.state[ai.index(row + 1, col)] != '0'))
					{
						return 1;
					}
				}
			}

			return 0;
		}

		private bool IsValidPosition(int row, int col)
		{
			return row >= 0 && row < numRows && col >= 0 && col < numColumns;
		}

		private List<int> GetValidMoves(int[] colPointers)
		{
			List<int> moves = new List<int>();
			for (int i = 0; i < colPointers.Length; i++)
			{
				if (colPointers[i] >= 0)
					moves.Add(i);
			}
			return moves;
		}

		private int GetBestBlockingMove(CPUBrain ai, List<int> validMoves)
		{
			int bestCol = validMoves[0];
			int maxThreats = 0;

			string originalState = ai.state;
			int[] originalPointers = new int[ai.colPointers.Length];
			System.Array.Copy(ai.colPointers, originalPointers, ai.colPointers.Length);

			foreach (int col in validMoves)
			{
				int row = ai.colPointers[col];
				if (row < 0) continue;

				// Play move
				ai.playMove(col, "1");

				// Count how many opponent threats this blocks
				int threatsBlocked = 0;

				// Check in all directions if this prevents a 3-in-a-row for opponent
				if (CountNearbyOpponentPieces(ai, row, col, "2") >= 2)
					threatsBlocked++;

				// Restore state
				ai.state = originalState;
				System.Array.Copy(originalPointers, ai.colPointers, originalPointers.Length);

				if (threatsBlocked > maxThreats)
				{
					maxThreats = threatsBlocked;
					bestCol = col;
				}
			}

			return bestCol;
		}

		private int CountNearbyOpponentPieces(CPUBrain ai, int row, int col, string opponentColor)
		{
			int count = 0;

			// Check all 8 directions
			int[] dx = {-1, -1, -1, 0, 0, 1, 1, 1};
			int[] dy = {-1, 0, 1, -1, 1, -1, 0, 1};

			for (int i = 0; i < 8; i++)
			{
				int newRow = row + dx[i];
				int newCol = col + dy[i];

				if (IsValidPosition(newRow, newCol))
				{
					int index = ai.index(newRow, newCol);
					if (index >= 0 && index < ai.state.Length && ai.state[index].ToString() == opponentColor)
						count++;
				}
			}

			return count;
		}

		// Ends game, sets display and saves relevant data
		public void EndGame(Results result)
		{
			// Save data
			myData.winner = (int)result;
			saveData.Save(myData);

			// Manage stars awarded
			int starsWon = starDisplay.getResults(result);
			if (GameManager.saveData.starSystem[LEVEL_NUMBER] <= starsWon) {
				GameManager.saveData.starSystem[LEVEL_NUMBER] = starsWon;
			} // Update max level unlocked
			if (LEVEL_NUMBER == GameManager.saveData.maxLevelUnlocked) {
				GameManager.saveData.maxLevelUnlocked++;
			}
			GameManager.Save();

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
