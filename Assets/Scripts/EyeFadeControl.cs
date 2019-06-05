using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFadeControl : MonoBehaviour
{
	private Material origMat;
	private Material editMat;
	private MeshRenderer mr;

	private bool open;
	private bool close;

	private float curAlpha; // Alpha of mat
	private float mult = 0; // mult to slow the fade at the begging of opening

	private float softAlpha;
	private bool softOpen;

	// Level to load at the end of closing eyes (int.MinValue if no level to load)
	private int levelToLoad = int.MinValue;

	private AudioSource music;
	private float currentVolume;

	// Start is called before the first frame update
	void Start()
    {
		mr = this.transform.Find("EyeCover").GetComponent<MeshRenderer>();
		mr.enabled = true;
		origMat = mr.material;
		editMat = origMat;
		curAlpha = 1;
		softAlpha = 0;

		this.OpenEyes();
    }

	// Update is called once per frame
	void Update()
	{
		// Solve stalemates by opening the eyes 
		if(open == true && close == true)
		{
			open = true;
			close = false;
		}

		// Open eyes 
		if(open == true)
		{
			mult += Time.deltaTime;				// Add to mult so it has a slow start 
			curAlpha -= Time.deltaTime * mult;	// Set up the current alpha 
			editMat.color = new Color(0, 0, 0, curAlpha);
			
			if (curAlpha <= 0) // If we are done fading
			{
				mr.enabled = false;
				open = false;
			}
		}

		// Close eyes
		if(close == true)
		{
			Debug.Log("Increasing Alpha");

			if (music != null)
			{
				currentVolume /= 1.011f + Time.deltaTime;
				music.volume = currentVolume;
			}
			// Subtract mult so we have a slow end 
			if(mult > 0)
				mult -= Time.deltaTime / 2.1f;

			curAlpha += Time.deltaTime * mult;  // Set up the current alpha 
			editMat.color = new Color(0, 0, 0, curAlpha);

			if (curAlpha >= 1) // If we are done fading
			{
				close = false;
				if(levelToLoad != int.MinValue) // If there is a level to load
				{
					// If its a quit command, then quit. Else load the level 
					if (levelToLoad == -1) 
						Application.Quit();
					else
						UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
				}
			}
		}

		if(softOpen == true)
		{
			// Fade to black 
			softAlpha -= Time.deltaTime;  // Set up the current alpha 
			editMat.color = new Color(0, 0, 0, softAlpha);

			// If we are already at 1 just do nothing 
			if (softAlpha <= 0)
			{
				softOpen = false;
				mr.enabled = false;
			}
		}
	}

	// Open eyes 
	private void OpenEyes()
	{
		close = false;
		open = true;
		editMat = origMat;
		editMat.color = new Color(0, 0, 0, 1);
		mr.material = editMat;
		mr.enabled = true;
		curAlpha = 1;
		mult = 0;
	}

	/// <summary>
	/// Fade eyes to black and load a specified level 
	/// (-1 to quit, nothing to not load a level) (kill music true if you want to find and kill music)
	/// </summary>
	public void CloseEyes(int loadLevel = int.MinValue, bool killMusic = false)
	{
		open = false;
		close = true;
		editMat = origMat;
		editMat.color = new Color(0, 0, 0, 0);
		mr.material = editMat;
		mr.enabled = true;
		curAlpha = 0;
		mult = 1;

		if (killMusic == true)
		{
			GameObject musicObj = GameObject.FindWithTag("Music");
			if (musicObj != null)
			{
				music = musicObj.GetComponent<AudioSource>();
				currentVolume = music.volume;
			}
		}

		this.levelToLoad = loadLevel;
	}

	/// <summary>
	/// Closes eyes (Must be called in update or update like function)
	/// </summary>
	public void UpdateCloseEyes(bool quit = false)
	{
		// If we are fading in and out for level stuff ignore this close/open 
		if (open == true || close == true)
			return;

		// If we are already at 1 just do nothing 
		if (softAlpha >= 1)
		{
			if (quit == false)
				return;
			else
			{
				Application.Quit();
				Debug.Log("Quit");
			}
		}

		// Fade to black 
		if(quit == true)
			softAlpha += Time.deltaTime * .5f;  // Set up the current alpha 
		else
			softAlpha += Time.deltaTime * 5;  // Set up the current alpha 

		editMat.color = new Color(0, 0, 0, softAlpha);
		mr.enabled = true;
		softOpen = false;
	}

	/// <summary>
	/// Set your eyes to open after a UpdateCloseEyes() call 
	/// </summary>
	public void SoftEyeOpen()
	{
		// If we are fading in and out for level stuff ignore this close/open 
		if (open == true || close == true)
			return;

		softOpen = true;
	}


}
