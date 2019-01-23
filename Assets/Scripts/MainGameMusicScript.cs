// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MainGameMusicScript : MonoBehaviour 
{
	private AudioSource audioSorce;

	// A master list and a working list 
	private List<AudioClip> masterSongList;
	private List<AudioClip> workingSongList;

	private string MUSIC_FILE_PATH;

	// Make the main 
	void Start () 
    {
		// If we are coming through here again but we are the old music
		if (audioSorce != null)
			return;

		MUSIC_FILE_PATH = Application.persistentDataPath + "/MusicData.txt"; 

        // If there are multiple music objects destroy the newest one, so it is a continues song
        GameObject[] musicGameObjects = GameObject.FindGameObjectsWithTag("Music");
        if (musicGameObjects.Length > 1)
        {
            // Just to make sure
            if (musicGameObjects[1] != null)
                Destroy(musicGameObjects[1]); // Destroy the newest one
        }
        DontDestroyOnLoad(this.gameObject);

		audioSorce = this.GetComponent<AudioSource>();

		// Load in the correct song 
		// Check if we have already done this 
		if (masterSongList != null)
			return;

		// Make lists
		masterSongList = new List<AudioClip>();
		workingSongList = new List<AudioClip>();

		// Try to read the file and fill out list. If no file found. just put default song
		try
		{
			// Fill out the selected songs from the file 
			using (StreamReader sr = new StreamReader(MUSIC_FILE_PATH))
			{
				while (sr.EndOfStream == false)
				{
					string path = "Sounds/Music/" + sr.ReadLine();
					masterSongList.Add(Resources.Load<AudioClip>(path));
				}
			}
		}
		catch (FileNotFoundException e)
		{
			Debug.Log(e.FileName + " Not found!!!");
			masterSongList.Add(Resources.Load<AudioClip>("Sounds/Music/Sgo"));
		}

		// Fill out the working list so we can remove from it when playing the next song
		this.ListDeepCopy(masterSongList, ref workingSongList);

		// Pick a song to play
		this.PickSongAndPlay();
	}

	private void Update()
	{
		// If we finished playing a song, move to the next one
		if(audioSorce != null)
		{
			if (audioSorce.isPlaying == false)
				this.PickSongAndPlay();
		}
	}

	public void PickSongAndPlay()
	{
		// IF we did not pick any songs
		if (masterSongList.Count == 0)
			return;

		// If we have ran through all the songs and are going to start the loop again 
		if (workingSongList.Count == 0)
			this.ListDeepCopy(masterSongList, ref workingSongList);

		// Pick a random song from the working list, then remove it from the list
		int rand = Random.Range(0, workingSongList.Count);
		audioSorce.clip = workingSongList[rand];
		if(audioSorce.isActiveAndEnabled == true)
			audioSorce.Play();

		workingSongList.RemoveAt(rand);
	}

	// Make a deep copy from a master list to a working list 
	private void ListDeepCopy(List<AudioClip> mList, ref List<AudioClip> wList)
	{
		wList = new List<AudioClip>();
		for (int i = 0; i < mList.Count; i++)
			wList.Add(mList[i]);
	}
}
