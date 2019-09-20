using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelControl : MonoBehaviour
{
	private GameObject body;

	void Start()
	{
		body = GameObject.Find("Player").transform.Find("PlayerModel").gameObject;

		// Get the right body from player prefs and put it on
		this.UpdateBodyModel(PlayerPrefs.GetInt("BodyInd"), PlayerPrefs.GetInt("BodyType"));
	}

	public void UpdateBodyModel(int modelIndex, int bodyTypeIndex)
	{
		// Get rid of the old body
		Destroy(body);

		// Get what body type we are (A/0 = Androgens, M/1 = Male, F/2 = Female) 
		string bodyType = "A";
		if (bodyTypeIndex == 1)
			bodyType = "M";
		if (bodyTypeIndex == 2)
			bodyType = "F";


		// Make and place new body
		body = Instantiate<GameObject>(Resources.Load<GameObject>("PlayerModels/Body" + modelIndex + "/Body" + bodyType), this.transform);
		body.transform.localPosition = new Vector3(0, -.35f, -.1f);
		body.transform.localEulerAngles = new Vector3(0, 180, 0);
		body.name = "PlayerModel";
	}
}
