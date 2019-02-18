using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamLeaderButtonControl : MonoBehaviour, IButtonMaster
{
	public GenericButton[] buttons;
	public SteamLeaderBoard[] leaderBoards;


	// Start is called before the first frame update
	void Start()
    {
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Start();
		buttons[0].Select();
    }

	public void ButtonPress(string token)
	{
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Deselect();

		if (token == "YScore")
		{
			for (int i = 0; i < leaderBoards.Length; i++)
				leaderBoards[i].ChangeScoreBoardType(false);
		}
		else
		{
			for (int i = 0; i < leaderBoards.Length; i++)
				leaderBoards[i].ChangeScoreBoardType(true);
		}
	}

}
