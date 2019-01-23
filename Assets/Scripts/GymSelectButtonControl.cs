using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymSelectButtonControl : MonoBehaviour
{
	public GymSelectButton[] gsbs;

	void Start()
	{
		gsbs[0].Select(); // Select Gym one as default 
	}

	public void ClearAllButtons()
	{
		for(int i = 0; i < gsbs.Length; i++)
			gsbs[i].Deselect();
	}
	
	public int GetSelectedGym()
	{
		// Find the gym that is selected 
		for (int i = 0; i < gsbs.Length; i++)
		{
			if (gsbs[i].IsSelected() == true)
				return i;
		}

		// Return -1 for random gym
		return -1;
	}

}
