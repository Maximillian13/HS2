using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOutSelectControl : MonoBehaviour
{
	public WorkOutSelectButton[] buttons;
	public MainMenuButton continueButton;

	void Start()
	{
		buttons[0].Select(); // Select squat as default 
	}

	public void ClearAllButtons()
	{
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Deselect();
	}

	/// <summary>
	/// Set the game mode via player prefs
	/// </summary>
	public void SetGameModePlayerPref()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].IsSelected() == true)
				PlayerPrefs.SetInt("GameMode", i);
		}
	}

	/// <summary>
	/// Set the token for the continue button to load whatever menu
	/// </summary>
	public void SetContinueButtonToken(string t)
	{
		continueButton.SetButtonToken(t);
	}
}
