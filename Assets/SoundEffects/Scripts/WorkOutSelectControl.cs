using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOutSelectControl : MonoBehaviour, IButtonMaster
{
	public GenericButton[] buttons;
	public MainMenuButton continueButton;

	private bool started;

	void Start()
	{
		if (started == true)
			return;

		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Start();
		buttons[0].Select(); // Select squat as default 

		started = true;
	}

	void OnEnable()
	{
		if (started == false)
			this.Start();

		this.ButtonPress("Sqaut", null);
		buttons[0].Select(); // Select squat as default 
	}

	private void ClearAllButtons(GenericButton excludeButton)
	{
		// Deselect all but the excluded button
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i] != excludeButton)
				buttons[i].Deselect();
		}
	}

	/// <summary>
	/// Set the game mode via player prefs
	/// </summary>
	public void ButtonPress(string token, GenericButton sender)
	{
		this.ClearAllButtons(sender);
		if(token == "Squat")
			PlayerPrefs.SetInt(Constants.cardioMode, 0);
		else
			PlayerPrefs.SetInt(Constants.cardioMode, 1);
	}

	/// <summary>
	/// Set the squat cardio button or the normal squat button. String of what game mode
	/// </summary>
	public void SetSquatCardio(string gameMode)
	{
		bool squatCardio = false;
		if (gameMode == "ArcadeMode" || gameMode == "DailyChallenge")
			squatCardio = true;

		buttons[1].transform.parent.gameObject.SetActive(!squatCardio);
		buttons[2].transform.parent.gameObject.SetActive(squatCardio);
	}

	/// <summary>
	/// Set the token for the continue button to load whatever menu
	/// </summary>
	public void SetContinueButtonToken(string t)
	{
		continueButton.SetButtonToken(t);
	}
}
