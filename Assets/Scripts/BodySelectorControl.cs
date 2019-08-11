using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySelectorControl : MonoBehaviour, IButtonMaster
{
	public GenericButton leftButton;
	public GenericButton selectButton;
	public GenericButton rightButton;

	public GenericButton[] bodyButtons;

	private bool[] prevEnabled = new bool[3];

	public CosmeticsTrackControl trackControl;

	private BodyDisplayHelper[] bodyDisplayHelpers;

	private PlayerModelControl playerBodyCont;

	private AudioSource sound;

	private void Start()
	{
		Transform displayBodyParent = this.transform.parent.Find("DisplayBodies");
		bodyDisplayHelpers = new BodyDisplayHelper[displayBodyParent.childCount];
		for (int i = 0; i < bodyDisplayHelpers.Length; i++)
			bodyDisplayHelpers[i] = displayBodyParent.GetChild(i).GetComponent<BodyDisplayHelper>();

		leftButton.Start();
		selectButton.Start();
		rightButton.Start();

		for (int i = 0; i < bodyButtons.Length; i++)
			bodyButtons[i].Start();
		bodyButtons[PlayerPrefs.GetInt("BodyType")].SelectHighLight();

		playerBodyCont = GameObject.Find("Player").GetComponent<PlayerModelControl>();

		sound = this.GetComponent<AudioSource>();
	}

	// For when the button gets pressed
	public void ButtonPress(string token, GenericButton sender)
	{
		// Move bodies 
		if (token == "Left")
			trackControl.LeftPress();
		if (token == "Right")
			trackControl.RightPress();
		if (token == "Select")
			this.Select();

		// pick body types 
		if (token == "Androg")
			this.SwitchBodyType(0);
		if (token == "Male")
			this.SwitchBodyType(1);
		if (token == "Female")
			this.SwitchBodyType(2);
	}

	private void SwitchBodyType(int bodyTypeIndex)
	{
		// Deselect all other buttons 
		this.DeselectBodies(bodyTypeIndex);
		PlayerPrefs.SetInt("BodyType", bodyTypeIndex);

		// Switch player model with new body type 
		playerBodyCont.UpdateBodyModel(PlayerPrefs.GetInt("BodyInd"), bodyTypeIndex);

		// Switch track models with new body type
		for (int i = 0; i < bodyDisplayHelpers.Length; i++)
			bodyDisplayHelpers[i].SetActiveBodyType(bodyTypeIndex);
	}

	// Deselect all buttons besides the one with the token you pass
	private void DeselectBodies(int bodySkipInd)
	{
		for(int i = 0; i < bodyButtons.Length; i++)
		{
			if (i == bodySkipInd)
				continue;
			bodyButtons[i].Deselect();
		}

	}

	public void EnableDisableButton(string name, bool enable)
	{
		if (name == "Left")
			leftButton.EnableDisable(enable);
		if (name == "Right")
			rightButton.EnableDisable(enable);
		if (name == "Select")
			selectButton.EnableDisable(enable);
	}

	public void DisableAll()
	{
		prevEnabled[0] = leftButton.IsEnabled();
		prevEnabled[1] = rightButton.IsEnabled();
		prevEnabled[2] = selectButton.IsEnabled();

		leftButton.EnableDisable(false);
		rightButton.EnableDisable(false);
		selectButton.EnableDisable(false);
	}

	public void ReEnable()
	{
		leftButton.EnableDisable(prevEnabled[0]);
		rightButton.EnableDisable(prevEnabled[1]);
		selectButton.EnableDisable(prevEnabled[2]);
	}

	// Select the current body if unlocked
	private void Select()
	{
		// Play the sound
		sound.pitch = Random.Range(.95f, 1.05f);
		sound.Play();

		// Set it as the current body 
		int cosmeticIndex = trackControl.GetBodyIndex();
		playerBodyCont.UpdateBodyModel(cosmeticIndex, PlayerPrefs.GetInt("BodyType"));
		PlayerPrefs.SetInt("BodyInd", cosmeticIndex);
	}

	public void SetSelectButtonSelected(bool select)
	{
		if (playerBodyCont == null)
			this.Start();

		if (select == true)
			selectButton.SelectHighLight();
		else
			selectButton.Deselect();
	}


}
