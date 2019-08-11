using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GymAndSongSelectControl : MonoBehaviour, IButtonMaster
{
	
	public GenericButton[] gymButtons;
	public GenericButton[] songButtons;
	private string MUSIC_FILE_PATH;

	void Start()
	{
		MUSIC_FILE_PATH = Application.persistentDataPath + "/MusicData.txt";

		for (int i = 0; i < songButtons.Length; i++)
			songButtons[i].Start();
		for (int i = 0; i < gymButtons.Length; i++)
			gymButtons[i].Start();

		gymButtons[0].Select(); // Select Gym one as default 
		for (int i = 0; i < songButtons.Length; i++)
			songButtons[i].Select(); // Select all songs as default 
	}

	public void ButtonPress(string token, GenericButton sender)
	{
		if (token.Contains("Gym"))
			this.DeselectButtonSet(gymButtons, sender);
		else
		{
			GenericButton gb = this.FindButtonWithToken(token, songButtons);
			if (gb.IsSelected() == false)
			{
				if (this.AtleastOneSelected(songButtons) == false)
				{
					gb.ForceSelect();
					gb.Select();
				}
			}
		}
	}

	/// <summary>
	/// Check if any of the buttons are active 
	/// </summary>
	public bool AtleastOneSelected(GenericButton[] buttonSet)
	{
		for (int i = 0; i < buttonSet.Length; i++)
		{
			if (buttonSet[i].IsSelected() == true)
				return true;
		}
		return false;
	}

	/// <summary>
	/// Find the button in the passed array that has the passed token
	/// </summary>
	private GenericButton FindButtonWithToken(string token, GenericButton[] buttonSet)
	{
		for (int i = 0; i < buttonSet.Length; i++)
		{
			if (buttonSet[i].GetToken() == token)
				return buttonSet[i];
		}
		return null;
	}

	/// <summary>
	/// Deselect the given button set
	/// </summary>
	private void DeselectButtonSet(GenericButton[] bSet, GenericButton excludeButton)
	{
		// Deselect all but the excluded button
		for (int i = 0; i < bSet.Length; i++)
		{
			if (bSet[i] != excludeButton)
				bSet[i].Deselect();
		}
	}

	/// <summary>
	/// Make a file with the song names specified by the buttons
	/// </summary>
	public void MakeSongFile()
	{
		// Fill a list with all the gym possibilities 
		List<string> sList = new List<string>();
		for (int i = 0; i < songButtons.Length; i++)
		{
			if (songButtons[i].IsSelected() == true)
				sList.Add(songButtons[i].GetToken());
		}

		// Use the method to fill out the file with all the song names that we want
		this.MakeSongFile(sList);
	}

	/// <summary>
	/// Make a file with the song names specified by the handed in list
	/// </summary>
	public void MakeSongFile(List<string> songNames)
	{
		MUSIC_FILE_PATH = Application.persistentDataPath + "/MusicData.txt";
		// Fill out the file with all the song names that we want
		using (StreamWriter sw = new StreamWriter(MUSIC_FILE_PATH))
		{
			for (int i = 0; i < songNames.Count; i++)
				sw.WriteLine(songNames[i]);
		}
	}

	public int GetSelectedGym()
	{
		// Find the gym that is selected 
		for (int i = 0; i < gymButtons.Length; i++)
		{
			if (gymButtons[i].IsSelected() == true)
				return i;
		}

		// Return -1 for error
		return -1;
	}
}
