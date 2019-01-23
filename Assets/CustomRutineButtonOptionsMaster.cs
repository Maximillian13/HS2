using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRutineButtonOptionsMaster : MonoBehaviour
{
	// All the button sections
	public WarmUpSelectButton[] warmUpButtons;
	public BreakOptionsButton[] xSecondButtons;
	public BreakOptionsButton[] everyXWallsButtons;
	public WallDensityButton[] wallDensityButtons;

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

    // Update is called once per frame
    void Update()
    {
        
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
	public bool WallDensityButtonSelected()
	{
		for(int i = 0; i < wallDensityButtons.Length; i++)
		{
			if(wallDensityButtons[i].IsSelected() == true)
				return true;
		}
		return false;
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
		string[] customRutineStrings = new string[3];

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

		return customRutineStrings;
	}
}
