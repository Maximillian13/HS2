using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Steamworks;


public class MainMenuControl : MonoBehaviour
{
	private GameObject[] randScenes;
	private int lastRandInd;

	public GameObject[] specailScenes;

	public GameObject[] buttonSet;
	public Transform wall;
	public Transform specailWall;
	public Transform collectableSpawnerParent;

	private CollectibleSpawner[] collSpawners;

	private bool moveDown;
	private bool moveUp;
	private bool speacialMoveDown;
	private bool specailMoveUp;
	private bool specailCase;
	private float bottomTimer;
	private string buttonSetToken;

	private float loadTimer;
	private int loadIndex;

	private float quitTimer;

	//private GymSelectButtonControl gymSelectControl;
	//private SongSelectButtonControl songSelectControl;
	private GymAndSongSelectControl gymAndSongSelectControl;
	private WorkOutSelectControl gameModeSelector;
	// The start and back buttons for the gym select (will change depending on 
	// if we are coming from main menu or if we are coming from Custom Routine) 
	private MainMenuButton gymSongStart;
	private MainMenuButton gymSongBack;

	private DailyChallengeMaster dailyChallengeControl;
	private CustomRutineButtonOptionsMaster customRutineControl;

	private string CUSTOM_DATA_PATH;

	// Start is called before the first frame update
	void Start()
	{
		// Where the Custom Routine data should be located 
		CUSTOM_DATA_PATH = Application.persistentDataPath + "/CustomRoutineData.txt";

		// Delete old info about the custom routine
		if (File.Exists(CUSTOM_DATA_PATH) == true)
			File.Delete(CUSTOM_DATA_PATH);

		// Set up for the collectibles
		collSpawners = new CollectibleSpawner[collectableSpawnerParent.childCount];
		for (int i = 0; i < collSpawners.Length; i++)
			collSpawners[i] = collectableSpawnerParent.GetChild(i).GetComponent<CollectibleSpawner>();

		// Set up for rand scenes 
		GameObject randScenes = this.transform.Find("Random").gameObject;
		this.randScenes = new GameObject[randScenes.transform.childCount];
		for (int i = 0; i < this.randScenes.Length; i++)
			this.randScenes[i] = randScenes.transform.GetChild(i).gameObject;
		for (int i = 0; i < this.randScenes.Length; i++)
			this.randScenes[i].SetActive(false);

		// Set up for Special scenes 
		for (int i = 0; i < this.specailScenes.Length; i++)
			this.specailScenes[i].SetActive(false);

		// Active all button sets so we can get their components 
		for (int i = 0; i < buttonSet.Length; i++)
			buttonSet[i].SetActive(true);

		// Get control objects
		gymAndSongSelectControl = buttonSet[1].transform.Find("GymSongSelect").GetComponent<GymAndSongSelectControl>();
		gymSongStart = buttonSet[1].transform.Find("Start").GetChild(0).GetComponent<MainMenuButton>();
		gymSongBack = buttonSet[1].transform.Find("Back").GetChild(0).GetComponent<MainMenuButton>();

		dailyChallengeControl = buttonSet[2].transform.GetChild(0).GetComponent<DailyChallengeMaster>();
		customRutineControl = buttonSet[3].transform.GetChild(0).GetComponent<CustomRutineButtonOptionsMaster>();
		gameModeSelector = buttonSet[4].transform.GetChild(0).GetComponent<WorkOutSelectControl>();

		// Set up to display the main menu (and deactivate all the others)
		this.ActivateButtonSet("MainMenu");

		// Make it so these dont activate for like 100 years or something
		loadTimer = float.PositiveInfinity;
		quitTimer = float.PositiveInfinity;
		bottomTimer = float.PositiveInfinity;

		// Make sure weight is set properly 
		if (PlayerPrefs.GetInt("PlayerWeight") == 0)
			PlayerPrefs.SetInt("PlayerWeight", 160);
	}

