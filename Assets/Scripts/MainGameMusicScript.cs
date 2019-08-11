// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MainGameMusicScript : MonoBehaviour 
{
	private MP3PlayerControl mp3;
	private AudioSource audioSorce;

	private float currentSognPlayTime;

	// A master list and a working list 
	//private List<AudioClip> masterSongList;
	private List<AudioClip> workingSongList;

	private string MUSIC_FILE_PATH;

	private int songIndex;

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
			for (int i = 0; i < musicGameObjects.Length; i++)
				if (musicGameObjects[i].name != "KeepMusic")
					Destroy(musicGameObjects[i]);
        }
        DontDestroyOnLoad(this.gameObject);

		// Rename so we destroy the new one next time through 
		this.gameObject.name = "KeepMusic";

		this.GetComponent<MusicVolumeControl>().AudioSetUp();

		audioSorce = this.GetComponent<AudioSource>();

		// Load in the correct song 
		// Check if we have already done this 
		if (workingSongList != null)
			return;

		// Get the MP3 player
		mp3 = GameObject.FindWithTag("Mp3Player").GetComponent<MP3PlayerControl>();

		// Make lists
		//masterSongList = new List<AudioClip>();
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
					workingSongList.Add(Resources.Load<AudioClip>(path));
				}
			}
		}
		catch (FileNotFoundException e)
		{
			Debug.Log(e.FileName + " Not found!!!");
			workingSongList.Add(Resources.Load<AudioClip>("Sounds/Music/Sgo 2"));
		}

		// Randomize the working list 
		workingSongList = this.RandomizeList(workingSongList);

		// Play the next song (ind set at -1 to start at 0)
		songIndex = -1;
		this.PlayNextSong();


		// Old way of doing it 
		// Dont need the masted list for the MP3 style
		// Fill out the working list so we can remove from it when playing the next song
		//this.ListDeepCopy(masterSongList, ref workingSongList);
		// Pick a song to play
		//this.PickSongAndPlayRandom();
	}

	private void Update()
	{
		//Debug.Log("Null boy: " + (mp3 == null));
		//if(mp3 == null)
		//	mp3 = GameObject.FindWithTag("Mp3Player").GetComponent<MP3PlayerControl>();
		// If we finished playing a song, move to the next one
		if (audioSorce != null)
		{
			// If the music has stopped playing, start the next one
			if (audioSorce.isPlaying == false)
				this.PlayNextSong();
			else
				currentSognPlayTime += Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
			this.RestartOrPrevSong();

		if (Input.GetKeyDown(KeyCode.RightArrow))
			this.PlayNextSong();
	}

	/// <summary>
	/// Plays the next song on the random list
	/// </summary>
	public void PlayNextSong()
	{
		// If the list is empty dont do anything 
		if (workingSongList.Count == 0)
			return;

		// Increase and loop if we are at the end
		songIndex++;
		if (songIndex == workingSongList.Count) // Loop at end
			songIndex = 0;
		
		// Get the new song are play it
		audioSorce.clip = workingSongList[songIndex];
		if (audioSorce.isActiveAndEnabled == true)
			audioSorce.Play();

		// Try to find the mp3 
		if (mp3 == null)
			mp3 = this.TryToFindMp3();

		// If the we have the MP3 player display the new song 
		if (mp3 != null)
			mp3.UpdateSongName(workingSongList[songIndex].name);

		// Reset timer for new song 
		currentSognPlayTime = 0;
	}

	/// <summary>
	/// Restarts song or go to previous song if clicked at the begging of the clip
	/// </summary>
	public void RestartOrPrevSong()
	{
		// If the song has been playing for less than 3 seconds, go to prev song
		if(currentSognPlayTime < 2f)
			songIndex--;

		// If we are at the begging loop to the end 
		if (songIndex < 0)                      // Loop at the start
			songIndex = workingSongList.Count - 1;

		// Get new song and play it
		audioSorce.clip = workingSongList[songIndex];
		if (audioSorce.isActiveAndEnabled == true)
			audioSorce.Play();

		// Try to find the mp3 
		if (mp3 == null)
			mp3 = this.TryToFindMp3();

		// If the we have the MP3 player display the new song 
		if (mp3 != null)
			mp3.UpdateSongName(workingSongList[songIndex].name);

		// Reset timer for new song 
		currentSognPlayTime = 0;
	}

	/// <summary>
	/// Try to find the mp3
	/// </summary>
	private MP3PlayerControl TryToFindMp3()
	{
		GameObject mp3GO = GameObject.FindWithTag("Mp3Player");
		if (mp3GO != null)
			return mp3GO.GetComponent<MP3PlayerControl>();
		return null;
	}

	private List<AudioClip> RandomizeList(List<AudioClip> list)
	{
		// Cant randomize something so small 
		if (list.Count <= 1)
			return list;

		// Fill a list with indexes 
		List<int> listOfIndexes = new List<int>();
		for (int i = 0; i < list.Count; i++)
			listOfIndexes.Add(i);

		// Pick from the list of indexes and add those songs so they are assorted randomly 
		List<AudioClip> randomizedList = new List<AudioClip>();
		while(listOfIndexes.Count > 0)
		{
			int randIndex = Random.Range(0, listOfIndexes.Count);
			randomizedList.Add(list[listOfIndexes[randIndex]]);
			listOfIndexes.RemoveAt(randIndex);
		}

		// Return the random list
		return randomizedList;

	}
}
