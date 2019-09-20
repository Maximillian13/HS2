using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorOpener : MonoBehaviour
{
	public Transform[] doors;
	public Transform[] doorEndPoints;
	public bool swingOpen;

	private Vector3[] closePos;
	private Vector3[] openPos;

	private bool opening;
	private bool closing;

	private const float SLIDE_OPEN_SPEED = 4;
	private const float SWING_OPEN_SPEED = 150;

	// Start is called before the first frame update
	void Start()
    {
		opening = false;
		closing = false;

		closePos = new Vector3[doorEndPoints.Length];
		openPos = new Vector3[doorEndPoints.Length];

		for (int i = 0; i < closePos.Length; i++)
			openPos[i] = doorEndPoints[i].position;

		for (int i = 0; i < openPos.Length; i++)
			closePos[i] = doors[i].position;
	}

	// Update is called once per frame
	void Update()
	{
		this.OpeningClosingDoor();
	}

	private void OpeningClosingDoor()
	{
		if (opening == true)
		{
			closing = false;
			if (swingOpen == false)
			{
				doors[0].position = Vector3.MoveTowards(doors[0].position, openPos[0], SLIDE_OPEN_SPEED * Time.deltaTime);
				doors[1].position = Vector3.MoveTowards(doors[1].position, openPos[1], SLIDE_OPEN_SPEED * Time.deltaTime);
				if (Vector3.Distance(doors[0].position, openPos[0]) < .05f)
				{
					doors[0].position = openPos[0];
					doors[1].position = openPos[1];
					opening = false;
				}
			}
			else
			{
				doors[0].localEulerAngles = new Vector3(0, doors[0].localEulerAngles.y + Time.deltaTime * SWING_OPEN_SPEED, 0);
				doors[1].localEulerAngles = new Vector3(0, doors[1].localEulerAngles.y - Time.deltaTime * SWING_OPEN_SPEED, 0);
				if(doors[0].localEulerAngles.y >= 90)
				{
					doors[0].localEulerAngles = new Vector3(0, 90, 0);
					doors[1].localEulerAngles = new Vector3(0, -90, 0);
					opening = false;
				}
			}
		}

		if (closing == true)
		{
			if (swingOpen == false)
			{
				doors[0].position = Vector3.MoveTowards(doors[0].position, closePos[0], SLIDE_OPEN_SPEED * Time.deltaTime);
				doors[1].position = Vector3.MoveTowards(doors[1].position, closePos[1], SLIDE_OPEN_SPEED * Time.deltaTime);
				if (Vector3.Distance(doors[0].position, closePos[0]) < .05f)
				{
					doors[0].position = closePos[0];
					doors[1].position = closePos[1];
					closing = false;
				}
			}
			else
			{
				doors[0].localEulerAngles = new Vector3(0, doors[0].localEulerAngles.y - Time.deltaTime * SWING_OPEN_SPEED, 0);
				doors[1].localEulerAngles = new Vector3(0, doors[1].localEulerAngles.y + Time.deltaTime * SWING_OPEN_SPEED, 0);
				if (doors[0].localEulerAngles.y <= 0 || doors[0].localEulerAngles.y > 330)
				{
					doors[0].localEulerAngles = Vector3.zero;
					doors[1].localEulerAngles = Vector3.zero;
					closing = false;
				}
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player")
		{
			opening = true;
			closing = false;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			opening = false;
			closing = true;
		}
	}
}
