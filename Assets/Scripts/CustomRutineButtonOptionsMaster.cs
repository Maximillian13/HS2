using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomRutineButtonOptionsMaster : MonoBehaviour, IButtonMaster
{
	// All the button sections
	public GenericButton warmUpButton;
	public GenericButton haveBreaksButton;
	public GenericButton handGuardButton;
	public GenericButton switchGameModeButton;
	public GenericButton[] breakLengthDownUp;
	public GenericButton[] wallsUntilBreakDownUp;
	public GenericButton[] wallDensityButtons;
	public GenericButton[] wallOpeningButtons;
	public GenericButton[] livesButtons;
	public GenericButton[] speedMultButtons;

	public TextMeshPro[] breakTexts;
	public TextMeshPro livesText;
	public TextMeshPro speedMultText;

	private int breakLengthCounter;
	private int wallsUntilBreakCounter;

	private int lives;
	private float speedMult = 1;

	// Start is called before the first frame update
	void Start()
    {
		// Set up all buttons 
		warmUpButton.Start();
		haveBreaksButton.Start();
		handGuardButton.Start();
		switchGameModeButton.Start();
		for (int i = 0; i < breakLengthDownUp.Length; i++)
			breakLengthDownUp[i].Start();
		for (int i = 0; i < wallsUntilBreakDownUp.Length; i++)
			wallsUntilBreakDownUp[i].Start();
		for (int i = 0; i < wallDensityButtons.Length; i++)
			wallDensityButtons[i].Start();
		for (int i = 0; i < wallOpeningButtons.Length; i++)
			wallOpeningButtons[i].Start();
		for (int i = 0; i < livesButtons.Length; i++)
			livesButtons[i].Start();
		for (int i = 0; i < speedMultButtons.Length; i++)
			speedMultButtons[i].Start();

		// Default settings 
		warmUpButton.Select();
		handGuardButton.Select();
		switchGameModeButton.Deselect();
		haveBreaksButton.Deselect();
		this.EnableOrDisableBreakButtons(false);

		// Select all the wall densities 
		for (int i = 0; i < wallDensityButtons.Length; i++)
			wallDensityButtons[i].Select();
		for (int i = 0; i < wallOpeningButtons.Length; i++)
			wallOpeningButtons[i].Select();

		// Set up for the counters
		breakLengthCounter = 30;
		wallsUntilBreakCounter = 50;
	}

	public void ButtonPress(string token)
	{
		if(token == "HaveBreaksYes")
			this.EnableOrDisableBreakButtons(!haveBreaksButton.IsSelected());

		if (token.Contains("BreakLength"))
			this.HandleBreakLength(token);

		if (token.Contains("WallsUntilBreak"))
			this.HandleWallUntilBreak(token);

		if (token.Contains("Lives"))
			this.HandleLives(token);

		if (token.Contains("SpeedMult"))
			this.HandleSpeedMult(token);

		if (token.Contains("Thickness"))
		{
			GenericButton gb = this.FindButtonWithToken(token, wallDensityButtons);
			if (gb.IsSelected() == false)
			{
				if (this.AtleastOneSelected(wallDensityButtons) == false)
				{
					gb.ForceSelect();
					gb.Select();
				}
			}
		}

		if (token.Contains("Opening"))
		{
			GenericButton gb = this.FindButtonWithToken(token, wallOpeningButtons);
			if (gb.IsSelected() == false)
			{
				if (this.AtleastTwoSelected(wallOpeningButtons) == false)
				{
					gb.ForceSelect();
					gb.Select();
				}
			}
		}

	}

	/// <summary>
	/// Handles the button presses for the speed mult option
	/// </summary>
	private void HandleLives(string token)
	{
		if (token.Contains("Down"))
		{
			lives -= 1;
			if (lives < 1)
				lives = 1;
		}
		else
		{
			lives += 1;
			if (lives > 6)
				lives = 6;
		}
		// if we are at six, mark as infinity and make it so the player can not lose 
		if (lives == 6)
			livesText.text = "INF";
		else
			livesText.text = lives.ToString();
	}

	/// <summary>
	/// Handles the button presses for the speed mult option
	/// </summary>
	private void HandleSpeedMult(string token)
	{
		if (token.Contains("Down"))
		{
			speedMult -= .1f;
			if (speedMult < .8f)
				speedMult = .8f;
		}
		else
		{
			speedMult += .1f;
			if (speedMult > 1.2f)
				speedMult = 1.2f;
		}
		speedMultText.text = speedMult.ToString();
	}

	/// <summary>
	/// Handles the up and down presses for the Break length option
	/// </summary>
	private void HandleBreakLength(string token)
	{
		if(token.Contains("Down"))
		{
			breakLengthCounter -= 5;
			if (breakLengthCounter < 10)
				breakLengthCounter = 10;
		}
		else
		{
			breakLengthCounter += 5;
			if (breakLengthCounter > 120)
				breakLengthCounter = 120;
		}
		breakTexts[0].text = breakLengthCounter.ToString();
	}

	/// <summary>
	/// Handles the up and down presses for the wall until break option
	/// </summary>
	private void HandleWallUntilBreak(string token)
	{
		if (token.Contains("Down"))
		{
			wallsUntilBreakCounter -= 5;
			if (wallsUntilBreakCounter < 10)
				wallsUntilBreakCounter = 10;
		}
		else
		{
			wallsUntilBreakCounter += 5;
			if (wallsUntilBreakCounter > 999)
				wallsUntilBreakCounter = 999;
		}
		breakTexts[1].text = wallsUntilBreakCounter.ToString();
	}

	/// <summary>
	/// Enables or disables the break option buttons and sets the text accordingly 
	/// </summary>
	/// <param name="en"></param>
	private void EnableOrDisableBreakButtons(bool en)
	{
		for (int i = 0; i < breakLengthDownUp.Length; i++)
			breakLengthDownUp[i].gameObject.SetActive(en);

		for (int i = 0; i < wallsUntilBreakDownUp.Length; i++)
			wallsUntilBreakDownUp[i].gameObject.SetActive(en);

		switchGameModeButton.gameObject.SetActive(en);

		// Switch text to display correctly depending on the active state 
		if(en == true)
		{
			breakTexts[0].text = breakLengthCounter.ToString();
			breakTexts[1].text = wallsUntilBreakCounter.ToString();
		}
		else
		{
			breakTexts[0].text = "N/A";
			breakTexts[1].text = "N/A";
		}
	}

	/// <summary>
	/// Deselect all the warm up buttons 
	/// </summary>
	private void DeselectButtonSet(GenericButton[] bSet)
	{
		for (int i = 0; i < bSet.Length; i++)
			bSet[i].Deselect();
	}

	/// <summary>
	/// Check if any of the buttons are active 
	/// </summary>
	private bool AtleastOneSelected(GenericButton[] buttonSet)
	{
		for(int i = 0; i < buttonSet.Length; i++)
		{
			if(buttonSet[i].IsSelected() == true)
				return true;
		}
		return false;
	}

	/// <summary>
	/// Check if any two buttons are active 
	/// </summary>
	private bool AtleastTwoSelected(GenericButton[] buttonSet)
	{
		// Loop through all buttons in button set incrementing counter when finding a selected button
		int counter = 0;
		for (int i = 0; i < buttonSet.Length; i++)
		{
			if (buttonSet[i].IsSelected() == true)
				counter++;
		}

		// If less than 2 selected return false
		return counter > 1;
	}

	/// <summary>
	/// Find the button in the passed array that has the passed token
	/// </summary>
	private GenericButton FindButtonWithToken(string token, GenericButton[] buttonSet)
	{
		for(int i = 0; i < buttonSet.Length; i++)
		{
			if (buttonSet[i].GetToken() == token)
				return buttonSet[i];
		}
		return null;
	}

	/// <summary>
	/// Return a summary of the options picked\n
	/// [0] = True or false, true if we want warm up, false if we dont 
	/// [1] = How long the break in seconds, then after how many walls (-1 if never)
	/// [2] = What kind of walls true, 
	/// </summary>
	/// <returns></returns>
	public string[] GetCustomRutineSummary()
	{
		string[] customRutineStrings = new string[8];

		// If warmUpButton[0] is selected then warm up == true 
		customRutineStrings[0] = warmUpButton.IsSelected().ToString();
		
		// Set the value of the break length
		customRutineStrings[1] = breakLengthCounter + " ";

		// If we do not have breaks set the wall count to int.Max, else set it to the specified value
		if(haveBreaksButton.IsSelected() == false)
			customRutineStrings[1] += int.MaxValue;
		else
			customRutineStrings[1] += wallsUntilBreakCounter;

		// Hand placement on or off
		customRutineStrings[2] = handGuardButton.IsSelected().ToString();

		// Check all the different wall densities and fill them in order of 1, 2, 3 (true if button is active, false if not)
		customRutineStrings[3] = wallDensityButtons[0].IsSelected() == true ? "True" : "False";
		customRutineStrings[3] += wallDensityButtons[1].IsSelected() == true ? " True " : " False ";
		customRutineStrings[3] += wallDensityButtons[2].IsSelected() == true ? "True" : "False";

		// Check all the different wall types for cardio and fill them in order of left, mid, right (true if button is active, false if not)
		customRutineStrings[4] = wallOpeningButtons[0].IsSelected() + " " + wallOpeningButtons[1].IsSelected() + " " + wallOpeningButtons[2].IsSelected();

		// Fill in the option if we will be switching mode on breaks
		customRutineStrings[5] = switchGameModeButton.IsSelected().ToString();

		// Fill out the amount of lives and speed multiplier
		customRutineStrings[6] = lives.ToString();
		customRutineStrings[7] = speedMult.ToString();

		return customRutineStrings;
	}
}
