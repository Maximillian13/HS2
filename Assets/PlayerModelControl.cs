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
		this.UpdateBodyModel(PlayerPrefs.GetInt("BodyInd"));
	}

	public void UpdateBodyModel(int modelIndex)
	{
		// Get rid of the old body
		Destroy(body);

		// Make and place new body
		body = Instantiate<GameObject>(Resources.Load<GameObject>("PlayerModels/Body" + modelIndex), this.transform);
		body.transform.localPosition = Vector3.zero;
		body.name = "PlayerModel";
	}
}
