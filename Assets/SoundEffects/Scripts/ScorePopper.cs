using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopper : MonoBehaviour
{
	public ParticleSystem[] pSytems;
	private ParticleSystem.MainModule[] pSytemsMain;
	private ParticleSystem.EmissionModule a;

	private GameObject[,] scoreSprites2D = new GameObject[4,3];

	private void Start()
	{
		pSytemsMain = new ParticleSystem.MainModule[pSytems.Length];
		for (int i = 0; i < pSytems.Length; i++)
			pSytemsMain[i] = pSytems[i].main;

		// Fill out the 2d array 
		for(int i = 0; i < 4; i ++)
		{
			for (int j = 0; j < 3; j++)
				scoreSprites2D[i, j] = Resources.Load<GameObject>("Sprites/" + i + "" + j);
		}
	}

	public void PopScoreMessage(int scoreUpInd, float size = .05f)
	{
		GameObject messageInstance = Instantiate(scoreSprites2D[scoreUpInd, Random.Range(0, 3)]);
		messageInstance.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .5f, this.transform.position.z);
		messageInstance.transform.localScale = new Vector3(size, size, size);
		//messageInstance.GetComponent<ScorePopperMessage>().SetMessage();
	}

	/// <summary>
	/// Triggers the particle system (Duration must be a array of 3 float with 0=middle 1=right 2=left)
	/// </summary>
	public void TriggerParticleSystem(bool[] positions,  float[] durration)
	{
		for (int i = 0; i < pSytems.Length; i++)
		{
			// If its already playing we dont want to cause issues 
			if (pSytems[i].isPlaying == true)
				continue;

			// Set the durration and set it to play
			pSytemsMain[i].duration = durration[i];
			if (positions[i] == true)
				pSytems[i].Play();
		}
	}
}
