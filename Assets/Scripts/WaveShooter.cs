// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveShooter : MonoBehaviour 
{
    // Game objects that say what wave the player is on
    private Rigidbody[] waves = new Rigidbody[4];
	private Rigidbody[] livesLeft = new Rigidbody[5];
	private Rigidbody infinitySign;
    private Vector3 popForce = new Vector3(0, 1300, 0); // How the wave message will be shot out
	private bool[] poppedWaves = new bool[4];

	private void Start()
	{
		waves[0] = Resources.Load<Rigidbody>("MainGame/Wave1");
		waves[1] = Resources.Load<Rigidbody>("MainGame/Wave2");
		waves[2] = Resources.Load<Rigidbody>("MainGame/Wave3");
		waves[3] = Resources.Load<Rigidbody>("MainGame/Wave1");

		// Todo: replace with lives messages 
		livesLeft[0] = Resources.Load<Rigidbody>("MainGame/Wave1");
		livesLeft[1] = Resources.Load<Rigidbody>("MainGame/Wave1");
		livesLeft[2] = Resources.Load<Rigidbody>("MainGame/Wave2");
		livesLeft[3] = Resources.Load<Rigidbody>("MainGame/Wave1");
		livesLeft[4] = Resources.Load<Rigidbody>("MainGame/Wave2");
		// Todo: replace with infinite lives messages 
		infinitySign = Resources.Load<Rigidbody>("MainGame/Wave2");
	}

	/// <summary>
	/// Shoots a message out saying what wave the player is on (Handing in false will show lives left)
	/// </summary>
	public void PopMessage(int index) 
    {
		// Make sure index is good 
		if (index < 0 || index >= waves.Length)
			return;

		// Creates clone
		Rigidbody popUp = (Rigidbody)Instantiate(waves[index], this.transform.position, waves[index].transform.rotation);

		// Adds the force to send it in the air
		popUp.AddForce(popForce);
        popUp.AddTorque(new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30)));

		// Randomize the mats of the sign
		//popUp.GetComponent<RandomizeMats>().RandomizeMaterials();

		// Destroy it after 4 seconds
		popUp.GetComponent<ShrinkAndDestroy>().ShrinkDestroy(3);
			
		// Add this to poppedSigns so we can see what signs have been shown 
		poppedWaves[index] = true;
    }

	public void PopLives(int index)
	{
		// Make sure index is good 
		if (index >= livesLeft.Length)
			return;

		// Make pop up with lives left or infinite lives message 
		Rigidbody popUp; 
		if (index < 0)
			popUp = (Rigidbody)Instantiate(infinitySign, this.transform.position, infinitySign.transform.rotation); 
		else
			popUp = (Rigidbody)Instantiate(livesLeft[index], this.transform.position, livesLeft[index].transform.rotation);

		// Adds the force to send it in the air
		popUp.AddForce(popForce);
		popUp.AddTorque(new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30)));

		// Randomize the mats of the sign
		//popUp.GetComponent<RandomizeMats>().RandomizeMaterials();

		// Destroy it after 4 seconds
		popUp.GetComponent<ShrinkAndDestroy>().ShrinkDestroy(3);
	}

	/// <summary>
	/// Return if that wave has been shown (0 = wave1, 1 = wave2, 2 = wave3, 3 = Sgo)
	/// </summary>
	public bool HasBeenShown(int index)
	{
		return poppedWaves[index];
	}
}
