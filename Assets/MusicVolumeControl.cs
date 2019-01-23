using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeControl : MonoBehaviour
{
	private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = this.GetComponent<AudioSource>();
		audioSource.volume = (PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f;
    }

	/// <summary>
	/// Updates the volume to be whatever the PlayerPref value holds
	/// </summary>
	public void UpdateVolume()
	{
		audioSource.volume = (PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f;
	}
}
