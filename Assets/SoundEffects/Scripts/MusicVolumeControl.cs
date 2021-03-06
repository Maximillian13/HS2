﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeControl : MonoBehaviour
{
	private AudioSource audioSource;
	private MP3PlayerControl mp3;

    // Start is called before the first frame update
    public void AudioSetUp()
    {
		audioSource = this.GetComponent<AudioSource>();
		audioSource.volume = (PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f;

		if(GameObject.FindWithTag("Mp3Player") != null)
			mp3 = GameObject.FindWithTag("Mp3Player").GetComponent<MP3PlayerControl>();
		
		this.ChangeCurrentVolumeAndMP3(0); // Make it match the in game volume 

    }

	/// <summary>
	/// Updates the volume to be whatever the PlayerPref value holds
	/// </summary>
	public void UpdateVolume()
	{
		if (audioSource == null)
			this.AudioSetUp();
		audioSource.volume = (PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f;
	}

	/// <summary>
	/// Updates the current in game volume, will not save 
	/// </summary>
	public void ChangeCurrentVolumeAndMP3(int changeAmout)
	{
		// Get current volume an change it how ever much specified 
		float bigVal = audioSource.volume * 10.0f;
		int currAudioVol = (int)bigVal;
		int newAudioVol = currAudioVol + changeAmout;

		// If we go past one of the limits, set it back
		if (newAudioVol < 0)
			newAudioVol = 0;
		if (newAudioVol > 10)
			newAudioVol = 10;

		// Update the current volume
		audioSource.volume = newAudioVol / 10.0f;

		// If we have the MP3 change the level on there
		if (mp3 == null)
		{
			if (GameObject.FindWithTag("Mp3Player") != null)
				mp3 = GameObject.FindWithTag("Mp3Player").GetComponent<MP3PlayerControl>();
		}

		if(mp3 != null)
			mp3.UpdateVolumeLevel(newAudioVol);
	}
}
