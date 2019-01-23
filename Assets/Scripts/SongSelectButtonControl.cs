using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SongSelectButtonControl : MonoBehaviour
{
	public SongSelectButton[] buttons;
	private string MUSIC_FILE_PATH;

	void Start()
	{
		MUSIC_FILE_PATH = Application.persistentDataPath + "/MusicData.txt";

		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Select(); // Select all songs as default 
	}

	public void ClearAllButtons()
	{
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].Deselect();
	}

	/// <summary>
	/// Make a file with the song names specified by the buttons
	/// </summary>
	public void MakeSongFile()
	{
		// Fill a list with all the gym possibilities 
		List<string> sList = new List<string>();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].IsSelected() == true)
				sList.Add(buttons[i].GetSongName());
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
}
