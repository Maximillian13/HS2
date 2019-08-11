using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAndDestroy : MonoBehaviour
{
	private float destroyTimer = float.PositiveInfinity;
	private float speed = 1.01f;

	//private void Start()
	//{
	//	destroyTimer = float.PositiveInfinity;
	//}

	// Update is called once per frame
	void FixedUpdate()
    {
		if (Time.time > destroyTimer)
		{
			this.transform.localScale = new Vector3(this.transform.localScale.x / speed, this.transform.localScale.z / speed, this.transform.localScale.z / speed);
			if (this.transform.localScale.x < .01f)
				Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Shrink and destroy this object immediately 
	/// </summary>
	public void ShrinkDestroy()
	{
		// If destroyTimer is set to -1 it will start the destroy process right away
		destroyTimer = -1;
	}


	/// <summary>
	/// Shrink and destroy this object after "delay" seconds
	/// </summary>
	public void ShrinkDestroy(float delay)
	{
		destroyTimer = Time.time + delay;
	}

	public void ShrinkDestroySpeed(float s = 1.01f)
	{
		speed = s;
		destroyTimer = -1;
	}
}
