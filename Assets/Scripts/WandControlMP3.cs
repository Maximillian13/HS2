using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class WandControlMP3 : MonoBehaviour
{
	public bool leftHand;
	public SteamVR_Action_Vector2 tPad;
	private Vector2 touchPadPos;

	public SteamVR_Action_Vibration hapticAction;

	private MP3TriggerBox triggerBox;

	private SteamVR_Input_Sources hand;
	private int handInt;

	private MainGameMusicScript musicControl;
	private MusicVolumeControl musicVolControl;

	private bool holdingMp3;

	private Transform mp3Transform;

	private bool moveBack;
	private float alphaTimer;
	private Vector2 letGoEuler;

	// Use this for initialization
	void Start()
	{
		if (leftHand == true)
		{
			handInt = 0;
			hand = SteamVR_Input_Sources.LeftHand;
		}
		else
		{
			handInt = 1;
			hand = SteamVR_Input_Sources.RightHand;
		}

		GameObject musicGo = GameObject.FindWithTag("Music");
		if (musicGo != null)
		{
			musicControl = musicGo.GetComponent<MainGameMusicScript>();
			musicVolControl = musicGo.GetComponent<MusicVolumeControl>();
		}

		triggerBox = GameObject.Find("MP3Trigger").GetComponent<MP3TriggerBox>();
		mp3Transform = triggerBox.transform.Find("MP3Player");
	}

	// Update is called once per frame
	void Update()
	{
		// Move the mp3 player back to chest if need be 
		this.MoveMP3BackToChest();

		// On first press
		this.OnTriggerPress();

		// On hold
		this.OnTriggerHold();

		// On release
		this.OnTriggerRelease();
	}

	/// <summary>
	/// Moves the MP3 Player back to the chest (Must be called in update)
	/// </summary>
	private void MoveMP3BackToChest()
	{
		if (moveBack == true)
		{
			// IDEA: Would be cool to be able to throw out then it comes back to its correct position 
			float step = Time.deltaTime / 1.5f;
			mp3Transform.position = Vector3.MoveTowards(mp3Transform.position, triggerBox.transform.position, step);

			alphaTimer += Time.deltaTime;
			mp3Transform.localEulerAngles = Vector3.Lerp(letGoEuler, new Vector3(-90, 180, 0), alphaTimer);

			mp3Transform.localScale = Vector3.Lerp(mp3Transform.localScale, new Vector3(.4f, .4f, .4f), alphaTimer / 2);

			if (Vector3.Distance(mp3Transform.position, triggerBox.transform.position) < .1f)
			{
				mp3Transform.localScale = new Vector3(.4f, .4f, .4f);
				mp3Transform.SetParent(triggerBox.transform);
				mp3Transform.localEulerAngles = new Vector3(-90, 180, 0);
				mp3Transform.localPosition = Vector3.zero;
				moveBack = false;
			}
		}
	}

	/// <summary>
	/// Take care of the actions when first pressing the trigger
	/// </summary>
	private void OnTriggerPress()
	{
		if (SteamVR_Actions._default.TriggerPress.GetStateDown(hand))
		{
			// If the mp3 is not being held and it is the correct hand in the trigger box 
			if (triggerBox.MP3InHand == false && triggerBox.HandInTriggerBox == handInt)
			{
				triggerBox.MP3InHand = true;
				holdingMp3 = true;

				mp3Transform.SetParent(null);

				// Set up to grow the MP3 player
				alphaTimer = 0;
			}
		}
	}

	/// <summary>
	/// Take care of the actions when holding the trigger button
	/// </summary>
	public void OnTriggerHold()
	{
		if (SteamVR_Actions._default.TriggerPress.GetState(hand) && holdingMp3 == true)
		{
			alphaTimer += Time.deltaTime / 2;
			mp3Transform.localScale = Vector3.Lerp(mp3Transform.localScale, Vector3.one * 1.2f, alphaTimer);

			// Update position to stay with hand
			mp3Transform.position = this.transform.position + new Vector3(-.025f, .05f, -.025f);
			mp3Transform.localEulerAngles = this.transform.localEulerAngles;

			// If we press the track pad
			if (SteamVR_Actions._default.TrackPadPress.GetStateDown(hand))
			{
				// Get the axis of the touch pad and do what needs to be done for that press
				touchPadPos = tPad.GetAxis(hand);
				if (touchPadPos.x < .5f && touchPadPos.x > -.5f && touchPadPos.y > .5f) // Up
					musicVolControl.ChangeCurrentVolumeAndMP3(1);

				else if (touchPadPos.y < .5f && touchPadPos.y > -.5f && touchPadPos.x > .5f) // Right
					musicControl.PlayNextSong();

				else if (touchPadPos.x < .5f && touchPadPos.x > -.5f && touchPadPos.y < -.5f) // Down
					musicVolControl.ChangeCurrentVolumeAndMP3(-1);

				else if (touchPadPos.y < .5f && touchPadPos.y > -.5f && touchPadPos.x < -.5f) // Left
					musicControl.RestartOrPrevSong();
			}
		}
	}

	/// <summary>
	/// Take care of the actions when releasing the trigger button
	/// </summary>
	public void OnTriggerRelease()
	{
		if (SteamVR_Actions._default.TriggerPress.GetStateUp(hand) && holdingMp3 == true)
		{
			// Move the mp3 back 
			moveBack = true;

			triggerBox.MP3InHand = false;
			holdingMp3 = false;

			// Set up to shrink the MP3 player
			alphaTimer = 0;
			letGoEuler = mp3Transform.localEulerAngles;
		}
	}

	/// <summary>
	/// Pulse haptics on this controller 
	/// </summary>
	public void TriggerHaptic(float length, float strength)
	{
		hapticAction.Execute(0, length, 150, strength, hand);
	}
}
