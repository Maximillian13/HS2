﻿// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class HeightCalibrator : MonoBehaviour 
{
    // Fields
    public Transform head; // The transform of the headset
    public TextMesh infoBoard; // Letting the sure know the calibration stuff
    public HandPlacment hp; // Hand placement so the hands know when the game is started
    private float calHeight; // The float y where the head is 
    private float timer; // For calibration 
    private const float CALIBRATION_TIME = 7; // How long it takes for the this to calibrate the height 

    const float MIN_HEIGHT = 1.5f; // The minimum height the calibration can be

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
		calHeight = head.position.y + .1f;
        if(calHeight < MIN_HEIGHT)
        {
            calHeight = MIN_HEIGHT + .1f;
        }
    }

    // Update the message depending on how long the user has been playing
    void Update()
    {
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
    /// Returns the calibrated height of the player
    /// </summary>
    /// <returns>Returns the calibrated height of the player</returns>
    public float GetCalibratedHeight()
    {
        return calHeight;
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
