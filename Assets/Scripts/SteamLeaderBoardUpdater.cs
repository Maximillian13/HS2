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

	// Update is called once per frame
	void Update()
	{
		if (SteamManager.Initialized == true)
		{
			SteamAPI.RunCallbacks();
		}

		// Todo: Get rid of this shit
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (SteamManager.Initialized == true)
			{
				SteamAPICall_t handle = SteamUserStats.UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, (int)50, null, 0);
				LeaderboardScoreUploaded.Set(handle);
				Debug.Log("Scores uploaded");
				//print("UploadLeaderboardScore(" + m_SteamLeaderboard + ", ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, " + (int)100 + ", null, 0) - " + handle);
			}
		}
	}

	public void UpdateLeaderBoard(int numOfSquats)
	{
		if (SteamManager.Initialized == true)
		{
			SteamAPICall_t handle = SteamUserStats.UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, (int)numOfSquats, null, 0);
			LeaderboardScoreUploaded.Set(handle);
			Debug.Log("Scores uploaded");
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
