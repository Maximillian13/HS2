using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class DaySinceUnixTime
{
	// Seed the random with todays date added up so that it is a unique random every time
	public static int GetDaySinceUnixTime()
	{
		System.DateTimeOffset dto = new System.DateTimeOffset(System.DateTime.Now);
		return (int)(dto.ToUnixTimeSeconds() / 86400);  // Check back in 2038 to see if its broken 🤔
	}
}

public class DailyChallengeMaster : MonoBehaviour
{
	private TextMeshPro[] tms = new TextMeshPro[3];

	private System.Random seededRand;

	private bool warmUp;
	private bool[] squatWallTypes = new bool[3];
	private bool[] cardioWallTypes = new bool[3];
	private int breakAfter;
	private int breakFor;
	private bool switchModesOnBreak;
	private int gymNumber;
	private int lives;
	private float speed;

	private List<string> songs = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
		// Get all the text children 
		for (int i = 0; i < tms.Length; i++)
			tms[i] = this.transform.GetChild(i).GetComponent<TextMeshPro>();
	}

	/// <summary>
	/// Fill the description of the daily challenge
	/// </summary>
	public void FillDescription()
	{
		if (tms[0] == null)
			this.Start();

		// Clear song file
		songs.Clear();

		// Figure if we are in cardio mode 
		bool inCardioMode = PlayerPrefs.GetInt(Constants.cardioMode) == 1;

		// Seed the random with todays date added up so that it is a unique random every time
		int daysOfUnix = DaySinceUnixTime.GetDaySinceUnixTime();
		// (17909 on 1-13-2018) todo (might need an offset to deal with time zones)
		PlayerPrefs.SetInt("DailyChallengeID", daysOfUnix);

		seededRand = new System.Random(daysOfUnix);

		int r = seededRand.Next(2);

		if (inCardioMode == false) // Normal squat mode 
		{
			if (r == 0)
			{
				this.FillOut("Three-some", "The only walls comming towards you will be in sets of 3 and you will not have any proper warm up time. Good luck", false,
					new bool[] { false, false, true }, new bool[] { false, false, false }, 1, new string[] { "Sco 2" }, 1, 1.0f, 15, 21, 1, 4, false);
			}
			if (r == 1)
			{
				this.FillOut("Hot Singles In Your Area", "In this challenge you will only be facing singe walls, no warm up no breaks, Good luck!", false,
					new bool[] { true, false, false }, new bool[] { false, false, false }, 0, new string[] { "Sco 2", "In-House" }, 1, 1.0f, 0, 0, 0, 0, false);
			}
		}
		else // Cardio mode
		{
			if (r == 0)
			{
				this.FillOut("Long Jump", "No middle opening, get ready to side step far...", true,
					new bool[] { true, true, false }, new bool[] { true, false, true }, 2, new string[] { "Come On Yall" }, 3, 0.9f, 5, 11, 2, 4, true);
			}
			if (r == 1)
			{
				this.FillOut("Long Jump (Extreme)", "No middle opening, get ready to side step far...", true,
					new bool[] { false, false, false }, new bool[] { true, false, true }, 2, new string[] { "The Seperation" }, 3, 1, 5, 11, 2, 4, false);
			}
		}
	}

	private void FillOut(string title, string desc, bool warm, bool[] squatWalls, bool[] cardioWalls, int gymNum, string[] songArr, int livesAmount, float speedMult,
		int lowerAfter, int upperAfter, int breakForLower, int breakForUpper, bool switchGameModeOnBreak)
	{
		// Normal Challenge
		tms[0].text = title;
		tms[1].text = "Challenge Description:\n" + desc;
		warmUp = warm;

		// Walls
		for(int i = 0; i < squatWalls.Length; i++)
			squatWallTypes[i] = squatWalls[i];
		for (int i = 0; i < cardioWalls.Length; i++)
			cardioWallTypes[i] = cardioWalls[i];

		// Add Gym
		gymNumber = gymNum;

		// Add songs
		for(int i = 0; i < songArr.Length; i++)
			songs.Add(songArr[i]);

		// Lives and speed
		lives = livesAmount;
		speed = speedMult;

		// Random modifiers
		breakAfter = seededRand.Next(lowerAfter, upperAfter) * 10;
		breakFor = seededRand.Next(breakForLower, breakForUpper) * 10;

		// Switch game mode stuff
		switchModesOnBreak = switchGameModeOnBreak;

		// Make a string of all the songs
		string songString = songArr[0];
		for (int i = 1; i < songArr.Length; i++)
			songString += (" | " + songArr[i]);

		if (breakAfter == 0 || breakFor == 0)
		{
			tms[2].text = "Modifiers:\n" +
					"\t-Lives:" + livesAmount + "\n" +
					"\t-SpeedMult:" + speedMult + "\n" +
					"\t-Gym:" + this.ConvertIntToGymName(gymNumber) + "\n" +
					"\t-Song(s): " + songString;	
		}
		else
		{
			tms[2].text = "Modifiers:\n" +
			"\t-Break after: " + breakAfter + " walls\n" +
			"\t-Break for: " + breakFor + " seconds\n" +
			"\t-Lives:" + livesAmount + "\n" +
			"\t-SpeedMult:" + speedMult + "\n" +
			"\t-Switch Game Modes on Break:" + switchGameModeOnBreak + "\n" +
			"\t-Gym:" + this.ConvertIntToGymName(gymNumber) + "\n" +
			"\t-Song(s): " + songString;
		}
	}

	/// <summary>
	/// Get a string that holds all the custom info for this daily challenge
	/// </summary>
	public string[] GetDailyChallengeSummary()
	{
		string[] customRutineStrings = new string[8];

		// If warmUpButton[0] is selected then warm up == true 
		customRutineStrings[0] = warmUp.ToString();

		// Fill info of how often and how long breaks will happen (If no breaks set to in.maxval)
		if (breakFor == 0 || breakAfter == 0)
			customRutineStrings[1] = breakFor + " " + int.MaxValue; 
		else
			customRutineStrings[1] = breakFor + " " + breakAfter;

		// Hand placement always on
		customRutineStrings[2] = "True";

		// Check all the different wall densities and fill them in order of 1, 2, 3 (true if button is active, false if not)
		customRutineStrings[3] = squatWallTypes[0] + " " + squatWallTypes[1] + " " + squatWallTypes[2];

		// Check all the different wall types for cardio and fill them in order of 1, 2, 3 (true if button is active, false if not)
		customRutineStrings[4] = cardioWallTypes[0] + " " + cardioWallTypes[1] + " " + cardioWallTypes[2];

		// If we should switch game modes during breaks 
		customRutineStrings[5] = switchModesOnBreak.ToString();

		// Todo: Assign values for lives and speed mult
		// Fill out the amount of lives and speed multiplier
		customRutineStrings[6] = lives.ToString();
		customRutineStrings[7] = speed.ToString();

		return customRutineStrings;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.C))
		{
			PlayerPrefs.SetInt("DailySquatID", 0);
			Debug.Log("PP reset");
		}
		//if(Input.GetKey(KeyCode.R))
		//{
		//	// Todo: Delete for testing daily challenges
		//	PlayerPrefs.SetInt("DailySquatID", 0);
		//	PlayerPrefs.SetInt("DailyCardioID", 0);
		//	Debug.Log("reset");
		//}

		//if (Input.GetKey(KeyCode.S))
		//{
		//	PlayerPrefs.SetInt(Constants.cardioMode, 0);
		//	this.IncrementDailyChallengeStat();
		//}

		//if (Input.GetKey(KeyCode.C))
		//{
		//	PlayerPrefs.SetInt(Constants.cardioMode, 1);
		//	this.IncrementDailyChallengeStat();
		//}

	}

	/// <summary>
	/// Increment the score if it is the first time doing this daily challenge (one for squat and one for cardio)
	/// </summary>
	public void IncrementDailyChallengeStatIfNew()
	{
		// Get what player pref we are looking for (either cardio or squat)
		string playerPrefToFind = "DailySquatID";
		if (PlayerPrefs.GetInt(Constants.cardioMode) == 1) 
			playerPrefToFind = "DailyCardioID";

		int savedDay = PlayerPrefs.GetInt(playerPrefToFind);
		int CurrentDay = DaySinceUnixTime.GetDaySinceUnixTime();

		// If the player pref does not match the current day. It must be a new day, increment score
		if (savedDay != CurrentDay)
		{
			AchivmentAndStatControl.IncrementStat(Constants.totalDailyChallenges);
			PlayerPrefs.SetInt(playerPrefToFind, DaySinceUnixTime.GetDaySinceUnixTime());
		}
	}

	/// <summary>
	/// Get a list of all the names of the songs that should be played during this daily challenge
	/// </summary>
	public List<string> GetDailyChallengeSongs()
	{
		return songs;
	}

	/// <summary>
	/// Returns the index of the currently selected gym
	/// </summary>
	public int GetGymIndex()
	{
		return this.gymNumber;
	}

	/// <summary>
	/// Returns a string representation of a gym given the index of the gym
	/// </summary>
	private string ConvertIntToGymName(int gymInd)
	{
		if (gymInd == 0)
			return "Classic";
		else if (gymInd == 1)
			return "Night Club";
		else if (gymInd == 1)
			return "Future";
		else if (gymInd == 1)
			return "Victorian";
		else
			return "Random";
	}
}
