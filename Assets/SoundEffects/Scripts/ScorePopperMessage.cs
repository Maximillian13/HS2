using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopperMessage : MonoBehaviour
{
	private SpriteRenderer spriteRend;
	private float alpha = 1.5f;
	private float upTimer = -.01f;
	private float upDiv = 15;

	private void Start()
	{
		spriteRend = this.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		alpha -= Time.deltaTime;
		spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, alpha);

		upTimer += Time.deltaTime / 15;
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + upTimer, this.transform.position.z);

		if (alpha < 0)
			Destroy(this.gameObject);

	}

	public void SetMessage(string message, float speedGoingUpDiv = 15, float goingUpTimer = -.01f)
	{
		this.upTimer = goingUpTimer;
		this.upDiv = speedGoingUpDiv;
	}
}
