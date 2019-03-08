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
		this.UpdateVolumeLevel((int)(GameObject.FindWithTag("Music").GetComponent<AudioSource>().volume * 10));
	}

	public void UpdateSongName(string sn)
	{
		songName.text = "Name: " + sn;
	}

	public void UpdateVolumeLevel(int vol)
	{
		volumelevel.text = "Volume: " + vol.ToString();
	}

	

}
