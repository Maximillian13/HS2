using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour, IInteractibleButton
{
	public string buttonSetToken;   // What the button will do
	private MeshRenderer mr;        // For highlighting
	private PauseMenu master;

	// Setup
	void Start()
	{
		mr = this.GetComponent<MeshRenderer>();
		mr.enabled = false;
		master = this.transform.parent.parent.parent.GetComponent<PauseMenu>();
	}


	// At the start of an interaction
	public void PressButton()
	{
		if (buttonSetToken == "Back")
		{
			master.Unpause();
			mr.enabled = false;
		}

		if (buttonSetToken == "Reset")
		{
			master.ResetLevel();
			mr.enabled = false;
		}

		if (buttonSetToken == "MainMenu")
		{
			master.MainMenu();
			mr.enabled = false;
		}

		if (buttonSetToken == "HeightUp")
		{
			master.HeightUpDown(.1f);
		}

		if (buttonSetToken == "HeightDown")
		{
			master.HeightUpDown(-.1f);
		}
	}


	/// <summary>
	/// Highlight with color if highlight == true, if false take away highlight
	/// </summary>
	public void HighLight(bool highLight)
	{
		mr.enabled = highLight;
	}
}
