﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameModeMaster : MonoBehaviour
{
	public WallSpawner wallSpawner;
	public WallSpawnerCardio wallSpawnerCardio;
	public GameObject[] objsToDestroy;
	public PlayerHitBox playerHitBox;

	public PauseMenu[] pauseMenues;
	public GameObject[] gyms;

	private bool preventGameModeSwitch;
	private int livesLeft;
	private IEnumerator liveCorutine;

	private WaveShooter waveShooter;
	private DestroyWall destroyWall;
	private CalorieCounter calorieCounter;

	private float wallSpeedMultiplier = 1;

	private bool haveHandGuard = true;


	#region private fields
	private int wallSpawnCount; // How many walls have spawned 
	private bool[] waveTransiton = new bool[3]; // Bool for if the transition period is up
	private bool[] waveSpawns = new bool[3]; // Bool to control when to spawn

	// For when the game ends
	private bool stopSpawning;
	private float resetGame;
	private bool loadingReset;

	// Stuff for custom games
	private bool warmUp;
	private bool[] wallTypesAllowed = new bool[3];
	private bool[] cardioWallTypesAllowed = new bool[3];
	private bool onBreak;
	private int pauseAfterXWaves;
	private int secondsToPauseFor;
	private float onBreakTimer;
	private bool breakMessagePopped;
	private bool breakEndingPopped;
	private int wallCountAfterWarmUp; // How many walls have spawned
	private bool cardioMode;
	private bool switchModesOnBreak;

	// Consts
	private int NUMBER_OF_WALLS_WAVE_ONE = 10;
	private int NUMBER_OF_WALLS_WAVE_TWO = 25;
	private float HEIGHT_UNDER_GROUND = 3;

	// For calibrating the users height
	private HeightCalibrator hCal;

	private float timer; // Timer for how long between spawning walls
	private float transitionTimer; // For time in-between waves 
	private int rand; // Random int for deciding when to spawn multi-wall squat walls
	private int lastRand = -1;

	private bool preventLevelFromLoading;

	private string CUSTOM_ROUTINE_PATH;
	#endregion

	private bool customRoutineStatCheck;

	// Start is called before the first frame update
	void Start()
    {
		// Disable all gyms 
		for (int i = 0; i < gyms.Length; i++)
			gyms[i].SetActive(false);

		// Re - enable the one we want(or random if rand selected)
		int gymToEnable = PlayerPrefs.GetInt("GymInd");

		// If random is selected or error just pick randomly 
		if (gymToEnable >= gyms.Length || gymToEnable < 0)
			gymToEnable = Random.Range(0, gyms.Length);
		gyms[gymToEnable].SetActive(true); // Enable that gym

		// rename it so paths still work
		gyms[gymToEnable].name = "GYM";

		// Get everything
		CUSTOM_ROUTINE_PATH = Application.persistentDataPath + "/CustomRoutineData.txt";
		hCal = GameObject.Find("HeightCalibrator").GetComponent<HeightCalibrator>();
		waveShooter = GameObject.Find("WaveShooter").GetComponent<WaveShooter>();
		destroyWall = GameObject.Find("WallDestroyer").GetComponent<DestroyWall>();
		GameObject ccgo = GameObject.Find("CalorieCounterOrig");
		if (ccgo != null)
			calorieCounter = ccgo.GetComponent<CalorieCounter>();

		loadingReset = false;

		for (int i = 0; i < waveTransiton.Length; i++)
			waveTransiton[i] = true;

		timer = 0;
		livesLeft = 0;
		onBreakTimer = float.PositiveInfinity;

		// If the file doesnt exist then load the normal defaults, else read from file 
		if (File.Exists(CUSTOM_ROUTINE_PATH) == false)
		{
			// Enable all walls by default
			for (int i = 0; i < 3; i++)
			{
				wallTypesAllowed[i] = true;
				cardioWallTypesAllowed[i] = true;
			}

			if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeClassic)	// Clasic Mode
			{
				warmUp = true;
				pauseAfterXWaves = int.MaxValue;
				secondsToPauseFor = 0;
				switchModesOnBreak = false;
			}
			else																		// Arcade Mode
			{
				warmUp = false;
				//livesLeft = 3;
				pauseAfterXWaves = 50;
				secondsToPauseFor = 15;
				cardioMode = false;
				// If we are on cardio mode we are acatually on cardio/squat mode 
				if (PlayerPrefs.GetInt(Constants.cardioMode) == 1)
					switchModesOnBreak = true;
				else
					switchModesOnBreak = false;
			}
		}
		else
		{
			this.SetCustomRoutineData();

			// If we are in custom mode, check to update the stat
			if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeCustom)
			{
				customRoutineStatCheck = true;
				// Set the arcade pause menus
				for (int i = 0; i < pauseMenues.Length; i++)
					pauseMenues[i].SetPauseButton(Instantiate<GameObject>(Resources.Load<GameObject>("Other/PauseMenuCustom")));
			}
		}

		this.ActivateGameMode();
	}

	/// <summary>
	/// Switches game mode
	/// </summary>
	public void SwitchGameMode()
	{
		if(cardioMode == true)
		{
			wallSpawnerCardio.gameObject.SetActive(false);
			wallSpawner.gameObject.SetActive(true);
			this.EnableDisableHandGaurdStuff(true);
		}
		else
		{
			wallSpawnerCardio.gameObject.SetActive(true);
			wallSpawner.gameObject.SetActive(false);
			this.EnableDisableHandGaurdStuff(false);
		}

		// Switch modes 
		cardioMode = !cardioMode;
		playerHitBox.SetGameMode(cardioMode);

	}

	/// <summary>
	/// Checks if we are not spawning walls anymore. If true is returned then we want to quit out 
	/// of the method that is calling this (Update()) if false then we can continue on
	/// </summary>
	public bool CheckStopSpawning()
	{
		// If the player hit the wall
		if (stopSpawning == true)
		{
			// If we are currently reloading, then stop here
			if (loadingReset == true)
				return true;

			if (preventLevelFromLoading == true)
				return true;

			// Add up the time, once you get to 5 seconds reload the game
			resetGame += Time.deltaTime;
			if (resetGame >= 6)
			{
				GameObject.Find("[CameraRig]").transform.Find("Camera").GetComponent<EyeFadeControl>().CloseEyes(2);
				loadingReset = true;
			}
			return true; // Return here so we dont spawn anymore walls
		}
		return false; // If we are here we dont need to return out of the update that this is getting called by
	}

	public void TransitionHandler()
	{
		// Handle transitions between waves
		if (warmUp == true)
		{
			this.Transition(0, 0, 0, 0, hCal.GetCalibrationTime() + 2);
			this.Transition(NUMBER_OF_WALLS_WAVE_ONE, destroyWall.GetWallsDestroyed(), 1, 1, 10);
			this.Transition(NUMBER_OF_WALLS_WAVE_TWO, destroyWall.GetWallsDestroyed(), 2, 2, 10);
		}
		else
		{
			this.Transition(0, 0, 2, 3, hCal.GetCalibrationTime() + 2);
		}
	}

	public void SpawnWavesHandlerSquat(GameObject[] walls)
	{
		// Update the timer for when to spawn walls
		timer += Time.deltaTime;

		// If we are doing warm up
		if (warmUp == true)
		{
			if (waveSpawns[0] == true)
				this.SpawnWaveSquat(5, NUMBER_OF_WALLS_WAVE_ONE, 2f, wallTypesAllowed, walls);
			if (waveSpawns[1] == true)
				this.SpawnWaveSquat(3.5f, NUMBER_OF_WALLS_WAVE_TWO, 2.5f, wallTypesAllowed, walls);
			if (waveSpawns[2] == true)
			{
				if (onBreak == false)
					this.SpawnWaveSquat(2, int.MaxValue, 3f, wallTypesAllowed, walls);
			}
		}
		else // If we are skipping warm up
		{
			if (waveSpawns[2] == true)
			{
				if (onBreak == false)
					this.SpawnWaveSquat(2, int.MaxValue, 3f, wallTypesAllowed, walls);
			}
		}
	}

	public void SpawnWavesHandlerCardio(GameObject[] walls)
	{
		// Update the timer for when to spawn walls
		timer += Time.deltaTime;

		// If we are doing warm up
		if (warmUp == true)
		{
			if (waveSpawns[0] == true)
				this.SpawnWaveCardio(1.75f, NUMBER_OF_WALLS_WAVE_ONE, 2.75f, cardioWallTypesAllowed, walls);
			if (waveSpawns[1] == true)
				this.SpawnWaveCardio(1.75f, NUMBER_OF_WALLS_WAVE_TWO, 3, cardioWallTypesAllowed, walls);
			if (waveSpawns[2] == true)
			{
				if (onBreak == false)
					this.SpawnWaveCardio(1.25f, int.MaxValue, 3.5f, cardioWallTypesAllowed, walls);
			}
		}
		else // If we are skipping warm up
		{
			if (waveSpawns[2] == true)
			{
				if (onBreak == false)
					this.SpawnWaveCardio(1.25f, int.MaxValue, 3.5f, cardioWallTypesAllowed, walls);
			}
		}
	}

	private void SpawnWaveCardio(float timeBetweenSpawns, int maxWallsPerWave, float wallSpeed, bool[] allowedWalls, GameObject[] walls)
	{
		// If it has been long enough to spawn a new wave
		if (timer >= timeBetweenSpawns)
		{
			// Get the allowed wall count
			int tCount = 0;
			for (int i = 0; i < allowedWalls.Length; i++)
				if (allowedWalls[i] == true)
					tCount++;

			// Pick a new rand
			int rand = Random.Range(0, walls.Length);

			// If the allowed walls is more than 2 to save from getting stuck in the loop
			if (tCount > 1)
			{
				while (rand == lastRand || allowedWalls[rand] == false)
					rand = Random.Range(0, 3);
				lastRand = rand;
			}

			// Make and move wall
			// Spawn correct wall at correct position, update how many walls have spawned
			Vector3 spawnPos = new Vector3(this.transform.position.x - .025f, this.transform.position.y - HEIGHT_UNDER_GROUND, this.transform.position.z - 2.4f);
			GameObject wallClone = (GameObject)Instantiate(walls[rand], spawnPos, walls[rand].transform.rotation);

			// Add to respective counts
			wallSpawnCount++;
			if (waveSpawns[2] == true)
				wallCountAfterWarmUp++;

			this.StartWallMovment(wallClone, wallSpeed * wallSpeedMultiplier); // Start the movement of the wall
			timer = 0; // Reset timer for next spawn

			// Update the custom routine stat if we are in that mode and 10 walls have spawned (To avoid boosting) 
			if(customRoutineStatCheck == true)
			{
				if (wallSpawnCount >= 10)
				{
					AchivmentAndStatControl.IncrementStat(Constants.totalCustomRoutines);
					customRoutineStatCheck = false;
				}
			}
		}
	}

	/// <summary>
	/// Figure out what game mode we are playing and activate/deactivate whatever is needed
	/// </summary>
	private void ActivateGameMode()
	{
		// Check the game mode using player prefs
		cardioMode = PlayerPrefs.GetInt(Constants.cardioMode) == 1 ? true : false;

		// We take care of this in the arcade section
		if (PlayerPrefs.GetInt(Constants.gameMode) == Constants.gameModeArcade)
			cardioMode = false;

		if (cardioMode == true)
		{
			wallSpawner.gameObject.SetActive(false);
			this.EnableDisableHandGaurdStuff(false);
			playerHitBox.EnableDisableHands(false);
		}
		else
			wallSpawnerCardio.gameObject.SetActive(false);

		playerHitBox.SetGameMode(cardioMode);
	}


	/// <summary>
	/// Fill out all the custom routine information reading from the file created in the main menu 
	/// </summary>
	private void SetCustomRoutineData()
	{
		using (StreamReader sr = new StreamReader(CUSTOM_ROUTINE_PATH))
		{
			// Set warm up
			warmUp = bool.Parse(sr.ReadLine());

			// Set break info
			string[] breakTokens = sr.ReadLine().Split(' ');
			secondsToPauseFor = int.Parse(breakTokens[0]);
			pauseAfterXWaves = int.Parse(breakTokens[1]);

			// Get the hand guard setting 
			haveHandGuard = bool.Parse(sr.ReadLine());
			if (PlayerPrefs.GetInt(Constants.cardioMode) == 0)
				this.EnableDisableHandGaurdStuff(false);
			else
				this.EnableDisableHandGaurdStuff(true);

			// Set what walls are allowed
			string[] wallTokens = sr.ReadLine().Split(' ');
			for (int i = 0; i < wallTypesAllowed.Length; i++)
				wallTypesAllowed[i] = bool.Parse(wallTokens[i]);

			// Set what cardio walls are allowed
			wallTokens = sr.ReadLine().Split(' ');
			for (int i = 0; i < cardioWallTypesAllowed.Length; i++)
				cardioWallTypesAllowed[i] = bool.Parse(wallTokens[i]);

			// Set if we should switch game modes when going on break
			switchModesOnBreak = bool.Parse(sr.ReadLine());

			// Get lives 
			int liveCount = int.Parse(sr.ReadLine());
			// Make it endless
			if (liveCount == 6)
				livesLeft = int.MaxValue;
			else
				livesLeft = liveCount; 

			// Get the speed mult
			wallSpeedMultiplier = float.Parse(sr.ReadLine());
		}
	}


	/// <summary>
	/// This will handle the transition between waves
	/// </summary>
	/// <param name="numOfWalls">Number of walls per wave</param>
	/// <param name="destroyedWalls">How many walls have been destroyed</param>
	/// <param name="wtIndex">The index of the wave bool you want to change</param>
	/// <param name="wavePopNum">The index of the wave sign you want to show</param>
	/// <param name="tranTime">Length of transition time in seconds</param>
	private void Transition(int numOfWalls, int destroyedWalls, int wtIndex, int wavePopNum, float tranTime)
	{
		// If the amount is spawned = total amount to spawn and all the walls in that wave are destroyed
		if (waveTransiton[wtIndex] == true && wallSpawnCount >= numOfWalls && wallSpawnCount >= destroyedWalls)
		{
			// First time through
			if (transitionTimer == 0)
			{
				// Turn off all wave spawning
				for (int i = 0; i < waveSpawns.Length; i++)
					waveSpawns[i] = false;
			}

			// Update timer
			transitionTimer += Time.deltaTime;

			// Show the wave transition message if it has not been shown yet 
			if (transitionTimer >= tranTime && waveShooter.HasBeenShown(wavePopNum) == false && stopSpawning == false)
				waveShooter.PopMessage(wavePopNum);

			// If its been the amount of time specified above start the next wave
			if (transitionTimer >= tranTime + 5)
			{
				// Turn on current wave, reset and block from re-entering this wave transition
				waveSpawns[wtIndex] = true;
				waveTransiton[wtIndex] = false;
				transitionTimer = 0;

				// Check to see if we are using the calorie counter
				if (calorieCounter != null)
					calorieCounter.SetPrevTime(Time.time);
			}
		}
	}

	/// <summary>
	/// Handle the break logic, if we return true we are on break still, else we are not
	/// </summary>
	public bool HandleOnBreak()
	{
		// Check to see if we are ready to go back in the game 
		if (Time.time >= onBreakTimer + 7)
		{
			onBreak = false;
			// Only enable when in squat mode and hand gaurd is off 
			playerHitBox.EnableDisableHands(cardioMode == false && haveHandGuard == true);

			// Check to see if we are using the calorie counter
			if (calorieCounter != null)
				calorieCounter.SetPrevTime(Time.time);
		}

		// 3 second before the waves start again, give a warning (+7 for the delay when poping originally)
		if (Time.time >= onBreakTimer - 2.5f + 7 && breakEndingPopped == false)
		{
			// Switch the game modes if that is something we want
			if (switchModesOnBreak == true && preventGameModeSwitch == false)
				this.SwitchGameMode();
			preventGameModeSwitch = false;

			if(stopSpawning == false) // Check if the game is still going 
				waveShooter.PopBreakMessage(false);
			breakEndingPopped = true;

		}

		// Pop up message telling we are on break 7 seconds after wave start (7 so we are standing and not currently squatting through a wall)
		if (Time.time >= onBreakTimer - secondsToPauseFor + 7 && breakMessagePopped == false)
		{
			if (stopSpawning == false) // Check if the game is still going 
				waveShooter.PopBreakMessage(true);
			breakMessagePopped = true;
		}

		// If we are currently on break exit here 
		if (onBreak == true)
			return true;

		// Check for if we want to go on break because of how many walls we have been through 
		if (waveSpawns[2] == true)
		{
			// If we are at the number we start the pause for
			if (wallCountAfterWarmUp >= pauseAfterXWaves)
			{
				onBreak = true;
				onBreakTimer = Time.time + secondsToPauseFor;
				breakMessagePopped = false;
				breakEndingPopped = false;
				wallCountAfterWarmUp = 0;
				playerHitBox.EnableDisableHands(false);
			}
		}

		return false;
	}

	/// <summary>
	/// This will Spawn a wave of walls
	/// </summary>
	/// <param name="timeBetweenSpawns">Time in seconds between wall spawns</param>
	/// <param name="maxWallsPerWave">How many walls per wave</param>
	/// <param name="wallSpeed">How fast the walls move</param>
	/// <param name="allowedWalls">Array of what walls are allowed [0] = 1, [1] = 2, [2] = 3</param>
	private void SpawnWaveSquat(float timeBetweenSpawns, int maxWallsPerWave, float wallSpeed, bool[] allowedWalls, GameObject[] walls)
	{
		// If it has been long enough to spawn a new wave
		if (timer >= timeBetweenSpawns)
		{
			// Make and move wall
			GameObject wallClone = null;
			int wallCountToAdd = 0;
			while (wallClone == null)
			{
				// Set a random number to deiced if to make a multi wall or not
				rand = Random.Range(0, 9);
				if ((rand == 7 || rand == 8) && allowedWalls[2] == true) // Make a 3 long multi-wall
				{
					// Spawn correct wall at correct position, update how many walls have spawned
					Vector3 spawnPos = new Vector3(this.transform.position.x - .025f, this.transform.position.y - HEIGHT_UNDER_GROUND, this.transform.position.z - 1.5f);
					wallClone = (GameObject)Instantiate(walls[2], spawnPos, walls[2].transform.rotation);
					wallCountToAdd = 3;
					break;
				}
				if ((rand == 4 || rand == 5 || rand == 6) && allowedWalls[1] == true) // Make a 2 long multi-wall 
				{
					Vector3 spawnPos = new Vector3(this.transform.position.x - .025f, this.transform.position.y - HEIGHT_UNDER_GROUND, this.transform.position.z - 1.95f);
					wallClone = (GameObject)Instantiate(walls[1], spawnPos, walls[1].transform.rotation);
					wallCountToAdd = 2;
					break;
				}
				if (allowedWalls[0] == true)// Make a normal wall
				{
					Vector3 spawnPos = new Vector3(this.transform.position.x - .025f, this.transform.position.y - HEIGHT_UNDER_GROUND, this.transform.position.z - 2.4f);
					wallClone = (GameObject)Instantiate(walls[0], spawnPos, walls[0].transform.rotation);
					wallCountToAdd = 1;
					break;
				}
			}

			// Add to respective counts
			wallSpawnCount += wallCountToAdd;
			if (waveSpawns[2] == true)
				wallCountAfterWarmUp += wallCountToAdd;

			this.StartWallMovment(wallClone, wallSpeed * wallSpeedMultiplier); // Start the movement of the wall
			timer = 0; // Reset timer for next spawn

			// Update the custom routine stat if we are in that mode and 10 walls have spawned (To avoid boosting) 
			if (customRoutineStatCheck == true)
			{
				if (wallSpawnCount >= 10)
				{
					AchivmentAndStatControl.IncrementStat(Constants.totalCustomRoutines);
					customRoutineStatCheck = false;
				}
			}
		}
	}

	/// <summary>
	/// This will start the movement of any given Squat-Wall
	/// </summary>
	/// <param name="wallClone">Squat-Wall to start moving</param>
	/// <param name="speed">The speed that you want to set for your wall to move at</param>
	private void StartWallMovment(GameObject wallClone, float speed)
	{
		MoveWall moveWall = wallClone.GetComponent<MoveWall>();
		moveWall.Speed(speed);
		moveWall.SetGibReady();
	}


	/// <summary>
	/// Called when the player passes through a wall
	/// </summary>
	public void PassedThroughWall(bool cardio)
	{
		float calOffset = 0;
		if (warmUp == true)
			calOffset = -.002f;

		// Check to see if we are using the calorie counter
		if (calorieCounter != null)
			calorieCounter.UpdateCount(Time.time, cardio, calOffset);
	}

	/// <summary>
	/// Go on break for passed in time 
	/// </summary>
	public void GoOnBreakImdate(float breakLength)
	{
		onBreak = true;
		onBreakTimer = Time.time + breakLength;
		breakEndingPopped = false;
		breakMessagePopped = true;
		preventGameModeSwitch = true;
		playerHitBox.EnableDisableHands(false);
	}

	/// <summary>
	/// Return the amount of times left we can fail
	/// </summary>
	public int GetAmountOfLivesLeft()
	{
		return this.livesLeft;
	}

	/// <summary>
	/// Decrement the amount of fails we have left by 1
	/// </summary>
	public void DecrementAmountOfLives()
	{
		this.livesLeft--;
	}

	/// <summary>
	/// Pop how many lives left (handed value) after handed wait time (-1 for infinity)
	/// </summary>
	public void PopLivesLeft(float waitTime, int livesLeft)
	{
		liveCorutine = this.WaitAndPop(waitTime, livesLeft);
		this.StartCoroutine(liveCorutine);
	}

	private IEnumerator WaitAndPop(float waitTime, int livesLeft)
	{
		while(true)
		{
			yield return new WaitForSeconds(waitTime);
			if(stopSpawning == false) // Check if the game is still going 
				waveShooter.PopLives(livesLeft);
			this.StopCoroutine(liveCorutine);		
		}
	}


	/// <summary>
	/// Will tell the game to stop spawning waves and reset the level
	/// </summary>
	public void EndGame()
	{
		stopSpawning = true;
		// Hide all warning stuff
		this.EnableDisableHandGaurdStuff(false);
	}

	private void EnableDisableHandGaurdStuff(bool enable)
	{
		for (int i = 0; i < objsToDestroy.Length; i++)
			objsToDestroy[i].gameObject.SetActive(enable);
	}

	/// <summary>
	/// Returns if we are currently on break
	/// </summary>
	public bool GetOnBreak()
	{
		return onBreak;
	}

	/// <summary>
	/// Gets if the hand guard should be enabled 
	/// </summary>
	public bool GetHandGuard()
	{
		return this.haveHandGuard;
	}

	/// <summary>
	/// Prevents game from loading a new level
	/// </summary>
	public bool PreventLevelFromLoading
	{
		get { return this.preventLevelFromLoading; }
		set { this.preventLevelFromLoading = value; }
	}
}
