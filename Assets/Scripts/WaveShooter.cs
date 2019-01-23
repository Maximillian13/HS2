// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveShooter : MonoBehaviour 
{
    // Game objects that say what wave the player is on
    private Rigidbody[] waves = new Rigidbody[4];
    private Vector3 popForce = new Vector3(0, 1300, 0); // How the wave message will be shot out
	private bool[] poppedWaves = new bool[4];

	private void Start()
	{
		waves[0] = Resources.Load<Rigidbody>("MainGame/Wave1");
		waves[1] = Resources.Load<Rigidbody>("MainGame/Wave2");
		waves[2] = Resources.Load<Rigidbody>("MainGame/Wave3");
		waves[3] = Resources.Load<Rigidbody>("MainGame/Sgo");
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			this.PopWave(1);
	}

	/// <summary>
	/// Shoots a message out saying what wave the player is on
	/// </summary>
	/// <param name="waveIndex">What wave message to shoot out (Corresponds directly to the wave)</param>
	public void PopWave(int waveIndex) // Called from Wave spawner
    {
        if(waveIndex <= waves.Length)
        {
            // Creates clone
            Rigidbody wavePopUp = (Rigidbody)Instantiate(waves[waveIndex], this.transform.position, waves[waveIndex].transform.rotation);

            // Adds the force to send it in the air
            wavePopUp.AddForce(popForce);
            wavePopUp.AddTorque(new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30)));

			// Randomize the mats of the sign
			wavePopUp.GetComponent<RandomizeMats>().RandomizeMaterials();

			// Destroy it after 4 seconds
			wavePopUp.GetComponent<ShrinkAndDestroy>().ShrinkDestroy(3);
			

			// Add this to poppedSigns so we can see what signs have been shown 
			poppedWaves[waveIndex] = true;
        }
    }

	/// <summary>
	/// Return if that wave has been shown (0 = wave1, 1 = wave2, 2 = wave3, 3 = Sgo)
	/// </summary>
	public bool HasBeenShown(int index)
	{
		return poppedWaves[index];
	}
}
