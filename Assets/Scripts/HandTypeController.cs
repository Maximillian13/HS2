// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Valve.VR;

public class HandTypeController : MonoBehaviour
{
	public GameObject[] hands;
	public bool leftHand;
	private SteamVR_Input_Sources hand;

	public SteamVR_Action_Vector2 tPad;
	// To know where on the touch pad its being pressed
	private Vector2 touchPadPos;

	private AudioSource music;

	// Use this for initialization
	void Start()
	{
		if (leftHand == true)
			hand = SteamVR_Input_Sources.LeftHand;
		else
			hand = SteamVR_Input_Sources.RightHand;

		GameObject musicGo = GameObject.Find("Music");
		if (musicGo != null)
			music = musicGo.GetComponent<AudioSource>();

		// Get the right hand from player prefs and put it on
		this.UpdateHandModels(PlayerPrefs.GetInt("HandInd"));
		this.HandSelector(0);
	}

	public void UpdateHandModels(int modelIndex)
	{
		// Destroy all old hands
		for (int i = 0; i < hands.Length; i++)
			Destroy(hands[i].gameObject);

		// Get all the hands 
		string whichHand = leftHand == true ? "Left" : "Right";
		hands[0] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandRest"), this.transform);
		hands[1] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandThumbUp"), this.transform);
		hands[2] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandFlip"), this.transform);
		hands[3] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandClenched"), this.transform);
		hands[4] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandClap"), this.transform);
		hands[5] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandPoint"), this.transform);

		// Rename everything
		hands[0].name = "HandRest";
		hands[1].name = "HandThumbUp";
		hands[2].name = "HandFlip";
		hands[3].name = "HandClenched";
		hands[4].name = "HandClap";
		hands[5].name = "HandPoint";

		// Position the hands correctly
		for (int i = 0; i < hands.Length; i++)
			hands[i].transform.localPosition = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
		if (SteamVR_Input._default.inActions.MenuPress.GetState(SteamVR_Input_Sources.RightHand) && SceneManager.GetActiveScene().buildIndex == 2)
		{
			if(music != null)
				music.mute = !music.mute;
		}
		if (SteamVR_Input._default.inActions.TriggerPress.GetState(hand)) // If the user presses the "trigger" button
		{
			this.HandSelector(1);
		}
		else if (SteamVR_Input._default.inActions.GripPress.GetState(hand)) // If the user presses the side-button
		{
			this.HandSelector(5);
		}
		else if (SteamVR_Input._default.inActions.TrackPadPress.GetState(hand)) // If the user presses the tack-pad
		{
			touchPadPos = tPad.GetAxis(hand);
			if (touchPadPos.x < .5f && touchPadPos.x > -.5f && touchPadPos.y > .5f) // Up
				this.HandSelector(3);

			else if (touchPadPos.x < .5f && touchPadPos.x > -.5f && touchPadPos.y < -.5f) // Down
				this.HandSelector(4);

			else if (touchPadPos.y < .5f && touchPadPos.y > -.5f && touchPadPos.x < -.5f) // Left
				this.HandSelector(2);

			else if (touchPadPos.y < .5f && touchPadPos.y > -.5f && touchPadPos.x > .5f) // Right
				this.HandSelector(2);
		}
		else // Normal hands 
		{
			this.HandSelector(0);
		}
	}

	private void HandSelector(int index)
	{
		for (int x = 0; x < hands.Length; x++)
		{
			if (x != index)
				hands[x].SetActive(false);
			else
				hands[x].SetActive(true);
		}
	}
}
