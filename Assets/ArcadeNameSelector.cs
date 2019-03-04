﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArcadeNameSelector : MonoBehaviour, IButtonMaster
{
	public TextMeshPro[] fullNameText;
	public TextMeshPro currLetterText;
	public TextMeshPro nextText;

	private int playerScore = 0;
	private string[] charecters;
	private int currentCharIndex;
	private int currNameCharIndex;

	private bool grow;
	private bool shrink;
	private float timer;

	// Todo: Fix order of score, Git rid of hand box when selecting 

	// Start is called before the first frame update
	void Start()
	{
		charecters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N",
			"O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", ".", "?", "!" };
	}

	public void ButtonPress(string token)
	{
		if (token == "Left" || token == "Right")
			this.LeftRightPress(token);

		if (token == "BackSpace")
			this.BackSpace();

		if (token == "Next")
			this.Next();
	}
	
	// Move the letters left and right (loop if need be)
	private void LeftRightPress(string direction)
	{
		if(direction == "Left")
		{
			currentCharIndex--;
			if (currentCharIndex < 0)
				currentCharIndex = charecters.Length - 1;
		}
		else
		{
			currentCharIndex++;
			if (currentCharIndex >= charecters.Length)
				currentCharIndex = 0;
		}

		currLetterText.text = charecters[currentCharIndex];
	}

	public void BackSpace()
	{
		// If we are at the first one we cant go back any further
		if (currNameCharIndex == 0)
			return;

		// In case we were on a blank 
		if(currNameCharIndex < fullNameText.Length)
			fullNameText[currNameCharIndex].text = "_";

		// Move back
		currNameCharIndex--;

		// Set that char to be "_" and reset timer to prevent quick blinking 
		fullNameText[currNameCharIndex].text = "_";
		timer = 0;

		// Change the next button to say next instead of submit
		nextText.text = "Next";
	}

	public void Next()
	{
		// If we are done 
		if(currNameCharIndex == fullNameText.Length)
		{
			string name = fullNameText[0].text + fullNameText[1].text + fullNameText[2].text;

			// Check for bad words >:(
			if(name == "FAG" || name == "NIG" || name == "KKK" || name == "HH!" || name == "NGR" || name == "CUM" || name == "JAP")
				return;

			// Send to the endgame method
			this.UploadAndDestroy(name);
			return;
		}

		fullNameText[currNameCharIndex].text = charecters[currentCharIndex];
		currNameCharIndex++;

		if (currNameCharIndex == 3)
			nextText.text = "Submit";
	}

	/// <summary>
	/// Upload score and shrink/destroy this object 
	/// </summary>
	public void UploadAndDestroy(string name)
	{
		LocalArcadeLeaderBoard localLB = GameObject.Find("LocalArcadeLeaderBoard").GetComponent<LocalArcadeLeaderBoard>();
		localLB.AddEntryAndShowPlacment(name, playerScore);
		grow = false;
		shrink = true;

		GameModeMaster gameMaster = GameObject.Find("GameModeMaster").GetComponent<GameModeMaster>();
		gameMaster.PreventLevelFromLoading = false;
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.A))
		//	this.LeftRightPress("Left");
		//if (Input.GetKeyDown(KeyCode.D))
		//	this.LeftRightPress("Right");
		//if (Input.GetKeyDown(KeyCode.E))
		//	this.Next();
		//if (Input.GetKeyDown(KeyCode.Q))
		//	this.BackSpace();

		//if (Input.GetKeyDown(KeyCode.B))
		//	grow = true;
		//if (Input.GetKeyDown(KeyCode.C))
		//	shrink = true;

		// Switch between "_" and " " to simulate blinking
		timer += Time.deltaTime;
		if(timer >= .5f)
		{
			// If it is completely filled out nothing should blink
			if(currNameCharIndex < fullNameText.Length)
				fullNameText[currNameCharIndex].text = (fullNameText[currNameCharIndex].text == "_") ? " " : "_";

			timer = 0;
		}
	}

	// Grow or shrink this
	void FixedUpdate()
	{
		if (grow == true)
		{
			this.transform.localScale = new Vector3(this.transform.localScale.x + .033f, this.transform.localScale.y + .025f, this.transform.localScale.z + .004166f);
			if(this.transform.localScale.x >= 2)
			{
				this.transform.localScale = new Vector3(2, 1.5f, .25f);
				grow = false;
			}
		}

		if(shrink == true)
		{
			this.transform.localScale = new Vector3(this.transform.localScale.x - .033f, this.transform.localScale.y - .025f, this.transform.localScale.z - .004166f);
			if (this.transform.localScale.x < 0)
				Destroy(this.gameObject);
		}
	}

	public void SetToSize()
	{
		grow = true;
	}

	public void SetPlayerScore(int score)
	{
		this.playerScore = score;
	}

}
