using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitControl : MonoBehaviour
{
	private EyeFadeControl eyeControl;

	// Start is called before the first frame update
	void Start()
	{
		eyeControl = GameObject.Find("[CameraRig]").transform.Find("Camera").GetComponent<EyeFadeControl>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "MainCamera")
			eyeControl.UpdateCloseEyes(true);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "MainCamera")
			eyeControl.SoftEyeOpen();
	}
}
