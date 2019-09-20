// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using TMPro;

public class HeightCalibrator : MonoBehaviour 
{
    // Fields
    public Transform head; // The transform of the headset
    public HandPlacment hp; // Hand placement so the hands know when the game is started
	public PlayerHitBox phb;
	private GuideRail guideRailControl; // The control for how high the guide rail should go
	public CheckUp checkUp; // To set how high the squatter must go to have the next wall count 
    private float calHeight; // The float y where the head is 
    private float timer; // For calibration 
    private const float CALIBRATION_TIME = 7; // How long it takes for the this to calibrate the height 

	private GameObject infoBoard; // Letting the sure know the calibration stuff
	private GameObject[] texts = new GameObject[6];

	const float MIN_HEIGHT = 1.5f;		// The minimum height the calibration can be
	const float MAX_HEIGHT = 2.5f;		// The max height the calibration can be


	void Start()
    {
		infoBoard = this.transform.GetChild(0).gameObject;
		for (int i = 0; i < texts.Length; i++)
			texts[i] = infoBoard.transform.GetChild(i).gameObject;

		for (int i = 1; i < texts.Length; i++)
			texts[i].SetActive(false);

		// Do this after the amount of calibration time
		this.StartCoroutine(this.LateStart(CALIBRATION_TIME));
		guideRailControl = GameObject.Find("GYM").transform.Find("SquatTrack").Find("SquatBars").GetComponent<GuideRail>();
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

		guideRailControl.RaiseRail(this.GetWallAdjustedHeight() + .5f);	// Adjust for different coordinates (.5 for the offset of the rail/walls)
		checkUp.SetHeight(calHeight - .3f);             // Adjust for different coordinates 
	}

    // Update the message depending on how long the user has been playing
    void Update()
    {
		if (infoBoard == null)
			return;

        timer += Time.deltaTime;
        if(timer > 4 && timer < 5)
        {
			texts[3].SetActive(true);
        }
        if (timer > 5 && timer < 6)
        {
			texts[3].SetActive(false);
			texts[2].SetActive(true);
		}
        if (timer > 6 && timer < 7)
        {
			texts[2].SetActive(false);
			texts[1].SetActive(true);
		}
        if (timer > 7 && timer < 8)
        {
			texts[0].SetActive(false);
			texts[1].SetActive(false);
			texts[4].SetActive(true);
			texts[5].SetActive(true);
        }
        if (timer > 8 && timer < 9)
        {
			// Hide the board and start the game
			infoBoard.GetComponent<ShrinkAndDestroy>().ShrinkDestroySpeed(1.03f);
            hp.GameStarted();
			phb.GameStarted();
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
		guideRailControl.RaiseRail(calHeight / 1.5f);    // Adjust for different coordinates 
		GameObject[] wallGOs = GameObject.FindGameObjectsWithTag("SquatWall");
		for (int i = 0; i < wallGOs.Length; i++)
			wallGOs[i].GetComponent<MoveWall>().ChangeHeight(this.GetWallAdjustedHeight());

		//checkUp.SetHeight(calHeight - .3f);             // Adjust for different coordinates 
	}
	
    /// <summary>
    /// Returns the calibrated height of the player
    /// </summary>
    public float GetWallAdjustedHeight()
    {
		//return calHeight - 1.15f;
		return calHeight / 3f;
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
