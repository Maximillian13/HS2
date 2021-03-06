﻿// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerHitBox : MonoBehaviour 
{
	private bool cardioMode;
    private int squatCardioScore; // Player score
    public TextMeshPro tScore; // Text mesh for the score
    public CheckUp checkUp; // If the user went up after a wall
    private int checkUpCounter; // For when there are the multi-walls
    private bool gameOver;
	private bool gameStarted;

	private WallSoundEffects wallSoundEffects; // (Bruh)
	private string mostRecentWallType;

	public GameObject[] handPlacement = new GameObject[2];

	private GameModeMaster gameModeMaster;
	private ScorePopper scorePopper;
	private PersonalHighScore highScore;

	private int scoreConsecutiveCounter;
	private int nextConsecutiveTarget;


	private bool arcadeMode;

	//private SquatCounter squatCounter;
	private SteamLeaderBoardUpdater SteamLeaderBoardUpdater;

	private string GAME_MODE_PATH;
	private const int FIRST_TARGET = 100;

    // Set-up
	void Start () 
    {
		// If we are not in a gym destroy this component 
		if (tScore == null)
			Destroy(this);

		gameModeMaster = GameObject.Find("GameModeMaster").GetComponent<GameModeMaster>();
		scorePopper = this.transform.Find("ScorePopper").GetComponent<ScorePopper>();
		highScore = GameObject.Find("PersonalHighScore").GetComponent<PersonalHighScore>();

		squatCardioScore = 0;
		tScore.text = squatCardioScore.ToString();

		if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeArcade)
		{
			arcadeMode = true;
			scoreConsecutiveCounter = 0;
			nextConsecutiveTarget = FIRST_TARGET;
		}

		wallSoundEffects = this.GetComponent<WallSoundEffects>();

		//squatCounter = GameObject.Find("SquatWallCounterAchievments").GetComponent<SquatCounter>();
		SteamLeaderBoardUpdater = GameObject.Find("UpdateSteamLeaderBoard").GetComponent<SteamLeaderBoardUpdater>();

		// Get rid of the hand guard if we are not using it 
		this.EnableDisableHands(cardioMode == false && gameModeMaster.GetHandGuard() == true);
	}

	/// <summary>
	/// Set the game from normal to cardio (True for cardio, False for normal)
	/// </summary>
	public void SetGameMode(bool cardio)
	{
		this.cardioMode = cardio;
	}

    // If something hits the player
    void OnTriggerEnter(Collider other)
    {
		// For the normal game
		if (cardioMode == false)
		{
			// If its the Top of the wall reload level (Eventually explode boxes and show player high-score)
			if (other.name == "SquatWallUp")
			{
				this.CheckIfEndGame();
				mostRecentWallType = "Fail"; // For sound Effects
			}
			// If its the bottom (squatted under the wall) increase the player score
			if (other.name == "SquatWallDown")
			{
				if (checkUp.Ready == true)
				{
					squatCardioScore++;
					tScore.text = squatCardioScore.ToString();

					gameModeMaster.PassedThroughWall(false);

					// Save the total stat and check for achievements 
					AchivmentAndStatControl.IncrementStat(Constants.totalSquatWallCount);
					int totalSquatStat = AchivmentAndStatControl.GetStat(Constants.totalSquatWallCount);
					if (totalSquatStat != -1)
						AchivmentAndStatControl.CheckAllTotalSquatAchivments(totalSquatStat);

					if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeClassic)
					{
						AchivmentAndStatControl.SetStat(Constants.highestSquatConsec, squatCardioScore); // (Will only update stat if larger)
						AchivmentAndStatControl.CheckAllConsecutiveSquatAchivments(squatCardioScore);
					}

					// If it is a normal sized squat wall make the player stand back up right away
					if (other.transform.parent.parent == null || other.transform.parent.parent.name == "SquatWall(Clone)")
					{
						checkUp.Ready = false;

						mostRecentWallType = "Single"; // For sound Effects

						if (arcadeMode == true)
						{
							scoreConsecutiveCounter += 10;
							// Check if we got a combo, if we did it will display a combo, if not display what we just did 
							if (this.TestHighScoreConsecCounter() == false)
							{
								scorePopper.PopScoreMessage(0, .04f);
								highScore.UpdateYourScore(10);
							}
							else
							{
								mostRecentWallType = "Combo"; // For sound Effects
							}
						}
					}
					else if (other.transform.parent.parent.name == "SquatWallx2(Clone)")    // If squat wall is 2 long, allow 2
					{
						mostRecentWallType = "Double"; // For sound Effects
						this.ResetAfterNumberOfWalls(2);
					}
					else                                                                    // If squat wall is 3 long, allow 3	
					{
						mostRecentWallType = "Tripple"; // For sound Effects
						this.ResetAfterNumberOfWalls(3);
					}

				}
				else // Did not come up from a squat
				{
					this.CheckIfEndGame();
				}
			}
		}
		else // For cardio mode
		{
			// If the player hits the wall
			if (other.name == "CardioWallSolid")
			{
				this.CheckIfEndGame();
			}
			// If the player goes through the open spot
			if (other.name == "CardioWallOpen")
			{
				squatCardioScore++;
				tScore.text = squatCardioScore.ToString();

				if (arcadeMode == true)
				{
					scoreConsecutiveCounter += 10;
					// Check if we got a combo, if we did it will display a combo, if not display what we just did 
					if (this.TestHighScoreConsecCounter() == false)
					{
						scorePopper.PopScoreMessage(0, .04f);
						highScore.UpdateYourScore(10);
					}
					else
					{
						mostRecentWallType = "Combo"; // For sound Effects
					}
				}

				gameModeMaster.PassedThroughWall(true);

				// Save the total stat and check for achievements 
				AchivmentAndStatControl.IncrementStat(Constants.totalCardioWallCount);
				int totalCardioStats = AchivmentAndStatControl.GetStat(Constants.totalCardioWallCount);
				if (totalCardioStats != -1)
					AchivmentAndStatControl.CheckAllTotalCardioAchivments(totalCardioStats);

				if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeClassic)
				{
					// Check if we have reached any consecutive achievements and set stat
					AchivmentAndStatControl.SetStat(Constants.highestCardioConsec, squatCardioScore); // (Will only update stat if larger)
					AchivmentAndStatControl.CheckAllConsecutiveCardioAchivments(squatCardioScore);
				}
			}
		}
    }

	/// <summary>
	/// Make the player stand back up after multiple walls
	/// </summary>
	/// <param name="numberOfWalls">Number of wall that the player is squatting</param>
	private void ResetAfterNumberOfWalls(int numberOfWalls)
    {
        if (checkUpCounter == numberOfWalls - 1)
        {
            checkUp.Ready = false;
            checkUpCounter = 0;
			if (numberOfWalls == 2)
			{
				if (arcadeMode == true)
				{
					scoreConsecutiveCounter += 15;
					// Check if we got a combo, if we did it will display a combo, if not display what we just did 
					if (this.TestHighScoreConsecCounter() == false)
					{
						scorePopper.PopScoreMessage(1, .05f);
						highScore.UpdateYourScore(15);
					}
					else
					{
						mostRecentWallType = "Combo"; // For sound Effects
					}
				}
			}
			else
			{
				if (arcadeMode == true)
				{
					scoreConsecutiveCounter += 20;
					// Check if we got a combo, if we did it will display a combo, if not display what we just did 
					if (this.TestHighScoreConsecCounter() == false)
					{
						scorePopper.PopScoreMessage(2, .06f);
						highScore.UpdateYourScore(20);
					}
					else
					{
						mostRecentWallType = "Combo"; // For sound Effects
					}
				}
			}
		}
		else
        {
            checkUpCounter++;
        }
    }

	/// <summary>
	/// Check to see if we should end the game or put the player on a pause
	/// </summary>
	private void CheckIfEndGame()
	{
		// If we are on break we should not be penalizing the player
		if (gameModeMaster.GetOnBreak() == true)
			return;

		if(gameModeMaster.GetAmountOfLivesLeft() != int.MaxValue)
			gameModeMaster.DecrementAmountOfLives();

		// If you hit the wall reset your consecutive counter
		scoreConsecutiveCounter = 0;
		nextConsecutiveTarget = FIRST_TARGET;

		// Disable hand block so it doesnt get in your way
		this.EnableDisableHands(false);

		if (gameModeMaster.GetAmountOfLivesLeft() <= 0)
		{
			this.EndGame();
		}
		else
		{
			// Destroy walls
			this.DestroyAllWalls(true);
			// Tell the main game to go on break
			gameModeMaster.GoOnBreakImdate(10);
			if (gameModeMaster.GetAmountOfLivesLeft() != int.MaxValue)
				gameModeMaster.PopLivesLeft(4, gameModeMaster.GetAmountOfLivesLeft() );
			else
				gameModeMaster.PopLivesLeft(4, -1);

			// Reset everything to normal 
			checkUp.Ready = true;
			checkUpCounter = 0;
		}
	}

	/// <summary>
	/// Destroy all walls and end the game
	/// </summary>
    public void EndGame(bool viaPauseMenu = false)
    {
		// If we have already done the ending stuff return out
		if (gameOver == true)
			return;

		//// If we are in classic mode, update the consecutive cardio
		//if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeClassic)
		//{
		//	if (PlayerPrefs.GetInt(Constants.cardioMode) == 1)
		//	{
		//		// Check if we have reached any consecutive achievements and set stat
		//		AchivmentAndStatControl.SetStat(Constants.highestCardioConsec, squatCardioScore); // (Will only update stat if larger)
		//		AchivmentAndStatControl.CheckAllConsecutiveCardioAchivments(squatCardioScore);
		//	}
		//	else
		//	{
		//		AchivmentAndStatControl.SetStat(Constants.highestSquatConsec, squatCardioScore); // (Will only update stat if larger)
		//		AchivmentAndStatControl.CheckAllConsecutiveSquatAchivments(squatCardioScore);
		//	}
		//}

		// Save Score if in classic mode or if in daily challenge 
		if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeClassic || PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeDaily)
			SteamLeaderBoardUpdater.UpdateLeaderBoard(squatCardioScore);

		// If arcade, update based on the score 
		if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeArcade)
		{
			// Save score to steam leader board
			int currScore = highScore.GetYourScore();
			SteamLeaderBoardUpdater.UpdateLeaderBoard(currScore);
			AchivmentAndStatControl.SetStat(Constants.highScore, currScore);

			// Dont enable the player score pop up 
			if (viaPauseMenu == false)
			{
				// Pop up the name select
				ArcadeNameSelector nameSelector = GameObject.Find("ArcadeNameSelector").GetComponent<ArcadeNameSelector>();
				nameSelector.SetPlayerScore(currScore);
				nameSelector.SetToSize();
			}

			// Prevent loading level so we can pick name
			GameObject.Find("GameModeMaster").GetComponent<GameModeMaster>().PreventLevelFromLoading = true;
		}

		if (GameObject.Find("GYM") != null)
		{
			Transform st = GameObject.Find("GYM").transform.Find("SquatTrack");
			st.Find("SquatBars").GetComponent<GuideRail>().LowerRail();
			st.Find("DoorA").GetComponent<SquatTrackDoor>().CloseDoor();
			st.Find("DoorB").GetComponent<SquatTrackDoor>().CloseDoor();
		}

		// Disable hand block so it doesnt get in your way
		this.EnableDisableHands(false);

		// Gib all walls
		this.DestroyAllWalls(!viaPauseMenu);

		// Let the arcade mode handle ending the game if we are in arcade mode
		GameModeMaster gameMaster = GameObject.Find("GameModeMaster").GetComponent<GameModeMaster>();
		gameMaster.EndGame();

		gameOver = true;
	}

	/// <summary>
	/// Destroy all currently up walls
	/// </summary>
	private void DestroyAllWalls(bool playSound)
	{
		// Let shit fly
		if (GameObject.Find("GYM") != null)
		{
			Transform st = GameObject.Find("GYM").transform.Find("SquatTrack");
			st.Find("SquatBars").GetComponent<GuideRail>().PauseBoxCols();
		}

		// Get all the wall gibs
		GameObject[] gibsGO = GameObject.FindGameObjectsWithTag("Gibs");
		// Loop through all the gibs
		for (int x = 0; x < gibsGO.Length; x++)
		{
			Rigidbody gibs = gibsGO[x].GetComponent<Rigidbody>(); // Get the rigid body
			if (gibs.GetComponent<GibControl>().BlowUpGib == true)
			{
				if (gibs.transform.parent != null && gibs.transform.parent.parent != null)
				{
					Transform t = gibs.transform.parent.parent; // Get the grandparent (Squat-Wall)
					gibs.GetComponent<MeshRenderer>().enabled = true; // Make it so the player can see the gibs
					ShrinkAndDestroy shrinkAndDestroy = gibs.gameObject.GetComponent<ShrinkAndDestroy>();
					shrinkAndDestroy.ShrinkDestroy(10);
					gibs.transform.parent = null; // Un-parent the gibs
					gibs.isKinematic = false; // Make physics work on them
					gibs.AddForce(new Vector3(Random.Range(-750, 750), Random.Range(750, 1000), Random.Range(-300, 300))); // Add random force
					gibs.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(100, 100), Random.Range(-100, 100))); // Add random torque
					Destroy(t.gameObject); // Destroy the squat wall
				}
			}
		}

		// Make sound
		if (gameStarted == true)
			wallSoundEffects.PlayClip(WallSoundEffects.WallSoundEffectClip.fail);
	}

	/// <summary>
	/// Test to see if we have passed a consecutive target and make a message about it (true if we did got a combo, false if not)
	/// </summary>
	private bool TestHighScoreConsecCounter()
	{
		if(scoreConsecutiveCounter >= nextConsecutiveTarget)
		{
			// Fireworks 
			if (nextConsecutiveTarget <= 250)       // Simple
			{
				scorePopper.TriggerParticleSystem(new bool[] { true, false, false }, new float[] { 2, 0, 0 });
				wallSoundEffects.PlayClip(WallSoundEffects.WallSoundEffectClip.combo2);
			}
			else if (nextConsecutiveTarget <= 500)   // Med
			{
				scorePopper.TriggerParticleSystem(new bool[] { false, true, true }, new float[] { 0, 3, 3 });
				wallSoundEffects.PlayClip(WallSoundEffects.WallSoundEffectClip.combo3);
			}
			else                                    // Extreme 
			{
				scorePopper.TriggerParticleSystem(new bool[] { true, true, true }, new float[] { 5, 5, 5 });
				wallSoundEffects.PlayClip(WallSoundEffects.WallSoundEffectClip.combo5);
			}

			// Increase by a forth of the target, display, increase, then update to the new target 
			int increaseAmount = 100;
			scorePopper.PopScoreMessage(3,.1f);
			highScore.UpdateYourScore(increaseAmount);
			nextConsecutiveTarget *= 2;

			// Return true if we had a combo
			return true;
		}

		// Return false if we did not get a combo
		return false;
	}


	/// <summary>
	/// Enable/Disable the hand placement box 
	/// </summary>
	public void EnableDisableHands(bool enable)
	{
		for(int i = 0; i < handPlacement.Length; i++)
		{
			if (handPlacement[i] != null)
				handPlacement[i].SetActive(enable);
		}
	}

	public void GameStarted()
	{
		gameStarted = true;
	}
}
