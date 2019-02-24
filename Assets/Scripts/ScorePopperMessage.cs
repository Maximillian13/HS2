using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopperMessage : MonoBehaviour
{
	private TextMeshPro scoreText;
	private float alpha = 1.5f;
	private float upTimer = -.01f;
	// Start is called before the first frame update
	void Start()
	{
		scoreText = this.GetComponent<TextMeshPro>();
	}

	// Update is called once per frame
	void Update()
	{
		alpha -= Time.deltaTime;
		scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);

		upTimer += Time.deltaTime / 15;
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + upTimer, this.transform.position.z);

		if (alpha < 0)
			Destroy(this.gameObject);

	}

	public void SetMessage(string message)
	{
		if (scoreText == null)
			this.Start();

		scoreText.text = message;
	}
}
