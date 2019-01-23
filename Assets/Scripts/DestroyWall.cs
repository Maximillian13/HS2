// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class DestroyWall : MonoBehaviour 
{
    // How many walls destroyed 
    private int wallsDestroyed;

	// Set-up
	void Start () 
    {
        wallsDestroyed = 0;
	}

    // The collision goes off
    void OnTriggerEnter(Collider other)
    {
        // If its the squat wall
        if(other.tag == "SquatWall")
        {
            // Set the walls destroyed to how many walls went in to the trigger box
            if (other.name == "SquatWall(Clone)")
                wallsDestroyed++;
			if (other.name == "SquatWallx3(Clone)")
				wallsDestroyed += 3;
            if (other.name == "SquatWallx5(Clone)")
                wallsDestroyed += 5;

			if (other.name.Contains("CardioWall"))
				wallsDestroyed++;

            // Move the wall down when it hits 
            MoveWall moveWall = other.GetComponent<MoveWall>();
            moveWall.MoveDown();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If its the squat wall
        if (other.tag == "SquatWall")
        {
            // Destroy the squat wall game object
            Destroy(other.gameObject);
        }
    }

    /// <summary> Gets how many walls have been destroyed </summary>
    /// <returns>Number of walls destroyed</returns>
    public int GetWallsDestroyed()
    {
        return wallsDestroyed;
    }
}
