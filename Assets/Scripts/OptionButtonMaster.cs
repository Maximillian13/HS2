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
	public Transform volumeWheel;
	public Transform needle;
	private int prevVolVal;
	//public GenericButton calorieCounterButton;
	//public GenericButton[] weightUpDown;

	//public TextMeshPro musicVolText;
	//public TextMeshPro weightText;

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
		prevVolVal = musicVol;

		// Set up the volume wheel 
		if (prevVolVal == 0)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, -90);
		if (prevVolVal == 1)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, -65);
		if (prevVolVal == 2)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, -50);
		if (prevVolVal == 3)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, -32);
		if (prevVolVal == 4)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, -15);
		if (prevVolVal == 5)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, 0);
		if (prevVolVal == 6)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, 15);
		if (prevVolVal == 7)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, 32);
		if (prevVolVal == 8)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, 50);
		if (prevVolVal == 9)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, 65);
		if (prevVolVal == 10)
			volumeWheel.transform.localEulerAngles = new Vector3(0, 0, 90);
		//musicVolText.text = musicVol.ToString();

		volControl = GameObject.Find("MainMenuMusic").GetComponent<MusicVolumeControl>();

		//calorieCounterButton.Start();
		// Turn on cal counter button if its has been told to be on in the past
		//int useCalCounterInt = PlayerPrefs.GetInt("UseCalorieCounter");
		//if (useCalCounterInt == 1)
		//	calorieCounterButton.SelectHighLight();

		//// Set up buttons for snapTele
		//for (int i = 0; i < weightUpDown.Length; i++)
		//	weightUpDown[i].Start();
	}

	private void FixedUpdate()
	{
		volumeWheel.localEulerAngles = new Vector3(0, 0, volumeWheel.localEulerAngles.z);
		volumeWheel.localPosition = new Vector3(0, 1.83f, .5f);


		float wheelAngle = volumeWheel.localEulerAngles.z;
		if (wheelAngle > 180)
			wheelAngle -= 360;

		if (wheelAngle > 90)
			volumeWheel.localEulerAngles = new Vector3(0, 0, 90);
		if (wheelAngle < -90)
			volumeWheel.localEulerAngles = new Vector3(0, 0, -90);
		needle.localEulerAngles = volumeWheel.localEulerAngles;

		if (wheelAngle < -73.65f)
			this.MusicVolumePress(0);
		else if (wheelAngle < -57.3f)
			this.MusicVolumePress(1);
		else if (wheelAngle < -40.95f)
			this.MusicVolumePress(2);
		else if (wheelAngle < -24.6f)
			this.MusicVolumePress(3);
		else if (wheelAngle < -8.25f)
			this.MusicVolumePress(4);
		else if (wheelAngle < 8.1f)
			this.MusicVolumePress(5);
		else if (wheelAngle < 24.45f)
			this.MusicVolumePress(6);
		else if (wheelAngle < 40.8f)
			this.MusicVolumePress(7);
		else if (wheelAngle < 57.15f)
			this.MusicVolumePress(8);
		else if (wheelAngle < 73.5f)
			this.MusicVolumePress(9);
		else
			this.MusicVolumePress(10);

	}

	public void ButtonPress(string buttonToken)
	{
		// Set the dom hand
		if (buttonToken == "DomLeft" || buttonToken == "DomRight")
		{
			this.DeslectDomHandButtons();
			this.DomHandPress(buttonToken);
		}

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
	private void MusicVolumePress(int val)
	{
		if (val == prevVolVal)
			return;

		Debug.Log("Vol Set to: " + val);

		// Save the prev value so we dont do this every update only on val change 
		prevVolVal = val;

		//musicVolText.text = musicVol.ToString();
		PlayerPrefs.SetInt("MusicVol", val - 10);

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

		//weightText.text = weight.ToString();
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
