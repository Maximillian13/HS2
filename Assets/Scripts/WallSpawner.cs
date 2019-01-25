// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class WallSpawner : MonoBehaviour 
{
	// Holds a ton of information on stuff that needs to be shared between
	// "WallSpawner.cs" and "WallSpawnerCardio.cs"
	private GameModeMaster master;

	// Fields 
	private GameObject[] walls = new GameObject[3];

	// Set-up
	void Start()
	{
		master = GameObject.Find("GameModeMaster").GetComponent<GameModeMaster>();

		walls[0] = Resources.Load<GameObject>("Walls/SquatWall");
		walls[1] = Resources.Load<GameObject>("Walls/SquatWallx2");
		walls[2] = Resources.Load<GameObject>("Walls/SquatWallx3");

	}

	// Update is called once per frame
	void Update () 
    {
		// Run the check code and if true return now
		if (master.CheckStopSpawning() == true)
			return;

		// Handle transitions
		master.TransitionHandler();

		// Check if we are on a break
		if (master.HandleOnBreak() == true)
			return;

		master.SpawnWavesHandlerSquat(walls);
	}
}
