using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRotation : MonoBehaviour
{
	public float rotSpeed = 10;
	public bool rigidBody;
	private float curRot;

	private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
		curRot = Random.Range(0, 360);
		if (rigidBody == true)
			rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (rigidBody == false)
		{
			curRot += Time.deltaTime * rotSpeed;
			this.transform.eulerAngles = new Vector3(0, curRot, 0);
		}
		else
		{
			if (rb.angularVelocity.y < rotSpeed / 1.2f)
				rb.angularVelocity = new Vector3(0, rotSpeed, 0);
		}
    }
}
