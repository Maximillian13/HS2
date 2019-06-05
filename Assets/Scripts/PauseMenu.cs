// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Valve.VR;

public class PauseMenu : MonoBehaviour
{
	public bool leftHand;               // What hand
	private SteamVR_Input_Sources hand; // Hand

	// The Pause Menu
	public GameObject pauseMenu;
	private bool forceClose;

	private WandControlMenuInteraction[] menuInterations = new WandControlMenuInteraction[2];

	private PlayerHitBox hitBox;
	private HeightCalibrator hCal;

	private Vector3 pausePos;

	private void Start()
	{
		hitBox = GameObject.Find("Player").GetComponent<PlayerHitBox>();
		hCal = GameObject.Find("HeightCalibrator").GetComponent<HeightCalibrator>();

		if (leftHand == true)
			hand = SteamVR_Input_Sources.LeftHand;
		else
			hand = SteamVR_Input_Sources.RightHand;

		// Get the wand info for when we force close 
		menuInterations[0] = this.transform.parent.Find("Controller (left)").GetComponent<WandControlMenuInteraction>();
		menuInterations[1] = this.transform.parent.Find("Controller (right)").GetComponent<WandControlMenuInteraction>();

		pausePos = new Vector3(0, .075f, -.1f);
	}

	// Update is called once per frame
	void Update()
	{
		// Toggle the menu on and off
		if (SteamVR_Actions._default.MenuPress.GetStateDown(hand))
		{
			// If our dom hand is right and this is on the right hand, return
			if (PlayerPrefs.GetInt("DomHand") == 0 && leftHand == false)
				return;

			// If our dom hand is left and this is on the left hand, return
			if (PlayerPrefs.GetInt("DomHand") == 1 && leftHand == true)
				return;

			// If we are still allowed to open the menu
			if (forceClose == false)
				this.SwitchPauseState();
		}
	}

	void SwitchPauseState()
	{
		if(pauseMenu.activeSelf == true)
			this.Unpause();
		else
			pauseMenu.SetActive(true);
	}

	public void Unpause()
	{
		menuInterations[0].ForceExitAllButtons();
		menuInterations[1].ForceExitAllButtons();
		pauseMenu.SetActive(false);
	}

	public void ResetLevel()
	{
		// Save info and reload the level
		hitBox.EndGame(true);
		GameObject.Find("[CameraRig]").transform.Find("Camera").GetComponent<EyeFadeControl>().CloseEyes(2);
		this.Unpause();
		forceClose = true;
	}

	public void MainMenu()
	{
		// Save info and load the main menu
		hitBox.EndGame(true);
		GameObject.Find("[CameraRig]").transform.Find("Camera").GetComponent<EyeFadeControl>().CloseEyes(1, true);
		this.Unpause();
		forceClose = true;
	}

	public void HeightUpDown(float val)
	{
		hCal.ChangeHeight(val);
	}

	public void SetPauseButton(GameObject newPauseMenu)
	{
		if (this.pauseMenu != null)
			Destroy(this.pauseMenu.gameObject);

		this.pauseMenu = newPauseMenu;
		this.pauseMenu.transform.parent = this.transform;
		this.pauseMenu.transform.localPosition = pausePos;
		this.pauseMenu.gameObject.SetActive(false);
	}
}
