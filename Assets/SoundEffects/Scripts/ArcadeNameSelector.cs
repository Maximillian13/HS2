using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArcadeNameSelector : MonoBehaviour, IButtonMaster
{
	public TextMeshPro[] fullNameText;
	public TextMeshPro currLetterText;
	public MeshRenderer mr;
	public Material[] greens;

	private int playerScore = 0;
	private string[] charecters;
	private int currentCharIndex;
	private int currNameCharIndex;

	private float growTimer = float.PositiveInfinity;
	private bool shrink;
	private float timer;

	// Start is called before the first frame update
	void Start()
	{
		charecters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N",
			"O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", ".", "?", "!" };
	}

	public void ButtonPress(string token, GenericButton sender)
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
		mr.materials[0] = greens[0];
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
			mr.materials[0] = greens[1];
	}

	/// <summary>
	/// Upload score and shrink/destroy this object 
	/// </summary>
	public void UploadAndDestroy(string name)
	{
		LocalArcadeLeaderBoard localLB = GameObject.Find("LocalArcadeLeaderBoard").GetComponent<LocalArcadeLeaderBoard>();
		localLB.AddEntryAndShowPlacment(name, playerScore);
		growTimer = float.PositiveInfinity;
		shrink = true;

		GameModeMaster gameMaster = GameObject.Find("GameModeMaster").GetComponent<GameModeMaster>();
		gameMaster.PreventLevelFromLoading = false;
	}

	// Update is called once per frame
	void Update()
	{
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
		if (Time.time > growTimer)
		{
			float scaleTime = Time.deltaTime * 2;
			this.transform.localScale = new Vector3(this.transform.localScale.x + scaleTime, this.transform.localScale.y + scaleTime, this.transform.localScale.z + scaleTime);
			if(this.transform.localScale.x >= 1)
			{
				this.transform.localScale = Vector3.one;
				growTimer = float.PositiveInfinity;
			}
		}

		if(shrink == true)
		{
			float scaleTime = Time.deltaTime * 2;
			this.transform.localScale = new Vector3(this.transform.localScale.x - scaleTime, this.transform.localScale.y - scaleTime, this.transform.localScale.z - scaleTime);
			if (this.transform.localScale.x < 0)
				Destroy(this.gameObject);
		}
	}

	public void SetToSize()
	{
		growTimer = Time.time + 1;
	}

	public void SetPlayerScore(int score)
	{
		this.playerScore = score;
	}

}
