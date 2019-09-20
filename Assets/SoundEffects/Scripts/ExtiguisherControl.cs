using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtiguisherControl : MonoBehaviour
{
	private Transform tipPos;
	private FixedJoint fixedJoint;
	private ParticleSystem pSystem;
	private Rigidbody rBody;
	private AudioSource sound;
	private float timer;
	private bool playing;

	private Vector3 OFFSET = new Vector3(0, .2f, .225f);
	private const float STRENGTH = 25;

    // Start is called before the first frame update
    void Start()
    {
		fixedJoint = this.GetComponent<FixedJoint>();
		rBody = this.GetComponent<Rigidbody>();
		tipPos = this.transform.Find("Tip");
		pSystem = tipPos.GetComponent<ParticleSystem>();
		timer = float.NegativeInfinity;

		sound = this.GetComponent<AudioSource>();

		//GameObject g = new GameObject("QQQ");
		//g.transform.position = this.transform.position + new Vector3(0, .2f, .225f);
    }

    // Update is called once per frame
    void Update()
    {
        if(fixedJoint == null && playing == false)
		{
			pSystem.Play();
			playing = true;
			timer = Time.time + 6;
			this.transform.SetParent(null);
			sound.volume = (PlayerPrefs.GetInt("MusicVol") + 10) / 10.0f;
			sound.Play();
		}

		if (Time.time < timer)
		{
			//Vector3 worldForcePosition = transform.TransformPoint(tipPos.localPosition);
			//Vector3 dir = transform.TransformPoint(tipPos.forward * STRENGTH);

			//rBody.AddForceAtPosition(Vector3.one * STRENGTH / 10, worldForcePosition);

			rBody.AddForceAtPosition(-this.transform.right * STRENGTH, this.transform.position + OFFSET);
		}

	}
}
