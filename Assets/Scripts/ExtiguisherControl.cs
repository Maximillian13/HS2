using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtiguisherControl : MonoBehaviour
{
	private Transform tipPos;
	private FixedJoint fixedJoint;
	private ParticleSystem pSystem;
	private Rigidbody rBody;
	private float timer;
	private bool playing;


	private const float STRENGTH = 25;

    // Start is called before the first frame update
    void Start()
    {
		fixedJoint = this.GetComponent<FixedJoint>();
		rBody = this.GetComponent<Rigidbody>();
		tipPos = this.transform.Find("Tip");
		pSystem = tipPos.GetComponent<ParticleSystem>();
		timer = float.NegativeInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        if(fixedJoint == null && playing == false)
		{
			pSystem.Play();
			playing = true;
			timer = Time.time + 6;
		}

		if (Time.time < timer) 
			rBody.AddForceAtPosition(-tipPos.forward * (STRENGTH + Random.Range(-3f,3f)), tipPos.position);

    }
}
