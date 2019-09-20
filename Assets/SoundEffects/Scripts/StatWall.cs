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
			statSummary = "Total Squat Walls Passed: " + AchivmentAndStatControl.GetStat(Constants.totalSquatWallCount) + "\n";
			statSummary += "Classic Mode Highest Consecutive Squats: " + AchivmentAndStatControl.GetStat(Constants.highestSquatConsec) + "\n";
			statSummary += "Total Cardio Walls Passed: " + AchivmentAndStatControl.GetStat(Constants.totalCardioWallCount) + "\n";
			statSummary += "Classic Mode Highest Consecutive Cardio Walls: " + AchivmentAndStatControl.GetStat(Constants.highestCardioConsec) + "\n";
			statSummary += "Number of Daily Challenges Completed: " + AchivmentAndStatControl.GetStat(Constants.totalDailyChallenges) + "\n";
			statSummary += "Number of Custom Routines Played: " + AchivmentAndStatControl.GetStat(Constants.totalCustomRoutines) + "\n";
			statSummary += "Punching Bag Hits: " + AchivmentAndStatControl.GetStat(Constants.punchingBagPunches) + "\n";
			statSummary += "Arcade Mode High Score: " + AchivmentAndStatControl.GetStat(Constants.highScore);
		}

		statText.text = statSummary;
	}

	public void ReloadStats()
	{
		this.Start();
	}
}
