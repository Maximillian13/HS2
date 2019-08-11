// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Valve.VR;

public class HandTypeController : MonoBehaviour
{
	private GameObject[] hands = new GameObject[12];
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
			if(hands[i] != null)
				Destroy(hands[i].gameObject);

		// Get all the hands 
		string whichHand = leftHand == true ? "Left" : "Right";
		hands[0] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandRest"), this.transform);
		hands[1] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandPoint"), this.transform);
		hands[2] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandClenched"), this.transform);
		hands[3] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandThumbsUp"), this.transform);
		hands[4] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandShaka"), this.transform);
		hands[5] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandPeace"), this.transform);
		hands[6] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandHorns"), this.transform);
		hands[7] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandHeart"), this.transform);
		hands[8] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandCrossed"), this.transform);
		hands[9] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandPinch"), this.transform);
		hands[10] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandGun"), this.transform);
		hands[11] = Instantiate<GameObject>(Resources.Load<GameObject>("Hands/Hands" + modelIndex + "/" + whichHand + "/HandFlip"), this.transform);

		// Rename everything
		hands[0].name = "HandRest";
		hands[1].name = "HandPoint";
		hands[2].name = "HandClenched";
		hands[3].name = "HandThumbsUp";
		hands[4].name = "HandShaka";
		hands[5].name = "HandPeace";
		hands[6].name = "HandHorns";
		hands[7].name = "HandHeart";
		hands[8].name = "HandCrossed";
		hands[9].name = "HandPinch";
		hands[10].name = "HandGun";
		hands[11].name = "HandFlip";

		// Position the hands correctly
		for (int i = 0; i < hands.Length; i++)
			hands[i].transform.localPosition = new Vector3(0, 0, -.1f);
	}

	// Update is called once per frame
	void Update()
	{
		if (SteamVR_Actions._default.MenuPress.GetState(SteamVR_Input_Sources.RightHand) && SceneManager.GetActiveScene().buildIndex == 2)
		{
			if(music != null)
				music.mute = !music.mute;
		}
		if (SteamVR_Actions._default.TriggerPress.GetState(hand)) // If the user presses the "trigger" button
		{
			this.HandSelector(2);
		}
		else if (SteamVR_Actions._default.GripPress.GetState(hand)) // If the user presses the side-button
		{
			this.HandSelector(1);
		}
		else if (SteamVR_Actions._default.TrackPadPress.GetState(hand)) // If the user presses the tack-pad
		{
			touchPadPos = tPad.GetAxis(hand);
			if (touchPadPos.x < .5f && touchPadPos.x > -.5f && touchPadPos.y > .5f) // Up
				this.HandSelector(PlayerPrefs.GetInt("EmoteUp"));

			else if (touchPadPos.y < .5f && touchPadPos.y > -.5f && touchPadPos.x > .5f) // Right
				this.HandSelector(PlayerPrefs.GetInt("EmoteRight"));

			else if (touchPadPos.x < .5f && touchPadPos.x > -.5f && touchPadPos.y < -.5f) // Down
				this.HandSelector(PlayerPrefs.GetInt("EmoteDown"));

			else if (touchPadPos.y < .5f && touchPadPos.y > -.5f && touchPadPos.x < -.5f) // Left
				this.HandSelector(PlayerPrefs.GetInt("EmoteLeft"));
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
