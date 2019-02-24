using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Steamworks;

public class SteamLeaderBoard : MonoBehaviour
{
	public string lbName;
	private TextMesh titleText; // The text mesh that will display the title
	private TextMesh bodyText; // The text mesh that will display the leader board
	private string handleName;

	private SteamLeaderboard_t m_SteamLeaderboard;
	private CallResult<LeaderboardFindResult_t> LeaderboardFindResult;
	private CallResult<LeaderboardScoresDownloaded_t> LeaderboardScoresDownloaded;

	// Bug: Seems like it isnt displaying in game on the very first time after a clean install (Clear registry)

	public void OnEnable()
	{
		if (this == null)
			return;

		// Find the text mesh
		titleText = this.transform.Find("Title").GetComponent<TextMesh>();
		bodyText = this.transform.Find("Body").GetComponent<TextMesh>();
		handleName = lbName;

		// IF there is nothing default to the classic mode 
		if (handleName == "")
			handleName = "HS2ClassicModeLeaderBoard";

		// Todo: Make sure that all this daily scoreboard buisness is working how we want it
		// Daily challenge pulling from player prefs 
		if (handleName == "DailyChallengeSquat")
		{
			if (DaySinceUnixTime.GetDaySinceUnixTime() == PlayerPrefs.GetInt(Constants.dailyChallengeIDToken))
				handleName = "Squat" + PlayerPrefs.GetInt(Constants.dailyChallengeIDToken);
			else
				handleName = "Empty";
		}
		if (handleName == "DailyChallengeCardio")
		{
			if (DaySinceUnixTime.GetDaySinceUnixTime() == PlayerPrefs.GetInt(Constants.dailyChallengeIDToken))
				handleName = "Cardio" + PlayerPrefs.GetInt(Constants.dailyChallengeIDToken);
			else
				handleName = "Empty";
		}

		// If its "BasedOffMode" then check what game mode we are on and get that 
		if (lbName == "BasedOffMode")
			this.SetCorrectLeaderBoard();

		// Set up for the leader board to be updated
		GameObject lbUpdateGO = GameObject.Find("UpdateSteamLeaderBoard");
		if (lbUpdateGO != null)
			lbUpdateGO.GetComponent<SteamLeaderBoardUpdater>().InitLeaderboard(handleName);

		LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(this.OnLeaderboardFindResult);
		LeaderboardScoresDownloaded = CallResult<LeaderboardScoresDownloaded_t>.Create(this.OnLeaderboardScoresDownloaded);

		// Check if the steam connection is good 
		if (SteamManager.Initialized == true)
		{
			SteamAPICall_t handle = SteamUserStats.FindOrCreateLeaderboard(handleName, ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
			LeaderboardFindResult.Set(handle);
		}

		// Set the leader board to be the one around the player
		this.ChangeScoreBoardType(false);
	}

	// Closes all sockets and kills all threads (This prevents unity from freezing)
	private void OnApplicationQuit()
	{
		if (SteamManager.Initialized == true)
		{
			SteamAPI.RunCallbacks();
			SteamAPI.ReleaseCurrentThreadMemory();
			SteamAPI.Shutdown();
		}
	}

	private void SetCorrectLeaderBoard()
	{
		// Get what game mode (Classic, Daily, Custom) and if we are in cardio mode 
		int gameMode = PlayerPrefs.GetInt(Constants.gameMode);
		bool cardioMode = PlayerPrefs.GetInt("CardioMode") == 1;

		if (gameMode == Constants.gameModeClassic) 
		{
			if (cardioMode == true)
			{
				titleText.text = "CLASSIC MODE (CARDIO)";
				handleName = "HS2ClassicModeCardioLeaderBoard";
			}
			else
			{
				titleText.text = "CLASSIC MODE (SQUAT)";
				handleName = "HS2ClassicModeLeaderBoard";
			}
		}
		else if (gameMode == Constants.gameModeDaily) 
		{
			if (cardioMode == true)
			{
				titleText.text = "DAILY CHALLENGE (CARDIO)";
				if(DaySinceUnixTime.GetDaySinceUnixTime() == PlayerPrefs.GetInt(Constants.dailyChallengeIDToken))
					handleName = "Cardio" + PlayerPrefs.GetInt(Constants.dailyChallengeIDToken);
				else
					handleName = "Empty";
			}
			else
			{
				titleText.text = "DAILY CHALLENGE (SQUAT)";
				if (DaySinceUnixTime.GetDaySinceUnixTime() == PlayerPrefs.GetInt(Constants.dailyChallengeIDToken))
					handleName = "Squat" + PlayerPrefs.GetInt(Constants.dailyChallengeIDToken);
				else
					handleName = "Empty";
			}
		}
		else if (gameMode == Constants.gameModeArcade)
		{
			titleText.text = "ARCADE MODE";
			handleName = "HS2ArcadeLeaderBoard";
		}
		else                    // Custom Routine
		{
			// Delete score board because there is not one for custom routine 
			Destroy(this.gameObject);
		}
	}

	void Update()
	{
		// Run callbacks if steam is all good
		if (SteamManager.Initialized == true)
		{
			SteamAPI.RunCallbacks();
		}
	}

	void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t param, bool ioError)
	{
		// IF the score board has been destroyed 
		if (bodyText == null)
			return;

		if (ioError)
		{
			// Handle score download failure
			Debug.Log("Fail");
			return;
		}

		// You should probably check whether param.m_hSteamLeaderboard is
		// the one you want to handle.
		bool entryAdded = false;
		bodyText.text = "";

		List<string> entries = new List<string>();
		for (int i = 0; i < param.m_cEntryCount; ++i)
		{
			LeaderboardEntry_t entry;
			// I assume you don't store any details
			// Not sure if the array is required to be non-null
			if (SteamUserStats.GetDownloadedLeaderboardEntry(param.m_hSteamLeaderboardEntries, i, out entry, null, 0))
			{
				entryAdded = true;
				string playerName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser);
				entries.Add(string.Format("Position {0}  |  {1} With {2} Walls", entry.m_nGlobalRank, playerName, entry.m_nScore));
				//entries.Add(string.Format("{0}: rank {1}, score {2}", playerName, entry.m_nGlobalRank, entry.m_nScore));
				bodyText.text = bodyText.text + " " + entries[i] + "\n";
			}
		}

		if(entryAdded == false)
			bodyText.text = "Exercise to see where you land...";
	}


	private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_bLeaderboardFound != 0)
		{
			m_SteamLeaderboard = pCallback.m_hSteamLeaderboard;
		}
	}

	/// <summary>
	/// Change if this leader board displays in high score mode or around user mode
	/// </summary>
	public void ChangeScoreBoardType(bool hScore)
	{
		if (SteamManager.Initialized == true)
		{
			if (hScore == true)
			{
				SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 1);

				// Set the downloaded score 
				LeaderboardScoresDownloaded.Set(handle);
			}
			else
			{
				SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -2, 2);

				// Set the downloaded score 
				LeaderboardScoresDownloaded.Set(handle);
			}
		}
	}


}
