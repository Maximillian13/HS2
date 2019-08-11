using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLevelLoader : MonoBehaviour
{
	public Transform[] doors;
	public Transform[] doorEndPoints;
	public GameObject sightBlocker;

	public DailyChallengeMaster dailyChalMaster;
	public MainMenuControl mainMenuControl;

	private Vector3[] closePos = new Vector3[2];
	private Vector3[] openPos = new Vector3[2];

	private int loadIndex;

	private bool opening;
	private bool closing;

	private const float OPEN_SPEED = 4;


	// Start is called before the first frame update
	void Start()
    {
		opening = false;
		closing = false;
		loadIndex = -1;

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
			doors[0].position = Vector3.MoveTowards(doors[0].position, openPos[0], OPEN_SPEED * Time.deltaTime);
			doors[1].position = Vector3.MoveTowards(doors[1].position, openPos[1], OPEN_SPEED * Time.deltaTime);
			if (Vector3.Distance(doors[0].position, openPos[0]) < .05f)
			{
				doors[0].position = openPos[0];
				doors[1].position = openPos[1];
				opening = false;
			}
		}

		if (closing == true)
		{
			opening = false;
			doors[0].position = Vector3.MoveTowards(doors[0].position, closePos[0], OPEN_SPEED * Time.deltaTime);
			doors[1].position = Vector3.MoveTowards(doors[1].position, closePos[1], OPEN_SPEED * Time.deltaTime);
			if (Vector3.Distance(doors[0].position, closePos[0]) < .05f)
			{
				doors[0].position = closePos[0];
				doors[1].position = closePos[1];
				closing = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player")
			return;
		if (loadIndex == -1)
			return;

		closing = true;
		GameObject.Find("[CameraRig]").transform.Find("Camera").GetComponent<EyeFadeControl>().CloseEyes(2, true);

		// Save if we did a dialy challenge
		if (mainMenuControl.IsDailyChal() == true)
			dailyChalMaster.IncrementDailyChallengeStatIfNew();
	}

	public void SetLoadIndex(int li)
	{
		this.loadIndex = li;
		PlayerPrefs.SetInt("GymInd", li);
	}

	public void OpenDoors(bool dailyChallenge = false)
	{
		sightBlocker.SetActive(false);
		this.opening = true;
	}

	public void CloseDoors()
	{
		sightBlocker.SetActive(true);
		this.closing = true;
	}
}
