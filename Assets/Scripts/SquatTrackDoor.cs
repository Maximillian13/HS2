using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatTrackDoor : MonoBehaviour
{
	public Transform openTrans;

	private Vector3 closedPos;
	private Vector3 openPos;

	private bool open;
	private bool close;

	// Start is called before the first frame update
	void Start()
    {
		closedPos = this.transform.position;
		openPos = openTrans.position;

		// Open when the round level is loaded 
		open = true;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.J))
			open = true;

		if (open == true)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, openPos, .025f);
			if (Vector3.Distance(this.transform.position, openPos) < .05f)
			{
				open = false;
				this.transform.position = openPos;
			}
		}

		if (close == true)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, closedPos, .025f);
			if (Vector3.Distance(this.transform.position, closedPos) < .05f)
			{
				close = false;
				this.transform.position = closedPos;
			}
		}
	}

	/// <summary>
	/// Smoothly opens the door.
	/// </summary>
	public void OpenDoor()
	{
		open = true;
	}

	/// <summary>
	/// Smoothly closes the door.
	/// </summary>
	public void CloseDoor()
	{
		close = true;
		open = false;
	}
}
