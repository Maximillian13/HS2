﻿// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class UpDownBob : MonoBehaviour 
{
    // To know which way to bob
    public float upHeight = 4;
    public float downHeight = 2;
    public bool squater;
    public bool biker;
	public bool runner;
    private float speed;

    public bool forceDown;
    public bool forceUp;

    bool goDown;

	// Use this for initialization
	void Start () 
    {
        // Pick at random which way it bobs first
	    if(Random.Range(0,2) == 0)
            goDown = false;
        else
            goDown = true;

        if(forceDown == true)
            goDown = true;
        if(forceUp == true)
            goDown = false;

        if(squater == true)
            speed = Random.Range(.3f, 2f);
        else if(biker == true)
            speed = Random.Range(1, 2);
		else if (runner == true)
			speed = Random.Range(1, 1.5f);
		else
			speed = .15f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        // If it gets to a cretin hight or depth make it switch direction 
	    if(this.transform.localPosition.y > upHeight)//(Using constant numbers now because we dont need it in multiple places might want to change to public values eventually)
            goDown = true;
        if (this.transform.localPosition.y < downHeight)
            goDown = false;

        // Make the object bob up or down
        if(goDown == true)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - (speed * Time.smoothDeltaTime), this.transform.localPosition.z);
        else
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (speed * Time.smoothDeltaTime), this.transform.localPosition.z);
        
	}
}
