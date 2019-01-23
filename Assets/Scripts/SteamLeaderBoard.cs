using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Steamworks;

public class SteamLeaderBoard : MonoBehaviour
{
	//public TextMesh tm; // The text mesh that will display the leader board
	//private bool haveLeaderBoard; // The bool that is responsible for knowing when the leader board is downloaded 

	//private SteamLeaderboard_t m_SteamLeaderboard;
	//private CallResult<LeaderboardFindResult_t> LeaderboardFindResult;
	//private CallResult<LeaderboardScoresDownloaded_t> LeaderboardScoresDownloaded;

	//public void OnEnable()
	//{
	//	LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
	//	LeaderboardScoresDownloaded = CallResult<LeaderboardScoresDownloaded_t>.Create(onLeaderboardScoresDownloaded);

	//	// Check if the steam connection is good 
	//	if (SteamManager.Initialized == true)
	//	{
	//		SteamAPICall_t handle = SteamUserStats.FindLeaderboard("HOT_SQUAT_LB");
	//		LeaderboardFindResult.Set(handle);
	//	}
	//}

	//void Update()
	//{
	//	// Run callbacks if steam is all good
	//	if (SteamManager.Initialized == true)
	//	{
	//		SteamAPI.RunCallbacks();
	//	}

	//	// After .5 seconds load in the leader board
	//	if (Time.timeSinceLevelLoad > .5f && haveLeaderBoard == false)
	//	{
	//		// Check if the steam connection is good 
	//		if (SteamManager.Initialized == true)
	//		{
	//			SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -2, 2);
	//			// Set the downloaded score 
	//			LeaderboardScoresDownloaded.Set(handle);
	//			haveLeaderBoard = true;
	//		}
	//	}
	//}

	//void onLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t param, bool ioError)
	//{
	//	if (ioError)
	//	{
	//		// Handle score download failure
	//		Debug.Log("Fail");
	//		return;
	//	}

	//	// You should probably check whether param.m_hSteamLeaderboard is
	//	// the one you want to handle.

	//	tm.text = "";
	//	List<string> entries = new List<string>();
	//	for (int i = 0; i < param.m_cEntryCount; ++i)
	//	{
	//		LeaderboardEntry_t entry;
	//		// I assume you don't store any details
	//		// Not sure if the array is required to be non-null
	//		if (SteamUserStats.GetDownloadedLeaderboardEntry(param.m_hSteamLeaderboardEntries, i, out entry, null, 0))
	//		{
	//			string playerName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser);
	//			entries.Add(string.Format("Position {0} || {1} with {2} squats", entry.m_nGlobalRank, playerName, entry.m_nScore));
	//			//entries.Add(string.Format("{0}: rank {1}, score {2}", playerName, entry.m_nGlobalRank, entry.m_nScore));
	//			tm.text = tm.text + " " + entries[i] + "\n";
	//		}
	//	}
	//}


	//private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure)
	//{
	//	if (pCallback.m_bLeaderboardFound != 0)
	//	{
	//		m_SteamLeaderboard = pCallback.m_hSteamLeaderboard;
	//	}
	//}


}
