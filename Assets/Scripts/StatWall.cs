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
			Debug.Log(SteamUserStats.GetStat("TotalSquatWallCount", out statValue));
			statSummary = "Total Squat Walls Passed: " + statValue + "\n";

			SteamUserStats.GetStat("HighestSquatConsec", out statValue);
			statSummary += "Classic Mode Highest Consecutive Squats: " + statValue + "\n";

			SteamUserStats.GetStat("TotalCardioWallCount", out statValue);
			statSummary += "Total Cardio Walls Passed: " + statValue + "\n";

			SteamUserStats.GetStat("HighestCardioConsec", out statValue);
			statSummary += "Classic Mode Highest Consecutive Cardio Walls: " + statValue + "\n";

			SteamUserStats.GetStat("TotalDailyChallenges", out statValue);
			statSummary += "Number of Daily Challenges Completed: " + statValue + "\n";

			SteamUserStats.GetStat("TotalCustomRoutines", out statValue);
			statSummary += "Number of Custom Routines Played: " + statValue;
		}

		statText.text = statSummary;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.N))
		{
			AchivmentAndStatControl.SetStat("TotalCustomRoutines", 1);
			Debug.Log("Out of static -> " + SteamUserStats.SetStat("TotalCustomRoutines", 1));
			this.Start();
		}
	}
}
