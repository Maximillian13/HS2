using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticSelectionControl : MonoBehaviour
{
	public bool gloves;
	public CosmeticSelectionButton[] buttons;

	void Start()
	{
		// Setup for all the buttons
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Start();

		// Should select whatever one is being currently used 
		int activeButton = 0;
		if (gloves == true)
			activeButton = PlayerPrefs.GetInt("HandInd");
		else
			activeButton = PlayerPrefs.GetInt("BodyInd");

		// Select the currently equipped item 
		buttons[activeButton].HighlightButton(); 
	}

	public void ClearAllButtons()
	{
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Deselect();
	}

	public int GetSelectedGym()
	{
		// Find the gym that is selected 
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].IsSelected() == true)
				return i;
		}

		// Return -1 for random gym
		return -1;
	}
}
