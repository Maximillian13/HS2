// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

// Dont need this class as of now but maybe in the future
public class WandControlMenuInteraction : MonoBehaviour
{
	public bool leftHand;               // What hand'
	public SteamVR_Action_Vector2 tPad;
	private Vector2 touchPadPos;

	private SteamVR_Input_Sources hand; // Hand

	// List of all buttons that you are in the collider with 
	private List<IInteractibleButton> collidedButtons = new List<IInteractibleButton>();
	private List<EmoteSelectionButton> emoteButtons = new List<EmoteSelectionButton>();


	public AudioSource[] audioSorces;


	// Set hand
	private void Start()
	{
		if (leftHand == true)
			hand = SteamVR_Input_Sources.LeftHand;
		else
			hand = SteamVR_Input_Sources.RightHand;

		audioSorces = this.GetComponents<AudioSource>();
	}

	// Called every frame
	void Update()
	{
		// Clear if we have multiple 
		if (emoteButtons.Count > 1)
			emoteButtons.Clear();

		// IF the trigger is pressed 
		if (SteamVR_Actions._default.TriggerPress.GetStateDown(hand))
		{
			// This is litrally disgusting, do not look bellow 😞

			// If we are just interacting with exactly 1 button
			if (collidedButtons.Count == 1)
			{
				collidedButtons[0].PressButton(); // Go to the button script and do what it says
				try
				{
					GenericButton gb = (GenericButton)collidedButtons[0];
					if (gb.GetSpecailSound() == "Outfit")
					{
						audioSorces[1].volume = ((PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f) / 2f;
						audioSorces[1].Play();
						//Debug.Log(audioSorces[1].clip.name + " Played at vol: " + audioSorces[1].volume);
						return;
					}
				}
				catch
				{
					try
					{
						CosmeticSelectionButton gb = (CosmeticSelectionButton)collidedButtons[0];
						audioSorces[2].volume = ((PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f) / 2f;
						audioSorces[2].Play();
						//Debug.Log(audioSorces[2].clip.name + " Played at vol: " + audioSorces[2].volume);
						return;
					}
					catch { }
				}
				//audioSorce.pitch = Random.Range(.95f, 1.05f);
				audioSorces[0].volume = ((PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f) / 4f;
				audioSorces[0].Play();
				//Debug.Log(audioSorces[0].clip.name + " Played at vol: " + audioSorces[0].volume);
			}
		}

		// If the track pad is pressed (For emote selection)
		if (SteamVR_Actions._default.TrackPadPress.GetStateDown(hand))
		{
			// If we are just interacting with exactly 1 button
			if (emoteButtons.Count == 1)
			{
				emoteButtons[0].PressButton(tPad.GetAxis(hand)); // Go to the button script and do what it says
				audioSorces[0].volume = ((PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f) / 4f;
				audioSorces[0].Play();
			}
		}
	}

	// Enters button
	void OnTriggerEnter(Collider other)
	{
		// Find what its touching
		IInteractibleButton collidedButton = other.GetComponent<IInteractibleButton>();
		// If its a button
		if (collidedButton != null)
		{
			// Highlight it
			collidedButton.HighLight(true);
			// Add the button to the list 
			collidedButtons.Add(collidedButton);
		}


		// Emote button list
		EmoteSelectionButton emoteButton = other.GetComponent<EmoteSelectionButton>();
		// If its a button
		if (emoteButton != null)
		{
			// Highlight it
			emoteButton.HighLight(true);
			// Add the button to the list 
			emoteButtons.Add(emoteButton);
		}
	}

	//// Enters button
	void OnTriggerStay(Collider other)
	{
		// Find what its touching
		IInteractibleButton collidedButton = other.GetComponent<IInteractibleButton>();
		// If its a button
		if (collidedButton != null)
		{
			// Highlight it
			collidedButton.HighLight(true);
			// Add the button to the list 
			if (collidedButtons.Contains(collidedButton) == false)
				collidedButtons.Add(collidedButton);
		}


		// Emote button list
		EmoteSelectionButton emoteButton = other.GetComponent<EmoteSelectionButton>();
		// If its a button
		if (emoteButton != null)
		{
			// Highlight it
			emoteButton.HighLight(true);
			// Add the button to the list 
			if (emoteButtons.Contains(emoteButton) == false)
				emoteButtons.Add(emoteButton);
		}
	}

	// Leaves button
	void OnTriggerExit(Collider other)
	{
		// Find what its touching
		IInteractibleButton collidedButton = other.GetComponent<IInteractibleButton>();
		// If its a button
		if (collidedButton != null)
		{
			// Set the color to be un-highlighted
			collidedButton.HighLight(false);
			// Remove the button from the list
			collidedButtons.Remove(collidedButton);
		}

		// Emote button list
		EmoteSelectionButton emoteButton = other.GetComponent<EmoteSelectionButton>();
		// If its a button
		if (emoteButton != null)
		{
			// Highlight it
			emoteButton.HighLight(false);
			// Add the button to the list 
			emoteButtons.Remove(emoteButton);
		}
	}

	/// <summary>
	/// Un-highlights and removes all buttons from list
	/// </summary>
	public void ForceExitAllButtons()
	{
		// Un-highlight all 
		for (int i = 0; i < collidedButtons.Count; i++)
			collidedButtons[i].HighLight(false);

		// Remove all from the list
		while (collidedButtons.Count > 0)
			collidedButtons.RemoveAt(0);
	}

}

