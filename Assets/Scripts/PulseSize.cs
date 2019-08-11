using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseSize : MonoBehaviour
{
	private const float SCALE_FACTOR = 1400;

    // Update is called once per frame
    void Update()
    {
		this.transform.localScale = new Vector3(
			this.transform.localScale.x + (Mathf.Sin(Time.time * 3) / SCALE_FACTOR),
			this.transform.localScale.y + (Mathf.Sin(Time.time * 3) / SCALE_FACTOR),
			this.transform.localScale.z + (Mathf.Sin(Time.time * 3) / SCALE_FACTOR));
    }
}
