using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSoundEffects : MonoBehaviour
{
	public enum WallSoundEffectClip
	{
		combo2,
		combo3,
		combo5,
		fail
	}

	public AudioClip[] soundEffects;
	private AudioSource audioSorce;

	// Start is called before the first frame update
	void Start()
	{
		audioSorce = this.GetComponent<AudioSource>();
	}


	public void PlayClip(WallSoundEffectClip clip, float delay = 0)
	{
		audioSorce.clip = soundEffects[(int)clip];
		//audioSorce.pitch = Random.Range(.95f, 1.05f);
		audioSorce.volume = (PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f;
		audioSorce.PlayDelayed(delay);
	}

}
