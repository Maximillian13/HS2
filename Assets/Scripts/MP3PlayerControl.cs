using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MP3PlayerControl : MonoBehaviour
{
	public TextMeshPro songName;
	public TextMeshPro volumelevel;

	private void Start()
	{
		// Set the current volume
		AudioSource audioSource = GameObject.FindWithTag("Music").GetComponent<AudioSource>();
		this.UpdateVolumeLevel((int)(audioSource.volume * 10));
		this.UpdateSongName(audioSource.clip.name);
	}

	public void UpdateSongName(string sn)
	{
		songName.text = sn;
	}

	public void UpdateVolumeLevel(int vol)
	{
		volumelevel.text = vol.ToString();
	}

	

}
