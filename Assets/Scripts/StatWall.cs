using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;

public class StatWall : MonoBehaviour
{
	public TextMeshPro statText;

    // Start is called before the first frame update
    void Start()
    {
		// Error message to be changed if we connect to steam
		string statSummary = "Error connecting to Steam stats, try relaunching Steam...";

		// If we connect to steam fill out with stats 
		if (SteamManager.Initialized == true)
		{
			int statValue = -1;
			SteamUserStats.GetStat(Constants.totalSquatWallCount, out statValue);
			statSummary = "Total Squat Walls Passed: " + statValue + "\n";

			SteamUserStats.GetStat(Constants.highestSquatConsec, out statValue);
			statSummary += "Classic Mode Highest Consecutive Squats: " + statValue + "\n";

			SteamUserStats.GetStat(Constants.totalCardioWallCount, out statValue);
			statSummary += "Total Cardio Walls Passed: " + statValue + "\n";

			SteamUserStats.GetStat(Constants.highestCardioConsec, out statValue);
			statSummary += "Classic Mode Highest Consecutive Cardio Walls: " + statValue + "\n";

			SteamUserStats.GetStat(Constants.totalDailyChallenges, out statValue);
			statSummary += "Number of Daily Challenges Completed: " + statValue + "\n";

			SteamUserStats.GetStat(Constants.totalCustomRoutines, out statValue);
			statSummary += "Number of Custom Routines Played: " + statValue + "\n";

			SteamUserStats.GetStat(Constants.totalCaloriesBurned, out statValue);
			statSummary += "Total Calories Burned: " + statValue + "\n";

			SteamUserStats.GetStat(Constants.highScore, out statValue);
			statSummary += "Arcade Mode High Score: " + statValue;
		}

		statText.text = statSummary;
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
