using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton : MonoBehaviour, IInteractibleButton
{
	public bool press;
	public string token;
	public bool highlightingWanted;
	public bool multiHighlightAllow;
	public bool toggleButton;
	public bool goToMasterOnDeselect;
	public Material[] mats; // 0 = green, 1 = blue 
	private bool highlighted;
	private MeshRenderer mr; // For highlighting
	private bool selected;
	private bool isEnabled = true;

	public IButtonMaster master;

	// The buttons collider (Will be reset on enable/disable to resend trigger info)
	private BoxCollider bCollider;

	// Setup
	public void Start()
	{
		mr = this.GetComponent<MeshRenderer>();
		mr.enabled = false;
		bCollider = this.GetComponent<BoxCollider>();

		Transform p = this.transform.parent;
		while (p.GetComponent<IButtonMaster>() == null)
		{
			p = p.parent;
			if(p == null)
				throw new System.Exception("CANT FIND MASTER CONTOL!!!");
		}

		master = p.GetComponent<IButtonMaster>();
	}

	void Update()
	{
		if(press == true)
		{
			this.PressButton();
			press = false;
		}
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
		if (isEnabled == false)
			return;

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
		if (isEnabled == false)
			return;

		if (highlightingWanted == true)
		{
			this.SelectHighLight();
		}
		selected = true;
		master.ButtonPress(token, this);
	}

	public void SelectHighLight()
	{
		if (isEnabled == false)
			return;

		highlighted = true;
		mr.material = mats[1];
		mr.enabled = true;
		selected = true;
	}

	public void ForceSelect()
	{
		if (isEnabled == false)
			return;
		selected = true;
	}

	/// <summary>
	/// De-select this button
	/// </summary>
	public void Deselect(bool checkMaster = false)
	{
		if (isEnabled == false)
			return;

		highlighted = false;
		mr.material = mats[0];
		mr.enabled = false;
		selected = false;
		if (checkMaster == true)
			master.ButtonPress(token, this);
	}

	/// <summary>
	/// Highlight with color if highlight == true, if false take away highlight
	/// </summary>
	public void HighLight(bool highLight)
	{
		if (isEnabled == false)
			return;
		// If overridden, dont disable the highlight 
		if (selected == true && highlightingWanted == true)
		{
			mr.enabled = true;
			return;
		}
		mr.enabled = highLight;
	}


	/// <summary>
	/// Disable or enable button
	/// </summary>
	public void EnableDisable(bool enable)
	{
		if(enable == true)
		{
			// Bug: Fucks button up because on trigger exit isnt called when disabling a gameobject 😳
			//bCollider.enabled = false;
			//bCollider.enabled = true;
			this.isEnabled = true;
		}
		else
		{
			this.Deselect();
			isEnabled = false;
		}
	}

	/// <summary>
	/// Get if currently selected
	/// </summary>
	public bool IsEnabled()
	{
		return this.isEnabled;
	}

	public bool IsSelected()
	{
		return this.selected;
	}

	//public bool IsHighlighted()
	//{
	//	return this.highlighted;
	//}

	public string GetToken()
	{
		return this.token;
	}
}
