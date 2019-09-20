using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamLeaderBoardUpdater : MonoBehaviour
{
	private SteamLeaderboard_t m_SteamLeaderboard;
	private CallResult<LeaderboardScoreUploaded_t> LeaderboardScoreUploaded;
	private CallResult<LeaderboardFindResult_t> LeaderboardFindResult;
	private string lbNameTest;

	public void InitLeaderboard(string lbName)
	{ 
		if (SteamManager.Initialized == true)
		{
			LeaderboardScoreUploaded = CallResult<LeaderboardScoreUploaded_t>.Create(this.OnLeaderboardScoreUploaded);
			LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(this.OnLeaderboardFindResult);
			SteamAPICall_t handle = SteamUserStats.FindLeaderboard(lbName);
			LeaderboardFindResult.Set(handle);
			lbNameTest = lbName;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (SteamManager.Initialized == true)
		{
			SteamAPI.RunCallbacks();
		}
	}

	public void UpdateLeaderBoard(int numOfSquats)
	{
		if (SteamManager.Initialized == true)
		{
			SteamAPICall_t handle = SteamUserStats.UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, (int)numOfSquats, null, 0);
			LeaderboardScoreUploaded.Set(handle);
			SteamAPI.RunCallbacks();
		}
	}


	private void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t pCallback, bool bIOFailure)
	{
		//Debug.Log("[" + LeaderboardScoreUploaded_t.k_iCallback + " - LeaderboardScoreUploaded] - " + pCallback.m_bSuccess + " -- " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_nScore + " -- " + pCallback.m_bScoreChanged + " -- " + pCallback.m_nGlobalRankNew + " -- " + pCallback.m_nGlobalRankPrevious);
	}

	private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_bLeaderboardFound != 0)
		{
			m_SteamLeaderboard = pCallback.m_hSteamLeaderboard;
		}
	}
}
