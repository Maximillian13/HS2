using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamLeaderBoardUpdater : MonoBehaviour
{
	private SteamLeaderboard_t m_SteamLeaderboard;
	private CallResult<LeaderboardScoreUploaded_t> LeaderboardScoreUploaded;
	private CallResult<LeaderboardFindResult_t> LeaderboardFindResult;


	public void OnEnable()
	{
		if (SteamManager.Initialized == true)
		{
			LeaderboardScoreUploaded = CallResult<LeaderboardScoreUploaded_t>.Create(OnLeaderboardScoreUploaded);
			LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);

			SteamAPICall_t handle = SteamUserStats.FindLeaderboard("HS2ClassicModeLeaderBoard");
			LeaderboardFindResult.Set(handle);
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
			// Todo: Continue to test this. Seems to be an issue sending info to the steam server but will work on restarting steam (maybe just a dev issue) 
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
