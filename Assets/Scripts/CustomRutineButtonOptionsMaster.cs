using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRutineButtonOptionsMaster : MonoBehaviour, IButtonMaster
{
	// All the button sections
	public GenericButton[] warmUpButtons;
	public GenericButton[] xSecondButtons;
	public GenericButton[] everyXWallsButtons;
	public GenericButton[] wallDensityButtons;
	public bool[] wallCardioButtons = new bool[3];
	public bool switchOnBreak;

	// Start is called before the first frame update
	void Start()
    {
		// Set up all buttons 
		for (int i = 0; i < warmUpButtons.Length; i++)
			warmUpButtons[i].Start();
		for (int i = 0; i < xSecondButtons.Length; i++)
			xSecondButtons[i].Start();
		for (int i = 0; i < everyXWallsButtons.Length; i++)
			everyXWallsButtons[i].Start();
		for (int i = 0; i < wallDensityButtons.Length; i++)
			wallDensityButtons[i].Start();

		// Default settings 
		warmUpButtons[0].Select();
		xSecondButtons[0].Select();
		everyXWallsButtons[3].Select();

		// Select all the wall densities 
		for (int i = 0; i < wallDensityButtons.Length; i++)
			wallDensityButtons[i].Select();
    }

	public void ButtonPress(string token)
	{
		if (token == "WarmYes" || token == "WarmNo")
			this.DeselectAllWarmUpButtons();
		if (token.Contains("Seconds"))
			this.DeselectAllXSecondButtons();
		if (token.Contains("Walls"))
			this.DeselectAllEveryXWallsButtons();
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

	}

	/// <summary>
	/// Deselect all the warm up buttons 
	/// </summary>
	public void DeselectAllWarmUpButtons()
	{
		for (int i = 0; i < warmUpButtons.Length; i++)
			warmUpButtons[i].Deselect();
	}

	/// <summary>
	/// Deselect all the all x second buttons
	/// </summary>
	public void DeselectAllXSecondButtons()
	{
		for (int i = 0; i < xSecondButtons.Length; i++)
			xSecondButtons[i].Deselect();
	}

	/// <summary>
	/// Deselect all the every x walls buttons
	/// </summary>
	public void DeselectAllEveryXWallsButtons()
	{
		for (int i = 0; i < everyXWallsButtons.Length; i++)
			everyXWallsButtons[i].Deselect();
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
		customRutineStrings[0] = warmUpButtons[0].IsSelected().ToString();
		
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
		customRutineStrings[3] = wallCardioButtons[0] + " " + wallCardioButtons[1] + " " + wallCardioButtons[2];

		customRutineStrings[4] = switchOnBreak.ToString();

		return customRutineStrings;
	}
}
