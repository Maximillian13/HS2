using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour, IInteractibleButton
{
	public string buttonSetToken;   // What the button will do
	private MeshRenderer mr;        // For highlighting
	private MainMenuControl mmc;
	//private bool highLightOverRide;

	// Setup
	void Start()
	{
		mr = this.GetComponent<MeshRenderer>();
		mr.enabled = false;
		mmc = GameObject.Find("MainMenuControl").GetComponent<MainMenuControl>();
	}


	// At the start of an interaction
	public void PressButton()
	{
		// If token is not empty then move to whatever menu we are told
		if(buttonSetToken != "")
			mmc.TransitionToMenu(buttonSetToken); 
	}


	/// <summary>
	/// Highlight with color if highlight == true, if false take away highlight
	/// </summary>
	public void HighLight(bool highLight)
	{
		mr.enabled = highLight;
	}

	/// <summary>
	/// Set the token for this button
	/// </summary>
	public void SetButtonToken(string newToken)
	{
		this.buttonSetToken = newToken;
	}
}
