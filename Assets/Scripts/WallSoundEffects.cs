using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSoundEffects : MonoBehaviour
{
	public enum WallSoundEffectClip
	{
		squatWall,
		squatWallTwo,
		squatWallThree,
		cardioWall,
		combo,
		fail
	}

	public AudioClip[] soundEffects;
	private AudioSource audioSorce;

	// Start is called before the first frame update
	void Start()
	{
		audioSorce = this.GetComponent<AudioSource>();
	}


	public void PlayClip(WallSoundEffectClip clip)
	{
		audioSorce.clip = soundEffects[(int)clip];
		audioSorce.pitch = Random.Range(.95f, 1.05f);
		audioSorce.Play();
	}
}
