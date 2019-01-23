using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour, IInteractibleButton
{
	public int id;
	public bool highlightingWanted;
	public Material[] mats; // 0 = green, 1 = blue 
	private bool highlighted;
	private MeshRenderer mr; // For highlighting
	private bool selected;

	private OptionButtonMaster master;

	// Setup
	public void Start()
	{
		mr = this.GetComponent<MeshRenderer>();
		mr.enabled = false;
		master = this.transform.parent.parent.parent.GetComponent<OptionButtonMaster>();
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
		// Select or deselect button
		if(highlightingWanted == true)
			this.Select();
		else if (highlighted == false)
			this.Select();
	}

	/// <summary>
	/// Select this button
	/// </summary>
	public void Select()
	{
		master.OptionButtonPress(id);
		if (highlightingWanted == true)
		{
			highlighted = true;
			mr.material = mats[1];
			mr.enabled = true;
		}
		selected = true;
	}

	/// <summary>
	/// De-select this button
	/// </summary>
	public void Deselect()
	{
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
}
