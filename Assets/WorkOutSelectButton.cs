﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOutSelectButton : MonoBehaviour, IInteractibleButton
{
	public Material[] mats; // 0 = green, 1 = blue 
	private bool highlighted;
	private MeshRenderer mr; // For highlighting
	private bool selected;

	private WorkOutSelectControl master;

	// Setup
	void Start()
	{
		mr = this.GetComponent<MeshRenderer>();
		mr.enabled = false;
		master = this.transform.parent.parent.GetComponent<WorkOutSelectControl>();
	}

	// At the start of an interaction
	public void PressButton()
	{
		// Select or deselect button
		if (highlighted == false)
			this.Select();
	}

	void FixedUpdate()
	{
		// Shouldn't need this but its not working so Im going to do it anyway 🤔
		if (selected == true)
			mr.enabled = true;
	}

	/// <summary>
	/// Select this button
	/// </summary>
	public void Select()
	{
		if (master == null)
			this.Start();

		master.ClearAllButtons(); // Clear all highlighted buttons 

		highlighted = true;
		mr.material = mats[1];
		mr.enabled = true;
		selected = true;

		master.SetGameModePlayerPref();
	}

	/// <summary>
	/// De-select this button
	/// </summary>
	public void Deselect()
	{
		if (mr == null)
			this.Start();

		highlighted = false;
		mr.material = mats[0];
		mr.enabled = false;
		selected = false;
	}

	/// <summary>
	/// Highlight with color if highlight == true, if false take away highlight
	/// </summary>
	public void HighLight(bool highLight)
	{
		// If overridden, dont disable the highlight 
		if (selected == true)
		{
			mr.enabled = true;
			return;
		}
		mr.enabled = highLight;
	}


	public bool IsSelected()
	{
		return selected;
	}
}
