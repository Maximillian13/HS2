﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionButtonMaster : MonoBehaviour
{
	// A lot of this stuff seems backwards and weird but it is because PlayerPrefs starts at 0 when
	// the game is loaded for the first time. So by doing it this way we dont need any additional set up

	// All the button sections
	public OptionButton[] domHandButtons;
	public OptionButton[] musicVolButtons;
	public OptionButton[] snapTeleButtons;

	public TextMeshPro musicVolText;

	private TeleportControl[] handTport = new TeleportControl[2];

	private int musicVol = 10;

	private MusicVolumeControl volControl;

	// Start is called before the first frame update
	void Start()
	{
		handTport[0] = GameObject.Find("[CameraRig]").transform.Find("Controller (left)").GetComponent<TeleportControl>();
		handTport[1] = GameObject.Find("[CameraRig]").transform.Find("Controller (right)").GetComponent<TeleportControl>();

		// Set up all buttons 
		for (int i = 0; i < domHandButtons.Length; i++)
			domHandButtons[i].Start();

		// Get the default hand 
		if(PlayerPrefs.GetInt("DomHand") == 1)
			domHandButtons[0].Select();
		else
			domHandButtons[1].Select();

		// Get the volume (Music always starts at 10 and PlayerPrefs give a - offset)
		musicVol += PlayerPrefs.GetInt("MusicVol");
		musicVolText.text = musicVol.ToString();

		volControl = GameObject.Find("MainMenuMusic").GetComponent<MusicVolumeControl>();

		// Set up buttons for snapTele
		for (int i = 0; i < snapTeleButtons.Length; i++)
			snapTeleButtons[i].Start();
		// Select the correct one
		if (PlayerPrefs.GetInt("SnapTele") == 0)
			snapTeleButtons[1].Select();
		else
			snapTeleButtons[0].Select();
	}

	public void OptionButtonPress(int buttonId)
	{
		// Set the dom hand
		if (buttonId == 0 || buttonId == 1)
		{
			this.DeslectDomHandButtons();
			this.DomHandPress(buttonId);
		}

		if (buttonId == 2 || buttonId == 3)
			this.MusicVolumePress(buttonId);

		if (buttonId == 4 || buttonId == 5)
		{
			this.DeslectSnapTeleButtons();
			this.SnapTeleportPress(buttonId);
		}
	}

	// Set the dom hand based off what button is pressed
	private void DomHandPress(int buttonId)
	{
		// If left set DomHand to 1 and if right set DomHand to 0 
		if (buttonId == 0) 
			PlayerPrefs.SetInt("DomHand", 1);
		else
			PlayerPrefs.SetInt("DomHand", 0);

		// Update t-port hand
		handTport[0].DeleteTeleporterBase();
		handTport[1].DeleteTeleporterBase();
		handTport[0].CreaterTeleporterBase();
		handTport[1].CreaterTeleporterBase();
	}

	// Pressing on the volume button
	private void MusicVolumePress(int buttonId)
	{
		if(buttonId == 2)
		{
			musicVol--;
			if (musicVol < 0)
				musicVol = 0;
		}
		if (buttonId == 3)
		{
			musicVol++;
			if (musicVol > 10)
				musicVol = 10;
		}
		musicVolText.text = musicVol.ToString();
		PlayerPrefs.SetInt("MusicVol", musicVol - 10);

		// Update music volume 
		volControl.UpdateVolume();
	}

	private void SnapTeleportPress(int buttonId)
	{
		if (buttonId == 4)
			PlayerPrefs.SetInt("SnapTele", 1);
		else
			PlayerPrefs.SetInt("SnapTele", 0);

		handTport[0].UpdateSnapTeleport();
		handTport[1].UpdateSnapTeleport();
	}

	/// <summary>
	/// Deselect all the warm up buttons 
	/// </summary>
	private void DeslectDomHandButtons()
	{
		for (int i = 0; i < domHandButtons.Length; i++)
			domHandButtons[i].Deselect();
	}

	/// <summary>
	/// Deselect all the snap tele buttons
	/// </summary>
	private void DeslectSnapTeleButtons()
	{
		for (int i = 0; i < snapTeleButtons.Length; i++)
			snapTeleButtons[i].Deselect();
	}
}
