using UnityEngine;
using System.Collections;
using Steamworks;

public class SquatCounter : MonoBehaviour
{

	//private int amountOfSquats;
	//private const int ONE_ACHIV = 10; // Pancake Butt
	//private const int TWO_ACHIV = 25; // Average Caboose
	//private const int THREE_ACHIV = 75; // Rock Hard Bottom
	//private const int FOUR_ACHIV = 125; // Buns Of Steel
	//private const int FIVE_ACHIV = 175; // Golden Gluteus 
	//private const int SIX_ACHIV = 250; // No fun
	//private const int SEVEN_ACHIV = 400; // Only Pain
	//private const int EIGHT_ACHIV = 500; // Pure Gains

	//// Update is called once per frame
	//void Update()
	//{
	//	//Debug.Log(amountOfSquats);

	//	// Reset all achievements
	//	//if (Input.GetKeyDown(KeyCode.Escape))
	//	//{
	//	//    if (SteamManager.Initialized == true)
	//	//    {
	//	//        SteamUserStats.ResetAllStats(true);
	//	//        SteamUserStats.StoreStats();
	//	//        Debug.Log("Achiv deleted");
	//	//    }
	//	//}
	//}

	//public void UpdateStats()
	//{
	//	amountOfSquats++;

	//	this.CheckAchievment(ONE_ACHIV, 0);
	//	this.CheckAchievment(TWO_ACHIV, 1);
	//	this.CheckAchievment(THREE_ACHIV, 2);
	//	this.CheckAchievment(FOUR_ACHIV, 3);
	//	this.CheckAchievment(FIVE_ACHIV, 4);
	//	this.CheckAchievment(SIX_ACHIV, 11);
	//	this.CheckAchievment(SEVEN_ACHIV, 12);
	//	this.CheckAchievment(EIGHT_ACHIV, 13);
	//}

	//private void CheckAchievment(int numOfSquats, int achievmentIndex)
	//{
	//	if (amountOfSquats == numOfSquats)
	//	{
	//		if (SteamManager.Initialized == true)
	//		{
	//			this.UnlockAchievement(m_Achievements[achievmentIndex]);
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
