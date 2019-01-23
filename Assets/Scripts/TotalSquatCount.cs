using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Steamworks;

public class TotalSquatCount : MonoBehaviour
{
	//private BinaryWriter bw;
	//private BinaryReader br;

	//private string FOLDER_PATH = Directory.GetCurrentDirectory() + "\\Data";
	//private string FILE_PATH = Directory.GetCurrentDirectory() + "\\Data\\TS"; // The directory to put the text file
	//int numberOfSquats = 0; // prev number of squats so we dont override peoples score
	//private bool emptyFile; // If the file is empty

	//private const int ONE_ACHIV = 50; // Home Gym
	//private const int TWO_ACHIV = 100; // 1-Month Trial
	//private const int THREE_ACHIV = 250; // Cardio Casual
	//private const int FOUR_ACHIV = 500; // Gym Rat
	//private const int FIVE_ACHIV = 1000; // Leg Day, Every Day
	//private const int SIX_ACHIV = 10000; // King Of The Gym
	//private const int SEVEN_ACHIV = 25000; // Guardian Of The Gym

	//// Use this for initialization
	//void Start()
	//{

	//	// If the file does not exist 
	//	if (!File.Exists(FILE_PATH))
	//	{
	//		// Create the directory
	//		Directory.CreateDirectory(FOLDER_PATH);
	//		using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
	//		{
	//			bw.Write(0);
	//		}
	//	}

	//	// Just in-case some junk data gets in rewrite it just as a 0
	//	using (br = new BinaryReader(new FileStream(FILE_PATH, FileMode.Open)))
	//	{
	//		// If the data is good set prevNumberOfSquats to the current high score
	//		if (int.TryParse(br.ReadInt32().ToString(), out numberOfSquats) == false)
	//		{
	//			emptyFile = true;
	//		}
	//	}

	//	if (emptyFile == true)
	//	{
	//		using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
	//		{
	//			bw.Write(0);
	//		}
	//	}

	//}

	//// Called when the game end
	//public void UpdateStats()
	//{
	//	numberOfSquats++;
	//	SteamUserStats.SetStat("SquatCount", numberOfSquats);
	//	Debug.Log("Storing Steam Stat was successful = " + SteamUserStats.StoreStats());
	//	CheckAchievment(ONE_ACHIV, 5);
	//	CheckAchievment(TWO_ACHIV, 6);
	//	CheckAchievment(THREE_ACHIV, 7);
	//	CheckAchievment(FOUR_ACHIV, 8);
	//	CheckAchievment(FIVE_ACHIV, 9);
	//	CheckAchievment(SIX_ACHIV, 10);
	//	CheckAchievment(SEVEN_ACHIV, 14);

	//	using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
	//	{
	//		bw.Write(numberOfSquats);
	//	}
	//}

	//private void CheckAchievment(int numOfSquats, int achievmentIndex)
	//{
	//	if (numberOfSquats == numOfSquats)
	//	{
	//		if (SteamManager.Initialized == true)
	//		{
	//			UnlockAchievement(m_Achievements[achievmentIndex]);
	//			SteamUserStats.StoreStats();
	//		}
	//	}
	//}

	//#region Achiv
	//private enum Achievement : int
	//{
	//	// Per Round 
	//	PANCAKE_BUTT,
	//	AVERAGE_CABOOSE,
	//	ROCK_HARD_BOTTOM,
	//	BUNS_OF_STEEL,
	//	GOLDEN_GLUTEUS,

	//	// Total
	//	HOME_GYM,
	//	ONE_MONTH_TRIAL,
	//	CARDIO_CASUAL,
	//	GYM_RAT,
	//	LEG_DAY_EVERY_DAY,
	//	KING_OF_THE_GYM,

	//	//Extra
	//	NO_FUN,
	//	ONLY_PAIN,
	//	PURE_GAINS,
	//	GUARDIAN_OF_THE_GYM
	//};

	//private Achievement_t[] m_Achievements = new Achievement_t[]
	//{
 //       // Per Round
 //       new Achievement_t(Achievement.PANCAKE_BUTT, "Pancake Butt", "Do 10 squats in one round."),
	//	new Achievement_t(Achievement.AVERAGE_CABOOSE, "Average Caboose", "Do 25 squats in one round."),
	//	new Achievement_t(Achievement.ROCK_HARD_BOTTOM, "Rock Hard Bottom", "Do 75 squats in one round."),
	//	new Achievement_t(Achievement.BUNS_OF_STEEL, "Buns Of Steel", "Do 125 squats in one round."),
	//	new Achievement_t(Achievement.GOLDEN_GLUTEUS, "Golden Gluteus", "Do 175 squats in one round."),

 //       // Total
 //       new Achievement_t(Achievement.HOME_GYM, "Home Gym", "Do 50 squats in total."),
	//	new Achievement_t(Achievement.ONE_MONTH_TRIAL, "1-Month Trial", "Do 100 squats in total."),
	//	new Achievement_t(Achievement.CARDIO_CASUAL, "Cardio Casual", "Do 250 squats in total."),
	//	new Achievement_t(Achievement.GYM_RAT, "Gym Rat", "Do 500 squats in total."),
	//	new Achievement_t(Achievement.LEG_DAY_EVERY_DAY, "Leg Day, Every Day", "Do 1000 squats in total."),
	//	new Achievement_t(Achievement.KING_OF_THE_GYM, "King Of The Gym", "Do 10000 squats in total."),

 //       //Extra
 //       new Achievement_t(Achievement.NO_FUN, "No Fun", "Do 250 squats in one round."),
	//	new Achievement_t(Achievement.ONLY_PAIN, "Only Pain", "Do 400 squats in one round."),
	//	new Achievement_t(Achievement.PURE_GAINS, "Pure Gains", "Do 500 squats in one round."),
	//	new Achievement_t(Achievement.GUARDIAN_OF_THE_GYM, "Guardian Of The Gym", "Do 25000 squats in total.")
	//};



	////-----------------------------------------------------------------------------
	//// Purpose: Unlock this achievement
	////-----------------------------------------------------------------------------
	//private void UnlockAchievement(Achievement_t achievement)
	//{
	//	achievement.m_bAchieved = true;

	//	// mark it down
	//	SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
	//}


	//private class Achievement_t
	//{
	//	public Achievement m_eAchievementID;
	//	public string m_strName;
	//	public string m_strDescription;
	//	public bool m_bAchieved;

	//	/// <summary>
	//	/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
	//	/// </summary>
	//	/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
	//	/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
	//	/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
	//	public Achievement_t(Achievement achievementID, string name, string desc)
	//	{
	//		m_eAchievementID = achievementID;
	//		m_strName = name;
	//		m_strDescription = desc;
	//		m_bAchieved = false;
	//	}
	//}
	//#endregion
}
