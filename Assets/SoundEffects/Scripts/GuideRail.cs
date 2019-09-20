using UnityEngine;
using System.Collections;

public class GuideRail : MonoBehaviour
{ 
    private bool moveToCalPos;
	private bool moveToEnd;
	private float counter;
    private BoxCollider[] boxCol;
    private float heightUp = 1;
    const float HEIGHT_DOWN = -1.5f;

	private HeightCalibrator hCal;

	void Start()
    {
        counter = float.PositiveInfinity;
        boxCol = this.GetComponents<BoxCollider>();
		hCal = GameObject.Find("HeightCalibrator").GetComponent<HeightCalibrator>();
	}

	// Update is called once per frame
	void Update () 
    {
	    if(moveToCalPos == true)
        {
			float coeff = this.transform.position.y > heightUp ? -1 : 1;
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (.8f * coeff * Time.deltaTime), this.transform.localPosition.z);
			if(Vector3.Distance(this.transform.localPosition, new Vector3(this.transform.localPosition.x, heightUp, this.transform.localPosition.z)) < .1f)
			{
				this.transform.localPosition = new Vector3(this.transform.localPosition.x, heightUp, this.transform.localPosition.z);
				moveToCalPos = false;
			}
        }

        if (moveToEnd == true && this.transform.localPosition.y > HEIGHT_DOWN)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (-.8f * Time.deltaTime), this.transform.localPosition.z);
        }

        if(Time.time > counter + .75f && boxCol[0].enabled == false)
        {
            foreach(BoxCollider b in boxCol)
            {
                b.enabled = true;
            }
			counter = float.PositiveInfinity;
		}
	}

	/// <summary>
	/// Raises the rail to a specific height 
	/// </summary>
	public void RaiseRail(float hUp)
	{
		if (moveToEnd == true)
			return;
		moveToCalPos = true;
		this.heightUp = hUp;
	}

	/// <summary>
	/// Pause the colider for a short time to let the walls fly
	/// </summary>
	public void PauseBoxCols()
	{
		for (int i = 0; i < boxCol.Length; i++)
			boxCol[i].enabled = false;
        counter = Time.time;
	}

	/// <summary>
	/// Lower the rail to the bottom position
	/// </summary>
	public void LowerRail()
    {
		this.PauseBoxCols();
        moveToCalPos = false;
		moveToEnd = true;
    }
}
