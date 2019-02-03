using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public static class AchivmentAndStatControl
{
	private const int SQUAT_CONSEC0 = 10; // Pancake Butt
	private const int SQUAT_CONSEC1 = 100; // Average Caboose
	private const int SQUAT_CONSEC2 = 200; // Rock Hard Bottom
	private const int SQUAT_CONSEC3 = 300; // Buns Of Steel
	private const int SQUAT_CONSEC4 = 400; // Golden Gluteus 
	private const int SQUAT_CONSEC5 = 500; // Buns Of Steel
	private const int SQUAT_CONSEC6 = 750; // Golden Gluteus 

	private const int SQUAT_TOTAL0 = 500; // Home Gym // Todo: Change to 500
	private const int SQUAT_TOTAL1 = 1000; // 1-Month Trial
	private const int SQUAT_TOTAL2 = 2500; // Cardio Casual
	private const int SQUAT_TOTAL3 = 5000; // Gym Rat
	private const int SQUAT_TOTAL4 = 10000; // Leg Day, Every Day
	private const int SQUAT_TOTAL5 = 50000; // King Of The Gym
	private const int SQUAT_TOTAL6 = 100000; // Guardian Of The Gym

	/// <summary>
	/// Check if any of the Consecutive squat achievements have been made yet
	/// </summary>
	public static void CheckAllConsecutiveSquatAchivments(int amountOfSquats)
	{
		ChechAndUnlockConsecutiveAchivment(amountOfSquats, SQUAT_CONSEC0, Achievement.SQUAT_CONSEC0);
		ChechAndUnlockConsecutiveAchivment(amountOfSquats, SQUAT_CONSEC1, Achievement.SQUAT_CONSEC1);
		ChechAndUnlockConsecutiveAchivment(amountOfSquats, SQUAT_CONSEC2, Achievement.SQUAT_CONSEC2);
		ChechAndUnlockConsecutiveAchivment(amountOfSquats, SQUAT_CONSEC3, Achievement.SQUAT_CONSEC3);
		ChechAndUnlockConsecutiveAchivment(amountOfSquats, SQUAT_CONSEC4, Achievement.SQUAT_CONSEC4);
		ChechAndUnlockConsecutiveAchivment(amountOfSquats, SQUAT_CONSEC5, Achievement.SQUAT_CONSEC5);
		ChechAndUnlockConsecutiveAchivment(amountOfSquats, SQUAT_CONSEC6, Achievement.SQUAT_CONSEC6);
	}

	/// <summary>
	/// Unlock consecutive squat achievement if it has been made 
	/// </summary>
	private static void ChechAndUnlockConsecutiveAchivment(int currentSquats, int achivSquats, Achievement achivName)
	{
		if (currentSquats == achivSquats)
		{
			if (SteamManager.Initialized == true)
			{
				UnlockAchievement(m_Achievements[(int)achivName]);
				SteamUserStats.StoreStats();
			}
		}
	}

	/// <summary>
	/// Check if any of the total squat achievements have been made yet
	/// </summary>
	public static void CheckAllTotalSquatAchivments(int totalSquats)
	{
		ChechAndUnlockTotalAchivment(totalSquats, SQUAT_CONSEC0, Achievement.SQUAT_TOTAL0);
		ChechAndUnlockTotalAchivment(totalSquats, SQUAT_CONSEC1, Achievement.SQUAT_TOTAL1);
		ChechAndUnlockTotalAchivment(totalSquats, SQUAT_CONSEC2, Achievement.SQUAT_TOTAL2);
		ChechAndUnlockTotalAchivment(totalSquats, SQUAT_CONSEC3, Achievement.SQUAT_TOTAL3);
		ChechAndUnlockTotalAchivment(totalSquats, SQUAT_CONSEC4, Achievement.SQUAT_TOTAL4);
		ChechAndUnlockTotalAchivment(totalSquats, SQUAT_CONSEC5, Achievement.SQUAT_TOTAL5);
		ChechAndUnlockTotalAchivment(totalSquats, SQUAT_CONSEC6, Achievement.SQUAT_TOTAL6);
	}

	/// <summary>
	/// Unlock total squat achievement if it has been made 
	/// </summary>
	private static void ChechAndUnlockTotalAchivment(int totalSquats, int achivSquats, Achievement achivName)
	{
		if (totalSquats == achivSquats)
		{
			if (SteamManager.Initialized == true)
			{
				UnlockAchievement(m_Achievements[(int)achivName]);
				SteamUserStats.StoreStats();
			}
		}
	}

	/// <summary>
	/// Set the consecutive squat stat
	/// </summary>
	public static void SetConsecutiveSquatStat(int numberOfSquats)
	{
		SteamUserStats.SetStat("HighestSquatConsec", numberOfSquats);
		SteamUserStats.StoreStats();
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
		SQUAT_CONSEC5,
		SQUAT_CONSEC6,

		SQUAT_TOTAL0,
		SQUAT_TOTAL1,
		SQUAT_TOTAL2,
		SQUAT_TOTAL3,
		SQUAT_TOTAL4,
		SQUAT_TOTAL5,
		SQUAT_TOTAL6
	};

	private static Achievement_t[] m_Achievements = new Achievement_t[]
	{
        // Per Round
        new Achievement_t(Achievement.SQUAT_CONSEC0, "SQUAT_CONSEC0", "Do 10 squats in one round."),
		new Achievement_t(Achievement.SQUAT_CONSEC1, "SQUAT_CONSEC1", "Do 100 squats in one round."),
		new Achievement_t(Achievement.SQUAT_CONSEC2, "SQUAT_CONSEC2", "Do 200 squats in one round."),
		new Achievement_t(Achievement.SQUAT_CONSEC3, "SQUAT_CONSEC3", "Do 300 squats in one round."),
		new Achievement_t(Achievement.SQUAT_CONSEC4, "SQUAT_CONSEC4", "Do 400 squats in one round."),
		new Achievement_t(Achievement.SQUAT_CONSEC5, "SQUAT_CONSEC5", "Do 500 squats in one round."),
		new Achievement_t(Achievement.SQUAT_CONSEC6, "SQUAT_CONSEC6", "Do 750 squats in one round."),

		new Achievement_t(Achievement.SQUAT_TOTAL0, "SQUAT_TOTAL0", "Do 500 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL1, "SQUAT_TOTAL1", "Do 1000 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL2, "SQUAT_TOTAL2", "Do 2500 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL3, "SQUAT_TOTAL3", "Do 5000 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL4, "SQUAT_TOTAL4", "Do 10000 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL5, "SQUAT_TOTAL5", "Do 50000 squats in total."),
		new Achievement_t(Achievement.SQUAT_TOTAL6, "SQUAT_TOTAL6", "Do 100000 squats in total."),
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
