// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using TMPro;

public class HeightCalibrator : MonoBehaviour 
{
    // Fields
    public Transform head; // The transform of the headset
    public TextMeshPro infoBoard; // Letting the sure know the calibration stuff
    public HandPlacment hp; // Hand placement so the hands know when the game is started
	public GuideRail guideRailControl; // The control for how high the guide rail should go
	public CheckUp checkUp; // To set how high the squatter must go to have the next wall count 
    private float calHeight; // The float y where the head is 
    private float timer; // For calibration 
    private const float CALIBRATION_TIME = 7; // How long it takes for the this to calibrate the height 

    const float MIN_HEIGHT = 1.5f;		// The minimum height the calibration can be
	const float MAX_HEIGHT = 2.5f;		// The max height the calibration can be


	void Start()
    {
        // Do this after the amount of calibration time
        this.StartCoroutine(this.LateStart(CALIBRATION_TIME));
    }

    // Late start
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

		// Set the height to the current position of the y
		calHeight = head.position.y;
        if(calHeight < MIN_HEIGHT)
            calHeight = MIN_HEIGHT;
		if (calHeight > MAX_HEIGHT)
			calHeight = MAX_HEIGHT;

		//calHeight = 2; // for testing 
		guideRailControl.RaiseRail(calHeight - .6f);	// Adjust for different coordinates 
		checkUp.SetHeight(calHeight - .3f);             // Adjust for different coordinates 
	}

    // Update the message depending on how long the user has been playing
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.P))
			this.ChangeHeight(.1f);
        timer += Time.deltaTime;
        if(timer < 4)
        {
            infoBoard.text = "CALIBRATING...\nSTAND STRAIGHT\nAND STAY STILL.";
        }
        if(timer > 4 && timer < 5)
        {
            infoBoard.text = "3...";
        }
        if (timer > 5 && timer < 6)
        {
            infoBoard.text = "2...";
        }
        if (timer > 6 && timer < 7)
        {
            infoBoard.text = "1...";
        }
        if (timer > 7 && timer < 8)
        {
            infoBoard.text = "CALIBRATED.\nTHANK YOU.";
        }
        if (timer > 8 && timer < 9)
        {
            // Hide the board and start the game
            infoBoard.gameObject.SetActive(false);
            hp.GameStarted();
        }
    }


	/// <summary>
	/// Change the height of the cal zone, limited by the min and max heights 
	/// </summary>
	/// <param name="changeAmount"></param>
	public void ChangeHeight(float changeAmount)
	{
		// Change the cal height based off of what we hand in 
		calHeight += changeAmount;

		if (calHeight < MIN_HEIGHT)
			calHeight = MIN_HEIGHT;
		if (calHeight > MAX_HEIGHT)
			calHeight = MAX_HEIGHT;

		//calHeight = 2; // for testing 
		guideRailControl.RaiseRail(calHeight - .6f);    // Adjust for different coordinates 
		checkUp.SetHeight(calHeight - .3f);             // Adjust for different coordinates 
	}
	
    /// <summary>
    /// Returns the calibrated height of the player
    /// </summary>
    public float GetWallAdjustedHeight()
    {
        return calHeight - 1.1f;
    }

    /// <summary>
    /// Returns the how long the calibration took
    /// </summary>
    /// <returns>Returns the how long the calibration took</returns>
    public float GetCalibrationTime()
    {
        return CALIBRATION_TIME;
    }
    
}
