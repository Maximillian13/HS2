using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamShutDown : MonoBehaviour
{
	private void OnDestroy()
	{
		if (SteamManager.Initialized == true)
		{
			//SteamAPI.RunCallbacks();
			SteamAPI.Shutdown();
		}
	}
}
