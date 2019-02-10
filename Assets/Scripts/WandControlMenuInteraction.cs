// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

// Dont need this class as of now but maybe in the future
public class WandControlMenuInteraction : MonoBehaviour
{
	public bool leftHand;				// What hand
	private SteamVR_Input_Sources hand; // Hand

	// List of all buttons that you are in the collider with 
	private List<IInteractibleButton> collidedButtons = new List<IInteractibleButton>();

	// Set hand
	private void Start()
	{
		if (leftHand == true)
			hand = SteamVR_Input_Sources.LeftHand;
		else
			hand = SteamVR_Input_Sources.RightHand;
	}

	// Called every frame
	void Update()
	{
		//if (SteamVR_Input._default.inActions.MenuPress.GetStateDown(hand))
		//	UnityEngine.SceneManagement.SceneManager.LoadScene(0);

		// IF the trigger is pressed 
		if (SteamVR_Input._default.inActions.TriggerPress.GetStateDown(hand))
		{
			// If we are just interacting with exactly 1 button
			if(collidedButtons.Count == 1)
				collidedButtons[0].PressButton(); // Go to the button script and do what it says
		}
	}

	// Bug: When hovering over multiple option v fast it breaks button presses. (Keep testing but it might be fixed)

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

