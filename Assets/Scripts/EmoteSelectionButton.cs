using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteSelectionButton : MonoBehaviour
{
	public int emoteIndex;
	public Material[] mats; // 0 = green, 1 = blue 
	private EmoteSelectionControl master;
	private bool highlighted;
	private MeshRenderer mr; // For highlighting
	private bool selected;

	private GameObject[] emoteWheel;

	// Start is called before the first frame update
	void Start()
    {
		mr = this.GetComponent<MeshRenderer>();
		mr.enabled = false;

		master = this.transform.parent.parent.GetComponent<EmoteSelectionControl>();
		// Get all the emote wheel game objects (top, right, bottom, left) and disable them
		Transform wheelParent = this.transform.parent.Find("EmoteMap");
		emoteWheel = new GameObject[wheelParent.childCount];

		for (int i = 0; i < emoteWheel.Length; i++)
			emoteWheel[i] = wheelParent.GetChild(i).gameObject;

		for (int i = 0; i < emoteWheel.Length; i++)
			emoteWheel[i].SetActive(false);
	}


	// At the start of an interaction
	public void PressButton(Vector2 touchPos)
	{
		this.HighlightButton();

		if (touchPos.x < .5f && touchPos.x > -.5f && touchPos.y > .5f) // Up
			this.MoveEmoteMarker(0, "EmoteUp");

		else if (touchPos.y < .5f && touchPos.y > -.5f && touchPos.x > .5f) // Right
			this.MoveEmoteMarker(1, "EmoteRight");

		else if (touchPos.x < .5f && touchPos.x > -.5f && touchPos.y < -.5f) // Down
			this.MoveEmoteMarker(2, "EmoteDown");

		else if (touchPos.y < .5f && touchPos.y > -.5f && touchPos.x < -.5f) // Left
			this.MoveEmoteMarker(3, "EmoteLeft");
	}

	private void MoveEmoteMarker(int whatBind, string playerPrefName)
	{
		master.RemoveBinding(whatBind);
		emoteWheel[whatBind].SetActive(true);
		master.RemoveHighlightIfNess();
		PlayerPrefs.SetInt(playerPrefName, emoteIndex);
	}

	/// <summary>
	/// Get the emote wheel (0 = up, 1 = right, 2 = bottom, 3 = left)
	/// </summary>
	public GameObject[] GetEmoteWheel()
	{
		return emoteWheel;
	}

	/// <summary>
	/// Disables section of the emote wheel for this button (0 = up, 1 = right, 2 = bottom, 3 = left)
	/// </summary>
	public void DisableSpecificWheelSection(int section)
	{
		emoteWheel[section].SetActive(false);
	}

	/// <summary>
	/// Highlight the button but do not activate it
	/// </summary>
	public void HighlightButton()
	{
		highlighted = true;
		mr.material = mats[1];
		mr.enabled = true;
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
