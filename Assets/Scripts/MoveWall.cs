// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
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


    // Set-up
	void Start () 
    {
        moveUp = true;
        moveDown = false;
		if (cardioWall == false)
		{
			hCal = GameObject.Find("HeightCalibrator").GetComponent<HeightCalibrator>();
			this.StartCoroutine(this.LateStart(hCal.GetCalibrationTime() + .1f));
		}
		else
		{
			height = 0;
		}
    }
    // Set the height of the wall based on the player height
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        height = hCal.GetCalibratedHeight();
    }


	void Update () 
    {
        // If the wall has reached its max point tell it to stop moving up
        if(this.transform.localPosition.y >= height + .5f) // Todo: Test if +.5 is appropriate here (Need a tester)
        {
            moveUp = false;
        }

        // Move the wall up to position
        if(moveUp == true)
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + ((speed * 3) * Time.smoothDeltaTime), this.transform.position.z);

        else if (moveDown == true) // Move the wall down to death (moveDown will be set from the DestroyWall script) 
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - ((speed * 3) * Time.smoothDeltaTime), this.transform.position.z);

        else // If its not moving up or down move it forward 
            // Move the wall forward by increasing its position (If speed is 0 it will not move)
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - (speed * Time.smoothDeltaTime));
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

    public void SetGibReady()
    {
        GibControl[] gibs = this.gameObject.GetComponentsInChildren<GibControl>();
        foreach(GibControl g in gibs)
        {
            g.BlowUpGib = true;
        }
    }
}
