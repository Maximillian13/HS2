using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Steamworks;


public class MainMenuControl : MonoBehaviour
{
	public GameObject[] buttonSet;
	public Transform wall;
	public Transform specailWall;
	public Transform collectableSpawnerParent;

	private string buttonSetToken;

	private bool moveBack;
	private bool moveForward;
	private float buttonTimer;

	private Vector3 forwordPos;
	private Vector3 backPos;


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

	private MainMenuLevelLoader levelLoader;

	private int levelToLoad;
	private bool doingDailyChallenge;

	private string CUSTOM_DATA_PATH;

	// Start is called before the first frame update
	void Start()
	{
		// Where the Custom Routine data should be located 
		CUSTOM_DATA_PATH = Application.persistentDataPath + "/CustomRoutineData.txt";

		// If we have the main game music destroy it 
		GameObject oldMusic = GameObject.Find("KeepMusic");
		if (oldMusic != null)
			Destroy(oldMusic.gameObject);

		// Delete old info about the custom routine
		if (File.Exists(CUSTOM_DATA_PATH) == true)
			File.Delete(CUSTOM_DATA_PATH);

		// The position that the buttons will go to when transitioning
		forwordPos = new Vector3(-.3f, 1.55f, .861f);
		backPos = new Vector3(-.3f, 1.55f, 1.2f);
		buttonTimer = float.PositiveInfinity;

		// Active all button sets so we can get their components 
		for (int i = 0; i < buttonSet.Length; i++)
		{
			if(buttonSet[i] != null)
				buttonSet[i].SetActive(true);
		}


		// Get control objects
		gymAndSongSelectControl = buttonSet[1].transform.Find("GymSongSelect").GetComponent<GymAndSongSelectControl>();
		gymSongStart = buttonSet[1].transform.Find("Start").GetChild(0).GetComponent<MainMenuButton>();
		gymSongBack = buttonSet[1].transform.Find("Back").GetChild(0).GetComponent<MainMenuButton>();

		dailyChallengeControl = buttonSet[2].transform.GetChild(0).GetComponent<DailyChallengeMaster>();
		customRutineControl = buttonSet[3].transform.GetChild(0).GetComponent<CustomRutineButtonOptionsMaster>();
		gameModeSelector = buttonSet[4].transform.GetChild(0).GetComponent<WorkOutSelectControl>();

		levelLoader = GameObject.Find("MainMenuLevelLoader").GetComponent<MainMenuLevelLoader>();
		levelToLoad = -1;

		// Set up to display the main menu (and deactivate all the others)
		this.ActivateButtonSet("MainMenu");

		// Make sure weight is set properly 
		if (PlayerPrefs.GetInt("PlayerWeight") == 0)
			PlayerPrefs.SetInt("PlayerWeight", 160);

		AchivmentAndStatControl.CheckAllAchivments();
	}

	void Update()
	{

		if(Input.GetKeyDown(KeyCode.X))
		{
			AchivmentAndStatControl.IncrementStat(Constants.totalSquatWallCount);
			AchivmentAndStatControl.IncrementStat(Constants.highestSquatConsec, 1);
			AchivmentAndStatControl.IncrementStat(Constants.totalCardioWallCount, 15);
			AchivmentAndStatControl.IncrementStat(Constants.highestCardioConsec);
			AchivmentAndStatControl.IncrementStat(Constants.totalDailyChallenges, 10);
			AchivmentAndStatControl.IncrementStat(Constants.totalCustomRoutines, 10);
			AchivmentAndStatControl.IncrementStat(Constants.punchingBagPunches, 10);
			AchivmentAndStatControl.IncrementStat(Constants.highScore, 10);
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		}

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
			this.TransitionToMenu("GameMode ClassicMode");
		if (Input.GetKeyDown(KeyCode.W))
			this.TransitionToMenu("Stats");
		if(Input.GetKeyDown(KeyCode.D))
			this.TransitionToMenu("DailyChallenge");
		if (Input.GetKeyDown(KeyCode.F))
			this.TransitionToMenu("LoadLevel dc");

		this.MoveWall();
	}

