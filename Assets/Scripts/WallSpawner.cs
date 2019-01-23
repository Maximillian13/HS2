// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class WallSpawner : MonoBehaviour 
{
    // The squat walls
    public WaveShooter waveShooter;

    // The script responsible for destroying walls
    public DestroyWall destroyWall;

	// Fields 
	private GameObject[] walls = new GameObject[3];
    private int rand; // Random int for deciding when to spawn multi-wall squat walls
    private int wallSpawnCount; // How many walls have spawned 
	private float timer; // Timer for how long between spawning walls
    private float transitionTimer; // For time in-between waves 
    private bool[] waveTransiton = new bool[3]; // Bool for if the transition period is up
	private bool[] waveSpawns = new bool[3]; // Bool to control when to spawn


	// For when the game ends
	private bool stopSpawning;
    private float resetGame;
	private bool loadingReset;

	// Stuff for custom games
	private bool warmUp;
	private bool[] wallTypesAllowed = new bool[3];
	private bool onBreak;
	private int pauseAfterXWaves;
	private int secondsToPauseFor;
	private float onBreakTimer;
	private bool breakMessagePopped;
	private bool breakEndingPopped;
	private int wallCountAfterWarmUp; // How many walls have spawned 

	// For calibrating the users height
	private HeightCalibrator hCal;

    // Special condition for the first wave
    private bool waveOneTrans;

    // Consts
    private const int NUMBER_OF_WALLS_WAVE_ONE = 10;
	private const int NUMBER_OF_WALLS_WAVE_TWO = 25;
	private const float HEIGHT_UNDER_GROUND = 3;

	private string CUSTOM_ROUTINE_PATH;

	// Set-up
	void Start()
	{
		CUSTOM_ROUTINE_PATH = Application.persistentDataPath + "/CustomRoutineData.txt";

		timer = 5;
		loadingReset = false;
		hCal = GameObject.Find("HeightCalibrator").GetComponent<HeightCalibrator>();
		walls[0] = Resources.Load<GameObject>("Walls/SquatWall");
		walls[1] = Resources.Load<GameObject>("Walls/SquatWallx2");
		walls[2] = Resources.Load<GameObject>("Walls/SquatWallx3");

		for (int i = 0; i < waveTransiton.Length; i++)
			waveTransiton[i] = true;

		onBreakTimer = float.PositiveInfinity;

		// If the file doesnt exist then load the normal defaults, else read from file 
		if (File.Exists(CUSTOM_ROUTINE_PATH) == false)
		{
			warmUp = true;
			wallTypesAllowed[0] = true;
			wallTypesAllowed[1] = false;
			wallTypesAllowed[2] = false;
			pauseAfterXWaves = int.MaxValue;
			secondsToPauseFor = 0;
		}
		else
		{
			this.SetCustomRoutineData();
		}
	}

	// Update is called once per frame
	void Update () 
    {
        // If the player hit the wall
        if(stopSpawning == true)
        {
			// If we are currently reloading, then stop here
			if (loadingReset == true)
				return;

            // Add up the time, once you get to 5 seconds reload the game
            resetGame += Time.deltaTime;
            if(resetGame >= 8)
            {
				GameObject.Find("[CameraRig]").transform.Find("Camera").GetComponent<EyeFadeControl>().CloseEyes(2);
				loadingReset = true;
            }
            return; // Return here so we dont spawn anymore walls
        }

		// Handle transitions between waves
		if (warmUp == true)
		{
			this.Transition(0, 0, 0, 0, hCal.GetCalibrationTime() + 2);
			this.Transition(NUMBER_OF_WALLS_WAVE_ONE, destroyWall.GetWallsDestroyed(), 1, 1, 15);
			this.Transition(NUMBER_OF_WALLS_WAVE_TWO, destroyWall.GetWallsDestroyed(), 2, 2, 15);
		}
		else
		{
			this.Transition(0, 0, 2, 3, hCal.GetCalibrationTime() + 2);
		}

		// Do all the logic in HandleObBreak and if its true, then we are on break and should stop here
		if (this.HandleOnBreak() == true)
			return;

		// Update the timer for when to spawn walls
		timer += Time.deltaTime;

		// If we are doing warm up
		if (warmUp == true)
		{
			if (waveSpawns[0] == true)
				this.SpawnWave(5, NUMBER_OF_WALLS_WAVE_ONE, 1.5f, wallTypesAllowed);
			if (waveSpawns[1] == true)
				this.SpawnWave(3.5f, NUMBER_OF_WALLS_WAVE_TWO, 2, wallTypesAllowed);
			if (waveSpawns[2] == true)
			{
				if (onBreak == false)
					this.SpawnWave(2, int.MaxValue, 3, wallTypesAllowed);
			}
		}
		else // If we are skipping warm up
		{
			if (waveSpawns[2] == true)
			{
				if(onBreak == false)
					this.SpawnWave(2, int.MaxValue, 3, wallTypesAllowed);
			}
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
				for(int i = 0; i < waveSpawns.Length; i++)
					waveSpawns[i] = false;
			}

			// Update timer
			transitionTimer += Time.deltaTime; 

			// Show the wave transition message if it has not been shown yet 
			if (transitionTimer >= tranTime && waveShooter.HasBeenShown(wavePopNum) == false)
				waveShooter.PopWave(wavePopNum);

            // If its been the amount of time specified above start the next wave
            if (transitionTimer >= tranTime + 5)
            {
				// Turn on current wave, reset and block from re-entering this wave transition
				waveSpawns[wtIndex] = true;
                waveTransiton[wtIndex] = false;
                transitionTimer = 0;
            }
        }
	}

	/// <summary>
	/// This will Spawn a wave of walls
	/// </summary>
	/// <param name="timeBetweenSpawns">Time in seconds between wall spawns</param>
	/// <param name="maxWallsPerWave">How many walls per wave</param>
	/// <param name="wallSpeed">How fast the walls move</param>
	/// <param name="allowedWalls">Array of what walls are allowed [0] = 1, [1] = 2, [2] = 3</param>
	private void SpawnWave(float timeBetweenSpawns, int maxWallsPerWave, float wallSpeed, bool[] allowedWalls)
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
				if ((rand == 7 || rand == 8) &&  allowedWalls[2] == true) // Make a 3 long multi-wall
				{
					// Spawn correct wall at correct position, update how many walls have spawned
					Vector3 spawnPos = new Vector3(this.transform.position.x - .025f, this.transform.position.y - HEIGHT_UNDER_GROUND, this.transform.position.z - 1.5f);
					wallClone = (GameObject)Instantiate(walls[2], spawnPos, walls[2].transform.rotation);
					wallCountToAdd = 3;
					break;
				}
				if ((rand == 4 | rand == 5 || rand == 6) && allowedWalls[1] == true) // Make a 2 long multi-wall 
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

			this.StartWallMovment(wallClone, wallSpeed); // Start the movement of the wall
			timer = 0; // Reset timer for next spawn
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
	/// Handle the break logic, if we return true we are on break still, else we are not
	/// </summary>
	private bool HandleOnBreak()
	{
		// Check to see if we are ready to go back in the game 
		if (Time.time >= onBreakTimer)
			onBreak = false;

		// 3 second before the waves start again, give a warning 
		if (Time.time >= onBreakTimer - 2.5f  && breakEndingPopped == false)
		{
			waveShooter.PopWave(3);
			breakEndingPopped = true;
		}

		// Pop up message telling we are on break 5 seconds after wave start (5 so we are standing and not currently squatting through a wall)
		if (Time.time >= onBreakTimer - secondsToPauseFor + 7 && breakMessagePopped == false)
		{
			waveShooter.PopWave(3);
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
			}
		}

		return false;
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

			// Set what walls are allowed
			string[] wallTokens = sr.ReadLine().Split(' ');
			for(int i = 0; i < wallTypesAllowed.Length; i++)
				wallTypesAllowed[i] = bool.Parse(wallTokens[i]);
		}
	}

    /// <summary>
    /// Will tell the game to stop spawning waves and reset the level
    /// </summary>
    public void EndGame()
    {
        stopSpawning = true;
    }
}
