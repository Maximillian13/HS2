using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton : MonoBehaviour, IInteractibleButton
{
	public string token;
	public bool highlightingWanted;
	public bool multiHighlightAllow;
	public bool toggleButton;
	public bool goToMasterOnDeselect;
	public Material[] mats; // 0 = green, 1 = blue 
	private bool highlighted;
	private MeshRenderer mr; // For highlighting
	private bool selected;

	public IButtonMaster master;

	// Setup
	public void Start()
	{
		mr = this.GetComponent<MeshRenderer>();
		mr.enabled = false;

		// Grab the master script wherever it is 
		if (this.transform.parent.parent.GetComponent<IButtonMaster>() != null)
			master = this.transform.parent.parent.GetComponent<IButtonMaster>();
		else
			master = this.transform.parent.parent.parent.GetComponent<IButtonMaster>();
	}

	void FixedUpdate()
	{
		// Shouldn't need this but its not working so Im going to do it anyway 🤔
		if (selected == true && highlightingWanted == true)
			mr.enabled = true;
	}

	// At the start of an interaction
	public void PressButton()
	{
		// If a toggle button just turn it on and off
		if(toggleButton == true)
		{
			if (highlighted == false)
				this.Select();
			else
				this.Deselect(goToMasterOnDeselect);
			return;
		}

		// Select or deselect button
		if (multiHighlightAllow == false)
		{
			if (highlightingWanted == true)
				this.Select();
			else if (highlighted == false)
				this.Select();
		}
		else
		{
			if (highlighted == false)
				this.Select();
			else
				this.Deselect(true);
		}
	}

	/// <summary>
	/// Select this button
	/// </summary>
	public void Select()
	{
		master.ButtonPress(token);
		if (highlightingWanted == true)
		{
			this.SelectHighLight();
		}
		selected = true;
	}

	public void SelectHighLight()
	{
		highlighted = true;
		mr.material = mats[1];
		mr.enabled = true;
		selected = true;
	}

	public void ForceSelect()
	{
		selected = true;
	}

	/// <summary>
	/// De-select this button
	/// </summary>
	public void Deselect(bool checkMaster = false)
	{
		highlighted = false;
		mr.material = mats[0];
		mr.enabled = false;
		selected = false;
		if (checkMaster == true)
			master.ButtonPress(token);
	}

	/// <summary>
	/// Highlight with color if highlight == true, if false take away highlight
	/// </summary>
	public void HighLight(bool highLight)
	{
		// If overridden, dont disable the highlight 
		if (selected == true && highlightingWanted == true)
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

	public string GetToken()
	{
		return this.token;
	}
}
