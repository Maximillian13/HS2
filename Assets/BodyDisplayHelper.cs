using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDisplayHelper : MonoBehaviour
{
	public GameObject[] bodies;

    // Start is called before the first frame update
    void Start()
    {
		// Disable all bodies then re-enable the current body type 
		this.SetActiveBodyType(PlayerPrefs.GetInt("BodyType"));
    }

	/// <summary>
	/// Disable all bodies then re-enable the passed body type 
	/// </summary>
	/// <param name="bodyType">0 = Androg, 1 = Male, 2 = Female</param>
	public void SetActiveBodyType(int bodyType)
	{
		// Todo: remove when bodies are added 
		if (bodies.Length == 0)
			return;
		for (int i = 0; i < bodies.Length; i++)
			bodies[i].SetActive(false);
		bodies[bodyType].SetActive(true);
	}
}
