using UnityEngine;
using System.Collections;

public class GuideRail : MonoBehaviour
{

    private bool goUp;
    private float counter;
    private BoxCollider[] boxCol;
    const float HEIGHT_UP = 1;
    const float HEIGHT_DOWN = -1.5f;
	
    void Start()
    {
        goUp = true;
        counter = float.PositiveInfinity;
        boxCol = this.GetComponents<BoxCollider>();
    }

	// Update is called once per frame
	void Update () 
    {
	    if(goUp == true && this.transform.localPosition.y < HEIGHT_UP)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (.75f * Time.deltaTime), this.transform.localPosition.z);
        }

        if (goUp == false && this.transform.localPosition.y > HEIGHT_DOWN)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (-.75f * Time.deltaTime), this.transform.localPosition.z);
        }

        if(Time.time > counter + 1 && boxCol[0].enabled == false)
        {
            foreach(BoxCollider b in boxCol)
            {
                b.enabled = true;
            }
        }
	}

    public void LowerRail()
    {
        goUp = false;
        counter = Time.time;
    }
}
