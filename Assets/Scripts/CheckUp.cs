// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class CheckUp : MonoBehaviour 
{
    public Transform head; // Where your head is
    private HeightCalibrator hCal; // The object responsible for the height calibration
    private bool ready; // Whether or not the player has touched the top collider and is ready for the next wall
	
    // Get the height and set the top collision to it with minor adjustments after the calibration time
    void Start()
    {
        ready = true;
        hCal = GameObject.Find("HeightCalibrator").GetComponent<HeightCalibrator>();
        this.StartCoroutine(this.LateStart(hCal.GetCalibrationTime() + .1f));
    }

    // Late start
    IEnumerator LateStart(float waitTime)
    {
        // Get the height and set the top collision to it with minor adjustments
        yield return new WaitForSeconds(waitTime);
        this.transform.position = new Vector3(this.transform.position.x, hCal.GetCalibratedHeight() - .3f, this.transform.position.z);
    }

    // If the player hits it set it back to being ready (Set to not ready in other scripts)
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ready = true;
        }
    }

    /// <summary>
    /// weather or not the player is ready for the next wall
    /// </summary>
    public bool Ready
    {
        get
        {
            return ready;
        }
        set
        {
            ready = value;
        }
    }
}
