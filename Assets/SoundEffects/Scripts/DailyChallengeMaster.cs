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

		if (inCardioMode == false) // Normal squat mode 
		{
			int r = seededRand.Next(14);

			if (r == 0)
				this.FillOut("Threesome", "The only squat walls coming towards you will be in sets of three. Get ready for some thicc walls...", true,
					new bool[] { false, false, true }, new bool[] { false, false, false }, 1, new string[] { Constants.songInHouse, Constants.songTheSeparation }, 2, 1.0f, 15, 21, 1, 4, false);

			if (r == 1)
				this.FillOut("Hot Singles In Your Area", "Only single squat walls will come towards you - and they’re looking to mingle.", true,
					new bool[] { true, false, false }, new bool[] { false, false, false }, 2, new string[] { Constants.songSco2, Constants.songNewGlory }, 2, 1.0f, 10, 15, 1, 3, false);

			if (r == 2)
				this.FillOut("Double Day", "The only squat walls coming towards you will be in sets of two. Go get those juicy doubles!", true,
					new bool[] { false, true, false }, new bool[] { false, false, false }, 3, new string[] { Constants.songNewGlory, Constants.song80s}, 2, 1.0f, 15, 20, 2, 2, false);

			if (r == 3)
				this.FillOut("Threesome (Extreme)", "The only squat walls coming towards you will be in sets of three, walls are 10% faster, and there’s no breaks. Get ready for some thicc walls..", false, new bool[] { false, false, true }, new bool[] { false, false, false }, 4, new string[] { Constants.songTheSeparation, Constants.songNewGlory }, 1, 1.1f, 0, 0, 0, 0, false);

			if (r == 4)
				this.FillOut("Hot Singles In Your Area (Extreme)", "Only single squat walls will come towards you, they’re 30% faster, and there’s no breaks.", false,
					new bool[] { true, false, false }, new bool[] { false, false, false }, 1, new string[] { Constants.songSco2, Constants.song80s }, 1, 1.3f, 0, 0, 0, 0, false);

			if (r == 5)
				this.FillOut("Double Day (Extreme)", "The only squat walls coming towards you will be in sets of two, they’re 20% faster, and there’s no breaks.", false, new bool[] { false, true, false }, new bool[] { false, false, false }, 2, new string[] { Constants.songTheSeparation, Constants.song80s }, 1, 1.2f, 0, 0, 0, 0, false);

			if (r == 6)
				this.FillOut("Slow And Painful", "The only squat walls coming towards you will be in sets of three, and they’re 50% slower. This is going to be a slow and painful ride...", false, new bool[] { false, false, true }, new bool[] { false, false, false }, 3, new string[] { Constants.songInHouse }, 2, .7f, 4, 5, 2, 3, false);

			if (r == 7)
				this.FillOut("Slow And Painful (Extreme)", "The only squat walls coming towards you will be in sets of three, they’re 40% slower, and there’s no breaks. This is going to be a very slow and painful ride...", false, new bool[] { false, false, true }, new bool[] { false, false, false }, 4, new string[] { Constants.songInHouse, Constants.songTheSeparation }, 1, .6f, 0, 0, 0, 0, false);

			if (r == 8)
				this.FillOut("Short And Long", "The only squat walls coming towards you will either be in sets of 1 or 3, and they’re 10% faster!", true, new bool[] { true, false, true }, new bool[] { false, false, false }, 1, new string[] { Constants.songNewGlory, Constants.song80s }, 2, 1.1f, 15, 20, 1, 3, false);

			if (r == 9)
				this.FillOut("Short And Long (Extreme)", "The only squat walls coming towards you will either be in sets of 1 or 3, and they’re 20% faster, and there’s no breaks.", false, new bool[] { true, false, true }, new bool[] { false, false, false }, 1, new string[] { Constants.songNewGlory, Constants.song80s }, 1, 1.2f, 0, 0, 0, 0, false);

			if (r == 10)
				this.FillOut("Classic (Endless)", "We’re going old school - the classic hot squat experience with an infinite amount of lives and no breaks!", false, new bool[] { true, true, true }, new bool[] { false, false, false }, 0, new string[] { Constants.songSco2 }, 6, 1, 0, 0, 0, 0, false);

			if (r == 11)
				this.FillOut("Threesome (Endless)", "The only squat walls coming towards you will be in sets of three, there’s no breaks, and you get an infinite amount of lives!", false, new bool[] { false, false, true }, new bool[] { false, false, false }, 2, new string[] { Constants.songInHouse, Constants.songNewGlory, Constants.songNewGlory }, 6, 1, 0, 0, 0, 0, false);

			if (r == 12)
				this.FillOut("Double Day (Endless)", "The only squat walls coming towards you will be in sets of two, there’s no breaks, and you get an infinite amount of lives!", false, new bool[] { false, true, false }, new bool[] { false, false, false }, 3, new string[] { Constants.song80s, Constants.songTheSeparation, Constants.songInHouse}, 6, 1, 0, 0, 0, 0, false);

			if (r == 13)
				this.FillOut("Hot Singles In Your Area (Endless)", "Only single squat walls will come towards you, there’s no breaks, and you get an infinite amount of lives!", false, new bool[] { true, false, false}, new bool[] { false, false, false }, 4, new string[] { Constants.songSco2, Constants.song80s, Constants.songNewGlory }, 6, 1, 0, 0, 0, 0, false);

		}
		else // Cardio mode
		{
			int r = seededRand.Next(9);

			if (r == 0)
				this.FillOut("Long Jump", "Only cardio walls with no middle opening will come towards you, get ready to side step far!", true,
					new bool[] { false, false, false }, new bool[] { true, false, true }, 1, new string[] { Constants.songInHouse, Constants.song80s }, 2, 0.9f, 5, 11, 2, 4, false);

			if (r == 1)
				this.FillOut("Long Jump (Extreme)", "Only cardio walls with no middle opening will come towards you, and you only get 1 life and no breaks...get ready to side step far!", false,
					new bool[] { false, false, false }, new bool[] { true, false, true }, 2, new string[] { Constants.song80s, Constants.songSco2 }, 1, 1.0f, 0, 0, 0, 0, false);

			if (r == 2)
				this.FillOut("Long Jump Singles", "You’ll face only single squat walls and cardio walls with no middle opening. This is some fast paced action!", true,
					new bool[] { true, false, false }, new bool[] { true, false, true }, 3, new string[] { Constants.songTheSeparation, Constants.songNewGlory }, 2, 1, 7, 11, 1, 2, true);

			if (r == 3)
				this.FillOut("Long Jump Singles (Extreme)", "You’ll face only single squat walls and cardio walls with no middle opening. Oh, and you only get one life...this is some fast paced action!", false,
					new bool[] { true, false, false }, new bool[] { true, false, true }, 4, new string[] { Constants.songSco2, Constants.song80s, Constants.songTheSeparation}, 1, 1, 10, 15, 1, 1, true);

			if (r == 4)
				this.FillOut("Long Jump Triples", "You’ll face only triple squat walls and cardio walls with no middle opening. A little bit of this, a little bit of that...", true,
					new bool[] { false, false, true }, new bool[] { true, false, true }, 1, new string[] { Constants.songInHouse, Constants.songNewGlory }, 2, 1, 7, 11, 1, 2, true);

			if (r == 5)
				this.FillOut("Long Jump Triples (Extreme)", "You’ll face only triple squat walls and cardio walls with no middle opening. Plus, you only get 1 life, good luck!", false,
					new bool[] { false, false, true }, new bool[] { true, false, true }, 2, new string[] { Constants.songInHouse, Constants.songNewGlory, Constants.songTheSeparation}, 1, 1, 10, 15, 1, 1, true);

			if (r == 6)
				this.FillOut("Long Jump Doubles", "You’ll face only double squat walls and cardio walls with no middle opening. Good things come in pairs!", true,
					new bool[] { false, true, false }, new bool[] { true, false, true }, 3, new string[] { Constants.song80s, Constants.songTheSeparation, Constants.songSco2 }, 2, 1.0f, 7, 11, 1, 2, true);

			if (r == 7)
				this.FillOut("Long Jump Doubles (Extreme)", "You’ll face only double squat walls and cardio walls with no middle opening...and you only get 1 life and no breaks. Good things come in pairs!", false,
					new bool[] { false, true, false }, new bool[] { true, false, true }, 4, new string[] { Constants.songNewGlory, Constants.songSco2, Constants.songInHouse }, 1, 1.0f, 0, 0, 0, 0, true);

			if (r == 8)
				this.FillOut("Long Jump (Endless)", "You get infinite lives and only cardio walls with no middle opening. Long jump `till you can’t no more!", false,
					new bool[] { false, false, false }, new bool[] { true, false, true }, 4, new string[] { Constants.songNewGlory, Constants.song80s, Constants.songTheSeparation, Constants.songInHouse}, 6, 1.0f, 0, 0, 0, 0, false);

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
					"    -Warm Up: " + warmUp + "\n" +
					"    -Breaks: None\n" +
					"    -Lives: " + livesAmount + "\n" +
					"    -SpeedMult: " + speedMult + "\n" +
					"    -Gym: " + this.ConvertIntToGymName(gymNumber) + "\n" +
					"    -Song(s): " + songString;	
		}
		else
		{
			tms[2].text = "Modifiers:\n" +
			"    -Warm Up: " + warmUp +  "\n" + 
			"    -Breaks: On\n" +
			"    -Break after: " + breakAfter + " walls\n" +
			"    -Break for: " + breakFor + " seconds\n" +
			"    -Lives: " + livesAmount + "\n" +
			"    -SpeedMult: " + speedMult + "\n" +
			"    -Switch Game Modes: " + switchGameModeOnBreak + "\n" +
			"    -Gym: " + this.ConvertIntToGymName(gymNumber) + "\n" +
			"    -Song(s): " + songString;
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

		// Fill out the amount of lives and speed multiplier
		customRutineStrings[6] = lives.ToString();
		customRutineStrings[7] = speed.ToString();

		return customRutineStrings;
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
			return "Dark";
		else if (gymInd == 1)
			return "Club";
		else if (gymInd == 1)
			return "Victorian";
		else
			return "Random";
	}
}
