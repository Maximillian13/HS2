using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamLeaderButtonControl : MonoBehaviour, IButtonMaster
{
	public SteamLeaderBoard leaderBoard;
	public GenericButton[] buttons;
	public string[] lbHandels;
	public string[] lbNames;
	private int lbIndex;

	private int updateCounter;

	// Start is called before the first frame update
	void Start()
    {
		lbIndex = 0;
		updateCounter = 0;
	}

	public void FixedUpdate()
	{
		if (updateCounter == 15)
		{
			// Needs to be done this way so when it loads from the server it doesnt get fucked
			this.UpdateAroundYou();
			updateCounter = 0;
		}
		updateCounter++;
	}

	public void ButtonPress(string token, GenericButton sender)
	{
		// If clicking the next button
		if(token == "Next")
		{
			lbIndex++;
			if (lbIndex == lbHandels.Length)
				lbIndex = 0;

			leaderBoard.ChangeScoreBoard(lbHandels[lbIndex], lbNames[lbIndex]);
		}
	}

	public void UpdateAroundYou()
	{
		// Check what kind of board 
		if (buttons[1].IsSelected() == true)
			leaderBoard.ChangeScoreBoardAroundType(false);
		else
			leaderBoard.ChangeScoreBoardAroundType(true);
	}

}
