using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticsTrackControl : MonoBehaviour
{
	public BodySelectorControl buttonControl;
	public CosmeticUnlock[] bodies;
	private Transform[] pathNodes = new Transform[13];

	private int currentNodeInd;

	private int currentBodyIndex;

	private int moveInNodeInd;
	private int moveOutNodeInd;
	private int moveInInd;
	private int moveOutInd;
	private bool moveInDone;
	private bool moveOutDone;

	private bool moveLeft;
	private bool moveRight;
	private int moveToIndex;

	private const float SPEED = 4;

    // Start is called before the first frame update
    void Start()
    {
		//	Set up all the path nodes that we will travel
		for (int i = 0; i < pathNodes.Length; i++)
			pathNodes[i] = this.transform.GetChild(i);

		int startingBody = PlayerPrefs.GetInt("BodyInd");

		// Organize the bodies so that the starting body is in the middle, all less to the left and all more to the right 
		bodies[startingBody].transform.position = pathNodes[pathNodes.Length / 2].position;
		for (int i = 0; i < startingBody; i++)
			bodies[i].transform.position = pathNodes[0].position;
		for (int i = startingBody + 1; i < bodies.Length; i++)
			bodies[i].transform.position = pathNodes[pathNodes.Length - 1].position;

		currentBodyIndex = startingBody;
		buttonControl.SetSelectButtonSelected(true);
		bodies[currentBodyIndex].FadeInAll();

		if (currentBodyIndex == 0)
			buttonControl.EnableDisableButton("Left", false);
		if (currentBodyIndex == bodies.Length - 1)
			buttonControl.EnableDisableButton("Right", false);
	}

	/// <summary>
	/// Move the bodies to the left bringing in the body from the right
	/// </summary>
	public void RightPress()
	{
		if (currentBodyIndex == bodies.Length - 1)
			return;

		moveOutNodeInd = 6;
		moveInNodeInd = 12;

		moveLeft = true;
		moveOutInd = currentBodyIndex;
		currentBodyIndex++;
		moveInInd = currentBodyIndex;

		// Clear and reset selection if necessary 
		buttonControl.SetSelectButtonSelected(false);

		// Enable or disable that button depending on if the new body is unlocked
		buttonControl.EnableDisableButton("Select", bodies[currentBodyIndex].GetUnlocked());

		buttonControl.EnableDisableButton("Left", true);
		buttonControl.DisableAll();

		// Fade out and in the texts
		bodies[moveOutInd].FadeOutAll();
		bodies[moveInInd].FadeInAll();
	}

	/// <summary>
	/// Move the bodies to the right bringing in the body from the left
	/// </summary>
	public void LeftPress()
	{
		if (currentBodyIndex == 0)
			return;

		moveOutNodeInd = 6;
		moveInNodeInd = 0;

		moveRight = true;
		moveOutInd = currentBodyIndex;
		currentBodyIndex--;
		moveInInd = currentBodyIndex;

		// Clear and reset selection if necessary 
		buttonControl.SetSelectButtonSelected(false);

		// Enable or disable that button depending on if the new body is unlocked
		buttonControl.EnableDisableButton("Select", bodies[currentBodyIndex].GetUnlocked());

		buttonControl.EnableDisableButton("Right", true);
		buttonControl.DisableAll();

		// Fade out and in the texts
		bodies[moveOutInd].FadeOutAll();
		bodies[moveInInd].FadeInAll();
	}

	/// <summary>
	/// Handles the movement of the bodies (Must be called in update)
	/// </summary>
	/// <param name="indChangeAmout">How much to change the node index by, 1 or -1. (moving left = -1, moving right = 1)</param>
	/// <param name="moveOutEndInd">Where to have the body that is moving out stop (moving left = 0, moving right = last ind of nodes</param>
	private void MovmentHandler(int indChangeAmout, int moveOutEndInd)
	{
		if (moveOutDone == false)
		{
			// Move to next node
			bodies[moveOutInd].transform.position = Vector3.MoveTowards(bodies[moveOutInd].transform.position, pathNodes[moveOutNodeInd].position, SPEED * Time.deltaTime);

			// if close to node check to move to a new node or end if we are at the end 
			if (Vector3.Distance(pathNodes[moveOutNodeInd].position, bodies[moveOutInd].transform.position) < .01f)
			{
				if (moveOutNodeInd == moveOutEndInd)
					moveOutDone = true;
				moveOutNodeInd += indChangeAmout;
			}
		}

		if (moveInDone == false)
		{
			// Move to next node
			bodies[moveInInd].transform.position = Vector3.MoveTowards(bodies[moveInInd].transform.position, pathNodes[moveInNodeInd].position, SPEED * Time.deltaTime);

			// if close to node check to move to a new node or end if we are at the end 
			if (Vector3.Distance(pathNodes[moveInNodeInd].position, bodies[moveInInd].transform.position) < .01f)
			{
				if (moveInNodeInd == 6)
					moveInDone = true;
				moveInNodeInd += indChangeAmout;

			}
		}

		// When done, reset everything
		if (moveInDone == true && moveOutDone == true)
		{
			moveRight = false;
			moveLeft = false;
			moveOutDone = false;
			moveInDone = false;

			buttonControl.ReEnable();

			// Lock the ends 
			if (currentBodyIndex == 0)
				buttonControl.EnableDisableButton("Left", false);
			if (currentBodyIndex == bodies.Length - 1)
				buttonControl.EnableDisableButton("Right", false);

			// Highlight the select if we are on the selected body 
			if (PlayerPrefs.GetInt("BodyInd") == currentBodyIndex)
				buttonControl.SetSelectButtonSelected(true);
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.A))
			this.LeftPress();

		if (Input.GetKeyDown(KeyCode.S))
			this.RightPress();

		if (moveLeft == true)
			this.MovmentHandler(-1, 0);
		if (moveRight == true)
			this.MovmentHandler(1, pathNodes.Length - 1);

	}

	/// <summary>
	/// Returns the currently selected body
	/// </summary>
	public int GetBodyIndex()
	{
		return currentBodyIndex;
	}
}
