using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOutSelectControl : MonoBehaviour, IButtonMaster
{
	public GenericButton[] buttons;
	public MainMenuButton continueButton;

	void Start()
	{
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Start();
		buttons[0].Select(); // Select squat as default 
	}

	private void ClearAllButtons()
	{
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Deselect();
	}

	/// <summary>
	/// Set the game mode via player prefs
	/// </summary>
	public void ButtonPress(string token)
	{
		this.ClearAllButtons();
		if(token == "Squat")
			PlayerPrefs.SetInt("CardioMode", 0);
		else
			PlayerPrefs.SetInt("CardioMode", 1);
	}

	/// <summary>
	/// Set the token for the continue button to load whatever menu
	/// </summary>
	public void SetContinueButtonToken(string t)
	{
		continueButton.SetButtonToken(t);
	}
}
