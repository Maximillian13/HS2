using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionButtonMaster : MonoBehaviour, IButtonMaster
{
	// A lot of this stuff seems backwards and weird but it is because PlayerPrefs starts at 0 when
	// the game is loaded for the first time. So by doing it this way we dont need any additional set up

	// All the button sections
	public GenericButton[] domHandButtons;
	public GenericButton[] musicVolButtons;
	public GenericButton calorieCounterButton;
	public GenericButton[] weightUpDown;

	public TextMeshPro musicVolText;
	public TextMeshPro weightText;

	private TeleportControl[] handTport = new TeleportControl[2];

	private int weight = 160;
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

		calorieCounterButton.Start();
		// Turn on cal counter button if its has been told to be on in the past
		int useCalCounterInt = PlayerPrefs.GetInt("UseCalorieCounter");
		if (useCalCounterInt == 1)
			calorieCounterButton.SelectHighLight();

		// Set up buttons for snapTele
		for (int i = 0; i < weightUpDown.Length; i++)
			weightUpDown[i].Start();
	}

	public void ButtonPress(string buttonToken)
	{
		// Set the dom hand
		if (buttonToken == "DomLeft" || buttonToken == "DomRight")
		{
			this.DeslectDomHandButtons();
			this.DomHandPress(buttonToken);
		}

		if (buttonToken == "VolDown" || buttonToken == "VolUp")
			this.MusicVolumePress(buttonToken);

		if (buttonToken == "WeightDown" || buttonToken == "WeightUp")
			this.WeightUpDownPress(buttonToken);

		if (buttonToken == "CalorieCounter")
			this.CalorieCounterPress();
	}

	// Set the dom hand based off what button is pressed
	private void DomHandPress(string buttonToken)
	{
		// If left set DomHand to 1 and if right set DomHand to 0 
		if (buttonToken == "DomLeft") 
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
	private void MusicVolumePress(string buttonToken)
	{
		if(buttonToken == "VolDown")
		{
			musicVol--;
			if (musicVol < 0)
				musicVol = 0;
		}
		if (buttonToken == "VolUp")
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

	private void WeightUpDownPress(string buttonToken)
	{
		if (buttonToken == "WeightDown")
		{
			weight -= 5;
			if (weight < 100)
				weight = 100;
		}
		if (buttonToken == "WeightUp")
		{
			weight += 5;
			if (weight > 500)
				weight = 500;
		}

		weightText.text = weight.ToString();
		PlayerPrefs.SetInt("PlayerWeight", weight);
	}

	private void CalorieCounterPress()
	{

		int useCalCounterInt = PlayerPrefs.GetInt("UseCalorieCounter") == 0 ? 1 : 0;
		Debug.Log(useCalCounterInt);
		PlayerPrefs.SetInt("UseCalorieCounter", useCalCounterInt);
	}

	/// <summary>
	/// Deselect all the warm up buttons 
	/// </summary>
	private void DeslectDomHandButtons()
	{
		for (int i = 0; i < domHandButtons.Length; i++)
			domHandButtons[i].Deselect();
	}
}
