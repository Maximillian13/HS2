// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Valve.VR;

public class PressAnyButtonToContinue : MonoBehaviour
{
	private bool loading;
	private float timer;

	// Update is called once per frame
	void Update()
	{
		// If we are loading, stop here 
		if (loading == true)
			return;

		// Move the time up
		timer += Time.deltaTime;

		// If its been over 10 seconds load the next level
		if (timer > 10)
		{
			this.transform.parent.Find("Camera").GetComponent<EyeFadeControl>().CloseEyes(1);
			loading = true;
		}

		// If we detect any input load the level
		if (SteamVR_Actions._default.TriggerPress.GetStateDown(SteamVR_Input_Sources.Any) ||
			SteamVR_Actions._default.TrackPadPress.GetStateDown(SteamVR_Input_Sources.Any) ||
			SteamVR_Actions._default.MenuPress.GetStateDown(SteamVR_Input_Sources.Any) ||
			SteamVR_Actions._default.GripPress.GetStateDown(SteamVR_Input_Sources.Any))
		{
			this.transform.parent.Find("Camera").GetComponent<EyeFadeControl>().CloseEyes(1);
			loading = true;
		}
	}
}
