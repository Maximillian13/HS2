﻿// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class MoveWall : MonoBehaviour
{
	public bool cardioWall;
    // Speed of the wall
    private HeightCalibrator hCal;
    private bool moveUp;
    private bool moveDown;
    private float speed;
    private float height;

	private bool reposWhileMoving;

    // Set-up
	void Start () 
    {
        moveUp = true;
        moveDown = false;
		//if (cardioWall == false)
		{
			hCal = GameObject.Find("HeightCalibrator").GetComponent<HeightCalibrator>();
			height = hCal.GetWallAdjustedHeight();
		}
		//else
		{
		//	height = 0;
		}
    }


	void Update () 
    {
		if (moveUp == true || moveDown == true)
		{
			// If the wall has reached its max point tell it to stop moving up
			if (this.transform.localPosition.y >= height)
			{
				moveUp = false;
				this.transform.localPosition = new Vector3(this.transform.localPosition.x, height, this.transform.localPosition.z);
			}

			// Move the wall up to position
			if (moveUp == true)
				this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + ((speed * 3) * Time.smoothDeltaTime), this.transform.position.z);

			else if (moveDown == true) // Move the wall down to death (moveDown will be set from the DestroyWall script) 
				this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - ((speed * 3) * Time.smoothDeltaTime), this.transform.position.z);
		}

		else // If its not moving up or down move it forward 
		{
			if(reposWhileMoving == true)
			{
				float coeff = this.transform.position.y > height ? -1 : 1;
				this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (.8f * coeff * Time.deltaTime), this.transform.localPosition.z);
				if (Vector3.Distance(this.transform.localPosition, new Vector3(this.transform.localPosition.x, height, this.transform.localPosition.z)) < .1f)
				{
					this.transform.localPosition = new Vector3(this.transform.localPosition.x, height, this.transform.localPosition.z);
					reposWhileMoving = false;
				}
			}
			// Move the wall forward by increasing its position (If speed is 0 it will not move)
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - (speed * Time.smoothDeltaTime));
		}
    }

    ///<summary>Set the Speed of the wall</summary>
    /// <param name="s">The speed of the wall</param>
    public void Speed(float s)
    {
        speed = s;
    }

    public float GetSpeed()
    {
        return speed;
    }

    /// <summary>
    /// Will set the wall to move down indefinitely
    /// </summary>
    public void MoveDown()
    {
        moveDown = true;
    }

	public void ChangeHeight(float newHeight)
	{
		reposWhileMoving = true;
		height = newHeight;
	}

	public void SetGibReady()
    {
        GibControl[] gibs = this.gameObject.GetComponentsInChildren<GibControl>();
        foreach(GibControl g in gibs)
        {
            g.BlowUpGib = true;
        }
    }
}
