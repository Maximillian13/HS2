using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public static class AchivmentAndStatControl
{
	private const int SQUAT_CONSEC0 = 10; // Pancake Butt
	private const int SQUAT_CONSEC1 = 100; // Average Caboose
	private const int SQUAT_CONSEC2 = 200; // Rock Hard Bottom
	private const int SQUAT_CONSEC3 = 350; // Buns Of Steel
	private const int SQUAT_CONSEC4 = 500; // Golden Gluteus 

	private const int SQUAT_TOTAL0 = 500; // Home Gym // Todo: Change to 500
	private const int SQUAT_TOTAL1 = 1000; // 1-Month Trial
	private const int SQUAT_TOTAL2 = 2500; // Cardio Casual
	private const int SQUAT_TOTAL3 = 10000; // Gym Rat
	private const int SQUAT_TOTAL4 = 50000; // Leg Day, Every Day

	private const int DAILY_CHALLENGE0 = 1; 
	private const int DAILY_CHALLENGE1 = 7; 
	private const int DAILY_CHALLENGE2 = 14; 
	private const int DAILY_CHALLENGE3 = 30; 
	private const int DAILY_CHALLENGE4 = 50; 
	private const int DAILY_CHALLENGE5 = 100;


	private const int PUNCHING_BAG = 5000;

	/// <summary>
	/// Check if any of the Consecutive squat achievements have been made yet
	/// </summary>
	public static void CheckAllConsecutiveSquatAchivments(int amountOfSquats)
	{
		ChechAndUnlockAchivment(amountOfSquats, SQUAT_CONSEC0, Achievement.SQUAT_CONSEC0);
		ChechAndUnlockAchivment(amountOfSquats, SQUAT_CONSEC1, Achievement.SQUAT_CONSEC1);
		ChechAndUnlockAchivment(amountOfSquats, SQUAT_CONSEC2, Achievement.SQUAT_CONSEC2);
		ChechAndUnlockAchivment(amountOfSquats, SQUAT_CONSEC3, Achievement.SQUAT_CONSEC3);
		ChechAndUnlockAchivment(amountOfSquats, SQUAT_CONSEC4, Achievement.SQUAT_CONSEC4);
	}

	/// <summary>
	/// Check if any of the total squat achievements have been made yet
	/// </summary>
	public static void CheckAllTotalSquatAchivments(int totalSquats)
	{
		ChechAndUnlockAchivment(totalSquats, SQUAT_TOTAL0, Achievement.SQUAT_TOTAL0);
		ChechAndUnlockAchivment(totalSquats, SQUAT_TOTAL1, Achievement.SQUAT_TOTAL1);
		ChechAndUnlockAchivment(totalSquats, SQUAT_TOTAL2, Achievement.SQUAT_TOTAL2);
		ChechAndUnlockAchivment(totalSquats, SQUAT_TOTAL3, Achievement.SQUAT_TOTAL3);
		ChechAndUnlockAchivment(totalSquats, SQUAT_TOTAL4, Achievement.SQUAT_TOTAL4);
	}

	/// <summary>
	/// Check if any of the daily challenge achievements have been made yet
	/// </summary>
	public static void CheckAllDailyChallengeAchivments(int dailyChallengeNum)
	{
		ChechAndUnlockAchivment(dailyChallengeNum, DAILY_CHALLENGE0, Achievement.DAILY_CHALLENGE0);
		ChechAndUnlockAchivment(dailyChallengeNum, DAILY_CHALLENGE1, Achievement.DAILY_CHALLENGE1);
		ChechAndUnlockAchivment(dailyChallengeNum, DAILY_CHALLENGE2, Achievement.DAILY_CHALLENGE2);
		ChechAndUnlockAchivment(dailyChallengeNum, DAILY_CHALLENGE3, Achievement.DAILY_CHALLENGE3);
		ChechAndUnlockAchivment(dailyChallengeNum, DAILY_CHALLENGE4, Achievement.DAILY_CHALLENGE4);
		ChechAndUnlockAchivment(dailyChallengeNum, DAILY_CHALLENGE5, Achievement.DAILY_CHALLENGE5);
	}

	/// <summary>
	/// Check if any of the Consecutive cardio achievements have been made yet
	/// </summary>
	public static void CheckAllConsecutiveCardioAchivments(int amountOfCardioWalls)
	{
		// Todo: Add total Cardio Achievements
	}

	/// <summary>
	/// Check if any of the total cardio achievements have been made yet
	/// </summary>
	public static void CheckAllTotalCardioAchivments(int totalCardioWalls)
	{
		// Todo: Add total Cardio Achievements
	}

	/// <summary>
	/// Unlock total squat achievement if it has been made 
	/// </summary>
	private static void ChechAndUnlockAchivment(int totalSquats, int achivSquats, Achievement achivName)
	{
		if (totalSquats == achivSquats)
		{
			if (SteamManager.Initialized == true)
				UnlockAchievement(m_Achievements[(int)achivName]);
		}
	}

	/// <summary>
	/// Check all achivments incase you missed one 
	/// </summary>
	public static void CheckAllAchivments()
	{
		if (GetStat(Constants.totalSquatWallCount) >= SQUAT_TOTAL0)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_TOTAL0]);
		if (GetStat(Constants.totalSquatWallCount) >= SQUAT_TOTAL1)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_TOTAL1]);
		if (GetStat(Constants.totalSquatWallCount) >= SQUAT_TOTAL2)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_TOTAL2]);
		if (GetStat(Constants.totalSquatWallCount) >= SQUAT_TOTAL3)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_TOTAL3]);
		if (GetStat(Constants.totalSquatWallCount) >= SQUAT_TOTAL4)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_TOTAL4]);

		if (GetStat(Constants.highestSquatConsec) >= SQUAT_CONSEC0)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_CONSEC0]);
		if (GetStat(Constants.highestSquatConsec) >= SQUAT_CONSEC1)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_CONSEC1]);
		if (GetStat(Constants.highestSquatConsec) >= SQUAT_CONSEC2)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_CONSEC2]);
		if (GetStat(Constants.highestSquatConsec) >= SQUAT_CONSEC3)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_CONSEC3]);
		if (GetStat(Constants.highestSquatConsec) >= SQUAT_CONSEC4)
			UnlockAchievement(m_Achievements[(int)Achievement.SQUAT_CONSEC4]);

		if (GetStat(Constants.totalDailyChallenges) >= DAILY_CHALLENGE0)
			UnlockAchievement(m_Achievements[(int)Achievement.DAILY_CHALLENGE0]);
		if (GetStat(Constants.totalDailyChallenges) >= DAILY_CHALLENGE1)
			UnlockAchievement(m_Achievements[(int)Achievement.DAILY_CHALLENGE1]);
		if (GetStat(Constants.totalDailyChallenges) >= DAILY_CHALLENGE2)
			UnlockAchievement(m_Achievements[(int)Achievement.DAILY_CHALLENGE2]);
		if (GetStat(Constants.totalDailyChallenges) >= DAILY_CHALLENGE3)
			UnlockAchievement(m_Achievements[(int)Achievement.DAILY_CHALLENGE3]);
		if (GetStat(Constants.totalDailyChallenges) >= DAILY_CHALLENGE4)
			UnlockAchievement(m_Achievements[(int)Achievement.DAILY_CHALLENGE4]);
		if (GetStat(Constants.totalDailyChallenges) >= DAILY_CHALLENGE5)
			UnlockAchievement(m_Achievements[(int)Achievement.DAILY_CHALLENGE5]);

		if (GetStat(Constants.punchingBagPunches) >= PUNCHING_BAG)
			UnlockAchievement(m_Achievements[(int)Achievement.PUNCHING_BAG]);
	}

	public static void CheckPunchingBagAchiv()
	{
		if (GetStat(Constants.punchingBagPunches) >= PUNCHING_BAG)
			UnlockAchievement(m_Achievements[(int)Achievement.PUNCHING_BAG]);
	}

	/// <summary>
	/// Set a stat
	/// </summary>
	public static void SetStat(string statName, int statVal)
	{
		bool t;
		if (SteamManager.Initialized == true)
			t = SteamUserStats.SetStat(statName, statVal);

	}

	/// <summary>
	/// Get stat (-1 if error)
	/// </summary>
	public static int GetStat(string statName)
	{
		// Check we got steam up and running 
		if (SteamManager.Initialized == false)
			return -1;

		// Get stat, if failed, return -1
		int statOut;
		if (SteamUserStats.GetStat(statName, out statOut) == false)
			return -1;

		// Return stat
		return statOut;
	}

	/// <summary>
	/// Increments stat by 1 (or by specified amount)
	/// </summary>
	public static void IncrementStat(string statName, int amount = 1)
	{
		// Take current stat val, if we successfully get it, set that stat to that value + 1 (or specified val)
		if (SteamManager.Initialized == true)
		{
			int statVal = 0;
			if (SteamUserStats.GetStat(statName, out statVal) == true)
				SteamUserStats.SetStat(statName, statVal + amount);
		}
	}


	#region Achiv
	private enum Achievement : int
	{
		// Per Round 
		SQUAT_CONSEC0,
		SQUAT_CONSEC1,
		SQUAT_CONSEC2,
		SQUAT_CONSEC3,
		SQUAT_CONSEC4,

		// Total
		SQUAT_TOTAL0,
		SQUAT_TOTAL1,
		SQUAT_TOTAL2,
		SQUAT_TOTAL3,
		SQUAT_TOTAL4,

		// Daily
		DAILY_CHALLENGE0,
		DAILY_CHALLENGE1,
		DAILY_CHALLENGE2,
		DAILY_CHALLENGE3,
		DAILY_CHALLENGE4,
		DAILY_CHALLENGE5,

		// Random 
		PUNCHING_BAG

	};

	private static Achievement_t[] m_Achievements = new Achievement_t[]
	{
        // Per Round
        new Achievement_t(Achievement.SQUAT_CONSEC0, "SQUAT_CONSEC0", "Do 10 squats in a row in Classic Mode."),
		new Achievement_t(Achievement.SQUAT_CONSEC1, "SQUAT_CONSEC1", "Do 100 squats in a row in Classic Mode."),
		new Achievement_t(Achievement.SQUAT_CONSEC2, "SQUAT_CONSEC2", "Do 200 squats in a row in Classic Mode."),
		new Achievement_t(Achievement.SQUAT_CONSEC3, "SQUAT_CONSEC3", "Do 350 squats in a row in Classic Mode."),
		new Achievement_t(Achievement.SQUAT_CONSEC4, "SQUAT_CONSEC4", "Do 500 squats in a row in Classic Mode."),

		new Achievement_t(Achievement.SQUAT_TOTAL0, "SQUAT_TOTAL0", "Do 500 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL1, "SQUAT_TOTAL1", "Do 1,000 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL2, "SQUAT_TOTAL2", "Do 2,500 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL3, "SQUAT_TOTAL3", "Do 10,000 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL4, "SQUAT_TOTAL4", "Do 50,000 squats in total."),

		new Achievement_t(Achievement.DAILY_CHALLENGE0, "DAILY_CHALLENGE0", "Complete 1 daily challenge."),
		new Achievement_t(Achievement.DAILY_CHALLENGE1, "DAILY_CHALLENGE1", "Complete 7 daily challenge."),
		new Achievement_t(Achievement.DAILY_CHALLENGE2, "DAILY_CHALLENGE2", "Complete 14 daily challenge."),
		new Achievement_t(Achievement.DAILY_CHALLENGE3, "DAILY_CHALLENGE3", "Complete 30 daily challenge."),
		new Achievement_t(Achievement.DAILY_CHALLENGE4, "DAILY_CHALLENGE4", "Complete 50 daily challenge."),
		new Achievement_t(Achievement.DAILY_CHALLENGE5, "DAILY_CHALLENGE5", "Complete 100 daily challenge."),

		new Achievement_t(Achievement.PUNCHING_BAG, "Who Said This Game Was Only For Legs?", "Punch the punching bag 5,000 times.")
	};



	//-----------------------------------------------------------------------------
	// Purpose: Unlock this achievement
	//-----------------------------------------------------------------------------
	private static void UnlockAchievement(Achievement_t achievement)
	{
		achievement.m_bAchieved = true;

		// mark it down
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
	}


	private class Achievement_t
	{
		public Achievement m_eAchievementID;
		public string m_strName;
		public string m_strDescription;
		public bool m_bAchieved;

		/// <summary>
		/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
		/// </summary>
		/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
		/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
		/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
		public Achievement_t(Achievement achievementID, string name, string desc)
		{
			m_eAchievementID = achievementID;
			m_strName = name;
			m_strDescription = desc;
			m_bAchieved = false;
		}
	}
	#endregion
}

/// <summary>
/// Constants in this project, mostly player pref strings and tokens for stats
/// </summary>
public static class Constants
{
	public static string gameMode = "GameMode";
	public static string cardioMode = "CardioMode";
	public static string highScore = "HighScore";
	public static string totalSquatWallCount = "TotalSquatWallCount";
	public static string totalCardioWallCount = "TotalCardioWallCount";
	public static string highestCardioConsec = "HighestCardioConsec";
	public static string highestSquatConsec = "HighestCardioConsec";
	public static string totalDailyChallenges = "TotalDailyChallenges";
	public static string totalCustomRoutines = "TotalCustomRoutines";
	public static string totalCaloriesBurned = "TotalCaloriesBurned";
	public static string punchingBagPunches = "PunchingBagPunches";


	public static string dailyChallengeIDToken = "DailyChallengeID";


	public static int gameModeClassic = 0;
	public static int gameModeDaily = 1;
	public static int gameModeCustom = 2;
	public static int gameModeArcade = 3;
}
