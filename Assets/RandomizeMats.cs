using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMats : MonoBehaviour
{
	public Material[] mats;
	private MeshRenderer mr;

	public void RandomizeMaterials()
	{
		// Get the mesh renderer 
		mr = this.GetComponent<MeshRenderer>();

		// Prevent getting stuck in the below loop
		if (mats.Length < 2)
		{
			Debug.Log("To little mats assigned...");
			return;
		}

		// Set all the values randomly but dont let the same value happen twice
		int rand = -1;
		int lastRand = -1;
		for(int i = 0; i < mr.materials.Length; i++)
		{
			// Make sure we have a unique rand
			while (rand == lastRand)
				rand = Random.Range(0, mats.Length);

			// Set the mat and set the last rand so the check above works as intended
			mr.materials[i].color = mats[rand].color;
			lastRand = rand;
		}
	}
}
