using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticSelectionButton : MonoBehaviour, IInteractibleButton
{
	public bool gloves;
	public int cosmeticIndex;
	public Material[] mats; // 0 = green, 1 = blue 
	private bool highlighted;
	private MeshRenderer mr; // For highlighting
	private bool selected;

	private CosmeticSelectionControl master;
	private HandTypeController[] handTypeCont = new HandTypeController[2];
	private PlayerModelControl playerBodyCont;

	// Setup
	public void Start()
	{
		mr = this.GetComponent<MeshRenderer>();
		mr.enabled = false;
		Transform mTrans = this.transform;
		while (master == null)
		{
			mTrans = mTrans.parent;
			if(mTrans.GetComponent<CosmeticSelectionControl>() != null)
				master = mTrans.GetComponent<CosmeticSelectionControl>();
		}
		handTypeCont[0] = GameObject.Find("[CameraRig]").transform.Find("Controller (left)").GetComponent<HandTypeController>();
		handTypeCont[1] = GameObject.Find("[CameraRig]").transform.Find("Controller (right)").GetComponent<HandTypeController>();
		playerBodyCont = GameObject.Find("Player").GetComponent<PlayerModelControl>();
	}

	void FixedUpdate()
	{
		// Shouldn't need this but its not working so Im going to do it anyway 🤔
		if (selected == true)
			mr.enabled = true;
	}

	// At the start of an interaction
	public void PressButton()
	{
		// Select or deselect button
		if (highlighted == false)
			this.Select();
	}

	/// <summary>
	/// Highlight the button but do not activate it
	/// </summary>
	public void HighlightButton()
	{
		master.ClearAllButtons(); // Clear all highlighted buttons 
		highlighted = true;
		mr.material = mats[1];
		mr.enabled = true;
		selected = true;
	}

	/// <summary>
	/// Select this button
	/// </summary>
	public void Select()
	{
		// Highlight the button
		this.HighlightButton();

		// Change hands
		if (gloves == true)
		{
			handTypeCont[0].UpdateHandModels(cosmeticIndex);
			handTypeCont[1].UpdateHandModels(cosmeticIndex);
			PlayerPrefs.SetInt("HandInd", cosmeticIndex);
		}
		else
		{
			playerBodyCont.UpdateBodyModel(cosmeticIndex, PlayerPrefs.GetInt("BodyType"));
			PlayerPrefs.SetInt("BodyInd", cosmeticIndex);
		}
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