	/// <summary>
	/// Move the normal wall up and down
	/// </summary>
	private void MoveWall()
	{
		// Move the wall down
		if (moveBack == true)
		{
			wall.transform.position = Vector3.MoveTowards(wall.transform.position, backPos, .025f);
			if(Vector3.Distance(wall.transform.position, backPos) < .01f)
			{
				wall.transform.position = backPos;
				this.ActivateButtonSet(buttonSetToken); // What button set to activate
				buttonTimer = Time.time + .5f;
				moveBack = false;
				moveForward = true;
			}
		}

		if (moveForward == true)
		{
			// Once we are done waiting at  the bottom
			if (Time.time > buttonTimer)
			{
				wall.transform.position = Vector3.MoveTowards(wall.transform.position, forwordPos, .025f);
				if (Vector3.Distance(wall.transform.position, forwordPos) < .01f)
				{
					wall.transform.position = forwordPos;
					buttonTimer = float.PositiveInfinity;
					moveForward = false;
				}
			}
		}
	}


	/// <summary>
	/// Move the walls up and down and show the correct button set
	/// </summary>
	public void TransitionToMenu(string token)
	{
		moveBack = true;
		buttonSetToken = token;     // Move the new buttons in (Once the wall is at the bottom

	}

	/// <summary>
	/// What button set should be shown
	/// </summary>
	private void ActivateButtonSet(string setToActivate)
	{
		// Disable all button sets 
		for (int i = 0; i < buttonSet.Length; i++)
		{
			if (buttonSet[i] != null)
				buttonSet[i].SetActive(false);
		}

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

			// Reset the load level object
			levelLoader.SetLoadIndex(-1);
			levelLoader.CloseDoors();

			// Load in the correct buttons
			buttonSet[0].SetActive(true);
		}

		// If we are loading the workout type then set up the continue to be whatever we want to do next
		if(setToActivate.Contains("GameMode"))
		{
			string nextToLoad = setToActivate.Split(' ')[1];
			gameModeSelector.SetContinueButtonToken(nextToLoad);
			gameModeSelector.SetSquatCardio(nextToLoad);
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

		// Load the buttons for the wall and activate whatever this load type is
		if (setToActivate.Contains("LoadLevel"))
		{
			buttonSet[6].SetActive(true);
			// Load the right level and info with the info token given after the load "LoadLevel" keyword
			this.LoadLevel(setToActivate.Split(' ')[1]);
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
			PlayerPrefs.SetInt(Constants.gameMode, Constants.gameModeClassic);

			// Make the song file based off buttons
			gymAndSongSelectControl.MakeSongFile(); 
			levelToLoad = gymAndSongSelectControl.GetSelectedGym();
			doingDailyChallenge = false;
		}

		// Daily Challenge 
		if (loadType.ToLower() == "dc")
		{
			PlayerPrefs.SetInt(Constants.gameMode, Constants.gameModeDaily);

			// Make the song file based off the daily challenge
			gymAndSongSelectControl.MakeSongFile(dailyChallengeControl.GetDailyChallengeSongs());
			levelToLoad = dailyChallengeControl.GetGymIndex();

			// Save the daily challenge info
			string[] dailyC = dailyChallengeControl.GetDailyChallengeSummary();
			this.WriteDataToFile(dailyC, CUSTOM_DATA_PATH);

			doingDailyChallenge = true;
		}

		// Custom Routine
		if (loadType.ToLower() == "cr")
		{
			PlayerPrefs.SetInt(Constants.gameMode, Constants.gameModeCustom);

			// Make the song file based off buttons
			gymAndSongSelectControl.MakeSongFile();
			levelToLoad = gymAndSongSelectControl.GetSelectedGym();

			// Fill out the data for the custom routine
			string[] custR = customRutineControl.GetCustomRutineSummary();
			this.WriteDataToFile(custR, CUSTOM_DATA_PATH);
			doingDailyChallenge = false;
		}

		// Arcade Mode
		if (loadType.ToLower() == "am")
		{
			PlayerPrefs.SetInt(Constants.gameMode, Constants.gameModeArcade);

			// Make the song file based off buttons
			gymAndSongSelectControl.MakeSongFile();
			levelToLoad = gymAndSongSelectControl.GetSelectedGym();
			doingDailyChallenge = false;
		}

		levelLoader.SetLoadIndex(levelToLoad);
		levelLoader.OpenDoors();
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

	/// <summary>
	/// Returns if we are currently doing a dialy challenge
	/// </summary>
	public bool IsDailyChal()
	{
		return this.doingDailyChallenge;
	}
}
