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

	private List<string> songs = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
		// Get all the text children 
		for (int i = 0; i < tms.Length; i++)
			tms[i] = this.transform.GetChild(i).GetComponent<TextMeshPro>();

		// Init all arrays 
		for (int i = 0; i < squatWallTypes.Length; i++)
			squatWallTypes[i] = false;
		for (int i = 0; i < cardioWallTypes.Length; i++)
			cardioWallTypes[i] = false;

		// Fill out the description
		this.FillDescription();
	}

	private void OnEnable()
	{
		this.FillDescription();
	}


	/// <summary>
	/// Fill the description of the daily challenge
	/// </summary>
	public void FillDescription()
	{
		if (tms[0] == null)
			this.Start();

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
			PlayerPrefs.SetInt("DailySquatID", daysOfUnix);
			if (r == 0)
			{
				// Normal Challenge
				tms[0].text = "Three-some";
				tms[1].text = "Challenge Description:\nThe only wall coming towards you will be in sets of 3 and you wont have any proper warm-up. Good luck!";
				warmUp = false;
				squatWallTypes[0] = false;
				squatWallTypes[1] = false;
				squatWallTypes[2] = true;

				// Add songs
				songs.Add("909");

				// Random modifiers
				breakAfter = seededRand.Next(15, 21) * 10;
				breakFor = seededRand.Next(1, 4) * 10;
				tms[2].text = "Modifiers:\n" +
					"\t-Break after: " + breakAfter + " walls\n" +
					"\t-Break for: " + breakFor + " seconds\n" +
					"\t-Song(s): 909";
			}
			if (r == 1)
			{
				// Normal Challenge
				tms[0].text = "Hot Singles In Your Area";
				tms[1].text = "Challenge Description:\nIn this challenge you will only be facing singe walls, no warm up no breaks, Good luck!";
				warmUp = false;
				squatWallTypes[0] = true;
				squatWallTypes[1] = false;
				squatWallTypes[2] = false;

				// Add songs
				songs.Add("Sgo");
				songs.Add("909");

				// Random modifiers
				breakAfter = seededRand.Next(150, 201);
				breakFor = seededRand.Next(10, 31);
				tms[2].text = "Modifiers:\n" +
					"\t-Song(s): Sgo, 909";
			}
		}
		else // Cardio mode
		{
			if (r == 0)
			{
				tms[0].text = "Long Jump";
				tms[1].text = "Challenge Description:\nNo middle opening, get read to side step far...";
				warmUp = true;
				cardioWallTypes[0] = true;
				cardioWallTypes[1] = false;
				cardioWallTypes[2] = true;

				// Add songs
				songs.Add("909");

				// Random modifiers
				breakAfter = seededRand.Next(5, 11) * 10;
				breakFor = seededRand.Next(2, 4) * 10;
				tms[2].text = "Modifiers:\n" +
					"\t-Break after: " + breakAfter + " walls\n" +
					"\t-Break for: " + breakFor + " seconds\n" +
					"\t-Song(s): 909";
			}
			if (r == 1)
			{
				tms[0].text = "Spread Eagle Cross the Block";
				tms[1].text = "Challenge Description:\nNo middle opening, get read to side step far. Switches mode every break";
				warmUp = false;

				squatWallTypes[0] = true;
				squatWallTypes[1] = true;
				squatWallTypes[2] = true;
				cardioWallTypes[0] = true;
				cardioWallTypes[1] = false;
				cardioWallTypes[2] = true;

				switchModesOnBreak = true;

				// Add songs
				songs.Add("909");
				songs.Add("Sgo");

				// Random modifiers
				breakAfter = seededRand.Next(5, 11) * 10;
				breakFor = seededRand.Next(2, 4) * 10;
				tms[2].text = "Modifiers:\n" +
					"\t-Break after: " + breakAfter + " walls\n" +
					"\t-Break for: " + breakFor + " seconds\n" +
					"\t-Song(s): 909, Sgo";
			}
		}
	}

	/// <summary>
	/// Get a string that holds all the custom info for this daily challenge
	/// </summary>
	public string[] GetDailyChallengeSummary()
	{
		string[] customRutineStrings = new string[5];

		// If warmUpButton[0] is selected then warm up == true 
		customRutineStrings[0] = warmUp.ToString();

		// Fill info of how often and how long breaks will happen
		customRutineStrings[1] = breakFor + " ";
		customRutineStrings[1] += breakAfter;

		// Check all the different wall densities and fill them in order of 1, 2, 3 (true if button is active, false if not)
		customRutineStrings[2] = squatWallTypes[0] + " " + squatWallTypes[1] + " " + squatWallTypes[2];

		// Check all the different wall types for cardio and fill them in order of 1, 2, 3 (true if button is active, false if not)
		customRutineStrings[3] = cardioWallTypes[0] + " " + cardioWallTypes[1] + " " + cardioWallTypes[2];

		// If we should switch game modes during breaks 
		customRutineStrings[4] = switchModesOnBreak.ToString();

		return customRutineStrings;
	}

	private void Update()
	{
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
	public void IncrementDailyChallengeStat()
	{
		// Get what player pref we are looking for (either cardio or squat)
		string playerPrefToFind = "DailySquatID";
		if (PlayerPrefs.GetInt(Constants.cardioMode) == 1) 
			playerPrefToFind = "DailyCardioID";

		int pp = PlayerPrefs.GetInt(playerPrefToFind);
		int ud = DaySinceUnixTime.GetDaySinceUnixTime();

		// If the player pref does not match the current day. It must be a new day, increment score
		if (PlayerPrefs.GetInt(playerPrefToFind) != DaySinceUnixTime.GetDaySinceUnixTime())
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
}
