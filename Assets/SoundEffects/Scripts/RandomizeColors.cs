// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class RandomizeColors : MonoBehaviour 
{
    // What colors it can be
    public Material[] colors;

    // All renders of parent and gibs
    private Renderer[] r;

    private int rand;

	// Use this for initialization
	void Start ()
    {
        // Gets all renderer 
        GameObject topWall = this.transform.GetChild(0).gameObject;
        r = topWall.GetComponentsInChildren<Renderer>();

        // Pick a random number and apply that material to the objects
        rand = Random.Range(0, colors.Length);

        for (int x = 0; x < r.Length; x++)
        {
            if (r[x].name != "Jelly")
            {
                r[x].material = colors[rand];
                if (x > 0)
                {
                    r[x].GetComponent<MeshRenderer>().enabled = false; // Hide the gibs for now
                }
            }
        }
	}
}
