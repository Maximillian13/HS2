// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class CheckUp : MonoBehaviour 
{
    public Transform head; // Where your head is
    private bool ready; // Whether or not the player has touched the top collider and is ready for the next wall
	
    // Get the height and set the top collision to it with minor adjustments after the calibration time
    void Start()
    {
        ready = true;
    }

	/// <summary>
	/// Set the height of the check up box
	/// </summary>
	/// <param name="height"></param>
	public void SetHeight(float height)
	{
		this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);
	}

	// If the player hits it set it back to being ready (Set to not ready in other scripts)
	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            ready = true;
    }

    /// <summary>
    /// weather or not the player is ready for the next wall
    /// </summary>
    public bool Ready
    {
        get { return ready; }
        set { ready = value; }
    }
}
