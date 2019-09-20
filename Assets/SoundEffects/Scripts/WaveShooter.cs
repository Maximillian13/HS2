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
	private Rigidbody[] breakMessages = new Rigidbody[2];
    private Vector3 popForce = new Vector3(0, 1300, 0); // How the wave message will be shot out
	private bool[] poppedWaves = new bool[4];

	private const float TORQUE_FORCE = 20;

	private void Start()
	{
		// Wave messages 
		waves[0] = Resources.Load<Rigidbody>("MainGame/Wave1");
		waves[1] = Resources.Load<Rigidbody>("MainGame/Wave2");
		waves[2] = Resources.Load<Rigidbody>("MainGame/Wave3");
		waves[3] = Resources.Load<Rigidbody>("MainGame/Wave1");

		// Lives messages 
		livesLeft[0] = Resources.Load<Rigidbody>("MainGame/Hearts0");
		livesLeft[1] = Resources.Load<Rigidbody>("MainGame/Hearts1");
		livesLeft[2] = Resources.Load<Rigidbody>("MainGame/Hearts2");
		livesLeft[3] = Resources.Load<Rigidbody>("MainGame/Hearts3");
		livesLeft[4] = Resources.Load<Rigidbody>("MainGame/Hearts4");
		infinitySign = Resources.Load<Rigidbody>("MainGame/HeartsInf");

		// Break Messages
		breakMessages[0] = Resources.Load<Rigidbody>("MainGame/OnBreak");
		breakMessages[1] = Resources.Load<Rigidbody>("MainGame/OffBreak");
	}

	/// <summary>
	/// Shoots a message out saying what wave the player is on
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
        popUp.AddTorque(new Vector3(Random.Range(-TORQUE_FORCE, TORQUE_FORCE), Random.Range(-TORQUE_FORCE, TORQUE_FORCE), Random.Range(-TORQUE_FORCE, TORQUE_FORCE)));

		// Randomize the mats of the sign
		//popUp.GetComponent<RandomizeMats>().RandomizeMaterials();

		// Destroy it after 4 seconds
		popUp.GetComponent<ShrinkAndDestroy>().ShrinkDestroy(3);
			
		// Add this to poppedSigns so we can see what signs have been shown 
		poppedWaves[index] = true;
    }

	/// <summary>
	/// Pop how many lives left (5 for inf lives)
	/// </summary>
	/// <param name="index"></param>
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
		popUp.AddTorque(new Vector3(Random.Range(-TORQUE_FORCE, TORQUE_FORCE), Random.Range(-TORQUE_FORCE, TORQUE_FORCE), Random.Range(-TORQUE_FORCE, TORQUE_FORCE)));

		// Randomize the mats of the sign
		//popUp.GetComponent<RandomizeMats>().RandomizeMaterials();

		// Destroy it after 4 seconds
		popUp.GetComponent<ShrinkAndDestroy>().ShrinkDestroy(3);
	}

	/// <summary>
	/// Pops break message (true = On Break, false = Off Break)
	/// </summary>
	public void PopBreakMessage(bool onBreak)
	{
		// If on break = true set ind to 0 else 1 for off break 
		int ind = onBreak == true ? 0 : 1;

		// Make pop up with lives left or infinite lives message 
		Rigidbody popUp = (Rigidbody)Instantiate(breakMessages[ind], this.transform.position, breakMessages[ind].transform.rotation);

		// Adds the force to send it in the air
		popUp.AddForce(popForce);
		popUp.AddTorque(new Vector3(Random.Range(-TORQUE_FORCE, TORQUE_FORCE), Random.Range(-TORQUE_FORCE, TORQUE_FORCE), Random.Range(-TORQUE_FORCE, TORQUE_FORCE)));

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
