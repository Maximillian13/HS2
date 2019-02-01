using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRutineButtonOptionsMaster : MonoBehaviour, IButtonMaster
{
	// All the button sections
	public GenericButton warmUpButton;
	public GenericButton switchGameModeButton;
	public GenericButton[] xSecondButtons;
	public GenericButton[] everyXWallsButtons;
	public GenericButton[] wallDensityButtons;
	public GenericButton[] wallOpeningButtons;

	// Start is called before the first frame update
	void Start()
    {
		// Set up all buttons 
		warmUpButton.Start();
		switchGameModeButton.Start();
		for (int i = 0; i < xSecondButtons.Length; i++)
			xSecondButtons[i].Start();
		for (int i = 0; i < everyXWallsButtons.Length; i++)
			everyXWallsButtons[i].Start();
		for (int i = 0; i < wallDensityButtons.Length; i++)
			wallDensityButtons[i].Start();
		for (int i = 0; i < wallOpeningButtons.Length; i++)
			wallOpeningButtons[i].Start();

		// Default settings 
		warmUpButton.Select();
		switchGameModeButton.Deselect();
		xSecondButtons[0].Select();
		everyXWallsButtons[3].Select();

		// Select all the wall densities 
		for (int i = 0; i < wallDensityButtons.Length; i++)
			wallDensityButtons[i].Select();
		for (int i = 0; i < wallOpeningButtons.Length; i++)
			wallOpeningButtons[i].Select();
	}

	public void ButtonPress(string token)
	{
		if (token == "WarmYes" || token == "WarmNo")
			return;
		if (token.Contains("Seconds"))
			this.DeselectButtonSet(xSecondButtons);
		if (token.Contains("Walls"))
			this.DeselectButtonSet(everyXWallsButtons);
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
	/// Deselect all the warm up buttons 
	/// </summary>
	public void DeselectButtonSet(GenericButton[] bSet)
	{
		for (int i = 0; i < bSet.Length; i++)
			bSet[i].Deselect();
	}

	/// <summary>
	/// Check if any of the buttons are active 
	/// </summary>
	public bool AtleastOneSelected(GenericButton[] buttonSet)
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
	public bool AtleastTwoSelected(GenericButton[] buttonSet)
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
		string[] customRutineStrings = new string[5];

		// If warmUpButton[0] is selected then warm up == true 
		customRutineStrings[0] = warmUpButton.IsSelected().ToString();
		
		// Loop through the x second break and save how many seconds by taking the index and mult by 10
		for(int i = 0; i < xSecondButtons.Length; i++)
		{
			if (xSecondButtons[i].IsSelected() == true)
				customRutineStrings[1] = ((i + 1) * 10) + " ";
		}

		// Check Every X Wall and append that value on to the end of the string
		if(everyXWallsButtons[0].IsSelected() == true)
			customRutineStrings[1] += 25;
		if (everyXWallsButtons[1].IsSelected() == true)
			customRutineStrings[1] += 50;
		if (everyXWallsButtons[2].IsSelected() == true)
			customRutineStrings[1] += 100;
		if (everyXWallsButtons[3].IsSelected() == true)
			customRutineStrings[1] += int.MaxValue;

		// Check all the different wall densities and fill them in order of 1, 2, 3 (true if button is active, false if not)
		customRutineStrings[2] = wallDensityButtons[0].IsSelected() == true ? "True" : "False";
		customRutineStrings[2] += wallDensityButtons[1].IsSelected() == true ? " True " : " False ";
		customRutineStrings[2] += wallDensityButtons[2].IsSelected() == true ? "True" : "False";

		// Check all the different wall types for cardio and fill them in order of left, mid, right (true if button is active, false if not)
		customRutineStrings[3] = wallOpeningButtons[0].IsSelected() + " " + wallOpeningButtons[1].IsSelected() + " " + wallOpeningButtons[2].IsSelected();

		// Fill in the 
		customRutineStrings[4] = switchGameModeButton.IsSelected().ToString();

		return customRutineStrings;
	}
}
