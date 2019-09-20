// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Valve.VR;

public class PressAnyButtonToContinueWarning : MonoBehaviour
{
	public SpriteRenderer[] WarningMessages; // The messages 
	public SpriteRenderer pressAnyButtonToContinue; // The "Press any button to continue" Message
	private AudioSource music; // The music that will be playing

	private bool loading; // If its loading the next level
	private float timer; // How long until the user can skip the scene 
	private float alpha; // The alpha of all the messages 
	private float alphaBTC; // The alpha of the "Press any button to continue" Message
	private float vol; // The volume of the game music

	// Use this for initialization
	void Start()
	{
		// Get the controller and the music 
		if (GameObject.FindWithTag("Music") != null)
			music = GameObject.FindWithTag("Music").GetComponent<AudioSource>();

		// Set the alpha of the "Press any button to continue" Message to 0
		pressAnyButtonToContinue.color = new Color(pressAnyButtonToContinue.color.r, pressAnyButtonToContinue.color.g, pressAnyButtonToContinue.color.b, 0);

		// Set up others
		alpha = 1;
		alphaBTC = 0;
		vol = 1;
	}

	// Update is called once per frame
	void Update()
	{
		// Update how long you have been in the game
		timer += Time.deltaTime;

		// If its been 7.5 seconds 
		if (timer > 7.5f)
		{


			// Let the user skip
			if (loading == false)
			{
				alphaBTC += .01f;
				if (alphaBTC > 1)
				{
					alpha = 1;
				}

				// Let the user know they can skip
				pressAnyButtonToContinue.color = new Color(pressAnyButtonToContinue.color.r, pressAnyButtonToContinue.color.g, pressAnyButtonToContinue.color.b, alphaBTC);
				if (SteamVR_Actions._default.TriggerPress.GetStateDown(SteamVR_Input_Sources.Any) ||
					SteamVR_Actions._default.TrackPadPress.GetStateDown(SteamVR_Input_Sources.Any) ||
					SteamVR_Actions._default.MenuPress.GetStateDown(SteamVR_Input_Sources.Any) ||
					SteamVR_Actions._default.GripPress.GetStateDown(SteamVR_Input_Sources.Any))
				{
					loading = true;
				}
			}
		}
	}

	void FixedUpdate()
	{
		// If its ready to load the next level
		if (loading == true)
		{
			// Se the volume and warnings to 0
			for (int x = 0; x < WarningMessages.Length; x++)
			{
				alpha -= .003f;
				vol -= .0015f;
				WarningMessages[x].color = new Color(WarningMessages[x].color.r, WarningMessages[x].color.g, WarningMessages[x].color.b, alpha);
				pressAnyButtonToContinue.color = new Color(pressAnyButtonToContinue.color.r, pressAnyButtonToContinue.color.g, pressAnyButtonToContinue.color.b, alpha);
				if (music != null)
				{
					music.volume = vol;
				}
			}

			// If the alpha is -.25 (a little bit of time scenes it went away from the user)
			if (alpha < -.25f)
			{
				// Destroy music and load game
				if (music != null)
				{
					Destroy(music.gameObject);
				}
				SceneManager.LoadScene(2);
			}

		}
	}
}