	void Update()
	{
		// Todo: For the love of god dont forget to delete this
		// Reset all achievements
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (SteamManager.Initialized == true)
			{
				SteamUserStats.ResetAllStats(true);
				SteamUserStats.StoreStats();
				Debug.Log("Achiv deleted");
			}
		}

		if (Input.GetKeyDown(KeyCode.Q))
			this.TransitionToMenu("Outfits");
		if (Input.GetKeyDown(KeyCode.W))
			this.TransitionToMenu("Stats");
		if(Input.GetKeyDown(KeyCode.D))
			this.TransitionToMenu("DailyChallenge");
		if (Input.GetKeyDown(KeyCode.F))
			this.TransitionToMenu("LoadLevel dc");
		if (Input.GetKeyDown(KeyCode.O))
			this.TransitionToMenu("Options");

		// If we need to move the walls up or down
		this.MoveWall();
		this.MoveWallSpecail();

		// Check to see if its time to close our eyes and load or quit a the level
		this.TransitionTimer(loadIndex, ref loadTimer);
		this.TransitionTimer(-1, ref quitTimer);
	}

	/// <summary>
	/// Check if the timer has ran out and if it has, close eyes and load a level or quit the game
	/// </summary>
	/// <param name="loadInd">Level to load or -1 to quit</param>
	/// <param name="timer">What timer we are paying attention to</param>
	private void TransitionTimer(int loadInd, ref float timer)
	{
		// If we are ready to load a level, close eyes and load level
		if (Time.time > timer)
		{
			GameObject.Find("[CameraRig]").transform.Find("Camera").GetComponent<EyeFadeControl>().CloseEyes(loadIndex, true);
			timer = float.PositiveInfinity;
		}
	}

	/// <summary>
	/// Move the special wall up and down
	/// </summary>
	private void MoveWallSpecail()
	{
		// Move the special wall down
		if (speacialMoveDown == true)
		{
			specailWall.position = new Vector3(specailWall.position.x, specailWall.position.y - (15 * Time.deltaTime), specailWall.position.z);
			if (specailWall.position.y <= -0.749f)
			{
				specailWall.position = new Vector3(specailWall.position.x, -0.749f, specailWall.position.z);
				speacialMoveDown = false;
				if (specailCase == true)
					specailMoveUp = true;
			}
		}

		// Move the wall up
		if (specailMoveUp == true)
		{
			specailWall.position = new Vector3(specailWall.position.x, specailWall.position.y + (15 * Time.deltaTime), specailWall.position.z);
			if (specailWall.position.y >= .6f)
			{
				specailWall.position = new Vector3(specailWall.position.x, .6f, specailWall.position.z);
				specailMoveUp = false;
			}
		}
	}

	/// <summary>
	/// Move the normal wall up and down
	/// </summary>
	private void MoveWall()
	{
		// Move the wall down
		if (moveDown == true)
		{
			wall.position = new Vector3(wall.position.x, wall.position.y - (15 * Time.deltaTime), wall.position.z);
			if (wall.position.y <= -1)
			{
				wall.position = new Vector3(wall.position.x, -1, wall.position.z);
				this.ActivateButtonSet(buttonSetToken); // What button set to activate
				moveDown = false;
				bottomTimer = Time.time + .5f; // Wall stays down for .5 seconds
				if (specailCase == false)
					moveUp = true;
			}
		}

		// Once we are done waiting at  the bottom
		if (Time.time > bottomTimer)
		{
			// Move the wall up
			if (moveUp == true)
			{
				wall.position = new Vector3(wall.position.x, wall.position.y + (15 * Time.deltaTime), wall.position.z);
				if (wall.position.y >= 1)
				{
					wall.position = new Vector3(wall.position.x, 1, wall.position.z);
					moveUp = false;
					bottomTimer = float.PositiveInfinity;
				}
			}
		}
	}

	/// <summary>
	/// Move the walls up and down and show the correct button set
	/// </summary>
	public void TransitionToMenu(string token)
	{
		moveDown = true;			// Start the move of the wall
		buttonSetToken = token;     // Move the new buttons in (Once the wall is at the bottom

		if (token == "Outfits")
		{
			// Load all the collectibles
			for (int i = 0; i < collSpawners.Length; i++)
				collSpawners[i].SpawnCollectible();

			// Pick special outfit scene 
			this.PickScene(0);      
			specailMoveUp = true;
			specailCase = true;
		}
		else if (token == "Stats")
		{
			// Pick special outfit scene 
			this.PickScene(1);
			specailMoveUp = true;
			specailCase = true;
		}
		else
		{
			// If it was previously a special case dont randomize yet
			if (specailCase == false) 
				this.RandomizeScene();   

			// Not a special case anymore 
			speacialMoveDown = true;
			specailCase = false;
		}
	}

	/// <summary>
	/// What button set should be shown
	/// </summary>
	private void ActivateButtonSet(string setToActivate)
	{
		// Disable all button sets 
		for(int i = 0; i < buttonSet.Length; i++)
			buttonSet[i].SetActive(false);

		// Activate the correct button set
		// Goes to the main menu and deletes and custom data
		if (setToActivate == "MainMenu")
		{
			// If we are going back to the menu, we should delete the custom routine data
			if (File.Exists(CUSTOM_DATA_PATH) == true)
				File.Delete(CUSTOM_DATA_PATH);

			// Destroy the collectibles
			GameObject[] cObjs = GameObject.FindGameObjectsWithTag("Collectible");
			for (int i = 0; i < cObjs.Length; i++)
				cObjs[i].GetComponent<ShrinkAndDestroy>().ShrinkDestroy();

			// Load in the correct buttons
			buttonSet[0].SetActive(true);
		}

		// If we are loading the workout type then set up the continue to be whatever we want to do next
		if(setToActivate.Contains("GameMode"))
		{
			string nextToLoad = setToActivate.Split(' ')[1];
			gameModeSelector.SetContinueButtonToken(nextToLoad);
			buttonSet[4].SetActive(true);
		}

		// Go to the classic mode menu and set the buttons to take you to the game, or back to the main menu
		if (setToActivate == "ClassicMode")
		{
			buttonSet[1].SetActive(true);
			gymSongStart.SetButtonToken("LoadLevel cm");
			gymSongBack.SetButtonToken("GameMode ClassicMode");
		}

		if (setToActivate == "ArcadeMode")
		{
			buttonSet[1].SetActive(true);
			// Make it squat mode 
			PlayerPrefs.SetInt(Constants.cardioMode, 0);
			gymSongStart.SetButtonToken("LoadLevel am");
			gymSongBack.SetButtonToken("MainMenu");
		}

		if (setToActivate == "LevelSelect")
			buttonSet[1].SetActive(true);

		// Just loads daily challenge stuff
		if (setToActivate == "DailyChallenge")
		{
			dailyChallengeControl.FillDescription();
			buttonSet[2].SetActive(true);
		}

		// Go to the custom routine menu and set the buttons to take you to the gym select menu, or back to the custom routine
		if (setToActivate == "CustomRoutine")
		{
			buttonSet[3].SetActive(true);
			gymSongStart.SetButtonToken("LoadLevel cr");
			gymSongBack.SetButtonToken("CustomRoutine");
		}

		// Just loads the option stuff
		if (setToActivate == "Options")
		{
			buttonSet[5].SetActive(true);
		}

		// Load the buttons for the wall and activate whatever this load type is
		if (setToActivate.Contains("LoadLevel"))
		{
			buttonSet[6].SetActive(true);
			// Load the right level and info with the info token given after the load "LoadLevel" keyword
			this.LoadLevel(setToActivate.Split(' ')[1]);
		}

		// Load the buttons for the wall and activate whatever this load type is
		if (setToActivate == "Quit")
		{
			buttonSet[7].SetActive(true);
			quitTimer = Time.time + 2;
		}
	}

	/// <summary>
	/// Loads the specified level with the given token
	/// </summary>
	private void LoadLevel(string loadType)
	{
		// Classic Mode
		if(loadType.ToLower() == "cm")
		{
			loadIndex = 2;
			loadTimer = Time.time + 2;
			PlayerPrefs.SetInt(Constants.gameMode, Constants.gameModeClassic);

			// Make the song file based off buttons
			gymAndSongSelectControl.MakeSongFile(); 
			Debug.Log(gymAndSongSelectControl.GetSelectedGym());
		}

		// Daily Challenge 
		if (loadType.ToLower() == "dc")
		{
			loadIndex = 2;
			loadTimer = Time.time + 2;
			PlayerPrefs.SetInt(Constants.gameMode, Constants.gameModeDaily);

			// Make the song file based off the daily challenge
			gymAndSongSelectControl.MakeSongFile(dailyChallengeControl.GetDailyChallengeSongs());

			// Save the daily challenge info
			string[] dailyC = dailyChallengeControl.GetDailyChallengeSummary();
			this.WriteDataToFile(dailyC, CUSTOM_DATA_PATH);

			// Save that we did a daily challenge 
			dailyChallengeControl.IncrementDailyChallengeStat();
		}

		// Custom Routine
		if (loadType.ToLower() == "cr")
		{
			loadIndex = 2;
			loadTimer = Time.time + 2;
			PlayerPrefs.SetInt(Constants.gameMode, Constants.gameModeCustom);

			// Make the song file based off buttons
			gymAndSongSelectControl.MakeSongFile();

			// Fill out the data for the custom routine
			string[] custR = customRutineControl.GetCustomRutineSummary();
			this.WriteDataToFile(custR, CUSTOM_DATA_PATH);
		}

		// Arcade Mode
		if (loadType.ToLower() == "am")
		{
			loadIndex = 2;
			loadTimer = Time.time + 2;
			PlayerPrefs.SetInt(Constants.gameMode, Constants.gameModeArcade);

			// Make the song file based off buttons
			gymAndSongSelectControl.MakeSongFile();
		}


	}

	/// <summary>
	/// Randomize the scene that is behind the main menu 
	/// </summary>
	private void RandomizeScene()
	{
		// Get a new random index thats not the same as the last one 
		int randInd = -1;
		do
		{
			randInd = Random.Range(0, randScenes.Length);
		} while (randInd == lastRandInd);

		// Disable old scene
		this.DisableAllScenes();

		// Set what this scene is as the last scene for the next time 
		lastRandInd = randInd;

		// Activate the new one
		randScenes[randInd].SetActive(true);
	}

	/// <summary>
	/// Select the special scene you want to show up 
	/// </summary>
	private void PickScene(int index)
	{
		// Disable old scene
		this.DisableAllScenes();

		// Activate the new one
		specailScenes[index].SetActive(true);
	}

	/// <summary>
	/// Disables all random and special scenes 
	/// </summary>
	private void DisableAllScenes()
	{
		for (int i = 0; i < this.randScenes.Length; i++)
			this.randScenes[i].SetActive(false);

		for (int i = 0; i < this.specailScenes.Length; i++)
			this.specailScenes[i].SetActive(false);
	}

	private void WriteDataToFile(string[] data, string path)
	{
		// If the file has not been made
		if (File.Exists(path) == false)
		{
			StreamWriter sw = File.CreateText(path);
			sw.Close();
		}

		// Write all data on new lines 
		using (StreamWriter sw = new StreamWriter(path))
		{
			for(int i = 0; i < data.Length; i++)
				sw.WriteLine(data[i]);
		}
	}

	// Closes all sockets and kills all threads (This prevents unity from freezing)
	private void OnApplicationQuit()
	{
		if (SteamManager.Initialized == true)
		{
			SteamAPI.RunCallbacks();
			SteamAPI.Shutdown();
		}
	}
	//private void OnDestroy()
	//{
	//	if (SteamManager.Initialized == true)
	//	{
	//		SteamAPI.RunCallbacks();
	//		SteamAPI.Shutdown();
	//	}
	//}

}
