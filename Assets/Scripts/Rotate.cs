using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	// Update is called once per frame
	void Update () 
    {
        this.transform.Rotate(new Vector3(0, 0, 1), 8 * Time.deltaTime);
	}
}
